using Bogus;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfinityNetServer.Services.Tag.Domain.Repositories;
using InfinityNetServer.Services.Tag.Domain.Entities;

namespace InfinityNetServer.Services.Tag.Infrastructure.Data;

public static class DbInitialization
{

    public static void DropDatabase(this IServiceProvider serviceProvider)
    {
        using var serviceScope = serviceProvider.CreateScope();

        var dbContext = serviceScope.ServiceProvider.GetService<TagDbContext>();

        dbContext.Database.EnsureDeleted();
    }

    public static async void SeedEssentialData(this IServiceProvider serviceProvider)
    {
        using var serviceScope = serviceProvider.CreateScope();
        var dbContext = serviceScope.ServiceProvider.GetService<TagDbContext>();
        var postTagRepository = serviceScope.ServiceProvider.GetService<IPostTagRepository>();
        var commentTagRepository = serviceScope.ServiceProvider.GetService<ICommentTagRepository>();
        var identityClient = serviceScope.ServiceProvider.GetService<CommonIdentityClient>();
        var profileClient = serviceScope.ServiceProvider.GetService<CommonProfileClient>();
        var postClient = serviceScope.ServiceProvider.GetService<CommonPostClient>();
        var commentClient = serviceScope.ServiceProvider.GetService<CommonCommentClient>();

        var existingTagCount = await postTagRepository.GetAllAsync();
        if (existingTagCount.Count == 0)
        {
            var postTags = await GeneratePostTags(identityClient, profileClient, postClient);
            await postTagRepository.CreateAsync(postTags);

            var commentTags = await GenerateCommentTags(identityClient, profileClient, commentClient);
            await commentTagRepository.CreateAsync(commentTags);
        }
    }

    private static async Task<List<PostTag>> GeneratePostTags(
        CommonIdentityClient identityClient,
        CommonProfileClient profileClient,
        CommonPostClient postClient)
    {
        var accountIds = await identityClient.GetAccountIds();
        var userProfileIds = await profileClient.GetUserProfileIds();
        var postIds = await postClient.GetPostIds();

        var usedPairs = new HashSet<(string profileId, string postId)>();
        var tags = new List<PostTag>();
        var faker = new Faker();

        // Duyệt qua tất cả accountId và postId để tạo reaction nếu chưa có
        foreach (var profileId in userProfileIds)
        {
            foreach (var postId in postIds)
            {
                // Kiểm tra xem cặp này đã được sử dụng chưa
                if (!usedPairs.Contains((profileId, postId)))
                {
                    // Nếu chưa, thêm vào HashSet và tạo mới PostReaction
                    usedPairs.Add((profileId, postId));

                    var tag = new PostTag
                    {
                        CreatedBy = Guid.Parse(faker.PickRandom(accountIds)),
                        PostId = Guid.Parse(postId),
                        TaggedProfileId = Guid.Parse(profileId)

                    };

                    tags.Add(tag);
                }
            }
        }

        return tags;
    }

    private static async Task<List<CommentTag>> GenerateCommentTags(
        CommonIdentityClient identityClient,
        CommonProfileClient profileClient,
        CommonCommentClient commentClient)
    {
        var accountIds = await identityClient.GetAccountIds();
        var userProfileIds = await profileClient.GetUserProfileIds();
        var commentIds = await commentClient.GetCommentIds();

        var usedPairs = new HashSet<(string profileId, string commentId)>();
        var tags = new List<CommentTag>();
        var faker = new Faker();

        // Duyệt qua tất cả accountId và postId để tạo reaction nếu chưa có
        foreach (var profileId in userProfileIds)
        {
            foreach (var commentId in commentIds)
            {
                // Kiểm tra xem cặp này đã được sử dụng chưa
                if (!usedPairs.Contains((profileId, commentId)))
                {
                    // Nếu chưa, thêm vào HashSet và tạo mới PostReaction
                    usedPairs.Add((profileId, commentId));

                    var tag = new CommentTag
                    {
                        CreatedBy = Guid.Parse(faker.PickRandom(accountIds)),
                        CommentId = Guid.Parse(commentId),
                        TaggedProfileId = Guid.Parse(profileId)

                    };

                    tags.Add(tag);
                }
            }
        }

        return tags;
    }

}
