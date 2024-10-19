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

            var userInteractions = await GenerateUserInteractions(friendshipRepository);
            await interactionRepository.CreateAsync(userInteractions);

            var pageInteractions = await GeneratePageInteractions(identityClient, profileClient);
            await interactionRepository.CreateAsync(pageInteractions);
        }
    }

    private static async Task<List<Friendship>> GenerateFriendships(CommonIdentityClient identityClient)
    {
        var accountWithProfileIds = await identityClient.GetAccountsWithDefaultProfiles();

        var uniqueFriendships = new HashSet<(Guid SenderId, Guid ReceiverId)>();
        var friendships = new List<Friendship>();

        var faker = new Faker<Friendship>()
            .RuleFor(f => f.Id, f => Guid.NewGuid())
            .CustomInstantiator(f =>
            {
                var randomAccountWithProfileId = f.PickRandom(accountWithProfileIds);
                return new Friendship
                {
                    CreatedBy = Guid.Parse(randomAccountWithProfileId.Id),
                    Status = FriendshipStatus.Accepted,
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
                    if (attempts > 10) // Tránh vòng lặp vô tận nếu không tìm thấy cặp duy nhất
                    {
                        break;
                    }
                } while (receiverId == friendship.SenderId ||
                         uniqueFriendships.Contains((friendship.SenderId, receiverId)) ||
                         uniqueFriendships.Contains((receiverId, friendship.SenderId)));

                if (receiverId != friendship.SenderId)
                {
                    // Đảm bảo tính duy nhất theo cả hai chiềus
                    uniqueFriendships.Add((friendship.SenderId, receiverId));
                }

                return receiverId;
            });

        // Chọn số lượng mối quan hệ tình bạn ngẫu nhiên
        var maxFriendships = (new Faker()).Random.Int(20, Math.Min(accountWithProfileIds.Count * (accountWithProfileIds.Count - 1) / 2, 100)); // Tối đa 100 mối quan hệ

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
    private static async Task<List<Interaction>> GenerateUserInteractions(IFriendshipRepository friendshipRepository)
    {
        var friendships = await friendshipRepository.GetAllAsync();

        // HashSet để lưu trữ các FriendshipId đã chọn
        var usedFriendships = new HashSet<Guid>();
        var interactions = new List<Interaction>();

        var faker = new Faker<Interaction>()
            .CustomInstantiator(f =>
            {
                Friendship randomFriendship;
                do
                {
                    // Chọn một Friendship ngẫu nhiên mà chưa được sử dụng
                    randomFriendship = f.PickRandom(friendships);
                } while (usedFriendships.Contains(randomFriendship.Id));

                // Thêm FriendshipId vào HashSet để đảm bảo không bị trùng
                usedFriendships.Add(randomFriendship.Id);

                return new Interaction
                {
                    CreatedBy = randomFriendship.CreatedBy,
                    Type = InteractionType.Follow,
                    UserProfileId = randomFriendship.SenderId,
                    RelateProfileId = randomFriendship.ReceiverId
                };
            });

        int maxInteractions = (new Faker()).Random.Int(5, 20); // Chọn ngẫu nhiên số lượng interactions

        // Tạo interactions không trùng lặp
        for (int i = 0; i < maxInteractions; i++)
        {
            var interaction = faker.Generate();
            interactions.Add(interaction); // Thêm interaction vào danh sách
        }

        return interactions;
    }

    private static async Task<List<Interaction>> GeneratePageInteractions(CommonIdentityClient identityClient, CommonProfileClient profileClient)
    {
        var accountWithProfileIds = await identityClient.GetAccountsWithDefaultProfiles();
        var pageProfileIds = await profileClient.GetPageProfileIds();

        var uniqueInteractions = new HashSet<(Guid UserProfileId, Guid RelateProfileId)>();
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
                    UserProfileId = randomUserProfileId
                };
            })
            .RuleFor(i => i.Type, InteractionType.Follow)
            .RuleFor(i => i.RelateProfileId, (f, interaction) =>
            {
                Guid relateUserProfileId;
                int attempts = 0;
                const int maxAttempts = 10;

                do
                {
                    relateUserProfileId = Guid.Parse(f.PickRandom(pageProfileIds));
                    attempts++;

                    if (attempts > maxAttempts) break;

                } while (relateUserProfileId == interaction.UserProfileId ||
                         uniqueInteractions.Contains((interaction.UserProfileId, relateUserProfileId)) ||
                         uniqueInteractions.Contains((relateUserProfileId, interaction.UserProfileId)));

                uniqueInteractions.Add((interaction.UserProfileId, relateUserProfileId));
                return relateUserProfileId;
            });

        int maxInteractions = (new Faker()).Random.Int(5, 20); // Chọn ngẫu nhiên số lượng interactions

        // Tạo interactions và xử lý phần bất đồng bộ sau đó
        for (int i = 0; i < maxInteractions; i++)
        {
            var interaction = faker.Generate();
            interactions.Add(interaction); // Thêm interaction sau khi đã xử lý bất đồng bộ
        }

        return interactions;
    }

}
