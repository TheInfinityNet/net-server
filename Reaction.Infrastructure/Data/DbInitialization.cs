using Bogus;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.Services.Reaction.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfinityNetServer.Services.Reaction.Domain.Entities;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace InfinityNetServer.Services.Reaction.Infrastructure.Data;

public static class DbInitialization
{

    public static void DropDatabase(this IServiceProvider serviceProvider)
    {
        using var serviceScope = serviceProvider.CreateScope();

        var dbContext = serviceScope.ServiceProvider.GetService<ReactionDbContext>();

        dbContext.Database.EnsureDeleted();
    }

    public static void AutoMigration(this WebApplication webApplication)
    {
        using var serviceScope = webApplication.Services.CreateScope();

        var dbContext = serviceScope.ServiceProvider.GetRequiredService<ReactionDbContext>();

        if (!dbContext.Database.EnsureCreated()) return;

        dbContext.Database.MigrateAsync().Wait(); //generate all in folder Migration

    }

    public static async void SeedEssentialData(this IServiceProvider serviceProvider)
    {
        using var serviceScope = serviceProvider.CreateScope();
        serviceScope.ServiceProvider.GetService<ReactionDbContext>();
        var postReactionRepository = serviceScope.ServiceProvider.GetService<IPostReactionRepository>();
        var commentReactionRepository = serviceScope.ServiceProvider.GetService<ICommentReactionRepository>();
        var profileClient = serviceScope.ServiceProvider.GetService<CommonProfileClient>();
        var postClient = serviceScope.ServiceProvider.GetService<CommonPostClient>();
        var commentClient = serviceScope.ServiceProvider.GetService<CommonCommentClient>();

        var existingReactionCount = await postReactionRepository.GetAllAsync();
        if (existingReactionCount.Count == 0)
        {
            var postReactions = await GeneratePostReactions(profileClient, postClient);
            await postReactionRepository.CreateAsync(postReactions);

            var commentReactions = await GenerateCommentReactions(profileClient, commentClient);
            await commentReactionRepository.CreateAsync(commentReactions);
        }
    }

    private static async Task<List<PostReaction>> GeneratePostReactions(
        CommonProfileClient profileClient,
        CommonPostClient postClient)
    {
        var profileIds = await profileClient.GetProfileIds();
        var postIds = await postClient.GetPostIds();

        var usedPairs = new HashSet<(string accountId, string postId)>();
        var reactions = new List<PostReaction>();
        var faker = new Faker();

        // Ngẫu nhiên chọn số lượng accountIds và postIds
        var randomAccountCount = faker.Random.Int(1, profileIds.Count);
        var randomPostCount = faker.Random.Int(1, postIds.Count);

        // Chọn ngẫu nhiên một số accountIds và postIds để thả reaction
        var selectedProfileIds = faker.PickRandom(profileIds, randomAccountCount).ToList();
        var selectedPostIds = faker.PickRandom(postIds, randomPostCount).ToList();

        // Duyệt qua các accountIds và postIds đã được chọn ngẫu nhiên
        foreach (var profileId in selectedProfileIds)
        {
            // Ngẫu nhiên chọn số lượng post cho mỗi account
            var randomPostsForProfile = faker.PickRandom(selectedPostIds, faker.Random.Int(1, selectedPostIds.Count()));

            foreach (var postId in randomPostsForProfile)
            {
                // Kiểm tra xem cặp (accountId, postId) đã tồn tại chưa
                if (!usedPairs.Contains((profileId, postId)))
                {
                    // Nếu chưa tồn tại, thêm vào HashSet và tạo mới PostReaction
                    usedPairs.Add((profileId, postId));

                    var reaction = new PostReaction
                    {
                        CreatedBy = Guid.Parse(profileId),
                        ProfileId = Guid.Parse(profileId),
                        PostId = Guid.Parse(postId)
                    };

                    reactions.Add(reaction);
                }
            }
        }

        return reactions;
    }

    private static async Task<List<CommentReaction>> GenerateCommentReactions(
        CommonProfileClient profileClient,
        CommonCommentClient commentClient)
    {
        var profileIds = await profileClient.GetProfileIds();
        var commentIds = await commentClient.GetCommentIds();

        var usedPairs = new HashSet<(string accountId, string commentId)>();
        var reactions = new List<CommentReaction>();
        var faker = new Faker();

        // Ngẫu nhiên chọn số lượng accountIds và commentIds
        var randomAccountCount = faker.Random.Int(1, profileIds.Count);
        var randomCommentCount = faker.Random.Int(1, commentIds.Count);

        // Chọn ngẫu nhiên một số accountIds và commentIds để thả reaction
        var selectedProfileIds = faker.PickRandom(profileIds, randomAccountCount).ToList();
        var selectedCommentIds = faker.PickRandom(commentIds, randomCommentCount).ToList();

        // Duyệt qua các accountIds và commentIds đã được chọn ngẫu nhiên
        foreach (var profileId in selectedProfileIds)
        {
            // Ngẫu nhiên chọn số lượng comment cho mỗi account
            var randomCommentsForProfile = 
                faker.PickRandom(selectedCommentIds, faker.Random.Int(1, selectedCommentIds.Count()));

            foreach (var commentId in randomCommentsForProfile)
            {
                // Kiểm tra xem cặp (accountId, commentId) đã tồn tại chưa
                if (!usedPairs.Contains((profileId, commentId)))
                {
                    // Nếu chưa tồn tại, thêm vào HashSet và tạo mới CommentReaction
                    usedPairs.Add((profileId, commentId));

                    var reaction = new CommentReaction
                    {
                        CreatedBy = Guid.Parse(profileId),
                        ProfileId = Guid.Parse(profileId),
                        CommentId = Guid.Parse(commentId)
                    };

                    reactions.Add(reaction);
                }
            }
        }

        return reactions;
    }


}
