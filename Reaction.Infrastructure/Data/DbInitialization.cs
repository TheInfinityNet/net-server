using Bogus;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.Services.Reaction.Domain.Entities;
using InfinityNetServer.Services.Reaction.Domain.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        var relationshipClient = serviceScope.ServiceProvider.GetService<CommonRelationshipClient>();
        var postClient = serviceScope.ServiceProvider.GetService<CommonPostClient>();
        var commentClient = serviceScope.ServiceProvider.GetService<CommonCommentClient>();

        var existingReactionCount = await postReactionRepository.GetAllAsync();
        if (existingReactionCount.Count == 0)
        {
            IList<string> profileIds = await profileClient.GetProfileIds();
            IList<string> postIds = await postClient.GetPostIds();

            var (postReactions, commentReactions) = await GenerateReactions(commentClient, postClient, profileIds, postIds);

            await postReactionRepository.CreateAsync(postReactions);
            await commentReactionRepository.CreateAsync(commentReactions);

            //IList<string> commentIds = await commentClient.GetCommentIds();

            //var postReactions = GeneratePostReactions(profileIds, postIds);
            //await postReactionRepository.CreateAsync(postReactions);

            //var commentReactions = GenerateCommentReactions(profileIds, commentIds);
            //await commentReactionRepository.CreateAsync(commentReactions);
        }
    }

    private static async Task<(IList<PostReaction>, IList<CommentReaction>)> GenerateReactions(
         CommonCommentClient commentClient,
         CommonPostClient postClient,
         IList<string> profileIds,
         IList<string> postIds)
    {
        var whoCantSeeDict = await GetWhoCantSeeForAllPosts(postIds, postClient);
        var postReactions = new List<PostReaction>();
        var commentReactions = new List<CommentReaction>();
        var postReactionPairs = new HashSet<(Guid profileId, Guid postId)>();
        var commentReactionPairs = new HashSet<(Guid profileId, Guid commentId)>();

        var postReactionsFaker = new Faker<PostReaction>()
            .CustomInstantiator(f => new PostReaction
            {
                Type = f.PickRandom<ReactionType>(),
                CreatedAt = f.Date.Recent(f.Random.Int(1, 365))
            });

        var allCommentIdsByPostId = await GetAllCommentsForPosts(postIds, commentClient);
        Faker faker = new();

        // Chọn ngẫu nhiên 70% bài post để thêm reactions
        var selectedPostIds = faker.PickRandom(postIds, (int)(postIds.Count * 0.7)).ToList();

        foreach (var postId in selectedPostIds)
        {
            if (!whoCantSeeDict.TryGetValue(postId, out var whoCantSeeIds))
                whoCantSeeIds = [];

            var validProfileIds = profileIds.Select(Guid.Parse).Except(whoCantSeeIds).ToList();
            if (validProfileIds.Count == 0) continue;

            // Random số lượng reactions cho mỗi post (20 đến 30)
            int numPostReactions = faker.Random.Int(20, 30);

            for (int i = 0; i < numPostReactions; i++)
            {
                var postReaction = postReactionsFaker.Generate();
                postReaction.PostId = Guid.Parse(postId);

                var randomProfileId = faker.PickRandom(validProfileIds);

                if (postReactionPairs.Add((randomProfileId, postReaction.PostId)))
                {
                    postReaction.ProfileId = randomProfileId;
                    postReaction.CreatedBy = randomProfileId;
                    postReactions.Add(postReaction);
                }
            }

            // Xử lý reactions cho comments trong bài post
            if (allCommentIdsByPostId.TryGetValue(Guid.Parse(postId), out var commentIds))
            {
                // Chọn ngẫu nhiên 50% comments để thêm reactions
                var selectedCommentIds = faker.PickRandom(commentIds, (int)(commentIds.Count * 0.5)).ToList();

                foreach (var commentId in selectedCommentIds)
                {
                    // Random số lượng reactions cho mỗi comment (5 đến 10)
                    int numCommentReactions = faker.Random.Int(5, 10);

                    for (int i = 0; i < numCommentReactions; i++)
                    {
                        var commentReaction = new Faker<CommentReaction>()
                            .CustomInstantiator(f => new CommentReaction
                            {
                                CommentId = Guid.Parse(commentId),
                                Type = f.PickRandom<ReactionType>(),
                                CreatedAt = f.Date.Recent(f.Random.Int(1, 365))
                            }).Generate();

                        var randomProfileId = faker.PickRandom(validProfileIds);

                        if (commentReactionPairs.Add((randomProfileId, commentReaction.CommentId)))
                        {
                            commentReaction.ProfileId = randomProfileId;
                            commentReaction.CreatedBy = randomProfileId;
                            commentReactions.Add(commentReaction);
                        }
                    }
                }
            }
        }

        return (postReactions, commentReactions);
    }


    private static async Task<Dictionary<Guid, List<string>>> GetAllCommentsForPosts(
        IList<string> postIds,
        CommonCommentClient commentClient)
    {
        // Thực hiện gọi song song đến gRPC để lấy commentIds cho từng postId
        var tasks = postIds.Select(async postId =>
        {
            try
            {
                var commentIds = await commentClient.GetCommentIdsByPostId(postId);
                return new KeyValuePair<Guid, List<string>>(Guid.Parse(postId), commentIds.ToList());
            }
            catch (Exception ex)
            {
                // Log lỗi nếu có
                Console.WriteLine($"Error fetching comments for PostId {postId}: {ex.Message}");
                return new KeyValuePair<Guid, List<string>>(Guid.Parse(postId), []);
            }
        });

        // Đợi tất cả các task hoàn thành
        var results = await Task.WhenAll(tasks);

        // Trả về kết quả dưới dạng dictionary
        return results.ToDictionary(x => x.Key, x => x.Value);
    }


    public static async Task<Dictionary<string, IList<Guid>>> GetWhoCantSeeForAllPosts(
        IEnumerable<string> postIds,
        CommonPostClient postClient)
    {
        var whoCantSeeDict = new Dictionary<string, IList<Guid>>();

        // Gọi API một lần cho mỗi bài đăng để lấy thông tin ai không thể xem bài đăng đó
        var tasks = postIds.Select(async postId =>
        {
            var whoCantSeeForPost = await postClient.WhoCantSee(postId);

            // Lưu kết quả vào dictionary với key là postId
            whoCantSeeDict[postId] = whoCantSeeForPost.Select(Guid.Parse).ToList();
        });

        // Chờ tất cả các task hoàn thành
        await Task.WhenAll(tasks);

        return whoCantSeeDict;
    }

    private static List<PostReaction> GeneratePostReactions(
        IList<string> profileIds,
        IList<string> postIds)
    {
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
            var randomPostsForProfile = faker.PickRandom(selectedPostIds, faker.Random.Int(1, selectedPostIds.Count));

            foreach (var postId in randomPostsForProfile)
            {
                // Kiểm tra xem cặp (accountId, postId) đã tồn tại chưa
                if (!usedPairs.Contains((profileId, postId)))
                {
                    // Nếu chưa tồn tại, thêm vào HashSet và tạo mới PostReaction
                    usedPairs.Add((profileId, postId));

                    var reaction = new PostReaction
                    {
                        Type = faker.PickRandom<ReactionType>(),
                        ProfileId = Guid.Parse(profileId),
                        PostId = Guid.Parse(postId),
                        CreatedBy = Guid.Parse(profileId),
                        CreatedAt = faker.Date.Recent(faker.Random.Int(1, 365))
                    };

                    reactions.Add(reaction);
                }
            }
        }

        return reactions;
    }

    private static List<CommentReaction> GenerateCommentReactions(
        IList<string> profileIds,
        IList<string> commentIds)
    {
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
                faker.PickRandom(selectedCommentIds, faker.Random.Int(1, selectedCommentIds.Count));

            foreach (var commentId in randomCommentsForProfile)
            {
                // Kiểm tra xem cặp (accountId, commentId) đã tồn tại chưa
                if (!usedPairs.Contains((profileId, commentId)))
                {
                    // Nếu chưa tồn tại, thêm vào HashSet và tạo mới CommentReaction
                    usedPairs.Add((profileId, commentId));

                    var reaction = new CommentReaction
                    {
                        Type = faker.PickRandom<ReactionType>(),
                        ProfileId = Guid.Parse(profileId),
                        CommentId = Guid.Parse(commentId),
                        CreatedBy = Guid.Parse(profileId),
                        CreatedAt = faker.Date.Recent(faker.Random.Int(1, 365))
                    };

                    reactions.Add(reaction);
                }
            }
        }

        return reactions;
    }


}
