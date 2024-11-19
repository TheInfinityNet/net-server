using Bogus;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Others;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Domain.Entities;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.Services.Comment.Domain.Entities;
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

    public static async void SeedEssentialData(this IServiceProvider serviceProvider, int numberOfComments)
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
            Console.WriteLine("PostIds: " + postIds.Count);
            var comments = await GenerateComments(numberOfComments, postIds, postClient, profileClient, relationshipClient);
            await commentRepository.CreateAsync(comments);

            var parentComments = await commentRepository.GetAllAsync();
            var repliedComments = await GenerateRepliedComments(numberOfComments, 
                parentComments, postClient, profileClient, relationshipClient);
            await commentRepository.CreateAsync(repliedComments);
        }
    }

    private static async Task<IList<Domain.Entities.Comment>> GenerateComments(
        int count,
        IList<string> postIds,
        CommonPostClient postClient,
        CommonProfileClient profileClient,
        CommonRelationshipClient relationshipClient)
    {
        // Lấy danh sách profileIds
        var profileIds = await profileClient.GetProfileIds();

        // Gọi API một lần để lấy tất cả các người không thể xem các bài đăng
        var whoCantSeeDict = await GetWhoCantSeeForAllPosts(postIds, postClient);

        var comments = new List<Domain.Entities.Comment>();
        var faker = new Faker<Domain.Entities.Comment>()
            .RuleFor(c => c.PostId, f => Guid.Parse(f.PickRandom(postIds)))
            .CustomInstantiator(f => new Domain.Entities.Comment
            {
                FileMetadataId = f.Random.Bool() ? Guid.NewGuid() : null,
                CreatedAt = f.Date.Recent(f.Random.Int(1, 365))
            });

        // Sinh ra các comment cho mỗi bài đăng
        foreach (var _ in Enumerable.Range(0, count))
        {
            var comment = faker.Generate();

            // Lấy danh sách người không thể xem bài đăng
            if (!whoCantSeeDict.TryGetValue(comment.PostId.ToString(), out var whoCantSeeIds))
            {
                whoCantSeeIds = []; // Nếu không có thông tin, mặc định là không có ai bị cấm
            }

            // Loại bỏ các profile không thể xem bài đăng này
            var validProfileIds = profileIds.Select(Guid.Parse).Except(whoCantSeeIds).ToList();

            if (validProfileIds.Count == 0) break;

            Faker mFaker = new();
            // Gán profile cho comment
            comment.ProfileId = mFaker.PickRandom(validProfileIds);
            comment.CreatedBy = comment.ProfileId;

            // Lấy danh sách bạn bè và người theo dõi
            var friendIds = await relationshipClient.GetFriendIds(comment.ProfileId.ToString());
            var followerIds = await relationshipClient.GetFollowerIds(comment.ProfileId.ToString());
            var combinedIds = friendIds.Concat(followerIds).Distinct().ToList();

            // Lấy thông tin profileId và tên
            var profileIdsWithNames = await profileClient.GetProfileIdsWithNames(combinedIds);

            // Tạo nội dung comment
            comment.Content = GenerateCommentContent(profileIdsWithNames);

            comments.Add(comment);
        }

        return comments;
    }

    private static async Task<IList<Domain.Entities.Comment>> GenerateRepliedComments(
        int count,
        IList<Domain.Entities.Comment> parentComments,
        CommonPostClient postClient,
        CommonProfileClient profileClient,
        CommonRelationshipClient relationshipClient)
    {

        var postIds = parentComments.Select(c => c.PostId.ToString()).Distinct().ToList();
        var profileIds = await profileClient.GetProfileIds();

        // Lấy thông tin ai không thể xem cho tất cả bài đăng một lần
        var whoCantSeeDict = await GetWhoCantSeeForAllPosts(postIds, postClient);

        var comments = new List<Domain.Entities.Comment>();

        // Faker để tạo bình luận trả lời
        var faker = new Faker<Domain.Entities.Comment>()
            .CustomInstantiator(f =>
            {
                var parentComment = f.PickRandom(parentComments);
                return new Domain.Entities.Comment
                {
                    PostId = parentComment.PostId,
                    ParentId = parentComment.Id,
                    FileMetadataId = f.Random.Bool() ? Guid.NewGuid() : null,
                    CreatedAt = f.Date.Recent(f.Random.Int(1, 365))
                };
            });

        // Sinh ra các comment trả lời
        foreach (var _ in Enumerable.Range(0, count))
        {
            var comment = faker.Generate();

            // Lấy danh sách những người không thể xem bài đăng này
            // Lấy danh sách người không thể xem bài đăng
            if (!whoCantSeeDict.TryGetValue(comment.PostId.ToString(), out var whoCantSeeIds))
            {
                whoCantSeeIds = []; // Nếu không có thông tin, mặc định là không có ai bị cấm
            }

            // Loại bỏ các profile không thể xem bài đăng này
            var validProfileIds = profileIds.Select(Guid.Parse).Except(whoCantSeeIds).ToList();

            if (validProfileIds.Count == 0) break;

            Faker mFaker = new();
            // Gán profile cho comment
            comment.ProfileId = mFaker.PickRandom(validProfileIds);
            comment.CreatedBy = comment.ProfileId;

            // Lấy danh sách bạn bè và người theo dõi của comment's profile
            var friendIds = await relationshipClient.GetFriendIds(comment.ProfileId.ToString());
            var followerIds = await relationshipClient.GetFollowerIds(comment.ProfileId.ToString());
            var combinedIds = friendIds.Concat(followerIds).Distinct().ToList();

            // Lấy danh sách profile với tên từ combinedIds
            var profileIdsWithNames = await profileClient.GetProfileIdsWithNames(combinedIds);

            // Tạo nội dung comment
            comment.Content = GenerateCommentContent(profileIdsWithNames);

            comments.Add(comment);
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

    private static CommentContent GenerateCommentContent(IList<ProfileIdWithName> profileIdsWithNames)
    {
        var faker = new Faker();
        var commentContent = new CommentContent();
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



    //private static async Task<IList<Domain.Entities.Comment>> GenerateComments(
    //    int count,
    //    IList<string> postIds,
    //    CommonPostClient postClient,
    //    CommonProfileClient profileClient,
    //    CommonRelationshipClient relationshipClient)
    //{
    //    var profileIds = await profileClient.GetProfileIds();
    //    var faker = new Faker<Domain.Entities.Comment>()
    //        .RuleFor(ap => ap.PostId, f => Guid.Parse(f.PickRandom(postIds)))
    //        .CustomInstantiator(f =>
    //        {
    //            var fileMetadataId = f.Random.Bool() ? Guid.NewGuid() : (Guid?)null;
    //            return new Domain.Entities.Comment
    //            {
    //                FileMetadataId = fileMetadataId,
    //                CreatedAt = f.Date.Recent(f.Random.Int(1, 365))
    //            };
    //        });

    //    IList<Domain.Entities.Comment> comments = [];
    //    Faker faker1 = new();
    //    for (int i = 0; i < count; i++)
    //    {
    //        var comment = faker.Generate();
    //        var friendIds = await relationshipClient.GetFriendIds(comment.ProfileId.ToString());
    //        var folowerIds = await relationshipClient.GetFollowerIds(comment.ProfileId.ToString());
    //        var combinedIds = friendIds.Concat(folowerIds).Distinct().ToList();
    //        var profileIdsWithNames = await profileClient.GetProfileIdsWithNames(combinedIds);
    //        var whoCantSeeIds = await postClient.WhoCantSee(comment.PostId.ToString());

    //        Guid randomProfileId = Guid.Parse(faker1.PickRandom(profileIds));
    //        while (!whoCantSeeIds.Contains(randomProfileId.ToString()))
    //            randomProfileId = Guid.Parse(faker1.PickRandom(profileIds));

    //        comment.ProfileId = randomProfileId;
    //        comment.CreatedBy = randomProfileId;
    //        comment.Content = GenerateCommentContent(profileIdsWithNames);
    //        comments.Add(comment);
    //    }
    //    return comments;
    //}

    //private static async Task<IList<Domain.Entities.Comment>> GenerateRepliedComments(
    //    int count,
    //    IList<Domain.Entities.Comment> parentComments,
    //    CommonPostClient postClient,
    //    CommonProfileClient profileClient,
    //    CommonRelationshipClient relationshipClient)
    //{
    //    var profileIds = await profileClient.GetProfileIds();
    //    var faker = new Faker<Domain.Entities.Comment>()
    //        .CustomInstantiator(f => {
    //            var parentComment = f.PickRandom(parentComments);
    //            var createdBy = Guid.Parse(f.PickRandom(profileIds));
    //            var profileId = createdBy;
    //            var fileMetadataId = f.Random.Bool() ? Guid.NewGuid() : (Guid?)null;
    //            return new Domain.Entities.Comment
    //            {
    //                PostId = parentComment.PostId,
    //                ParentId = parentComment.Id,
    //                FileMetadataId = fileMetadataId,
    //                CreatedAt = f.Date.Recent(f.Random.Int(1, 365))
    //            };
    //        });

    //    IList<Domain.Entities.Comment> comments = [];
    //    Faker faker1 = new();
    //    for (int i = 0; i < count; i++)
    //    {
    //        var comment = faker.Generate();
    //        var friendIds = await relationshipClient.GetFriendIds(comment.ProfileId.ToString());
    //        var folowerIds = await relationshipClient.GetFollowerIds(comment.ProfileId.ToString());
    //        var combinedIds = friendIds.Concat(folowerIds).Distinct().ToList();
    //        var profileIdsWithNames = await profileClient.GetProfileIdsWithNames(combinedIds);

    //        var whoCantSeeIds = await postClient.WhoCantSee(comment.PostId.ToString());

    //        Guid randomProfileId = Guid.Parse(faker1.PickRandom(profileIds));
    //        while (!whoCantSeeIds.Contains(randomProfileId.ToString()))
    //            randomProfileId = Guid.Parse(faker1.PickRandom(profileIds));

    //        comment.ProfileId = randomProfileId;
    //        comment.CreatedBy = randomProfileId;
    //        comment.Content = GenerateCommentContent(profileIdsWithNames);
    //        comments.Add(comment);
    //    }
    //    return comments;
    //}

    //private static CommentContent GenerateCommentContent(IList<ProfileIdWithName> profileIdsWithNames)
    //{
    //    Faker faker = new();
    //    CommentContent commentContent = new();
    //    int facetCount = faker.Random.Int(1, profileIdsWithNames.Count < 3 ? profileIdsWithNames.Count : 3);
    //    int minContentLength = 50;
    //    int minStart = 0;
    //    IList<string> text = [faker.Lorem.Sentence(minContentLength)];
    //    IList<ProfileIdWithName> usedProfileIdsWithNames = [];

    //    for (int i = 0; i < facetCount; i++)
    //    {
    //        if (profileIdsWithNames == null || profileIdsWithNames.Count == 0) continue;

    //        var randomProfileIdWithName = faker.PickRandom(profileIdsWithNames);
    //        while (usedProfileIdsWithNames.Contains(randomProfileIdWithName))
    //            randomProfileIdWithName = faker.PickRandom(profileIdsWithNames);

    //        int tagLength = randomProfileIdWithName.Name.Length;

    //        if (minStart > minContentLength - tagLength)
    //        {
    //            minContentLength = minStart + tagLength + 1;

    //            while (text[^1].Length < minContentLength)
    //                text[^1] += faker.Lorem.Sentence(10);
    //        }

    //        int start = faker.Random.Int(minStart, minContentLength - tagLength);
    //        int end = start + tagLength;

    //        commentContent.TagFacets.Add(new TagFacet
    //        {
    //            Type = FacetType.Tag,
    //            ProfileId = Guid.Parse(randomProfileIdWithName.Id),
    //            Start = start,
    //            End = end
    //        });

    //        text[^1] = text[^1].Insert(start, $" @{randomProfileIdWithName.Name}");
    //        usedProfileIdsWithNames.Add(randomProfileIdWithName);

    //        minContentLength += 50;
    //        minStart = end;
    //        text.Add(faker.Lorem.Sentence(50));

    //    }

    //    commentContent.Text = string.Join(" ", text);

    //    return commentContent;
    //}

}
