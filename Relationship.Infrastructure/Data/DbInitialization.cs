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
        var maxFriendships = (new Faker()).Random.Int(20, 
            Math.Min(userProfileIds.Count * (userProfileIds.Count - 1) / 2, 100)); // Tối đa 100 mối quan hệ

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
                    ProfileId = randomFriendship.SenderId,
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

    private static async Task<List<Interaction>> GeneratePageInteractions(CommonProfileClient profileClient)
    {
        var userProfileIds = await profileClient.GetUserProfileIds();
        var pageProfileIds = await profileClient.GetPageProfileIds();

        var uniqueInteractions = new HashSet<(Guid UserProfileId, Guid RelateProfileId)>();
        var interactions = new List<Interaction>();

        var faker = new Faker<Interaction>()
            .RuleFor(i => i.Type, f => f.PickRandom<InteractionType>())
            .CustomInstantiator(f =>
            {
                var randomProfileId = f.PickRandom(userProfileIds);
                var randomAccountId = Guid.Parse(randomProfileId);
                var randomUserProfileId = Guid.Parse(randomProfileId);
                return new Interaction
                {
                    CreatedBy = randomAccountId,
                    ProfileId = randomUserProfileId
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

                } while (relateUserProfileId == interaction.ProfileId ||
                         uniqueInteractions.Contains((interaction.ProfileId, relateUserProfileId)) ||
                         uniqueInteractions.Contains((relateUserProfileId, interaction.ProfileId)));

                uniqueInteractions.Add((interaction.ProfileId, relateUserProfileId));
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
