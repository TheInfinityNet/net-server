using Bogus;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.Services.Profile.Domain.Entities;
using InfinityNetServer.Services.Profile.Domain.Enums;
using InfinityNetServer.Services.Profile.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Profile.Infrastructure.Data;

public static class DbInitialization
{

    public static void DropDatabase(this IServiceProvider serviceProvider)
    {
        using var serviceScope = serviceProvider.CreateScope();

        var dbContext = serviceScope.ServiceProvider.GetService<ProfileDbContext>();

        dbContext.Database.EnsureDeleted();
    }

    public static async void SeedEssentialData(this IServiceProvider serviceProvider, int numberOfProfiles)
    {
        using var serviceScope = serviceProvider.CreateScope();
        var dbContext = serviceScope.ServiceProvider.GetService<ProfileDbContext>();
        var pageRepository = serviceScope.ServiceProvider.GetService<IPageProfileRepository>();
        var userProfileRepository = serviceScope.ServiceProvider.GetService<IUserProfileRepository>();
        var identityClient = serviceScope.ServiceProvider.GetService<CommonIdentityClient>();

        var existingPageProfileCount = await pageRepository.GetAllAsync();
        if (existingPageProfileCount.Count == 0)
        {
            var userProfiles = await GenerateUserProfiles(identityClient);
            await userProfileRepository.CreateAsync(userProfiles);

            var profiles = await GeneratePageProfiles(numberOfProfiles, identityClient);
            await pageRepository.CreateAsync(profiles);
        }
    }

    private static async Task<List<PageProfile>> GeneratePageProfiles(int count, CommonIdentityClient identityClient)
    {
        var accountIds = await identityClient.GetAccountIds();
        var faker = new Faker<PageProfile>()
            .RuleFor(ap => ap.Name, f => f.Company.CompanyName())
            .CustomInstantiator(f =>
            {
                var randomAccountId = Guid.Parse(f.PickRandom(accountIds));
                return new PageProfile
                {
                    Type = ProfileType.Page,
                    OwnerId = randomAccountId,
                    CreatedBy = randomAccountId
                };
            })
            .RuleFor(ap => ap.Description, f => f.Lorem.Sentence());

        return faker.Generate(count);
    }

    private static async Task<List<UserProfile>> GenerateUserProfiles(CommonIdentityClient identityClient)
    {
        var accountWithProfileIds = await identityClient.GetAccountWithDefaultProfileIds();
        List<UserProfile> userProfiles = new List<UserProfile>();
        Faker faker = new Faker();

        foreach (var item in accountWithProfileIds)
        {

            UserProfile userProfile = new UserProfile
            {
                Id = Guid.Parse(item.DefaultUserProfileId),
                Type = ProfileType.User,
                AccountId = Guid.Parse(item.Id),
                CreatedBy = Guid.Parse(item.Id),
                Username = faker.Internet.UserName(),
                MobileNumber = faker.Phone.PhoneNumber(),
                FirstName = faker.Name.FirstName(),
                MiddleName = faker.Name.FirstName(),
                LastName = faker.Name.LastName(),
                Birthdate = new DateOnly(2000, 1, 1),
                Gender = faker.PickRandom<Gender>(),
                Bio = faker.Lorem.Sentence(100)
            };
            userProfiles.Add(userProfile);

        }

        return userProfiles;
    }

}
