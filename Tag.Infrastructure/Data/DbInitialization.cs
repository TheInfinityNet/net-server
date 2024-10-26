using Bogus;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfinityNetServer.Services.Tag.Domain.Repositories;
using InfinityNetServer.Services.Tag.Domain.Entities;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace InfinityNetServer.Services.Tag.Infrastructure.Data;

public static class DbInitialization
{

    public static void DropDatabase(this IServiceProvider serviceProvider)
    {
        using var serviceScope = serviceProvider.CreateScope();

        var dbContext = serviceScope.ServiceProvider.GetService<TagDbContext>();

        dbContext.Database.EnsureDeleted();
    }

    public static void AutoMigration(this WebApplication webApplication)
    {
        using var serviceScope = webApplication.Services.CreateScope();

        var dbContext = serviceScope.ServiceProvider.GetRequiredService<TagDbContext>();

        if (!dbContext.Database.EnsureCreated()) return;

        dbContext.Database.MigrateAsync().Wait(); //generate all in folder Migration

    }

    public static async void SeedEssentialData(this IServiceProvider serviceProvider)
    {
        using var serviceScope = serviceProvider.CreateScope();
        var dbContext = serviceScope.ServiceProvider.GetService<TagDbContext>();
        var postTagRepository = serviceScope.ServiceProvider.GetService<IPostTagRepository>();
        var commentTagRepository = serviceScope.ServiceProvider.GetService<ICommentTagRepository>();
        var profileClient = serviceScope.ServiceProvider.GetService<CommonProfileClient>();
        var postClient = serviceScope.ServiceProvider.GetService<CommonPostClient>();
        var commentClient = serviceScope.ServiceProvider.GetService<CommonCommentClient>();

        var existingTagCount = await postTagRepository.GetAllAsync();
        if (existingTagCount.Count == 0)
        {
            var postTags = await GeneratePostTags(profileClient, postClient);
            await postTagRepository.CreateAsync(postTags);

            var commentTags = await GenerateCommentTags(profileClient, commentClient);
            await commentTagRepository.CreateAsync(commentTags);
        }
    }

    private static async Task<List<PostTag>> GeneratePostTags(
        CommonProfileClient profileClient,
        CommonPostClient postClient)
    {
        var profileIds = await profileClient.GetProfileIds();
        var userProfileIds = await profileClient.GetUserProfileIds();
        var postIds = await postClient.GetPostIds();

        var usedPairs = new HashSet<(string profileId, string postId)>();
        var tags = new List<PostTag>();
        var faker = new Faker();

        // Ngẫu nhiên chọn số lượng userProfileIds và postIds
        var randomUserProfileCount = faker.Random.Int(1, userProfileIds.Count);
        var randomPostCount = faker.Random.Int(1, postIds.Count);

        // Chọn ngẫu nhiên một số userProfileIds và postIds để tạo tags
        var selectedUserProfileIds = faker.PickRandom(userProfileIds, randomUserProfileCount).ToList();
        var selectedPostIds = faker.PickRandom(postIds, randomPostCount).ToList();

        // Duyệt qua các profileIds và postIds đã chọn ngẫu nhiên
        foreach (var userProfileId in selectedUserProfileIds)
        {
            // Ngẫu nhiên chọn số lượng post cho mỗi profile
            var randomPostsForProfile = faker.PickRandom(selectedPostIds, faker.Random.Int(1, selectedPostIds.Count()));

            foreach (var postId in randomPostsForProfile)
            {
                // Kiểm tra xem cặp (profileId, postId) đã tồn tại chưa
                if (!usedPairs.Contains((userProfileId, postId)))
                {
                    // Nếu chưa tồn tại, thêm vào HashSet và tạo mới PostTag
                    usedPairs.Add((userProfileId, postId));
                    var randomProfileId = faker.PickRandom(profileIds);

                    var tag = new PostTag
                    {
                        CreatedBy = Guid.Parse(randomProfileId),
                        ProfileId = Guid.Parse(randomProfileId),
                        PostId = Guid.Parse(postId),
                        TaggedProfileId = Guid.Parse(userProfileId)
                    };

                    tags.Add(tag);
                }
            }
        }

        return tags;
    }


    private static async Task<List<CommentTag>> GenerateCommentTags(
        CommonProfileClient profileClient,
        CommonCommentClient commentClient)
    {
        var profileIds = await profileClient.GetProfileIds();
        var userProfileIds = await profileClient.GetUserProfileIds();
        var commentIds = await commentClient.GetCommentIds();

        var usedPairs = new HashSet<(string profileId, string commentId)>();
        var tags = new List<CommentTag>();
        var faker = new Faker();

        // Ngẫu nhiên chọn số lượng userProfileIds và commentIds
        var randomUserProfileCount = faker.Random.Int(1, userProfileIds.Count);
        var randomCommentCount = faker.Random.Int(1, commentIds.Count);

        // Chọn ngẫu nhiên một số userProfileIds và commentIds để tạo tags
        var selectedUserProfileIds = faker.PickRandom(userProfileIds, randomUserProfileCount).ToList();
        var selectedCommentIds = faker.PickRandom(commentIds, randomCommentCount).ToList();

        // Duyệt qua các profileIds và commentIds đã chọn ngẫu nhiên
        foreach (var userProfileId in selectedUserProfileIds)
        {
            // Ngẫu nhiên chọn số lượng comment cho mỗi profile
            var randomCommentsForProfile = faker.PickRandom(selectedCommentIds, faker.Random.Int(1, selectedCommentIds.Count()));

            foreach (var commentId in randomCommentsForProfile)
            {
                // Kiểm tra xem cặp (profileId, commentId) đã tồn tại chưa
                if (!usedPairs.Contains((userProfileId, commentId)))
                {
                    // Nếu chưa tồn tại, thêm vào HashSet và tạo mới CommentTag
                    usedPairs.Add((userProfileId, commentId));
                    var randomProfileId = faker.PickRandom(profileIds);

                    var tag = new CommentTag
                    {
                        CreatedBy = Guid.Parse(randomProfileId),
                        ProfileId = Guid.Parse(randomProfileId),
                        CommentId = Guid.Parse(commentId),
                        TaggedProfileId = Guid.Parse(userProfileId)
                    };

                    tags.Add(tag);
                }
            }
        }

        return tags;
    }


}
