using Bogus;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.Services.Reaction.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfinityNetServer.Services.Reaction.Domain.Entities;

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

        // Duyệt qua tất cả accountId và postId để tạo reaction nếu chưa có
        foreach (var accountId in accountIds)
        {
            foreach (var postId in postIds)
            {
                // Kiểm tra xem cặp này đã được sử dụng chưa
                if (!usedPairs.Contains((accountId, postId)))
                {
                    // Nếu chưa, thêm vào HashSet và tạo mới PostReaction
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

        var usedPairs = new HashSet<(string accountId, string postId)>();
        var reactions = new List<CommentReaction>();
        var faker = new Faker();

        // Duyệt qua tất cả accountId và postId để tạo reaction nếu chưa có
        foreach (var accountId in accountIds)
        {
            foreach (var commentId in commentIds)
            {
                // Kiểm tra xem cặp này đã được sử dụng chưa
                if (!usedPairs.Contains((accountId, commentId)))
                {
                    // Nếu chưa, thêm vào HashSet và tạo mới PostReaction
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
