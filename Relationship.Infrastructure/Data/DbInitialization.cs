using Bogus;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.Services.Relationship.Domain.Entities;
using InfinityNetServer.Services.Relationship.Domain.Enums;
using InfinityNetServer.Services.Relationship.Domain.Repositories;
using MassTransit.Initializers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Relationship.Infrastructure.Data;

public static class DbInitialization
{

    public static void DropDatabase(this IServiceProvider serviceProvider)
    {
        using var serviceScope = serviceProvider.CreateScope();

        var dbContext = serviceScope.ServiceProvider.GetService<RelationshipDbContext>();

        dbContext.Database.EnsureDeleted();
    }

    public static async void SeedEssentialData(this IServiceProvider serviceProvider)
    {
        using var serviceScope = serviceProvider.CreateScope();
        var dbContext = serviceScope.ServiceProvider.GetService<RelationshipDbContext>();
        var friendshipRepository = serviceScope.ServiceProvider.GetService<IFriendshipRepository>();
        var interactionRepository = serviceScope.ServiceProvider.GetService<IInteractionRepository>();
        var identityClient = serviceScope.ServiceProvider.GetService<CommonIdentityClient>();
        var profileClient = serviceScope.ServiceProvider.GetService<CommonProfileClient>();

        var existingFriendshipCount = await friendshipRepository.GetAllAsync();
        if (existingFriendshipCount.Count == 0)
        {
            var Friendships = await GenerateFriendships(identityClient);
            await friendshipRepository.CreateAsync(Friendships);

            var interactions = await GenerateInteractions(identityClient, profileClient, friendshipRepository);
            await interactionRepository.CreateAsync(interactions);
        }
    }

    private static async Task<List<Friendship>> GenerateFriendships(CommonIdentityClient identityClient)
    {
        var accountWithProfileIds = await identityClient.GetAccountWithDefaultProfileIds();

        var uniqueFriendships = new HashSet<(Guid SenderId, Guid ReceiverId)>();
        var friendships = new List<Friendship>();

        var faker = new Faker<Friendship>()
            .RuleFor(f => f.Id, f => Guid.NewGuid())
            .RuleFor(f => f.Status, f => f.PickRandom<FriendshipStatus>())
            .CustomInstantiator(f =>
            {
                var randomAccountWithProfileId = f.PickRandom(accountWithProfileIds);
                return new Friendship
                {
                    CreatedBy = Guid.Parse(randomAccountWithProfileId.Id),
                    SenderId = Guid.Parse(randomAccountWithProfileId.DefaultUserProfileId)
                };
            })
            .RuleFor(f => f.ReceiverId, (f, friendship) =>
            {
                Guid receiverId;
                int attempts = 0;
                do
                {
                    receiverId = Guid.Parse(f.PickRandom(accountWithProfileIds).DefaultUserProfileId);
                    attempts++;
                    if (attempts > 10) // Avoid infinite loop if unable to find a unique pair
                    {
                        break;
                    }
                } while (receiverId == friendship.SenderId ||
                         uniqueFriendships.Contains((friendship.SenderId, receiverId)) ||
                         uniqueFriendships.Contains((receiverId, friendship.SenderId)));

                if (receiverId != friendship.SenderId)
                {
                    // Ensure uniqueness in both directions
                    uniqueFriendships.Add((friendship.SenderId, receiverId));
                }

                return receiverId;
            });

        // Limit the number of generated friendships to avoid excessive processing
        var maxFriendships = Math.Min(accountWithProfileIds.Count, 100); // Set 100 as the max or use a different value

        for (int i = 0; i < maxFriendships; i++)
        {
            var friendship = faker.Generate();
            if (friendship.SenderId != friendship.ReceiverId)
            {
                friendships.Add(friendship);
            }
        }

        return friendships;
    }

    private static async Task<List<Interaction>> GenerateInteractions(
    CommonIdentityClient identityClient,
    CommonProfileClient profileClient,
    IFriendshipRepository friendshipRepository)
    {
        var accountWithProfileIds = await identityClient.GetAccountWithDefaultProfileIds();
        var profileIds = await profileClient.GetProfileIds();
        var friendships = await friendshipRepository.GetAllAsync();
        var friendshipIds = friendships.Select(f => f.Id.ToString()).ToList();

        var uniqueInteractions = new HashSet<(Guid ProfileId, Guid RelateProfileId)>();
        var interactions = new List<Interaction>();

        var faker = new Faker<Interaction>()
            .RuleFor(i => i.Type, f => f.PickRandom<InteractionType>())
            .CustomInstantiator(f =>
            {
                var randomAccountWithProfileId = f.PickRandom(accountWithProfileIds);
                var randomAccountId = Guid.Parse(randomAccountWithProfileId.Id);
                var randomUserProfileId = Guid.Parse(randomAccountWithProfileId.DefaultUserProfileId);
                return new Interaction
                {
                    CreatedBy = randomAccountId,
                    ProfileId = randomUserProfileId
                };
            })
            .RuleFor(i => i.RelateProfileId, (f, interaction) =>
            {
                Guid relateProfileId;
                int attempts = 0;
                const int maxAttempts = 10;

                do
                {
                    relateProfileId = Guid.Parse(f.PickRandom(profileIds));
                    attempts++;
                    if (attempts > maxAttempts) break;
                } while (relateProfileId == interaction.ProfileId ||
                         uniqueInteractions.Contains((interaction.ProfileId, relateProfileId)) ||
                         uniqueInteractions.Contains((relateProfileId, interaction.ProfileId)) ||
                         (interaction.Type == InteractionType.Block && relateProfileId == interaction.ProfileId));

                uniqueInteractions.Add((interaction.ProfileId, relateProfileId));
                return relateProfileId;
            })
            .RuleFor(i => i.FriendshipId, (f, interaction) =>
            {
                if (interaction.Type == InteractionType.Block || !friendshipIds.Any())
                {
                    return null; // Block interaction or no friendships
                }
                else
                {
                    return Guid.Parse(f.PickRandom(friendshipIds));
                }
            });

        int maxInteractions = 100; // Set limit for interactions

        for (int i = 0; i < maxInteractions; i++)
        {
            var interaction = faker.Generate();
            interactions.Add(interaction);
        }

        return interactions;
    }

}
