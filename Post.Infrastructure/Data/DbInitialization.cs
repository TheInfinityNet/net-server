using Bogus;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.Services.Post.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfinityNetServer.Services.Post.Domain.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Others;
using InfinityNetServer.Services.Post.Domain.Entities;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.BuildingBlocks.Domain.Entities;
using System.Linq;

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

        var existingPostCount = await postRepository.GetAllAsync();
        
        if (existingPostCount.Count == 0)
        {
            //IList<string> profileIds = await profileClient.GetProfileIds();
            //IList<GroupMemberWithGroup> groupMemberWithGroups = await groupClient.GetGroupMemberWithGroup();
            
            var posts = await GeneratePresentationPosts(numberOfPosts / 2, profileClient, relationshipClient);
            await postRepository.CreateAsync(posts);

            //var groupPosts = GeneratePresentationPosts(numberOfPosts / 2, groupMemberWithGroups);
            //await postRepository.CreateAsync(groupPosts);

            IList<Domain.Entities.Post> presentationPosts = await postRepository.GetAllAsync();
            IList<PostPrivacy> postPrivacies = await GeneratePostPrivacies(presentationPosts, relationshipClient);
            await postPrivacyRepository.CreateAsync(postPrivacies);

            Faker faker = new ();
            var subPosts = await GenerateSubPosts(faker.Random.Int(1, numberOfPosts), postRepository);
            await postRepository.CreateAsync(subPosts);

            var sharedPosts = await GenerateSharedPosts(
                faker.Random.Int(1, numberOfPosts), profileClient, postRepository);
            await postRepository.CreateAsync(sharedPosts);
        }
    }

    private static async Task<IList<Domain.Entities.Post>> GeneratePresentationPosts(
        int count, CommonProfileClient profileClient, CommonRelationshipClient relationshipClient)
    {
        IList<string> profileIds = await profileClient.GetProfileIds();
        var faker = new Faker<Domain.Entities.Post>()
            .CustomInstantiator(f =>
            {
                var randomProfileId = Guid.Parse(f.PickRandom(profileIds));
                return new Domain.Entities.Post
                {
                    Type = PostType.Text,
                    OwnerId = randomProfileId,
                    CreatedBy = randomProfileId,
                    CreatedAt = f.Date.Recent(f.Random.Int(1, 365))
                };
            });

        IList<Domain.Entities.Post> posts = [];
        for (int i = 0; i < count; i++)
        {
            var post = faker.Generate();
            var friendIds = await relationshipClient.GetFriendIds(post.OwnerId.ToString());
            var folowerIds = await relationshipClient.GetFollowerIds(post.OwnerId.ToString());
            var combinedIds = friendIds.Concat(folowerIds).Distinct().ToList();
            var profileIdsWithNames = await profileClient.GetProfileIdsWithNames(combinedIds);
            post.Content = GeneratePostContent(profileIdsWithNames);
            posts.Add(post);
        }
        return posts;
    }

    private static List<Domain.Entities.Post> GeneratePresentationPosts(
        int count, IList<GroupMemberWithGroup> groupMemberWithGroups)
    {
        var faker = new Faker<Domain.Entities.Post>()
            .CustomInstantiator(f =>
            {
                var randomGroupMemberWithGroups = f.PickRandom(groupMemberWithGroups);
                return new Domain.Entities.Post
                {
                    Type = PostType.Text,
                    Content = new()
                    {
                        Text = f.Lorem.Sentence(100)
                    },
                    GroupId = Guid.Parse(randomGroupMemberWithGroups.GroupId),
                    OwnerId = Guid.Parse(randomGroupMemberWithGroups.UserProfileId),
                    CreatedBy = Guid.Parse(randomGroupMemberWithGroups.UserProfileId) 
                };
            });

        return faker.Generate(count);
    }

    private static async Task<List<Domain.Entities.Post>> GenerateSubPosts(
        int count, IPostRepository postRepository)
    {
        var presentationPosts = await postRepository.GetAllAsync();
        var faker = new Faker<Domain.Entities.Post>()
            .CustomInstantiator(f =>
            {
                var randomPresentationPost = f.PickRandom(presentationPosts);
                var type = f.PickRandom<PostType>();
                var mediaId = (type != PostType.Text) ? Guid.NewGuid() : (Guid?)null;

                return new Domain.Entities.Post
                {
                    Type = type,
                    Content = new()
                    {
                        Text = f.Lorem.Sentence(10)
                    },
                    GroupId = randomPresentationPost.GroupId,
                    Presentation = randomPresentationPost,
                    OwnerId = randomPresentationPost.OwnerId,
                    FileMetadataId = mediaId,
                    CreatedBy = randomPresentationPost.CreatedBy,
                    CreatedAt = randomPresentationPost.CreatedAt
                };
            });

        return faker.Generate(count);
    }

    private static async Task<List<Domain.Entities.Post>> GenerateSharedPosts(
        int count,
        CommonProfileClient profileClient,
        IPostRepository postRepository)
    {
        var profileIds = await profileClient.GetProfileIds();
        var parentPosts = await postRepository.GetAllAsync();

        var faker = new Faker<Domain.Entities.Post>()
            .CustomInstantiator(f =>
            {
                var randomProfileId = f.PickRandom(profileIds);
                var randomParentPost = f.PickRandom(parentPosts);
                
                return new Domain.Entities.Post
                {
                    Type = PostType.Share,
                    Parent = randomParentPost,
                    Content = new()
                    {
                        Text = f.Lorem.Sentence(10)
                    },
                    OwnerId = Guid.Parse(randomProfileId),
                    CreatedBy = Guid.Parse(randomProfileId),
                    CreatedAt = f.Date.Recent(f.Random.Int(1, 365))
                };
            });

        return faker.Generate(count);
    }

    private static async Task<IList<PostPrivacy>> GeneratePostPrivacies(
        IList<Domain.Entities.Post> presentationPosts, CommonRelationshipClient relationshipClient)
    {
        var faker = new Faker();
        var postPrivacies = new List<PostPrivacy>();

        foreach (var post in presentationPosts)
        {
            var type = faker.PickRandom<PostPrivacyType>();
            var createdBy = post.CreatedBy.Value;
            var createdAt = post.CreatedAt;
            var postPrivacy = new PostPrivacy
            {
                PostId = post.Id,
                Type = type,
                CreatedBy = createdBy,
                CreatedAt = createdAt
            };

            if (type is PostPrivacyType.Exclude or PostPrivacyType.Include or PostPrivacyType.Custom)
            {
                var followerIds = await relationshipClient.GetFollowerIds(post.OwnerId.ToString());
                var followeeIds = await relationshipClient.GetFolloweeIds(post.OwnerId.ToString());
                var friendIds = await relationshipClient.GetFriendIds(post.OwnerId.ToString());

                var combinedIds = followerIds.Concat(followeeIds).Concat(friendIds).Distinct().ToList();

                if (combinedIds.Count > 0)
                {
                    int totalCount = faker.Random.Int(1, Math.Min(combinedIds.Count, 5));
                    var selectedIds = faker.PickRandom(combinedIds, Math.Min(combinedIds.Count, totalCount)).Distinct();

                    foreach (var profileId in selectedIds)
                        AddPrivacyDetail(postPrivacy, Guid.Parse(profileId), type, createdBy, createdAt);
                }
            }

            postPrivacies.Add(postPrivacy);
        }
        return postPrivacies;
    }

    private static void AddPrivacyDetail(
        PostPrivacy postPrivacy, Guid profileId, PostPrivacyType type, Guid createdBy, DateTime createdAt)
    {

        if (type == PostPrivacyType.Exclude || type == PostPrivacyType.Custom)
        {
            postPrivacy.PostPrivacyExcludes.Add(new PostPrivacyExclude
            {
                PostPrivacyId = postPrivacy.Id,
                ProfileId = profileId,
                CreatedBy = createdBy,
                CreatedAt = createdAt
            });
        }

        if (type == PostPrivacyType.Include || type == PostPrivacyType.Custom)
        {
            postPrivacy.PostPrivacyIncludes.Add(new PostPrivacyInclude
            {
                PostPrivacyId = postPrivacy.Id,
                ProfileId = profileId,
                CreatedBy = createdBy,
                CreatedAt = createdAt
            });
        }
    }

    private static PostContent GeneratePostContent(IList<ProfileIdWithName> profileIdsWithNames)
    {
        Faker faker = new();
        PostContent postContent = new();
        int facetCount = faker.Random.Int(1, profileIdsWithNames.Count < 3 ? profileIdsWithNames.Count : 3);
        int minContentLength = 50;
        int minStart = 0;
        IList<string> text = [faker.Lorem.Sentence(minContentLength)];
        IList<ProfileIdWithName> usedProfileIdsWithNames = [];

        for (int i = 0; i < facetCount; i++)
        {
            FacetType facetType = faker.PickRandom<FacetType>();

            if (facetType == FacetType.Hashtag)
            {
                var randomHashTag = faker.Lorem.Word();
                int hashtagLength = randomHashTag.Length;

                // Đảm bảo minStart không vượt qua minContentLength - hashtagLength
                if (minStart > minContentLength - hashtagLength)
                {
                    minContentLength = minStart + hashtagLength + 1;

                    // Đảm bảo chuỗi hiện tại đủ dài
                    while (text[^1].Length < minContentLength)
                        text[^1] += faker.Lorem.Sentence(10);
                }

                int start = faker.Random.Int(minStart, minContentLength - hashtagLength);
                int end = start + hashtagLength;

                postContent.HashtagFacets.Add(new HashtagFacet
                {
                    Type = FacetType.Hashtag,
                    Start = start,
                    End = end,
                    Tag = randomHashTag
                });

                text[^1] = text[^1].Insert(start, $" #{randomHashTag}");

                minContentLength += 20;
                minStart = end;

                // Thêm một câu mới để đảm bảo nội dung tiếp theo có chỗ chèn
                text.Add(faker.Lorem.Sentence(20));
            }

            else if (facetType == FacetType.Tag)
            {
                var randomProfileIdWithName = faker.PickRandom(profileIdsWithNames);
                while (usedProfileIdsWithNames.Contains(randomProfileIdWithName))
                    randomProfileIdWithName = faker.PickRandom(profileIdsWithNames);

                int tagLength = randomProfileIdWithName.Name.Length;

                if (minStart > minContentLength - tagLength)
                {
                    minContentLength = minStart + tagLength + 1;

                    while (text[^1].Length < minContentLength)
                        text[^1] += faker.Lorem.Sentence(10);
                }

                int start = faker.Random.Int(minStart, minContentLength - tagLength);
                int end = start + tagLength;

                postContent.TagFacets.Add(new TagFacet
                {
                    Type = FacetType.Tag,
                    ProfileId = Guid.Parse(randomProfileIdWithName.Id),
                    Start = start,
                    End = end
                });

                text[^1] = text[^1].Insert(start, $" @{randomProfileIdWithName.Name}");
                usedProfileIdsWithNames.Add(randomProfileIdWithName);

                minContentLength += 50;
                minStart = end;
                text.Add(faker.Lorem.Sentence(50));
            }

        }

        postContent.Text = string.Join(" ", text);

        return postContent;
    }

}
