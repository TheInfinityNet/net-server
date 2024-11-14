using Bogus;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.Services.Relationship.Domain.Entities;
using InfinityNetServer.Services.Relationship.Domain.Enums;
using InfinityNetServer.Services.Relationship.Domain.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace InfinityNetServer.Services.Relationship.Infrastructure.Data;

public static class DbInitialization
{

    public static void DropDatabase(this IServiceProvider serviceProvider)
    {
        using var serviceScope = serviceProvider.CreateScope();

        var dbContext = serviceScope.ServiceProvider.GetService<RelationshipDbContext>();

        dbContext.Database.EnsureDeleted();
    }

    public static void AutoMigration(this WebApplication webApplication)
    {
        using var serviceScope = webApplication.Services.CreateScope();

        var dbContext = serviceScope.ServiceProvider.GetRequiredService<RelationshipDbContext>();

        if (!dbContext.Database.EnsureCreated()) return;

        dbContext.Database.MigrateAsync().Wait(); //generate all in folder Migration

    }

    public static async void SeedEssentialData(this IServiceProvider serviceProvider)
    {
        using var serviceScope = serviceProvider.CreateScope();
        serviceScope.ServiceProvider.GetService<RelationshipDbContext>();
        var friendshipRepository = serviceScope.ServiceProvider.GetService<IFriendshipRepository>();
        var profileFollowRepository = serviceScope.ServiceProvider.GetService<IProfileFollowRepository>();
        var profileBlockRepository = serviceScope.ServiceProvider.GetService<IProfileBlockRepository>();
        var profileClient = serviceScope.ServiceProvider.GetService<CommonProfileClient>();

        var existingFriendshipCount = await friendshipRepository.GetAllAsync();
        if (existingFriendshipCount.Count == 0)
        {
            IList<string> userProfileIds = await profileClient.GetUserProfileIds();
            var newFriendships = GenerateFriendships(userProfileIds);
            await friendshipRepository.CreateAsync(newFriendships);

            IList<string> profileIds = await profileClient.GetProfileIds();
            var userFollows = GenerateProfileFollows(profileIds);
            await profileFollowRepository.CreateAsync(userFollows);

            var userBlocks = GenerateProfileBlocks(profileIds);
            await profileBlockRepository.CreateAsync(userBlocks);
        }
    }

    private static IList<Friendship> GenerateFriendships(IList<string> userProfileIds)
    {
        var uniqueFriendships = new HashSet<(Guid SenderId, Guid ReceiverId)>();
        IList<Friendship> friendships = [];

        var faker = new Faker<Friendship>()
            .CustomInstantiator(f =>
            {
                var randomProfileId = Guid.Parse(f.PickRandom(userProfileIds));
                return new Friendship
                {
                    Status = f.PickRandom<FriendshipStatus>(),
                    SenderId = randomProfileId,
                    CreatedBy = randomProfileId,
                    CreatedAt = f.Date.Recent(f.Random.Int(1, 365))
                };
            })
            .RuleFor(f => f.ReceiverId, (f, friendship) =>
            {
                Guid receiverId;
                int attempts = 0;
                do
                {
                    receiverId = Guid.Parse(f.PickRandom(userProfileIds));
                    attempts++;
                    if (attempts > 10) break;
                    
                } while (receiverId == friendship.SenderId ||
                         uniqueFriendships.Contains((friendship.SenderId, receiverId)) ||
                         uniqueFriendships.Contains((receiverId, friendship.SenderId)));

                if (receiverId != friendship.SenderId) 
                    uniqueFriendships.Add((friendship.SenderId, receiverId));
                
                return receiverId;
            });

        for (int i = 0; i < 2000; i++)
        {
            var friendship = faker.Generate();
            if (friendship.SenderId != friendship.ReceiverId) friendships.Add(friendship);
        }

        return friendships;
    }

    private static IList<ProfileFollow> GenerateProfileFollows(IList<string> profileIds)
    {
        var uniqueInteractions = new HashSet<(Guid ProfileId, Guid RelateProfileId)>();
        IList<ProfileFollow> interactions = [];

        var faker = new Faker<ProfileFollow>()
            .CustomInstantiator(f =>
            {
                Guid randomProfileId;
                Guid randomRelateProfileId;
                do
                {
                    randomProfileId = Guid.Parse(f.PickRandom(profileIds));
                    randomRelateProfileId = Guid.Parse(f.PickRandom(profileIds));
                }

                while (uniqueInteractions
                .Contains((randomProfileId, randomRelateProfileId)) ||
                       randomProfileId == randomRelateProfileId);

                uniqueInteractions.Add((randomProfileId, randomRelateProfileId));

                return new ProfileFollow
                {
                    FollowerId = randomProfileId,
                    FolloweeId = randomRelateProfileId,
                    CreatedBy = randomProfileId,
                    CreatedAt = f.Date.Recent(f.Random.Int(1, 365))
                };
            });

        for (int i = 0; i < 1500; i++)
        {
            var interaction = faker.Generate();
            interactions.Add(interaction);
        }

        return interactions;
    }

    private static IList<ProfileBlock> GenerateProfileBlocks(IList<string> profileIds)
    {
        var uniqueInteractions = new HashSet<(Guid ProfileId, Guid RelateProfileId)>();
        IList<ProfileBlock> interactions = [];

        var faker = new Faker<ProfileBlock>()
            .CustomInstantiator(f =>
            {
                Guid randomProfileId;
                Guid randomRelateProfileId;
                do
                {
                    randomProfileId = Guid.Parse(f.PickRandom(profileIds));
                    randomRelateProfileId = Guid.Parse(f.PickRandom(profileIds));
                }

                while (uniqueInteractions
                .Contains((randomProfileId, randomRelateProfileId)) ||
                       randomProfileId == randomRelateProfileId);

                uniqueInteractions.Add((randomProfileId, randomRelateProfileId));

                return new ProfileBlock
                {
                    BlockerId = randomProfileId,
                    BlockeeId = randomRelateProfileId,
                    CreatedBy = randomProfileId,
                    CreatedAt = f.Date.Recent(f.Random.Int(1, 365))
                };
            });

        for (int i = 0; i < 500; i++)
        {
            var interaction = faker.Generate();
            interactions.Add(interaction);
        }

        return interactions;
    }

}
