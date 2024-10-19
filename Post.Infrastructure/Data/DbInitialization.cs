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
        var groupClient = serviceScope.ServiceProvider.GetService<CommonGroupClient>();

        var existingPostCount = await postRepository.GetAllAsync();
        if (existingPostCount.Count == 0)
        {
            var posts = await GeneratePresentationPosts(numberOfPosts / 2, identityClient);
            await postRepository.CreateAsync(posts);

            var groupPosts = await GeneratePresentationPosts(numberOfPosts / 2, identityClient, groupClient);
            await postRepository.CreateAsync(groupPosts);

            Faker faker = new Faker();

            var subPosts = await GenerateSubPosts(faker.Random.Int(1, numberOfPosts), postRepository);
            await postRepository.CreateAsync(subPosts);

            var sharedPosts = await GenerateSharedPosts(faker.Random.Int(1, numberOfPosts), identityClient, postRepository);
            await postRepository.CreateAsync(sharedPosts);
        }
    }

    private static async Task<List<Domain.Entities.Post>> GeneratePresentationPosts(int count, CommonIdentityClient identityClient)
    {
        var accountWithDefaultProfiles = await identityClient.GetAccountsWithDefaultProfiles();
        var faker = new Faker<Domain.Entities.Post>()
            .CustomInstantiator(f =>
            {
                var randomAccountWithDefaultProfile = f.PickRandom(accountWithDefaultProfiles);
                return new Domain.Entities.Post
                {
                    Type = Domain.Enums.PostType.Text,
                    OwnerId = Guid.Parse(randomAccountWithDefaultProfile.DefaultUserProfileId),
                    CreatedBy = Guid.Parse(randomAccountWithDefaultProfile.Id)
                };
            })
            .RuleFor(ap => ap.Content, f => f.Lorem.Sentence());

        return faker.Generate(count);
    }

    private static async Task<List<Domain.Entities.Post>> GeneratePresentationPosts(int count, CommonIdentityClient identityClient, CommonGroupClient groupClient)
    {
        var groupMemberWithGroups = await groupClient.GetGroupMemberWithGroup();
        var posts = new List<Domain.Entities.Post>();

        var faker = new Faker<Domain.Entities.Post>()
            .CustomInstantiator(f =>
            {
                var randomGroupMemberWithGroups = f.PickRandom(groupMemberWithGroups);
                return new Domain.Entities.Post
                {
                    Type = Domain.Enums.PostType.Text,
                    GroupId = Guid.Parse(randomGroupMemberWithGroups.GroupId),
                    OwnerId = Guid.Parse(randomGroupMemberWithGroups.UserProfileId),
                    CreatedBy = Guid.Empty // Tạm thời để trống, sẽ được cập nhật sau
                };
            })
            .RuleFor(ap => ap.Content, f => f.Lorem.Sentence());

        var generatedPosts = faker.Generate(count);

        // Xử lý phần bất đồng bộ sau khi đối tượng Post được tạo
        foreach (var post in generatedPosts)
        {
            var accountId = await identityClient.GetAccountId(post.OwnerId.ToString());
            post.CreatedBy = Guid.Parse(accountId); // Cập nhật giá trị CreatedBy
            posts.Add(post);
        }

        return posts;
    }

    private static async Task<List<Domain.Entities.Post>> GenerateSubPosts(int count, IPostRepository postRepository)
    {
        var presentationPosts = await postRepository.GetAllAsync();
        var faker = new Faker<Domain.Entities.Post>()
            .CustomInstantiator(f =>
            {
                var randomPresentationPost = f.PickRandom(presentationPosts);
                var type = f.PickRandom<Domain.Enums.PostType>();
                var mediaId = (type == Domain.Enums.PostType.Media || type == Domain.Enums.PostType.AlbumMedia) ? Guid.NewGuid() : (Guid?)null;
                return new Domain.Entities.Post
                {
                    Type = type,
                    GroupId = randomPresentationPost.GroupId,
                    Presentation = randomPresentationPost,
                    OwnerId = randomPresentationPost.OwnerId,
                    MediaId = mediaId,
                    CreatedBy = randomPresentationPost.CreatedBy
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
        var accountWithDefaultProfiles = await identityClient.GetAccountsWithDefaultProfiles();
        var parentPosts = await postRepository.GetAllAsync();
        var parentPostIds = parentPosts.Select(p => p.Id).ToList();
        var faker = new Faker<Domain.Entities.Post>()
            .CustomInstantiator(f =>
            {
                var randomAccountWithDefaultProfile = f.PickRandom(accountWithDefaultProfiles);
                return new Domain.Entities.Post
                {
                    Type = Domain.Enums.PostType.Share,
                    ParentId = f.PickRandom(parentPostIds),
                    OwnerId = Guid.Parse(randomAccountWithDefaultProfile.DefaultUserProfileId),
                    CreatedBy = Guid.Parse(randomAccountWithDefaultProfile.Id)
                };
            })
            .RuleFor(ap => ap.Content, f => f.Lorem.Sentence());

        return faker.Generate(count);
    }

}
