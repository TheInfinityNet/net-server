using Bogus;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.Services.Relationship.Domain.Entities;
using InfinityNetServer.Services.Relationship.Domain.Enums;
using InfinityNetServer.Services.Relationship.Domain.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

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
        var interactionRepository = serviceScope.ServiceProvider.GetService<IInteractionRepository>();
        var profileClient = serviceScope.ServiceProvider.GetService<CommonProfileClient>();

        var existingFriendshipCount = await friendshipRepository.GetAllAsync();
        if (existingFriendshipCount.Count == 0)
        {
            IList<string> userProfileIds = await profileClient.GetUserProfileIds();
            var newFriendships = GenerateFriendships(userProfileIds);
            await friendshipRepository.CreateAsync(newFriendships);

            IList<Friendship> friendships = await friendshipRepository.GetAllAsync();
            var userInteractions = GenerateUserInteractions(friendships);
            await interactionRepository.CreateAsync(userInteractions);

            IList<string> pageProfileIds = await profileClient.GetPageProfileIds();
            var pageInteractions = GeneratePageInteractions(userProfileIds, pageProfileIds);
            await interactionRepository.CreateAsync(pageInteractions);
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

    private static IList<Interaction> GenerateUserInteractions(IList<Friendship> friendships)
    {
        var uniqueInteractions = new HashSet<(Guid ProfileId, Guid RelateProfileId)>();
        IList<Interaction> interactions = [];

        var faker = new Faker<Interaction>()
            .CustomInstantiator(f =>
            {
                Friendship randomFriendship;
                do randomFriendship = f.PickRandom(friendships);

                while (uniqueInteractions
                .Contains((randomFriendship.SenderId, randomFriendship.ReceiverId)) ||
                       randomFriendship.SenderId == randomFriendship.ReceiverId);

                uniqueInteractions.Add((randomFriendship.SenderId, randomFriendship.ReceiverId));

                return new Interaction
                {
                    Type = InteractionType.Follow,
                    Friendship = randomFriendship,
                    ProfileId = randomFriendship.SenderId,
                    RelateProfileId = randomFriendship.ReceiverId,
                    CreatedBy = randomFriendship.CreatedBy,
                    CreatedAt = f.Date.Recent(f.Random.Int(1, 365))
                };
            });

        for (int i = 0; i < 1000; i++)
        {
            var interaction = faker.Generate();
            interactions.Add(interaction);
        }

        return interactions;
    }

    private static IList<Interaction> GeneratePageInteractions(
        IList<string> userProfileIds, IList<string> pageProfileIds)
    {
        var uniqueInteractions = new HashSet<(Guid UserProfileId, Guid PageProfileId)>();
        IList<Interaction> interactions = [];

        var faker = new Faker<Interaction>()
            .CustomInstantiator(f =>
            {
                Guid userProfileId;
                Guid pageProfileId;
                int attempts = 0;
                const int maxAttempts = 10;

                do
                {
                    userProfileId = Guid.Parse(f.PickRandom(userProfileIds));
                    pageProfileId = Guid.Parse(f.PickRandom(pageProfileIds));
                    attempts++;
                }
                while ((userProfileId == pageProfileId 
                        || uniqueInteractions.Contains((userProfileId, pageProfileId))) 
                        && attempts < maxAttempts);

                uniqueInteractions.Add((userProfileId, pageProfileId));

                return new Interaction
                {
                    Type = InteractionType.Follow,
                    ProfileId = userProfileId,
                    RelateProfileId = pageProfileId,
                    CreatedBy = userProfileId,
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
