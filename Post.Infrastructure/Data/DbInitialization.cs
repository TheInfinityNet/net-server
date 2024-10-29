using Bogus;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.Services.Post.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfinityNetServer.Services.Post.Domain.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace InfinityNetServer.Services.Post.Infrastructure.Data;

public static class DbInitialization
{

    public static void DropDatabase(this IServiceProvider serviceProvider)
    {
        using var serviceScope = serviceProvider.CreateScope();

        var dbContext = serviceScope.ServiceProvider.GetService<PostDbContext>();

        dbContext.Database.EnsureDeleted();
    }

    public static void AutoMigration(this WebApplication webApplication)
    {
        using var serviceScope = webApplication.Services.CreateScope();

        var dbContext = serviceScope.ServiceProvider.GetRequiredService<PostDbContext>();

        if (!dbContext.Database.EnsureCreated()) return;

        dbContext.Database.MigrateAsync().Wait(); //generate all in folder Migration

    }

    public static async void SeedEssentialData(this IServiceProvider serviceProvider, int numberOfPosts)
    {
        using var serviceScope = serviceProvider.CreateScope();
        serviceScope.ServiceProvider.GetService<PostDbContext>();
        var postRepository = serviceScope.ServiceProvider.GetService<IPostRepository>();
        var profileClient = serviceScope.ServiceProvider.GetService<CommonProfileClient>();
        var groupClient = serviceScope.ServiceProvider.GetService<CommonGroupClient>();

        var existingPostCount = await postRepository.GetAllAsync();
        if (existingPostCount.Count == 0)
        {
            var posts = await GeneratePresentationPosts(numberOfPosts / 2, profileClient);
            await postRepository.CreateAsync(posts);

            var groupPosts = await GeneratePresentationPosts(numberOfPosts / 2, profileClient, groupClient);
            await postRepository.CreateAsync(groupPosts);

            Faker faker = new Faker();

            var subPosts = await GenerateSubPosts(faker.Random.Int(1, numberOfPosts), postRepository);
            await postRepository.CreateAsync(subPosts);

            var sharedPosts = await GenerateSharedPosts(faker.Random.Int(1, numberOfPosts), profileClient, postRepository);
            await postRepository.CreateAsync(sharedPosts);
        }
    }

    private static async Task<List<Domain.Entities.Post>> GeneratePresentationPosts(int count, CommonProfileClient profileClient)
    {
        var profileIds = await profileClient.GetProfileIds();
        var faker = new Faker<Domain.Entities.Post>()
            .CustomInstantiator(f =>
            {
                var randomProfileId = f.PickRandom(profileIds);
                return new Domain.Entities.Post
                {
                    Type = PostType.Text,
                    OwnerId = Guid.Parse(randomProfileId),
                    CreatedBy = Guid.Parse(randomProfileId)
                };
            })
            .RuleFor(ap => ap.Content, f => f.Lorem.Sentence());

        return faker.Generate(count);
    }

    private static async Task<List<Domain.Entities.Post>> GeneratePresentationPosts(
        int count, CommonProfileClient profileClient, CommonGroupClient groupClient)
    {
        var groupMemberWithGroups = await groupClient.GetGroupMemberWithGroup();

        var faker = new Faker<Domain.Entities.Post>()
            .CustomInstantiator(f =>
            {
                var randomGroupMemberWithGroups = f.PickRandom(groupMemberWithGroups);
                return new Domain.Entities.Post
                {
                    Type = PostType.Text,
                    GroupId = Guid.Parse(randomGroupMemberWithGroups.GroupId),
                    OwnerId = Guid.Parse(randomGroupMemberWithGroups.UserProfileId),
                    CreatedBy = Guid.Parse(randomGroupMemberWithGroups.UserProfileId) 
                };
            })
            .RuleFor(ap => ap.Content, f => f.Lorem.Sentence());

        return faker.Generate(count);
    }

    private static async Task<List<Domain.Entities.Post>> GenerateSubPosts(int count, IPostRepository postRepository)
    {
        var presentationPosts = await postRepository.GetAllAsync();
        var faker = new Faker<Domain.Entities.Post>()
            .CustomInstantiator(f =>
            {
                var randomPresentationPost = f.PickRandom(presentationPosts);
                var type = f.PickRandom<PostType>();
                var mediaId = (type == PostType.Video || type == PostType.Photo) 
                    ? Guid.NewGuid() : (Guid?)null;

                return new Domain.Entities.Post
                {
                    Type = type,
                    Privacy = randomPresentationPost.Privacy,
                    GroupId = randomPresentationPost.GroupId,
                    Presentation = randomPresentationPost,
                    OwnerId = randomPresentationPost.OwnerId,
                    FileMetadataId = mediaId,
                    CreatedBy = randomPresentationPost.CreatedBy
                };
            })
            .RuleFor(ap => ap.Content, f => f.Lorem.Sentence());

        return faker.Generate(count);
    }

    private static async Task<List<Domain.Entities.Post>> GenerateSharedPosts(
        int count,
        CommonProfileClient profileClient,
        IPostRepository postRepository)
    {
        var profileIds = await profileClient.GetProfileIds();
        var parentPosts = await postRepository.GetAllAsync();

        var faker = new Faker<Domain.Entities.Post>()
            .CustomInstantiator(f =>
            {
                var randomProfileId = f.PickRandom(profileIds);
                var randomParentPost = f.PickRandom(parentPosts);
                return new Domain.Entities.Post
                {
                    Type = PostType.Share,
                    Privacy = randomParentPost.Privacy,
                    Parent = randomParentPost,
                    OwnerId = Guid.Parse(randomProfileId),
                    CreatedBy = Guid.Parse(randomProfileId)
                };
            })
            .RuleFor(ap => ap.Content, f => f.Lorem.Sentence());

        return faker.Generate(count);
    }

}
