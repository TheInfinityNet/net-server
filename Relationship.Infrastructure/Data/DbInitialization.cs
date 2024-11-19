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
            IList<string> profileIds = await profileClient.GetProfileIds();
            (IList<ProfileBlock>, HashSet<(Guid, Guid)>) userBlocks = GenerateProfileBlocks(profileIds);
            await profileBlockRepository.CreateAsync(userBlocks.Item1);

            HashSet<(Guid, Guid)> profilesBlockEachOther = userBlocks.Item2;
            IList<string> userProfileIds = await profileClient.GetUserProfileIds();
            (IList<Friendship>, IList<ProfileFollow>) newFriendshipsWithFollows = GenerateFriendshipsWithFollows(userProfileIds, profilesBlockEachOther);
            await friendshipRepository.CreateAsync(newFriendshipsWithFollows.Item1);
            await profileFollowRepository.CreateAsync(newFriendshipsWithFollows.Item2);

            IList<string> pageProfileIds = await profileClient.GetPageProfileIds();
            IList<ProfileFollow> pageFollows = GeneratePageFollows(pageProfileIds, profilesBlockEachOther);
            await profileFollowRepository.CreateAsync(pageFollows);

        }
    }

    private static (IList<ProfileBlock>, HashSet<(Guid, Guid)>) GenerateProfileBlocks(IList<string> profileIds)
    {
        var uniqueInteractions = new HashSet<(Guid, Guid)>();
        var interactions = new List<ProfileBlock>();

        var faker = new Faker<ProfileBlock>()
            .CustomInstantiator(f =>
            {
                Guid blockerId, blockeeId;

                // Chọn ngẫu nhiên và đảm bảo không bị trùng lặp
                do
                {
                    blockerId = Guid.Parse(f.PickRandom(profileIds));
                    blockeeId = Guid.Parse(f.PickRandom(profileIds));
                }
                while (blockerId == blockeeId || !uniqueInteractions.Add(NormalizePair(blockerId, blockeeId)));

                return new ProfileBlock
                {
                    BlockerId = blockerId,
                    BlockeeId = blockeeId,
                    CreatedBy = blockerId,
                    CreatedAt = f.Date.Recent(f.Random.Int(1, 365))
                };
            });

        for (int i = 0; i < 200; i++) interactions.Add(faker.Generate());

        return (interactions, uniqueInteractions);
    }

    private static (IList<Friendship>, IList<ProfileFollow>) GenerateFriendshipsWithFollows(
        IList<string> userProfileIds, HashSet<(Guid, Guid)> profilesBlockEachOther)
    {
        var uniqueFriendships = new HashSet<(Guid, Guid)>(profilesBlockEachOther);
        IList<Friendship> friendships = [];
        IList<ProfileFollow> profileFollows = [];

        var faker = new Faker<Friendship>()
            .CustomInstantiator(f =>
            {
                var senderId = Guid.Parse(f.PickRandom(userProfileIds));
                return new Friendship
                {
                    Status = f.PickRandom<FriendshipStatus>(),
                    SenderId = senderId,
                    CreatedBy = senderId,
                    CreatedAt = f.Date.Recent(f.Random.Int(1, 365))
                };
            })
            .RuleFor(f => f.ReceiverId, (f, friendship) =>
            {
                Guid receiverId;
                int attempts = 0;

                // Tạo receiverId hợp lệ
                do
                {
                    receiverId = Guid.Parse(f.PickRandom(userProfileIds));
                    attempts++;
                } while ((receiverId == friendship.SenderId ||
                          !uniqueFriendships.Add(NormalizePair(friendship.SenderId, receiverId))) &&
                          attempts < 10);

                return receiverId;
            });

        for (int i = 0; i < 2000; i++)
        {
            var friendship = faker.Generate();
            if (friendship.SenderId != friendship.ReceiverId)
            {
                friendships.Add(friendship);

                // Tạo các ProfileFollow
                AddProfileFollow(profileFollows, friendship.SenderId, friendship.ReceiverId, friendship.CreatedAt);
                AddProfileFollow(profileFollows, friendship.ReceiverId, friendship.SenderId, friendship.CreatedAt);
            }
        }

        return (friendships, profileFollows);
    }

    private static IList<ProfileFollow> GeneratePageFollows(
        IList<string> pageProfileIds, HashSet<(Guid, Guid)> profilesBlockEachOther)
    {
        var uniqueProfileFollows = new HashSet<(Guid, Guid)>(profilesBlockEachOther);
        IList<ProfileFollow> profileFollows = [];

        var faker = new Faker<ProfileFollow>()
            .CustomInstantiator(f =>
            {
                var followerId = Guid.Parse(f.PickRandom(pageProfileIds));
                return new ProfileFollow
                {
                    FollowerId = followerId,
                    CreatedBy = followerId,
                    CreatedAt = f.Date.Recent(f.Random.Int(1, 365))
                };
            })
            .RuleFor(f => f.FolloweeId, (f, profileFollow) =>
            {
                Guid followeeId;
                int attempts = 0;

                // Tạo receiverId hợp lệ
                do
                {
                    followeeId = Guid.Parse(f.PickRandom(pageProfileIds));
                    attempts++;
                } while ((followeeId == profileFollow.FollowerId ||
                          !uniqueProfileFollows.Add(NormalizePair(profileFollow.FollowerId, followeeId))) &&
                          attempts < 10);

                return followeeId;
            });

        for (int i = 0; i < 1000; i++)
        {
            var profileFollow = faker.Generate();
            if (profileFollow.FollowerId != profileFollow.FolloweeId)
                profileFollows.Add(profileFollow);
        }

        return profileFollows;
    }

    // Chuẩn hóa cặp để tránh trùng lặp hai chiều
    private static (Guid, Guid) NormalizePair(Guid id1, Guid id2)
        => id1.CompareTo(id2) < 0 ? (id1, id2) : (id2, id1);

    // Hàm thêm ProfileFollow để giảm trùng lặp
    private static void AddProfileFollow(IList<ProfileFollow> follows, Guid follower, Guid followee, DateTime createdAt)
    {
        follows.Add(new ProfileFollow
        {
            FollowerId = follower,
            FolloweeId = followee,
            CreatedBy = follower,
            CreatedAt = createdAt
        });
    }

    //private static (IList<ProfileBlock>, HashSet<(Guid, Guid)>) GenerateProfileBlocks(IList<string> profileIds)
    //{
    //    var uniqueInteractions = new HashSet<(Guid ProfileId, Guid RelateProfileId)>();
    //    IList<ProfileBlock> interactions = [];

    //    var faker = new Faker<ProfileBlock>()
    //        .CustomInstantiator(f =>
    //        {
    //            Guid randomProfileId;
    //            Guid randomRelateProfileId;
    //            do
    //            {
    //                randomProfileId = Guid.Parse(f.PickRandom(profileIds));
    //                randomRelateProfileId = Guid.Parse(f.PickRandom(profileIds));
    //            }

    //            while (uniqueInteractions
    //            .Contains((randomProfileId, randomRelateProfileId)) ||
    //                   randomProfileId == randomRelateProfileId);

    //            uniqueInteractions.Add((randomProfileId, randomRelateProfileId));

    //            return new ProfileBlock
    //            {
    //                BlockerId = randomProfileId,
    //                BlockeeId = randomRelateProfileId,
    //                CreatedBy = randomProfileId,
    //                CreatedAt = f.Date.Recent(f.Random.Int(1, 365))
    //            };
    //        });

    //    for (int i = 0; i < 200; i++)
    //    {
    //        var interaction = faker.Generate();
    //        interactions.Add(interaction);
    //    }

    //    return (interactions, uniqueInteractions);
    //}

    //private static (IList<Friendship>, IList<ProfileFollow>) GenerateFriendshipsWithFollows(
    //    IList<string> userProfileIds, HashSet<(Guid, Guid)> profilesBlockEachOther)
    //{
    //    var uniqueFriendships = profilesBlockEachOther;
    //    IList<Friendship> friendships = [];
    //    IList<ProfileFollow> profileFollows = [];

    //    var faker = new Faker<Friendship>()
    //        .CustomInstantiator(f =>
    //        {
    //            var randomProfileId = Guid.Parse(f.PickRandom(userProfileIds));
    //            return new Friendship
    //            {
    //                Status = f.PickRandom<FriendshipStatus>(),
    //                SenderId = randomProfileId,
    //                CreatedBy = randomProfileId,
    //                CreatedAt = f.Date.Recent(f.Random.Int(1, 365))
    //            };
    //        })
    //        .RuleFor(f => f.ReceiverId, (f, friendship) =>
    //        {
    //            Guid receiverId;
    //            int attempts = 0;
    //            do
    //            {
    //                receiverId = Guid.Parse(f.PickRandom(userProfileIds));
    //                attempts++;
    //                if (attempts > 10) break;

    //            } while (receiverId == friendship.SenderId ||
    //                     uniqueFriendships.Contains((friendship.SenderId, receiverId)) ||
    //                     uniqueFriendships.Contains((receiverId, friendship.SenderId)));

    //            if (receiverId != friendship.SenderId) 
    //                uniqueFriendships.Add((friendship.SenderId, receiverId));

    //            return receiverId;
    //        });

    //    for (int i = 0; i < 2000; i++)
    //    {
    //        var friendship = faker.Generate();
    //        if (friendship.SenderId != friendship.ReceiverId)
    //        {
    //            friendships.Add(friendship);

    //            profileFollows.Add(new ProfileFollow
    //            {
    //                FollowerId = friendship.SenderId,
    //                FolloweeId = friendship.ReceiverId,
    //                CreatedBy = friendship.SenderId,
    //                CreatedAt = friendship.CreatedAt
    //            });

    //            profileFollows.Add(new ProfileFollow
    //            {
    //                FollowerId = friendship.ReceiverId,
    //                FolloweeId = friendship.SenderId,
    //                CreatedBy = friendship.ReceiverId,
    //                CreatedAt = friendship.CreatedAt
    //            });
    //        }
    //    }

    //    return (friendships, profileFollows);
    //}

    //private static IList<ProfileFollow> GenerateProfileFollows(IList<string> profileIds)
    //{
    //    var uniqueInteractions = new HashSet<(Guid ProfileId, Guid RelateProfileId)>();
    //    IList<ProfileFollow> interactions = [];

    //    var faker = new Faker<ProfileFollow>()
    //        .CustomInstantiator(f =>
    //        {
    //            Guid randomProfileId;
    //            Guid randomRelateProfileId;
    //            do
    //            {
    //                randomProfileId = Guid.Parse(f.PickRandom(profileIds));
    //                randomRelateProfileId = Guid.Parse(f.PickRandom(profileIds));
    //            }

    //            while (uniqueInteractions
    //            .Contains((randomProfileId, randomRelateProfileId)) ||
    //                   randomProfileId == randomRelateProfileId);

    //            uniqueInteractions.Add((randomProfileId, randomRelateProfileId));

    //            return new ProfileFollow
    //            {
    //                FollowerId = randomProfileId,
    //                FolloweeId = randomRelateProfileId,
    //                CreatedBy = randomProfileId,
    //                CreatedAt = f.Date.Recent(f.Random.Int(1, 365))
    //            };
    //        });

    //    for (int i = 0; i < 1500; i++)
    //    {
    //        var interaction = faker.Generate();
    //        interactions.Add(interaction);
    //    }

    //    return interactions;
    //}

}
