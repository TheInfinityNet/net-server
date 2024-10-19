using Bogus;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfinityNetServer.Services.Group.Domain.Repositories;
using InfinityNetServer.Services.Group.Domain.Entities;
using InfinityNetServer.Services.Group.Domain.Enums;

namespace InfinityNetServer.Services.Group.Infrastructure.Data;

public static class DbInitialization
{

    public static void DropDatabase(this IServiceProvider serviceProvider)
    {
        using var serviceScope = serviceProvider.CreateScope();

        var dbContext = serviceScope.ServiceProvider.GetService<GroupDbContext>();

        dbContext.Database.EnsureDeleted();
    }

    public static async void SeedEssentialData(this IServiceProvider serviceProvider, int numberOfGroups)
    {
        using var serviceScope = serviceProvider.CreateScope();
        var dbContext = serviceScope.ServiceProvider.GetService<GroupDbContext>();
        var groupRepository = serviceScope.ServiceProvider.GetService<IGroupRepository>();
        var groupMemberRepository = serviceScope.ServiceProvider.GetService<IGroupMemberRepository>();
        var identityClient = serviceScope.ServiceProvider.GetService<CommonIdentityClient>();
        var profileClient = serviceScope.ServiceProvider.GetService<CommonProfileClient>();

        var existingGroupCount = await groupRepository.GetAllAsync();
        if (existingGroupCount.Count == 0)
        {
            var groups = await GenerateGroups(numberOfGroups, identityClient);
            await groupRepository.CreateAsync(groups);

            Faker faker = new Faker();
            var groupMembers = await GenerateGroupMembers(identityClient, groupRepository);
            await groupMemberRepository.CreateAsync(groupMembers);
        }
    }

    private static async Task<List<Domain.Entities.Group>> GenerateGroups(
        int count,
        CommonIdentityClient identityClient)
    {
        var accountWithDefaultProfiles = await identityClient.GetAccountsWithDefaultProfiles();
        
        var faker = new Faker<Domain.Entities.Group>()
            .CustomInstantiator(f =>
            {
                var randomAccountWithDefaultProfile = f.PickRandom(accountWithDefaultProfiles);
                return new Domain.Entities.Group
                {
                    Name = f.Lorem.Sentence(3),
                    Description = f.Lorem.Sentence(50),
                    OwnerId = Guid.Parse(randomAccountWithDefaultProfile.DefaultUserProfileId),
                    CreatedBy = Guid.Parse(randomAccountWithDefaultProfile.Id)
                };
            });

        return faker.Generate(count);
    }

    private static async Task<List<GroupMember>> GenerateGroupMembers(CommonIdentityClient identityClient, IGroupRepository groupRepository)
    {
        var accountWithDefaultProfiles = await identityClient.GetAccountsWithDefaultProfiles();
        var groups = await groupRepository.GetAllAsync();

        var faker = new Faker();

        // Sử dụng HashSet để kiểm tra cặp profileId và groupId duy nhất
        var uniquePairs = new HashSet<(Guid ProfileId, Guid GroupId)>();
        var groupMembers = new List<GroupMember>();

        foreach (var group in groups)
        {
            // Tạo số lượng thành viên ngẫu nhiên cho mỗi group
            var memberCount = faker.Random.Int(1, 10); // Mỗi group có từ 1 đến 10 members

            for (int i = 0; i < memberCount; i++)
            {
                var randomAccountWithDefaultProfile = faker.PickRandom(accountWithDefaultProfiles);
                var randomGroup = faker.PickRandom(groups);

                var userProfileId = Guid.Parse(randomAccountWithDefaultProfile.DefaultUserProfileId);

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
                        CreatedBy = Guid.Parse(randomAccountWithDefaultProfile.Id)
                    };
                    groupMembers.Add(member);
                }
            }
        }

        return groupMembers;
    }

}
