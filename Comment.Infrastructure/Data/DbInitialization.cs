using Bogus;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.Services.Comment.Domain.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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

        var existingCommentCount = await commentRepository.GetAllAsync();
        if (existingCommentCount.Count == 0)
        {
            var comments = await GenerateComments(numberOfComments, profileClient, postClient);
            await commentRepository.CreateAsync(comments);

            var repliedComments = 
                await GenerateRepliedComments(numberOfComments, profileClient, postClient, commentRepository);
            await commentRepository.CreateAsync(repliedComments);
        }
    }

    private static async Task<List<Domain.Entities.Comment>> GenerateComments(
        int count,
        CommonProfileClient profileClient,
        CommonPostClient postClient)
    {
        var profileIds = await profileClient.GetProfileIds();
        var postIds = await postClient.GetPostIds();
        var faker = new Faker<Domain.Entities.Comment>()
            .RuleFor(ap => ap.PostId, f => Guid.Parse(f.PickRandom(postIds)))
            .RuleFor(ap => ap.Content, f => f.Lorem.Sentence())
            .CustomInstantiator(f =>
            {
                var createdBy = Guid.Parse(f.PickRandom(profileIds));
                var profileId = createdBy;
                var mediaId = (f.Random.Bool()) ? Guid.NewGuid() : (Guid?)null;
                return new Domain.Entities.Comment
                {
                    CreatedBy = createdBy,
                    ProfileId = profileId,
                    MediaId = mediaId
                };
            });

        return faker.Generate(count);
    }

    private static async Task<List<Domain.Entities.Comment>> GenerateRepliedComments(
        int count,
        CommonProfileClient profileClient,
        CommonPostClient postClient,
        ICommentRepository commentRepository)
    {
        var profileIds = await profileClient.GetProfileIds();
        var postIds = await postClient.GetPostIds();
        var parentComments = await commentRepository.GetAllAsync();

        var faker = new Faker<Domain.Entities.Comment>()
            .RuleFor(ap => ap.PostId, f => Guid.Parse(f.PickRandom(postIds)))
            .RuleFor(ap => ap.ParentComment, f => f.PickRandom(parentComments))
            .RuleFor(ap => ap.Content, f => f.Lorem.Sentence())
            .CustomInstantiator(f => {
                var createdBy = Guid.Parse(f.PickRandom(profileIds));
                var profileId = createdBy;
                var mediaId = (f.Random.Bool()) ? Guid.NewGuid() : (Guid?)null;
                return new Domain.Entities.Comment
                {
                    CreatedBy = createdBy,
                    ProfileId = profileId,
                    MediaId = mediaId
                };
            });

        return faker.Generate(count);
    }

}
