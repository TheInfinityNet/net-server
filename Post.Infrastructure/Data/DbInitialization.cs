using Bogus;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Others;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Domain.Entities;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.Services.Post.Application.Services;
using InfinityNetServer.Services.Post.Domain.Entities;
using InfinityNetServer.Services.Post.Domain.Enums;
using InfinityNetServer.Services.Post.Domain.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Post.Infrastructure.Data;

public static class DbInitialization
{

    public static void DropDatabase(this IServiceProvider serviceProvider)
    {
        using var serviceScope = serviceProvider.CreateScope();

        var dbContext = serviceScope.ServiceProvider.GetService<PostDbContext>();

        dbContext.Database.EnsureDeleted();
    }

    public static void AutoMigration(this WebApplication webApplication)
    {
        using var serviceScope = webApplication.Services.CreateScope();

        var dbContext = serviceScope.ServiceProvider.GetRequiredService<PostDbContext>();

        if (!dbContext.Database.EnsureCreated()) return;

        dbContext.Database.MigrateAsync().Wait(); //generate all in folder Migration

    }

    public static async void SeedEssentialData(this IServiceProvider serviceProvider, int numberOfPosts)
    {
        using var serviceScope = serviceProvider.CreateScope();
        serviceScope.ServiceProvider.GetService<PostDbContext>();
        var postRepository = serviceScope.ServiceProvider.GetService<IPostRepository>();
        var postPrivacyRepository = serviceScope.ServiceProvider.GetService<IPostPrivacyRepository>();
        var profileClient = serviceScope.ServiceProvider.GetService<CommonProfileClient>();
        var groupClient = serviceScope.ServiceProvider.GetService<CommonGroupClient>();
        var relationshipClient = serviceScope.ServiceProvider.GetService<CommonRelationshipClient>();
        var postService = serviceScope.ServiceProvider.GetService<IPostService>();

        var existingPostCount = await postRepository.GetAllAsync();
        
        if (existingPostCount.Count == 0)
        {
            //IList<string> profileIds = await profileClient.GetProfileIds();
            //IList<GroupMemberWithGroup> groupMemberWithGroups = await groupClient.GetGroupMemberWithGroup();
            //var groupPosts = GeneratePresentationPosts(numberOfPosts / 2, groupMemberWithGroups);
            //await postRepository.CreateAsync(groupPosts);

            var posts = await GeneratePresentationPosts(numberOfPosts, profileClient, relationshipClient);
            await postRepository.CreateAsync(posts);

            IList<Domain.Entities.Post> presentationPosts = await postRepository.GetAllAsync();
            IList<PostPrivacy> postPrivacies = await GeneratePostPrivacies(presentationPosts, relationshipClient);
            await postPrivacyRepository.CreateAsync(postPrivacies);

            var sharedPosts = await GenerateSharedPosts(numberOfPosts / 2, profileClient, postService);
            await postRepository.CreateAsync(sharedPosts);

            presentationPosts = await postRepository.GetAllSharePostsAsync();
            postPrivacies = await GeneratePostPrivacies(presentationPosts, relationshipClient);
            await postPrivacyRepository.CreateAsync(postPrivacies);

            var subPosts = await GenerateSubPosts(numberOfPosts * 2, postRepository);
            await postRepository.CreateAsync(subPosts);
        }
    }

    private static async Task<IList<Domain.Entities.Post>> GeneratePresentationPosts(
    int count, CommonProfileClient profileClient, CommonRelationshipClient relationshipClient)
    {
        // Tải trước dữ liệu profile và relationship
        var profileIds = await profileClient.GetProfileIds();
        var relationshipData = await LoadRelationships(profileIds, relationshipClient);

        // Sử dụng Faker để sinh dữ liệu
        var postFaker = new Faker<Domain.Entities.Post>()
            .RuleFor(p => p.Type, PostType.Text)
            .RuleFor(p => p.OwnerId, f => Guid.Parse(f.PickRandom(profileIds)))
            .RuleFor(p => p.CreatedAt, f => f.Date.Recent(365))
            .RuleFor(p => p.CreatedBy, (f, p) => p.OwnerId);

        var posts = new List<Domain.Entities.Post>();
        await Parallel.ForEachAsync(Enumerable.Range(0, count), async (_, _) =>
        {
            var post = postFaker.Generate();
            var profileIdsWithNames = await profileClient.GetProfileIdsWithNames(relationshipData[post.OwnerId.ToString()]);
            post.Content = GeneratePostContent(profileIdsWithNames);

            lock (posts) posts.Add(post);
        });

        return posts;
    }

    // Hàm phụ để tải relationship data
    private static async Task<Dictionary<string, List<string>>> LoadRelationships(
        IList<string> profileIds, CommonRelationshipClient relationshipClient)
    {
        var tasks = profileIds.ToDictionary(
            id => id,
            id => Task.WhenAll(
                relationshipClient.GetFriendIds(id),
                relationshipClient.GetFollowerIds(id),
                relationshipClient.GetFolloweeIds(id)
            ));

        var results = await Task.WhenAll(tasks.Values);
        return tasks.Keys.Zip(results, (id, relationships) => new
        {
            id,
            combined = relationships[0].Concat(relationships[1]).Distinct().ToList()
        }).ToDictionary(x => x.id, x => x.combined);
    }

    private static async Task<List<Domain.Entities.Post>> GenerateSubPosts(
    int count, IPostRepository postRepository)
    {
        var presentationPosts = await postRepository.GetAllPresentationPostsAsync();

        var subPostFaker = new Faker<Domain.Entities.Post>()
            .RuleFor(p => p.Type, f => f.Random.Bool(0.8f) ? PostType.Photo : PostType.Video)
            .RuleFor(p => p.Presentation, f => f.PickRandom(presentationPosts))
            .RuleFor(p => p.Content, f => new PostContent { Text = f.Lorem.Sentence(10) })
            .RuleFor(p => p.OwnerId, (f, p) => p.Presentation.OwnerId)
            .RuleFor(p => p.CreatedBy, (f, p) => p.Presentation.CreatedBy)
            .RuleFor(p => p.CreatedAt, (f, p) => p.Presentation.CreatedAt)
            .RuleFor(p => p.GroupId, (f, p) => p.Presentation.GroupId)
            .RuleFor(p => p.FileMetadataId, Guid.NewGuid());

        return subPostFaker.Generate(count);
    }

    private static async Task<IList<Domain.Entities.Post>> GenerateSharedPosts(
        int count, CommonProfileClient profileClient, IPostService postService)
    {
        // Lấy danh sách profileId và parentPostId
        var profileIds = await profileClient.GetProfileIds();
        var parentPosts = await postService.GetAllPresentationIds();

        // Chuẩn bị Faker để tạo các Post
        var sharedPostFaker = new Faker<Domain.Entities.Post>()
            .RuleFor(p => p.Type, PostType.Share)
            .RuleFor(p => p.ParentId, f => Guid.Parse(f.PickRandom(parentPosts)))
            .RuleFor(p => p.Content, f => new PostContent { Text = f.Lorem.Sentence(10) })
            .RuleFor(p => p.CreatedAt, f => f.Date.Recent(365));

        IList<Domain.Entities.Post> sharedPosts = [];

        foreach (var _ in Enumerable.Range(0, count))
        {
            var sharedPost = sharedPostFaker.Generate();

            // Lấy danh sách những profile không thể xem
            var whoCantSeeIds = await postService.WhoCantSee(sharedPost.ParentId.ToString());
            var filteredProfileIds = profileIds.Except(whoCantSeeIds).ToList();

            // Đảm bảo có profile hợp lệ
            if (filteredProfileIds.Count == 0) break;

            var randomProfileId = Guid.Parse(new Faker().PickRandom(filteredProfileIds));
            sharedPost.OwnerId = randomProfileId;
            sharedPost.CreatedBy = randomProfileId;

            sharedPosts.Add(sharedPost);
        }

        return sharedPosts;
    }

    private static async Task<IList<PostPrivacy>> GeneratePostPrivacies(
        IList<Domain.Entities.Post> presentationPosts, CommonRelationshipClient relationshipClient)
    {
        var faker = new Faker();
        var postPrivacies = new List<PostPrivacy>();

        foreach (var post in presentationPosts)
        {
            var type = faker.PickRandom<PostPrivacyType>();
            var privacy = CreatePostPrivacy(post, type);

            if (type is PostPrivacyType.Exclude or PostPrivacyType.Include or PostPrivacyType.Custom)
            {
                var combinedIds = await GetCombinedIds(post.OwnerId.ToString(), relationshipClient);

                if (combinedIds.Count != 0)
                {
                    var selectedIds = faker.PickRandom(combinedIds, faker.Random.Int(1, Math.Min(combinedIds.Count, 5)));

                    foreach (var profileId in selectedIds)
                        AddPrivacyDetail(privacy, Guid.Parse(profileId), type, post.CreatedBy.Value, post.CreatedAt);
                }
            }

            postPrivacies.Add(privacy);
        }

        return postPrivacies;
    }

    private static void AddPrivacyDetail(
        PostPrivacy postPrivacy, Guid profileId, PostPrivacyType type, Guid createdBy, DateTime createdAt)
    {

        if (type == PostPrivacyType.Exclude || type == PostPrivacyType.Custom)
            postPrivacy.PostPrivacyExcludes.Add(new PostPrivacyExclude
            {
                PostPrivacyId = postPrivacy.Id,
                ProfileId = profileId,
                CreatedBy = createdBy,
                CreatedAt = createdAt
            });

        if (type == PostPrivacyType.Include || type == PostPrivacyType.Custom)
            postPrivacy.PostPrivacyIncludes.Add(new PostPrivacyInclude
            {
                PostPrivacyId = postPrivacy.Id,
                ProfileId = profileId,
                CreatedBy = createdBy,
                CreatedAt = createdAt
            });
    }

    // Hàm phụ tạo PostPrivacy
    private static PostPrivacy CreatePostPrivacy(Domain.Entities.Post post, PostPrivacyType type)
    {
        return new PostPrivacy
        {
            PostId = post.Id,
            Type = type,
            CreatedBy = post.CreatedBy.Value,
            CreatedAt = post.CreatedAt
        };
    }

    // Hàm phụ lấy combined IDs
    private static async Task<List<string>> GetCombinedIds(
        string ownerId, CommonRelationshipClient relationshipClient)
    {
        var tasks = new[]
        {
        relationshipClient.GetFollowerIds(ownerId),
        relationshipClient.GetFolloweeIds(ownerId),
        relationshipClient.GetFriendIds(ownerId)
    };

        var results = await Task.WhenAll(tasks);
        return results.SelectMany(r => r).Distinct().ToList();
    }

    private static PostContent GeneratePostContent(IList<ProfileIdWithName> profileIdsWithNames)
    {
        Faker faker = new();
        StringBuilder contentBuilder = new();
        var postContent = new PostContent();
        var usedProfileIds = new HashSet<ProfileIdWithName>();

        int start = 0;
        for (int i = 0; i < faker.Random.Int(1, Math.Min(3, profileIdsWithNames.Count)); i++)
        {
            var facetType = faker.PickRandom<FacetType>();

            if (facetType == FacetType.Hashtag)
            {
                var hashtag = faker.Lorem.Word();
                contentBuilder.Append($" #{hashtag}");
                postContent.HashtagFacets.Add(new HashtagFacet
                {
                    Type = FacetType.Hashtag,
                    Start = start,
                    End = start + hashtag.Length
                });
            }
            else if (facetType == FacetType.Tag)
            {
                var profile = faker.PickRandom(profileIdsWithNames.Where(p => !usedProfileIds.Contains(p)));
                contentBuilder.Append($" @{profile.Name}");
                postContent.TagFacets.Add(new TagFacet
                {
                    Type = FacetType.Tag,
                    ProfileId = Guid.Parse(profile.Id),
                    Start = start,
                    End = start + profile.Name.Length
                });
                usedProfileIds.Add(profile);
            }

            start = contentBuilder.Length;
        }

        postContent.Text = contentBuilder.ToString();
        return postContent;
    }



    //private static async Task<IList<Domain.Entities.Post>> GeneratePresentationPosts(
    //    int count, CommonProfileClient profileClient, CommonRelationshipClient relationshipClient)
    //{
    //    IList<string> profileIds = await profileClient.GetProfileIds();
    //    var faker = new Faker<Domain.Entities.Post>()
    //        .CustomInstantiator(f =>
    //        {
    //            var randomProfileId = Guid.Parse(f.PickRandom(profileIds));
    //            return new Domain.Entities.Post
    //            {
    //                Type = PostType.Text,
    //                OwnerId = randomProfileId,
    //                CreatedBy = randomProfileId,
    //                CreatedAt = f.Date.Recent(f.Random.Int(1, 365))
    //            };
    //        });

    //    IList<Domain.Entities.Post> posts = [];
    //    for (int i = 0; i < count; i++)
    //    {
    //        var post = faker.Generate();
    //        var friendIds = await relationshipClient.GetFriendIds(post.OwnerId.ToString());
    //        var folowerIds = await relationshipClient.GetFollowerIds(post.OwnerId.ToString());
    //        var combinedIds = friendIds.Concat(folowerIds).Distinct().ToList();
    //        var profileIdsWithNames = await profileClient.GetProfileIdsWithNames(combinedIds);
    //        post.Content = GeneratePostContent(profileIdsWithNames);
    //        posts.Add(post);
    //    }
    //    return posts;
    //}

    //private static async Task<List<Domain.Entities.Post>> GenerateSubPosts(
    //    int count, IPostRepository postRepository)
    //{
    //    var presentationPosts = await postRepository.GetAllAsync();
    //    var faker = new Faker<Domain.Entities.Post>()
    //        .CustomInstantiator(f =>
    //        {
    //            var randomPresentationPost = f.PickRandom(presentationPosts);
    //            var type = f.PickRandom<PostType>();
    //            var mediaId = (type != PostType.Text) ? Guid.NewGuid() : (Guid?)null;

    //            return new Domain.Entities.Post
    //            {
    //                Type = type,
    //                Content = new()
    //                {
    //                    Text = f.Lorem.Sentence(10)
    //                },
    //                GroupId = randomPresentationPost.GroupId,
    //                Presentation = randomPresentationPost,
    //                OwnerId = randomPresentationPost.OwnerId,
    //                FileMetadataId = mediaId,
    //                CreatedBy = randomPresentationPost.CreatedBy,
    //                CreatedAt = randomPresentationPost.CreatedAt
    //            };
    //        });

    //    return faker.Generate(count);
    //}

    //private static async Task<List<Domain.Entities.Post>> GenerateSharedPosts(
    //    int count,
    //    CommonProfileClient profileClient,
    //    IPostRepository postRepository)
    //{
    //    var profileIds = await profileClient.GetProfileIds();
    //    var parentPosts = await postRepository.GetAllAsync();

    //    var faker = new Faker<Domain.Entities.Post>()
    //        .CustomInstantiator(f =>
    //        {
    //            var randomProfileId = f.PickRandom(profileIds);
    //            var randomParentPost = f.PickRandom(parentPosts);

    //            return new Domain.Entities.Post
    //            {
    //                Type = PostType.Share,
    //                Parent = randomParentPost,
    //                Content = new()
    //                {
    //                    Text = f.Lorem.Sentence(10)
    //                },
    //                OwnerId = Guid.Parse(randomProfileId),
    //                CreatedBy = Guid.Parse(randomProfileId),
    //                CreatedAt = f.Date.Recent(f.Random.Int(1, 365))
    //            };
    //        });

    //    return faker.Generate(count);
    //}

    //private static async Task<IList<PostPrivacy>> GeneratePostPrivacies(
    //    IList<Domain.Entities.Post> presentationPosts, CommonRelationshipClient relationshipClient)
    //{
    //    var faker = new Faker();
    //    var postPrivacies = new List<PostPrivacy>();

    //    foreach (var post in presentationPosts)
    //    {
    //        var type = faker.PickRandom<PostPrivacyType>();
    //        var createdBy = post.CreatedBy.Value;
    //        var createdAt = post.CreatedAt;
    //        var postPrivacy = new PostPrivacy
    //        {
    //            PostId = post.Id,
    //            Type = type,
    //            CreatedBy = createdBy,
    //            CreatedAt = createdAt
    //        };

    //        if (type is PostPrivacyType.Exclude or PostPrivacyType.Include or PostPrivacyType.Custom)
    //        {
    //            var followerIds = await relationshipClient.GetFollowerIds(post.OwnerId.ToString());
    //            var followeeIds = await relationshipClient.GetFolloweeIds(post.OwnerId.ToString());
    //            var friendIds = await relationshipClient.GetFriendIds(post.OwnerId.ToString());

    //            var combinedIds = followerIds.Concat(followeeIds).Concat(friendIds).Distinct().ToList();

    //            if (combinedIds.Count > 0)
    //            {
    //                int totalCount = faker.Random.Int(1, Math.Min(combinedIds.Count, 5));
    //                var selectedIds = faker.PickRandom(combinedIds, Math.Min(combinedIds.Count, totalCount)).Distinct();

    //                foreach (var profileId in selectedIds)
    //                    AddPrivacyDetail(postPrivacy, Guid.Parse(profileId), type, createdBy, createdAt);
    //            }
    //        }

    //        postPrivacies.Add(postPrivacy);
    //    }
    //    return postPrivacies;
    //}

    //private static PostContent GeneratePostContent(IList<ProfileIdWithName> profileIdsWithNames)
    //{
    //    Faker faker = new();
    //    PostContent postContent = new();
    //    int facetCount = faker.Random.Int(1, profileIdsWithNames.Count < 3 ? profileIdsWithNames.Count : 3);
    //    int minContentLength = 50;
    //    int minStart = 0;
    //    IList<string> text = [faker.Lorem.Sentence(minContentLength)];
    //    IList<ProfileIdWithName> usedProfileIdsWithNames = [];

    //    for (int i = 0; i < facetCount; i++)
    //    {
    //        FacetType facetType = faker.PickRandom<FacetType>();

    //        if (facetType == FacetType.Hashtag)
    //        {
    //            var randomHashTag = faker.Lorem.Word();
    //            int hashtagLength = randomHashTag.Length;

    //            // Đảm bảo minStart không vượt qua minContentLength - hashtagLength
    //            if (minStart > minContentLength - hashtagLength)
    //            {
    //                minContentLength = minStart + hashtagLength + 1;

    //                // Đảm bảo chuỗi hiện tại đủ dài
    //                while (text[^1].Length < minContentLength)
    //                    text[^1] += faker.Lorem.Sentence(10);
    //            }

    //            int start = faker.Random.Int(minStart, minContentLength - hashtagLength);
    //            int end = start + hashtagLength;

    //            postContent.HashtagFacets.Add(new HashtagFacet
    //            {
    //                Type = FacetType.Hashtag,
    //                Start = start,
    //                End = end,
    //                Tag = randomHashTag
    //            });

    //            text[^1] = text[^1].Insert(start, $" #{randomHashTag}");

    //            minContentLength += 20;
    //            minStart = end;

    //            // Thêm một câu mới để đảm bảo nội dung tiếp theo có chỗ chèn
    //            text.Add(faker.Lorem.Sentence(20));
    //        }

    //        else if (facetType == FacetType.Tag)
    //        {
    //            var randomProfileIdWithName = faker.PickRandom(profileIdsWithNames);
    //            while (usedProfileIdsWithNames.Contains(randomProfileIdWithName))
    //                randomProfileIdWithName = faker.PickRandom(profileIdsWithNames);

    //            int tagLength = randomProfileIdWithName.Name.Length;

    //            if (minStart > minContentLength - tagLength)
    //            {
    //                minContentLength = minStart + tagLength + 1;

    //                while (text[^1].Length < minContentLength)
    //                    text[^1] += faker.Lorem.Sentence(10);
    //            }

    //            int start = faker.Random.Int(minStart, minContentLength - tagLength);
    //            int end = start + tagLength;

    //            postContent.TagFacets.Add(new TagFacet
    //            {
    //                Type = FacetType.Tag,
    //                ProfileId = Guid.Parse(randomProfileIdWithName.Id),
    //                Start = start,
    //                End = end
    //            });

    //            text[^1] = text[^1].Insert(start, $" @{randomProfileIdWithName.Name}");
    //            usedProfileIdsWithNames.Add(randomProfileIdWithName);

    //            minContentLength += 50;
    //            minStart = end;
    //            text.Add(faker.Lorem.Sentence(50));
    //        }

    //    }

    //    postContent.Text = string.Join(" ", text);

    //    return postContent;
    //}

    //private static List<Domain.Entities.Post> GeneratePresentationPosts(
    //int count, IList<GroupMemberWithGroup> groupMemberWithGroups)
    //{
    //    var faker = new Faker<Domain.Entities.Post>()
    //        .CustomInstantiator(f =>
    //        {
    //            var randomGroupMemberWithGroups = f.PickRandom(groupMemberWithGroups);
    //            return new Domain.Entities.Post
    //            {
    //                Type = PostType.Text,
    //                Content = new()
    //                {
    //                    Text = f.Lorem.Sentence(100)
    //                },
    //                GroupId = Guid.Parse(randomGroupMemberWithGroups.GroupId),
    //                OwnerId = Guid.Parse(randomGroupMemberWithGroups.UserProfileId),
    //                CreatedBy = Guid.Parse(randomGroupMemberWithGroups.UserProfileId)
    //            };
    //        });

    //    return faker.Generate(count);
    //}

}
