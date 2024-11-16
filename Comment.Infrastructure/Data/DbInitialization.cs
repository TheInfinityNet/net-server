using Bogus;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.Services.Comment.Domain.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using InfinityNetServer.BuildingBlocks.Domain.Entities;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.Services.Comment.Domain.Entities;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Others;
using System.Threading.Tasks;
using System.Linq;

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
            var comments = await GenerateComments(numberOfComments, postIds, profileClient, relationshipClient);
            await commentRepository.CreateAsync(comments);

            var parentComments = await commentRepository.GetAllAsync();
            var repliedComments = await GenerateRepliedComments(
                numberOfComments * 2, parentComments, profileClient, relationshipClient);
            await commentRepository.CreateAsync(repliedComments);
        }
    }

    private static async Task<IList<Domain.Entities.Comment>> GenerateComments(
        int count,
        IList<string> postIds,
        CommonProfileClient profileClient,
        CommonRelationshipClient relationshipClient)
    {
        var profileIds = await profileClient.GetProfileIds();
        var faker = new Faker<Domain.Entities.Comment>()
            .RuleFor(ap => ap.PostId, f => Guid.Parse(f.PickRandom(postIds)))
            .CustomInstantiator(f =>
            {
                var createdBy = Guid.Parse(f.PickRandom(profileIds));
                var profileId = createdBy;
                var fileMetadataId = f.Random.Bool() ? Guid.NewGuid() : (Guid?)null;
                return new Domain.Entities.Comment
                {
                    ProfileId = profileId,
                    FileMetadataId = fileMetadataId,
                    CreatedBy = createdBy,
                    CreatedAt = f.Date.Recent(f.Random.Int(1, 365))
                };
            });

        IList<Domain.Entities.Comment> comments = [];
        for (int i = 0; i < count; i++)
        {
            var comment = faker.Generate();
            var friendIds = await relationshipClient.GetFriendIds(comment.ProfileId.ToString());
            var folowerIds = await relationshipClient.GetFollowerIds(comment.ProfileId.ToString());
            var combinedIds = friendIds.Concat(folowerIds).Distinct().ToList();
            var profileIdsWithNames = await profileClient.GetProfileIdsWithNames(combinedIds);
            comment.Content = GenerateCommentContent(profileIdsWithNames);
            comments.Add(comment);
        }
        return comments;
    }

    private static async Task<IList<Domain.Entities.Comment>> GenerateRepliedComments(
        int count,
        IList<Domain.Entities.Comment> parentComments,
        CommonProfileClient profileClient,
        CommonRelationshipClient relationshipClient)
    {
        var profileIds = await profileClient.GetProfileIds();
        var faker = new Faker<Domain.Entities.Comment>()
            .CustomInstantiator(f => {
                var parentComment = f.PickRandom(parentComments);
                var createdBy = Guid.Parse(f.PickRandom(profileIds));
                var profileId = createdBy;
                var fileMetadataId = f.Random.Bool() ? Guid.NewGuid() : (Guid?)null;
                return new Domain.Entities.Comment
                {
                    PostId = parentComment.PostId,
                    ParentId = parentComment.Id,
                    ProfileId = profileId,
                    FileMetadataId = fileMetadataId,
                    CreatedBy = createdBy,
                    CreatedAt = f.Date.Recent(f.Random.Int(1, 365))
                };
            });

        IList<Domain.Entities.Comment> comments = [];
        for (int i = 0; i < count; i++)
        {
            var comment = faker.Generate();
            var friendIds = await relationshipClient.GetFriendIds(comment.ProfileId.ToString());
            var folowerIds = await relationshipClient.GetFollowerIds(comment.ProfileId.ToString());
            var combinedIds = friendIds.Concat(folowerIds).Distinct().ToList();
            var profileIdsWithNames = await profileClient.GetProfileIdsWithNames(combinedIds);
            comment.Content = GenerateCommentContent(profileIdsWithNames);
            comments.Add(comment);
        }
        return comments;
    }

    private static CommentContent GenerateCommentContent(IList<ProfileIdWithName> profileIdsWithNames)
    {
        Faker faker = new();
        CommentContent commentContent = new();
        int facetCount = faker.Random.Int(1, profileIdsWithNames.Count < 3 ? profileIdsWithNames.Count : 3);
        int minContentLength = 50;
        int minStart = 0;
        IList<string> text = [faker.Lorem.Sentence(minContentLength)];
        IList<ProfileIdWithName> usedProfileIdsWithNames = [];

        for (int i = 0; i < facetCount; i++)
        {
            if (profileIdsWithNames == null || profileIdsWithNames.Count == 0) continue;

            var randomProfileIdWithName = faker.PickRandom(profileIdsWithNames);
            while (usedProfileIdsWithNames.Contains(randomProfileIdWithName))
                randomProfileIdWithName = faker.PickRandom(profileIdsWithNames);

            int tagLength = randomProfileIdWithName.Name.Length;

            if (minStart > minContentLength - tagLength)
            {
                minContentLength = minStart + tagLength + 1;

                while (text[^1].Length < minContentLength)
                    text[^1] += faker.Lorem.Sentence(10);
            }

            int start = faker.Random.Int(minStart, minContentLength - tagLength);
            int end = start + tagLength;

            commentContent.TagFacets.Add(new TagFacet
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

        commentContent.Text = string.Join(" ", text);

        return commentContent;
    }

}
