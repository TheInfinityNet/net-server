using Bogus;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Others;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.Services.Comment.Domain.Enums;
using InfinityNetServer.Services.Comment.Domain.Repositories;
using MassTransit.Util;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagFacet = InfinityNetServer.BuildingBlocks.Domain.Entities.TagFacet;

namespace InfinityNetServer.Services.Comment.Infrastructure.Data;

public static class DbInitialization
{

    public static void DropDatabase(this IServiceProvider serviceProvider)
    {
        using var serviceScope = serviceProvider.CreateScope();

        var dbContext = serviceScope.ServiceProvider.GetService<CommentDbContext>();

        dbContext.Database.EnsureDeleted();
    }

    public static void AutoMigration(this WebApplication webApplication)
    {
        using var serviceScope = webApplication.Services.CreateScope();

        var dbContext = serviceScope.ServiceProvider.GetRequiredService<CommentDbContext>();

        if (!dbContext.Database.EnsureCreated()) return;

        dbContext.Database.MigrateAsync().Wait(); //generate all in folder Migration

    }

    public static async void SeedEssentialData(this IServiceProvider serviceProvider)
    {
        using var serviceScope = serviceProvider.CreateScope();
        serviceScope.ServiceProvider.GetService<CommentDbContext>();
        var commentRepository = serviceScope.ServiceProvider.GetService<ICommentRepository>();
        var profileClient = serviceScope.ServiceProvider.GetService<CommonProfileClient>();
        var postClient = serviceScope.ServiceProvider.GetService<CommonPostClient>();
        var relationshipClient = serviceScope.ServiceProvider.GetService<CommonRelationshipClient>();

        var existingCommentCount = await commentRepository.GetAllAsync();
        if (existingCommentCount.Count == 0)
        {
            IList<string> postIds = await postClient.GetPostIds();
            var comments = await GenerateComments(postIds, postClient, profileClient, relationshipClient);
            await commentRepository.CreateAsync(comments);

            var parentComments = await commentRepository.GetAllAsync();
            var repliedComments = await GenerateRepliedComments(parentComments, postClient, profileClient, relationshipClient);
            await commentRepository.CreateAsync(repliedComments);
        }
    }

    private static async Task<IList<Domain.Entities.Comment>> GenerateComments(
        IList<string> postIds,
        CommonPostClient postClient,
        CommonProfileClient profileClient,
        CommonRelationshipClient relationshipClient)
    {
        // Lấy danh sách profileIds
        var profileIds = await profileClient.GetProfileIds();

        // Gọi API một lần để lấy tất cả các người không thể xem các bài đăng
        var whoCantSeeDict = await GetWhoCantSeeForAllPosts(postIds, postClient);

        // Danh sách comment sẽ được trả về
        var comments = new List<Domain.Entities.Comment>();

        // Faker cho comment
        var commentsFaker = new Faker<Domain.Entities.Comment>()
            .CustomInstantiator(f => new Domain.Entities.Comment
            {
                FileMetadataId = f.Random.Bool() ? Guid.NewGuid() : null,
                CreatedAt = f.Date.Recent(f.Random.Int(1, 365))
            });

        // Faker để chọn ngẫu nhiên 70% bài post
        var faker = new Faker();
        var selectedPostIds = faker.PickRandom(postIds, (int)(postIds.Count * 0.7)).ToList();

        // Sinh comment cho từng bài post được chọn
        foreach (var postId in selectedPostIds)
        {
            // Random số lượng comment cho bài post này
            int numComments = faker.Random.Int(10, 20);

            // Lấy danh sách người không thể xem bài đăng
            if (!whoCantSeeDict.TryGetValue(postId, out var whoCantSeeIds))
                whoCantSeeIds = []; // Nếu không có thông tin, mặc định là không có ai bị cấm

            // Loại bỏ các profile không thể xem bài đăng này
            var validProfileIds = profileIds.Select(Guid.Parse).Except(whoCantSeeIds).ToList();

            // Bỏ qua nếu không có profile hợp lệ
            if (validProfileIds.Count == 0) continue;

            // Tạo comment
            foreach (var _ in Enumerable.Range(0, numComments))
            {
                var comment = commentsFaker.Generate();

                // Gán thông tin cho comment
                comment.PostId = Guid.Parse(postId);
                comment.ProfileId = faker.PickRandom(validProfileIds);
                comment.CreatedBy = comment.ProfileId;

                comment.Type = comment.FileMetadataId == null ? CommentType.Text
                    : faker.PickRandom(new CommentType[] { CommentType.Photo, CommentType.Video });

                // Lấy danh sách bạn bè và người theo dõi (nếu cần tạo content phụ thuộc)
                var friendIds = await relationshipClient.GetFriendIds(comment.ProfileId.ToString());
                var followerIds = await relationshipClient.GetFollowerIds(comment.ProfileId.ToString());
                var combinedIds = friendIds.Concat(followerIds).Distinct().ToList();

                // Lấy thông tin profileId và tên
                var profileIdsWithNames = await profileClient.GetProfileIdsWithNames(combinedIds);

                // Tạo nội dung comment
                comment.Content = GenerateCommentContent(profileIdsWithNames);

                comments.Add(comment);
            }
        }

        return comments;
    }

    private static async Task<IList<Domain.Entities.Comment>> GenerateRepliedComments(
        IList<Domain.Entities.Comment> parentComments,
        CommonPostClient postClient,
        CommonProfileClient profileClient,
        CommonRelationshipClient relationshipClient)
    {
        // Lấy danh sách postIds và profileIds
        var postIds = parentComments.Select(c => c.PostId.ToString()).Distinct().ToList();
        var profileIds = await profileClient.GetProfileIds();

        // Lấy thông tin ai không thể xem bài đăng
        var whoCantSeeDict = await GetWhoCantSeeForAllPosts(postIds, postClient);

        var comments = new List<Domain.Entities.Comment>();

        // Faker để tạo bình luận trả lời
        var commentsFaker = new Faker<Domain.Entities.Comment>()
            .CustomInstantiator(f => new Domain.Entities.Comment
            {
                FileMetadataId = f.Random.Bool() ? Guid.NewGuid() : null,
                CreatedAt = f.Date.Recent(f.Random.Int(1, 365))
            });

        var faker = new Faker();

        // Chọn ngẫu nhiên 60% comment để tạo reply comments
        var selectedParentComments = faker.PickRandom(parentComments, (int)(parentComments.Count * 0.6)).ToList();

        // Sinh reply comments cho từng comment cha
        foreach (var parentComment in selectedParentComments)
        {
            // Random số lượng reply comments cho mỗi comment cha
            int numReplies = faker.Random.Int(2, 5);

            // Lấy danh sách những người không thể xem bài đăng
            if (!whoCantSeeDict.TryGetValue(parentComment.PostId.ToString(), out var whoCantSeeIds))
                whoCantSeeIds = [];

            // Loại bỏ các profile không thể xem bài đăng này
            var validProfileIds = profileIds.Select(Guid.Parse).Except(whoCantSeeIds).ToList();
            if (validProfileIds.Count == 0) continue;

            for (int i = 0; i < numReplies; i++)
            {
                var comment = commentsFaker.Generate();

                // Gán thông tin cho comment
                comment.PostId = parentComment.PostId;
                comment.ParentId = parentComment.Id;
                comment.ProfileId = faker.PickRandom(validProfileIds);
                comment.CreatedBy = comment.ProfileId;

                comment.Type = comment.FileMetadataId == null ? CommentType.Text 
                    : faker.PickRandom(new CommentType[] { CommentType.Photo, CommentType.Video });

                // Lấy danh sách bạn bè và người theo dõi
                var friendIds = await relationshipClient.GetFriendIds(comment.ProfileId.ToString());
                var followerIds = await relationshipClient.GetFollowerIds(comment.ProfileId.ToString());
                var combinedIds = friendIds.Concat(followerIds).Distinct().ToList();

                // Lấy danh sách profile với tên từ combinedIds
                var profileIdsWithNames = await profileClient.GetProfileIdsWithNames(combinedIds);

                // Tạo nội dung comment
                comment.Content = GenerateCommentContent(profileIdsWithNames);

                comments.Add(comment);
            }
        }

        return comments;
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

    private static Domain.Entities.CommentContent GenerateCommentContent(IList<ProfileIdWithName> profileIdsWithNames)
    {
        var faker = new Faker();
        var commentContent = new Domain.Entities.CommentContent();
        var textBuilder = new StringBuilder(faker.Lorem.Sentence(50));
        var usedProfileIdsWithNames = new HashSet<string>();

        int maxFacets = Math.Min(3, profileIdsWithNames.Count);

        foreach (var _ in Enumerable.Range(0, faker.Random.Int(1, maxFacets)))
        {
            var profile = faker.PickRandom(profileIdsWithNames.Where(p => !usedProfileIdsWithNames.Contains(p.Id)));
            if (profile == null) continue;

            int start = faker.Random.Int(0, textBuilder.Length);
            string tag = $" @{profile.Name}";

            commentContent.TagFacets.Add(new TagFacet
            {
                Type = FacetType.Tag,
                ProfileId = Guid.Parse(profile.Id),
                Start = start,
                End = start + tag.Length
            });

            textBuilder.Insert(start, tag);
            usedProfileIdsWithNames.Add(profile.Id);
        }

        commentContent.Text = textBuilder.ToString();
        return commentContent;
    }

}
