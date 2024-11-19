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

    public static async void SeedEssentialData(this IServiceProvider serviceProvider)
    {
        using var serviceScope = serviceProvider.CreateScope();
        serviceScope.ServiceProvider.GetService<PostDbContext>();
        var postRepository = serviceScope.ServiceProvider.GetService<IPostRepository>();
        var postAudienceRepository = serviceScope.ServiceProvider.GetService<IPostAudienceRepository>();
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

            //var posts = await GeneratePresentationPosts(numberOfPosts, profileClient, relationshipClient);
            //await postRepository.CreateAsync(posts);

            //IList<Domain.Entities.Post> presentationPosts = await postRepository.GetAllAsync();
            //IList<PostAudience> postAudiences = await GeneratePostAudiences(presentationPosts, relationshipClient);
            //await postPrivacyRepository.CreateAsync(postAudiences);

            //var sharedPosts = await GenerateSharedPosts(numberOfPosts / 4, profileClient, postService);
            //await postRepository.CreateAsync(sharedPosts);

            //presentationPosts = await postRepository.GetAllSharePostsAsync();
            //postAudiences = await GeneratePostAudiences(presentationPosts, relationshipClient);
            //await postPrivacyRepository.CreateAsync(postAudiences);

            //var subPosts = await GenerateSubPosts(numberOfPosts, postRepository);
            //await postRepository.CreateAsync(subPosts);

            // Tải trước dữ liệu profile và relationship
            var profileIds = await profileClient.GetProfileIds();

            var textPosts = await GenerateTextPosts(100, profileClient, relationshipClient);
            var mediaPost = await GenerateMediaPosts(200, profileClient, relationshipClient);
            var multiMediaPosts = GenerateMultiMediaPosts(300, profileIds);

            var combinedPosts = textPosts.Concat(mediaPost).Concat(multiMediaPosts).ToList();
            await postRepository.CreateAsync(combinedPosts);

            var postAudiences = await GeneratePostAudiences(combinedPosts, relationshipClient);
            await postAudienceRepository.CreateAsync(postAudiences);

            var sharedPosts = await GenerateSharedPosts(100, profileClient, relationshipClient, postService);
            await postRepository.CreateAsync(sharedPosts);

            postAudiences = await GeneratePostAudiences(sharedPosts, relationshipClient);
            await postAudienceRepository.CreateAsync(postAudiences);
        }
    }

    private static async Task<IList<Domain.Entities.Post>> GenerateTextPosts(
        int count, CommonProfileClient profileClient, CommonRelationshipClient relationshipClient)
    {
        var profileIds = await profileClient.GetProfileIds();
        var relationshipData = await LoadRelationships(profileIds, relationshipClient);

        var postFaker = new Faker<Domain.Entities.Post>()
            .RuleFor(p => p.Id, _ => Guid.NewGuid())
            .RuleFor(p => p.Type, PostType.Text)
            .RuleFor(p => p.OwnerId, f => Guid.Parse(f.PickRandom(profileIds)))
            .RuleFor(p => p.CreatedAt, f => f.Date.Recent(365))
            .RuleFor(p => p.CreatedBy, (f, p) => p.OwnerId);

        var tasks = Enumerable.Range(0, count).Select(async _ =>
        {
            var post = postFaker.Generate();
            var profileIdsWithNames = await profileClient.GetProfileIdsWithNames(relationshipData[post.OwnerId.ToString()]);
            post.Content = GeneratePostContent(profileIdsWithNames);
            return post;
        });

        return await Task.WhenAll(tasks);
    }

    private static async Task<IList<Domain.Entities.Post>> GenerateMediaPosts(
        int count, CommonProfileClient profileClient, CommonRelationshipClient relationshipClient)
    {
        var profileIds = await profileClient.GetProfileIds();
        var relationshipData = await LoadRelationships(profileIds, relationshipClient);

        var postFaker = new Faker<Domain.Entities.Post>()
            .RuleFor(p => p.Id, _ => Guid.NewGuid())
            .RuleFor(p => p.Type, f => f.Random.Bool(0.5f) ? PostType.Photo : PostType.Video)
            .RuleFor(p => p.FileMetadataId, _ => Guid.NewGuid())
            .RuleFor(p => p.OwnerId, f => Guid.Parse(f.PickRandom(profileIds)))
            .RuleFor(p => p.CreatedAt, f => f.Date.Recent(365))
            .RuleFor(p => p.CreatedBy, (f, p) => p.OwnerId);

        var tasks = Enumerable.Range(0, count).Select(async _ =>
        {
            var post = postFaker.Generate();
            var profileIdsWithNames = await profileClient.GetProfileIdsWithNames(relationshipData[post.OwnerId.ToString()]);
            post.Content = GeneratePostContent(profileIdsWithNames);
            return post;
        });

        return await Task.WhenAll(tasks);
    }

    private static async Task<IList<Domain.Entities.Post>> GenerateSharedPosts(
        int count, CommonProfileClient profileClient, CommonRelationshipClient relationshipClient, IPostService postService)
    {
        var profileIds = await profileClient.GetProfileIds() ?? throw new InvalidOperationException("No profile IDs found.");
        var relationshipData = await LoadRelationships(profileIds, relationshipClient);
        var parentPosts = await postService.GetAllPresentationIds() ?? throw new InvalidOperationException("No parent posts found.");

        var sharedPostFaker = new Faker<Domain.Entities.Post>()
            .RuleFor(p => p.Id, _ => Guid.NewGuid())
            .RuleFor(p => p.Type, PostType.Share)
            .RuleFor(p => p.ParentId, f => Guid.Parse(f.PickRandom(parentPosts)))
            .RuleFor(p => p.Content, f => new PostContent { Text = f.Lorem.Sentence(10) })
            .RuleFor(p => p.CreatedAt, f => f.Date.Recent(365));

        var tasks = Enumerable.Range(0, count).Select(async _ =>
        {
            var sharedPost = sharedPostFaker.Generate();

            var whoCantSeeIds = await postService.WhoCantSee(sharedPost.ParentId.ToString());
            var filteredProfileIds = profileIds.Except(whoCantSeeIds).ToList();

            if (filteredProfileIds.Count == 0) return null; // Bỏ qua nếu không còn profile nào

            var randomProfileId = Guid.Parse(new Faker().PickRandom(filteredProfileIds));
            sharedPost.OwnerId = randomProfileId;
            sharedPost.CreatedBy = randomProfileId;

            var profileIdsWithNames = await profileClient.GetProfileIdsWithNames(relationshipData[randomProfileId.ToString()]);
            sharedPost.Content = GeneratePostContent(profileIdsWithNames);

            return sharedPost;
        });

        return (await Task.WhenAll(tasks)).Where(p => p != null).ToList(); // Loại bỏ null
    }

    private static IList<Domain.Entities.Post> GenerateMultiMediaPosts(int count, IList<string> profileIds)
    {
        var random = new Random();

        // Sử dụng Faker để sinh dữ liệu cho presentation posts
        var postFaker = new Faker<Domain.Entities.Post>()
            .RuleFor(p => p.Id, f => Guid.NewGuid()) // Đảm bảo GUID duy nhất
            .RuleFor(p => p.Type, PostType.MultiMedia)
            .RuleFor(p => p.Content, f => new PostContent { Text = f.Lorem.Sentence(10) })
            .RuleFor(p => p.OwnerId, f => Guid.Parse(f.PickRandom(profileIds)))
            .RuleFor(p => p.CreatedAt, f => f.Date.Recent(365))
            .RuleFor(p => p.CreatedBy, (f, p) => p.OwnerId)
            .RuleFor(p => p.SubPosts, []);

        IList<Domain.Entities.Post> presentationPosts = postFaker.Generate(count);

        foreach (var presentationPost in presentationPosts)
        {
            var subPostFaker = new Faker<Domain.Entities.Post>()
                .RuleFor(p => p.Id, f => Guid.NewGuid()) // Đảm bảo GUID duy nhất
                .RuleFor(p => p.Type, f => f.Random.Bool(0.8f) ? PostType.Photo : PostType.Video)
                .RuleFor(p => p.FileMetadataId, f => Guid.NewGuid())
                .RuleFor(p => p.Parent, presentationPost) // Liên kết cha
                .RuleFor(p => p.ParentId, f => presentationPost.Id)
                .RuleFor(p => p.Content, f => new PostContent { Text = f.Lorem.Sentence(10) })
                .RuleFor(p => p.OwnerId, f => presentationPost.OwnerId)
                .RuleFor(p => p.CreatedBy, f => presentationPost.CreatedBy)
                .RuleFor(p => p.CreatedAt, f => presentationPost.CreatedAt)
                .RuleFor(p => p.GroupId, f => presentationPost.GroupId);

            int randomSubPostCount = random.Next(2, 5);
            var subPosts = subPostFaker.Generate(randomSubPostCount);

            // Thêm subposts vào danh sách SubPosts của presentationPost
            foreach (var subPost in subPosts)
            {
                presentationPost.SubPosts.Add(subPost);
            }
        }

        return presentationPosts;
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

    //private static async Task<IList<Domain.Entities.Post>> GeneratePresentationPosts(
    //    int count, CommonProfileClient profileClient, CommonRelationshipClient relationshipClient)
    //{
    //    // Tải trước dữ liệu profile và relationship
    //    var profileIds = await profileClient.GetProfileIds();
    //    var relationshipData = await LoadRelationships(profileIds, relationshipClient);

    //    // Sử dụng Faker để sinh dữ liệu
    //    var postFaker = new Faker<Domain.Entities.Post>()
    //        .RuleFor(p => p.Type, PostType.Text)
    //        .RuleFor(p => p.OwnerId, f => Guid.Parse(f.PickRandom(profileIds)))
    //        .RuleFor(p => p.CreatedAt, f => f.Date.Recent(365))
    //        .RuleFor(p => p.CreatedBy, (f, p) => p.OwnerId);

    //    var posts = new List<Domain.Entities.Post>();
    //    await Parallel.ForEachAsync(Enumerable.Range(0, count), async (_, _) =>
    //    {
    //        var post = postFaker.Generate();
    //        var profileIdsWithNames = await profileClient.GetProfileIdsWithNames(relationshipData[post.OwnerId.ToString()]);
    //        post.Content = GeneratePostContent(profileIdsWithNames);

    //        lock (posts) posts.Add(post);
    //    });

    //    return posts;
    //}

    //private static async Task<List<Domain.Entities.Post>> GenerateSubPosts(
    //int count, IPostRepository postRepository)
    //{
    //    var presentationPosts = await postRepository.GetAllPresentationPostsAsync();

    //    var subPostFaker = new Faker<Domain.Entities.Post>()
    //        .RuleFor(p => p.Type, f => f.Random.Bool(0.8f) ? PostType.Photo : PostType.Video)
    //        .RuleFor(p => p.Presentation, f => f.PickRandom(presentationPosts))
    //        .RuleFor(p => p.Content, f => new PostContent { Text = f.Lorem.Sentence(10) })
    //        .RuleFor(p => p.OwnerId, (f, p) => p.Presentation.OwnerId)
    //        .RuleFor(p => p.CreatedBy, (f, p) => p.Presentation.CreatedBy)
    //        .RuleFor(p => p.CreatedAt, (f, p) => p.Presentation.CreatedAt)
    //        .RuleFor(p => p.GroupId, (f, p) => p.Presentation.GroupId)
    //        .RuleFor(p => p.FileMetadataId, Guid.NewGuid());

    //    return subPostFaker.Generate(count);
    //}

    private static async Task<IList<PostAudience>> GeneratePostAudiences(
        IList<Domain.Entities.Post> presentationPosts, CommonRelationshipClient relationshipClient)
    {
        var faker = new Faker();
        var postAudiences = new List<PostAudience>();

        foreach (var post in presentationPosts)
        {
            var type = GetRandomAudienceType();
            var audience = CreatePostAudience(post, type);

            if (type is PostAudienceType.Exclude or PostAudienceType.Include or PostAudienceType.Custom)
            {
                var combinedIds = await GetCombinedIds(post.OwnerId.ToString(), relationshipClient);

                if (combinedIds.Count != 0)
                {
                    var selectedIds = faker.PickRandom(combinedIds, faker.Random.Int(1, Math.Min(combinedIds.Count, 5)));

                    foreach (var profileId in selectedIds)
                        AddAudienceDetail(audience, Guid.Parse(profileId), type, post.CreatedBy.Value, post.CreatedAt);
                }
            }

            postAudiences.Add(audience);
        }

        return postAudiences;
    }

    public static PostAudienceType GetRandomAudienceType()
    {
        // Danh sách các giá trị với tần suất tương ứng
        var audienceTypes = new List<PostAudienceType>
        {
            PostAudienceType.Public, // 40%
            PostAudienceType.Public,
            PostAudienceType.Public,
            PostAudienceType.Public,

            PostAudienceType.Friends, // 20%
            PostAudienceType.Friends,

            PostAudienceType.Include, // 10%
            PostAudienceType.Include,

            PostAudienceType.Exclude, // 10%
            PostAudienceType.Exclude,

            PostAudienceType.Custom, // 10%
            PostAudienceType.Custom,

            PostAudienceType.OnlyMe // 10%
        };

        // Chọn một giá trị ngẫu nhiên từ danh sách trên
        var random = new Faker();
        return random.PickRandom(audienceTypes);
    }

    private static void AddAudienceDetail(
        PostAudience postAudience, Guid profileId, PostAudienceType type, Guid createdBy, DateTime createdAt)
    {

        if (type == PostAudienceType.Exclude || type == PostAudienceType.Custom)
            postAudience.Excludes.Add(new PostAudienceExclude
            {
                AudienceId = postAudience.Id,
                ProfileId = profileId,
                CreatedBy = createdBy,
                CreatedAt = createdAt
            });

        if (type == PostAudienceType.Include || type == PostAudienceType.Custom)
            postAudience.Includes.Add(new PostAudienceInclude
            {
                AudienceId = postAudience.Id,
                ProfileId = profileId,
                CreatedBy = createdBy,
                CreatedAt = createdAt
            });
    }

    // Hàm phụ tạo PostAudience
    private static PostAudience CreatePostAudience(Domain.Entities.Post post, PostAudienceType type)
    {
        return new PostAudience
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
                    Tag = hashtag,
                    Type = FacetType.Hashtag,
                    Start = start,
                    End = start + hashtag.Length
                });
            }
            else if (facetType == FacetType.Tag)
            {
                var profile = faker.PickRandom(profileIdsWithNames.Where(p => !usedProfileIds.Contains(p)));
                contentBuilder.Append($" @{profile.Name}");
                postContent.TagFacets.Add(new BuildingBlocks.Domain.Entities.TagFacet
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
