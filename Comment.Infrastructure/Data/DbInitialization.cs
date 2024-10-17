using Bogus;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.Services.Comment.Domain.Repositories;
using MassTransit.Initializers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Collections.Generic;
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

    public static async void SeedEssentialData(this IServiceProvider serviceProvider, int numberOfComments)
    {
        using var serviceScope = serviceProvider.CreateScope();
        var dbContext = serviceScope.ServiceProvider.GetService<CommentDbContext>();
        var commentRepository = serviceScope.ServiceProvider.GetService<ICommentRepository>();
        var identityClient = serviceScope.ServiceProvider.GetService<CommonIdentityClient>();
        var postClient = serviceScope.ServiceProvider.GetService<CommonPostClient>();

        var existingCommentCount = await commentRepository.GetAllAsync();
        if (existingCommentCount.Count == 0)
        {
            var comments = await GenerateComments(numberOfComments, identityClient, postClient);
            await commentRepository.CreateAsync(comments);

            var repliedComments = await GenerateRepliedComments(
                numberOfComments, identityClient, postClient, commentRepository);
            await commentRepository.CreateAsync(repliedComments);
        }
    }

    private static async Task<List<Domain.Entities.Comment>> GenerateComments(
        int count, 
        CommonIdentityClient identityClient,
        CommonPostClient postClient)
    {
        var accountIds = await identityClient.GetAccountIds();
        var postIds = await postClient.GetPostIds();
        var faker = new Faker<Domain.Entities.Comment>()
            .RuleFor(ap => ap.CreatedBy, f => Guid.Parse(f.PickRandom(accountIds)))
            .RuleFor(ap => ap.PostId, f => Guid.Parse(f.PickRandom(postIds)))
            .RuleFor(ap => ap.Content, f => f.Lorem.Sentence());

        return faker.Generate(count);
    }

    private static async Task<List<Domain.Entities.Comment>> GenerateRepliedComments(
    int count,
    CommonIdentityClient identityClient,
    CommonPostClient postClient,
    ICommentRepository commentRepository)
    {
        var accountIds = await identityClient.GetAccountIds();
        var postIds = await postClient.GetPostIds();
        var parentComments = await commentRepository.GetAllAsync();
        var parentCommentIds = parentComments.Select(p => p.Id.ToString()).ToList();
        var faker = new Faker<Domain.Entities.Comment>()
            .RuleFor(ap => ap.CreatedBy, f => Guid.Parse(f.PickRandom(accountIds)))
            .RuleFor(ap => ap.PostId, f => Guid.Parse(f.PickRandom(postIds)))
            .RuleFor(ap => ap.ParentId, f => Guid.Parse(f.PickRandom(parentCommentIds)))
            .RuleFor(ap => ap.Content, f => f.Lorem.Sentence());

        return faker.Generate(count);
    }

}
