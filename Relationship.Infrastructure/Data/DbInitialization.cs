using Bogus;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.Services.Relationship.Domain.Entities;
using InfinityNetServer.Services.Relationship.Domain.Enums;
using InfinityNetServer.Services.Relationship.Domain.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            var friendships = await GenerateFriendships(profileClient);
            await friendshipRepository.CreateAsync(friendships);

            var userInteractions = await GenerateUserInteractions(friendshipRepository);
            await interactionRepository.CreateAsync(userInteractions);

            var pageInteractions = await GeneratePageInteractions(profileClient);
            await interactionRepository.CreateAsync(pageInteractions);
        }
    }

    private static async Task<List<Friendship>> GenerateFriendships(CommonProfileClient profileClient)
    {
        var userProfileIds = await profileClient.GetUserProfileIds();
        var uniqueFriendships = new HashSet<(Guid SenderId, Guid ReceiverId)>();
        var friendships = new List<Friendship>();

        var faker = new Faker<Friendship>()
            .RuleFor(f => f.Id, f => Guid.NewGuid())
            .CustomInstantiator(f =>
            {
                var randomProfileId = f.PickRandom(userProfileIds);
                return new Friendship
                {
                    CreatedBy = Guid.Parse(randomProfileId),
                    Status = FriendshipStatus.Accepted,
                    SenderId = Guid.Parse(randomProfileId)
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

                if (receiverId != friendship.SenderId) uniqueFriendships.Add((friendship.SenderId, receiverId));
                

                return receiverId;
            });

        for (int i = 0; i < 2000; i++)
        {
            var friendship = faker.Generate();
            if (friendship.SenderId != friendship.ReceiverId) friendships.Add(friendship);
            
        }

        return friendships;
    }

    private static async Task<List<Interaction>> GenerateUserInteractions(IFriendshipRepository friendshipRepository)
    {
        var friendships = await friendshipRepository.GetAllAsync();
        var uniqueInteractions = new HashSet<(Guid ProfileId, Guid RelateProfileId)>();
        var interactions = new List<Interaction>();

        var faker = new Faker<Interaction>()
            .CustomInstantiator(f =>
            {
                Friendship randomFriendship;
                do
                {
                    randomFriendship = f.PickRandom(friendships);
                }
                while (uniqueInteractions.Contains((randomFriendship.SenderId, randomFriendship.ReceiverId)) ||
                       randomFriendship.SenderId == randomFriendship.ReceiverId);

                uniqueInteractions.Add((randomFriendship.SenderId, randomFriendship.ReceiverId));

                return new Interaction
                {
                    CreatedBy = randomFriendship.CreatedBy,
                    Type = InteractionType.Follow,
                    ProfileId = randomFriendship.SenderId,
                    RelateProfileId = randomFriendship.ReceiverId
                };
            });

        for (int i = 0; i < 1000; i++)
        {
            var interaction = faker.Generate();
            interactions.Add(interaction);
        }

        return interactions;
    }

    private static async Task<List<Interaction>> GeneratePageInteractions(CommonProfileClient profileClient)
    {
        var userProfileIds = await profileClient.GetUserProfileIds();
        var pageProfileIds = await profileClient.GetPageProfileIds();

        var uniqueInteractions = new HashSet<(Guid UserProfileId, Guid PageProfileId)>();
        var interactions = new List<Interaction>();

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
                while ((userProfileId == pageProfileId ||
                        uniqueInteractions.Contains((userProfileId, pageProfileId))) &&
                       attempts < maxAttempts);

                uniqueInteractions.Add((userProfileId, pageProfileId));

                return new Interaction
                {
                    CreatedBy = userProfileId,
                    Type = InteractionType.Follow,
                    ProfileId = userProfileId,
                    RelateProfileId = pageProfileId
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
