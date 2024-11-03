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
            IList<string> profileIds = await profileClient.GetProfileIds();
            IList<ProfileIdWithName> profileIdsWithNames = await profileClient.GetProfileIdsWithNames();
            //IList<GroupMemberWithGroup> groupMemberWithGroups = await groupClient.GetGroupMemberWithGroup();
            
            var posts = GeneratePresentationPosts(numberOfPosts / 2, profileIdsWithNames);
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

    private static List<Domain.Entities.Post> GeneratePresentationPosts(
    int count, IList<ProfileIdWithName> profileIdsWithNames)
    {
        var faker = new Faker<Domain.Entities.Post>()
            .CustomInstantiator(f =>
            {
                var randomProfileId = Guid.Parse(f.PickRandom(profileIdsWithNames).Id);
                return new Domain.Entities.Post
                {
                    Type = PostType.Text,
                    OwnerId = randomProfileId,
                    CreatedBy = randomProfileId,
                    CreatedAt = f.Date.Recent(f.Random.Int(1, 365))
                };
            })
            .RuleFor(ap => ap.Content, f => GeneratePostContent(profileIdsWithNames)); // Chuyển GeneratePostContent vào lambda

        return faker.Generate(count);
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
                        Text = f.Lorem.Sentence(100)
                    },
                    GroupId = randomPresentationPost.GroupId,
                    Presentation = randomPresentationPost,
                    OwnerId = randomPresentationPost.OwnerId,
                    FileMetadataId = mediaId,
                    CreatedBy = randomPresentationPost.CreatedBy,
                    CreatedAt = f.Date.Recent(f.Random.Int(1, 365))
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
                        Text = f.Lorem.Sentence(100)
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
        IList<PostPrivacy> postPrivacies = [];

        foreach (var post in presentationPosts)
        {
            Faker faker = new();

            int privacyCount = faker.Random.Int(1, 3);
            for (int i = 0; i < privacyCount; i++)
            {
                PostPrivacyType type = faker.PickRandom<PostPrivacyType>();
                PostPrivacy postPrivacy = new()
                {
                    PostId = post.Id,
                    Type = type,
                    CreatedBy = post.CreatedBy,
                    CreatedAt = faker.Date.Recent(faker.Random.Int(1, 365))
                };

                if (type == PostPrivacyType.Exclude || type == PostPrivacyType.Include)
                {
                    IList<string> followerIds = await relationshipClient.GetFollowers(post.OwnerId.ToString());

                    if (followerIds.Any()) // Kiểm tra nếu danh sách followerIds không rỗng
                    {
                        int followerCount = faker.Random.Int(1, Math.Min(followerIds.Count, 5));
                        for (int j = 0; j < followerCount; j++)
                        {
                            string selectedFollowerId = faker.PickRandom(followerIds);
                            Guid profileId = Guid.Parse(selectedFollowerId);

                            switch (type)
                            {
                                case PostPrivacyType.Exclude:
                                    postPrivacy.PostPrivacyExcludes.Add(new PostPrivacyExclude
                                    {
                                        PostPrivacyId = postPrivacy.Id,
                                        ProfileId = profileId,
                                        CreatedBy = post.CreatedBy,
                                        CreatedAt = faker.Date.Recent(faker.Random.Int(1, 365))
                                    });
                                    break;

                                case PostPrivacyType.Include:
                                    postPrivacy.PostPrivacyIncludes.Add(new PostPrivacyInclude
                                    {
                                        PostPrivacyId = postPrivacy.Id,
                                        ProfileId = profileId,
                                        CreatedBy = post.CreatedBy,
                                        CreatedAt = faker.Date.Recent(faker.Random.Int(1, 365))
                                    });
                                    break;
                            }

                            followerIds.Remove(selectedFollowerId); // Loại bỏ follower đã chọn để tránh trùng lặp
                        }
                    }
                }

                postPrivacies.Add(postPrivacy);
            }
        }

        return postPrivacies;
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

            if (facetType == FacetType.Tag)
            {
                var randomProfileIdWithName = faker.PickRandom(profileIdsWithNames);
                while (usedProfileIdsWithNames.Contains(randomProfileIdWithName))
                    randomProfileIdWithName = faker.PickRandom(profileIdsWithNames);

                int tagLength = randomProfileIdWithName.Name.Length;

                // Đảm bảo minStart không vượt qua minContentLength - tagLength
                if (minStart > minContentLength - tagLength)
                {
                    minContentLength = minStart + tagLength + 1;
                    text[^1] = text[^1] + faker.Lorem.Sentence(tagLength + 1);
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
            else if (facetType == FacetType.Hashtag)
            {
                var randomHashTag = faker.Lorem.Word();
                int hashtagLength = randomHashTag.Length;

                // Đảm bảo minStart không vượt qua minContentLength - hashtagLength
                if (minStart > minContentLength - hashtagLength)
                {
                    minContentLength = minStart + hashtagLength + 1;
                    text[^1] = text[^1] + faker.Lorem.Sentence(hashtagLength + 1);
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
                text.Add(faker.Lorem.Sentence(20));
            }
        }

        postContent.Text = string.Join(" ", text);

        return postContent;
    }

}
