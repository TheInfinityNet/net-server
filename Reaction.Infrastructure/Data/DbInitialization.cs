using Bogus;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.Services.Reaction.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfinityNetServer.Services.Reaction.Domain.Entities;
using System.Linq;

namespace InfinityNetServer.Services.Reaction.Infrastructure.Data;

public static class DbInitialization
{

    public static void DropDatabase(this IServiceProvider serviceProvider)
    {
        using var serviceScope = serviceProvider.CreateScope();

        var dbContext = serviceScope.ServiceProvider.GetService<ReactionDbContext>();

        dbContext.Database.EnsureDeleted();
    }

    public static async void SeedEssentialData(this IServiceProvider serviceProvider)
    {
        using var serviceScope = serviceProvider.CreateScope();
        var dbContext = serviceScope.ServiceProvider.GetService<ReactionDbContext>();
        var postReactionRepository = serviceScope.ServiceProvider.GetService<IPostReactionRepository>();
        var commentReactionRepository = serviceScope.ServiceProvider.GetService<ICommentReactionRepository>();
        var identityClient = serviceScope.ServiceProvider.GetService<CommonIdentityClient>();
        var postClient = serviceScope.ServiceProvider.GetService<CommonPostClient>();
        var commentClient = serviceScope.ServiceProvider.GetService<CommonCommentClient>();

        var existingReactionCount = await postReactionRepository.GetAllAsync();
        if (existingReactionCount.Count == 0)
        {
            var postReactions = await GeneratePostReactions(identityClient, postClient);
            await postReactionRepository.CreateAsync(postReactions);

            var commentReactions = await GenerateCommentReactions(identityClient, commentClient);
            await commentReactionRepository.CreateAsync(commentReactions);
        }
    }

    private static async Task<List<PostReaction>> GeneratePostReactions(
        CommonIdentityClient identityClient,
        CommonPostClient postClient)
    {
        var accountIds = await identityClient.GetAccountIds();
        var postIds = await postClient.GetPostIds();

        var usedPairs = new HashSet<(string accountId, string postId)>();
        var reactions = new List<PostReaction>();
        var faker = new Faker();

        // Ngẫu nhiên chọn số lượng accountIds và postIds
        var randomAccountCount = faker.Random.Int(1, accountIds.Count);
        var randomPostCount = faker.Random.Int(1, postIds.Count);

        // Chọn ngẫu nhiên một số accountIds và postIds để thả reaction
        var selectedAccountIds = faker.PickRandom(accountIds, randomAccountCount).ToList();
        var selectedPostIds = faker.PickRandom(postIds, randomPostCount).ToList();

        // Duyệt qua các accountIds và postIds đã được chọn ngẫu nhiên
        foreach (var accountId in selectedAccountIds)
        {
            // Ngẫu nhiên chọn số lượng post cho mỗi account
            var randomPostsForAccount = faker.PickRandom(selectedPostIds, faker.Random.Int(1, selectedPostIds.Count()));

            foreach (var postId in randomPostsForAccount)
            {
                // Kiểm tra xem cặp (accountId, postId) đã tồn tại chưa
                if (!usedPairs.Contains((accountId, postId)))
                {
                    // Nếu chưa tồn tại, thêm vào HashSet và tạo mới PostReaction
                    usedPairs.Add((accountId, postId));

                    var reaction = new PostReaction
                    {
                        CreatedBy = Guid.Parse(accountId),
                        PostId = Guid.Parse(postId)
                    };

                    reactions.Add(reaction);
                }
            }
        }

        return reactions;
    }

    private static async Task<List<CommentReaction>> GenerateCommentReactions(
        CommonIdentityClient identityClient,
        CommonCommentClient commentClient)
    {
        var accountIds = await identityClient.GetAccountIds();
        var commentIds = await commentClient.GetCommentIds();

        var usedPairs = new HashSet<(string accountId, string commentId)>();
        var reactions = new List<CommentReaction>();
        var faker = new Faker();

        // Ngẫu nhiên chọn số lượng accountIds và commentIds
        var randomAccountCount = faker.Random.Int(1, accountIds.Count);
        var randomCommentCount = faker.Random.Int(1, commentIds.Count);

        // Chọn ngẫu nhiên một số accountIds và commentIds để thả reaction
        var selectedAccountIds = faker.PickRandom(accountIds, randomAccountCount).ToList();
        var selectedCommentIds = faker.PickRandom(commentIds, randomCommentCount).ToList();

        // Duyệt qua các accountIds và commentIds đã được chọn ngẫu nhiên
        foreach (var accountId in selectedAccountIds)
        {
            // Ngẫu nhiên chọn số lượng comment cho mỗi account
            var randomCommentsForAccount = faker.PickRandom(selectedCommentIds, faker.Random.Int(1, selectedCommentIds.Count()));

            foreach (var commentId in randomCommentsForAccount)
            {
                // Kiểm tra xem cặp (accountId, commentId) đã tồn tại chưa
                if (!usedPairs.Contains((accountId, commentId)))
                {
                    // Nếu chưa tồn tại, thêm vào HashSet và tạo mới CommentReaction
                    usedPairs.Add((accountId, commentId));

                    var reaction = new CommentReaction
                    {
                        CreatedBy = Guid.Parse(accountId),
                        CommentId = Guid.Parse(commentId)
                    };

                    reactions.Add(reaction);
                }
            }
        }

        return reactions;
    }


}
