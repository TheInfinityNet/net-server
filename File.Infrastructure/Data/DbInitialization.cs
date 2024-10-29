using Bogus;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfinityNetServer.Services.File.Domain.Repositories;
using InfinityNetServer.Services.File.Domain.Entities;
using System.Linq;

namespace InfinityNetServer.Services.File.Infrastructure.Data;

public static class DbInitialization
{

    public static void DropDatabase(this IServiceProvider serviceProvider)
    {
        using var serviceScope = serviceProvider.CreateScope();

        var dbContext = serviceScope.ServiceProvider.GetService<FileDbContext>();

        //dbContext.Database.EnsureDeleted();
    }

    public static async void SeedEssentialData(this IServiceProvider serviceProvider)
    {
        using var serviceScope = serviceProvider.CreateScope();
        var dbContext = serviceScope.ServiceProvider.GetService<FileDbContext>();
        var fileMetadataRepository = serviceScope.ServiceProvider.GetService<IFileMetadataRepository>();
        var identityClient = serviceScope.ServiceProvider.GetService<CommonIdentityClient>();
        var profileClient = serviceScope.ServiceProvider.GetService<CommonProfileClient>();
        var postClient = serviceScope.ServiceProvider.GetService<CommonPostClient>();
        var commentClient = serviceScope.ServiceProvider.GetService<CommonCommentClient>();

        /*var existingTagCount = await fileMetadataRepository.GetAllAsync();
        if (existingTagCount.Count == 0)
        {
            var postTags = await GeneratePostTags(identityClient, profileClient, postClient);
            await fileMetadataRepository.CreateAsync(postTags);

            var commentTags = await GenerateCommentTags(identityClient, profileClient, commentClient);
            await commentTagRepository.CreateAsync(commentTags);
        }*/
    }

    /*private static async Task<List<PostTag>> GeneratePostTags(
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

        // Ngẫu nhiên chọn số lượng userProfileIds và postIds
        var randomProfileCount = faker.Random.Int(1, userProfileIds.Count);
        var randomPostCount = faker.Random.Int(1, postIds.Count);

        // Chọn ngẫu nhiên một số userProfileIds và postIds để tạo tags
        var selectedProfileIds = faker.PickRandom(userProfileIds, randomProfileCount).ToList();
        var selectedPostIds = faker.PickRandom(postIds, randomPostCount).ToList();

        // Duyệt qua các profileIds và postIds đã chọn ngẫu nhiên
        foreach (var profileId in selectedProfileIds)
        {
            // Ngẫu nhiên chọn số lượng post cho mỗi profile
            var randomPostsForProfile = faker.PickRandom(selectedPostIds, faker.Random.Int(1, selectedPostIds.Count()));

            foreach (var postId in randomPostsForProfile)
            {
                // Kiểm tra xem cặp (profileId, postId) đã tồn tại chưa
                if (!usedPairs.Contains((profileId, postId)))
                {
                    // Nếu chưa tồn tại, thêm vào HashSet và tạo mới PostTag
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

        // Ngẫu nhiên chọn số lượng userProfileIds và commentIds
        var randomProfileCount = faker.Random.Int(1, userProfileIds.Count);
        var randomCommentCount = faker.Random.Int(1, commentIds.Count);

        // Chọn ngẫu nhiên một số userProfileIds và commentIds để tạo tags
        var selectedProfileIds = faker.PickRandom(userProfileIds, randomProfileCount).ToList();
        var selectedCommentIds = faker.PickRandom(commentIds, randomCommentCount).ToList();

        // Duyệt qua các profileIds và commentIds đã chọn ngẫu nhiên
        foreach (var profileId in selectedProfileIds)
        {
            // Ngẫu nhiên chọn số lượng comment cho mỗi profile
            var randomCommentsForProfile = faker.PickRandom(selectedCommentIds, faker.Random.Int(1, selectedCommentIds.Count()));

            foreach (var commentId in randomCommentsForProfile)
            {
                // Kiểm tra xem cặp (profileId, commentId) đã tồn tại chưa
                if (!usedPairs.Contains((profileId, commentId)))
                {
                    // Nếu chưa tồn tại, thêm vào HashSet và tạo mới CommentTag
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
    }*/


}
