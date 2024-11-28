using Bogus;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfinityNetServer.Services.Group.Domain.Repositories;
using InfinityNetServer.Services.Group.Domain.Entities;
using InfinityNetServer.Services.Group.Domain.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;

namespace InfinityNetServer.Services.Group.Infrastructure.Data;

public static class DbInitialization
{

    public static void DropDatabase(this IServiceProvider serviceProvider)
    {
        using var serviceScope = serviceProvider.CreateScope();

        var dbContext = serviceScope.ServiceProvider.GetService<GroupDbContext>();

        dbContext.Database.EnsureDeleted();
    }

    public static void AutoMigration(this WebApplication webApplication)
    {
        using var serviceScope = webApplication.Services.CreateScope();

        var dbContext = serviceScope.ServiceProvider.GetRequiredService<GroupDbContext>();

        if (!dbContext.Database.EnsureCreated()) return;

        dbContext.Database.MigrateAsync().Wait(); //generate all in folder Migration

    }

    public static async void SeedEssentialData(this IServiceProvider serviceProvider, int numberOfGroups)
    {
        using var serviceScope = serviceProvider.CreateScope();
        serviceScope.ServiceProvider.GetService<GroupDbContext>();
        var groupRepository = serviceScope.ServiceProvider.GetService<IGroupRepository>();
        var groupMemberRepository = serviceScope.ServiceProvider.GetService<IGroupMemberRepository>();
        var profileClient = serviceScope.ServiceProvider.GetService<CommonProfileClient>();

        var existingGroupCount = await groupRepository.GetAllAsync();
        if (existingGroupCount.Count == 0)
        {
            var groups = await GenerateGroups(numberOfGroups, profileClient);
            await groupRepository.CreateAsync(groups);

            Faker faker = new Faker();
            var groupMembers = await GenerateGroupMembers(profileClient, groupRepository);
            await groupMemberRepository.CreateAsync(groupMembers);
        }
    }

    private static async Task<List<Domain.Entities.Group>> GenerateGroups(int count, CommonProfileClient profileClient)
    {
        var userProfileIds = await profileClient.GetUserProfileIds();

        var faker = new Faker<Domain.Entities.Group>()
            .CustomInstantiator(f =>
            {
                var randomUserProfile = f.PickRandom(userProfileIds);
                return new Domain.Entities.Group
                {
                    Name = f.Lorem.Sentence(3),
                    Description = f.Lorem.Sentence(50),
                    OwnerId = Guid.Parse(randomUserProfile),
                    CreatedBy = Guid.Parse(randomUserProfile)
                };
            });

        return faker.Generate(count);
    }

    private static async Task<List<GroupMember>> GenerateGroupMembers(CommonProfileClient profileClient, IGroupRepository groupRepository)
    {
        var userProfileIds = await profileClient.GetUserProfileIds();
        var groups = await groupRepository.GetAllAsync();

        var faker = new Faker();

        // Sử dụng HashSet để kiểm tra cặp profileId và groupId duy nhất
        var uniquePairs = new HashSet<(Guid UserProfileId, Guid GroupId)>();
        var groupMembers = new List<GroupMember>();

        foreach (var group in groups)
        {
            // Tạo số lượng thành viên ngẫu nhiên cho mỗi group
            var memberCount = faker.Random.Int(1, 10); // Mỗi group có từ 1 đến 10 members

            for (int i = 0; i < memberCount; i++)
            {
                var randomUserProfile = faker.PickRandom(userProfileIds);
                var randomGroup = faker.PickRandom(groups);

                var userProfileId = Guid.Parse(randomUserProfile);

                // Kiểm tra nếu cặp (profileId, groupId) đã tồn tại trong HashSet
                if (!uniquePairs.Contains((userProfileId, randomGroup.Id)))
                {
                    // Nếu chưa tồn tại, thêm cặp này vào HashSet và tạo GroupMember mới
                    uniquePairs.Add((userProfileId, randomGroup.Id));
                    var member = new GroupMember
                    {
                        Id = Guid.NewGuid(),
                        Role = faker.PickRandom<GroupMemberRole>(), // Enum for roles
                        Group = randomGroup,
                        UserProfileId = userProfileId,
                        CreatedBy = userProfileId
                    };
                    groupMembers.Add(member);
                }
            }
        }

        return groupMembers;
    }

}
