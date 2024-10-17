using Bogus;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.Services.Post.Domain.Repositories;
using MassTransit.Initializers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Post.Infrastructure.Data;

public static class DbInitialization
{

    public static void DropDatabase(this IServiceProvider serviceProvider)
    {
        using var serviceScope = serviceProvider.CreateScope();

        var dbContext = serviceScope.ServiceProvider.GetService<PostDbContext>();

        dbContext.Database.EnsureDeleted();
    }

    public static async void SeedEssentialData(this IServiceProvider serviceProvider, int numberOfPosts)
    {
        using var serviceScope = serviceProvider.CreateScope();
        var dbContext = serviceScope.ServiceProvider.GetService<PostDbContext>();
        var postRepository = serviceScope.ServiceProvider.GetService<IPostRepository>();
        var identityClient = serviceScope.ServiceProvider.GetService<CommonIdentityClient>();

        var existingPostCount = await postRepository.GetAllAsync();
        if (existingPostCount.Count == 0)
        {
            var posts = await GeneratePosts(numberOfPosts, identityClient);
            await postRepository.CreateAsync(posts);

            var sharedPosts = await GenerateSharedPosts(numberOfPosts, identityClient, postRepository);
            await postRepository.CreateAsync(sharedPosts);
        }
    }

    private static async Task<List<Domain.Entities.Post>> GeneratePosts(int count, CommonIdentityClient identityClient)
    {
        var accountIds = await identityClient.GetAccountIds();
        var faker = new Faker<Domain.Entities.Post>()
            .CustomInstantiator(f =>
            {
                var randomAccountId = Guid.Parse(f.PickRandom(accountIds));
                return new Domain.Entities.Post
                {
                    OwnerId = randomAccountId,
                    CreatedBy = randomAccountId
                };
            })
            .RuleFor(ap => ap.Content, f => f.Lorem.Sentence());

        return faker.Generate(count);
    }

    private static async Task<List<Domain.Entities.Post>> GenerateSharedPosts(
        int count,
        CommonIdentityClient identityClient,
        IPostRepository postRepository)
    {
        var accountIds = await identityClient.GetAccountIds();
        var parentPosts = await postRepository.GetAllAsync();
        var parentPostIds = parentPosts.Select(p => p.Id).ToList();
        var faker = new Faker<Domain.Entities.Post>()
            .CustomInstantiator(f =>
            {
                var randomAccountId = Guid.Parse(f.PickRandom(accountIds));
                return new Domain.Entities.Post
                {
                    OwnerId = randomAccountId,
                    CreatedBy = randomAccountId,
                    ParentId = f.PickRandom(parentPostIds)
                };
            })
            .RuleFor(ap => ap.Content, f => f.Lorem.Sentence());

        return faker.Generate(count);
    }

}
