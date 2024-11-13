using Bogus;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.Services.Profile.Domain.Entities;
using InfinityNetServer.Services.Profile.Domain.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Others;

namespace InfinityNetServer.Services.Profile.Infrastructure.Data;

public static class DbInitialization
{

    public static void DropDatabase(this IServiceProvider serviceProvider)
    {
        using var serviceScope = serviceProvider.CreateScope();

        var dbContext = serviceScope.ServiceProvider.GetService<ProfileDbContext>();

        dbContext.Database.EnsureDeleted();
    }

    public static void AutoMigration(this WebApplication webApplication)
    {
        using var serviceScope = webApplication.Services.CreateScope();

        var dbContext = serviceScope.ServiceProvider.GetRequiredService<ProfileDbContext>();

        if (!dbContext.Database.EnsureCreated()) return;

        dbContext.Database.MigrateAsync().Wait(); //generate all in folder Migration

    }

    public static async void SeedEssentialData(this IServiceProvider serviceProvider, int numberOfPageProfiles)
    {
        using var serviceScope = serviceProvider.CreateScope();
        serviceScope.ServiceProvider.GetService<ProfileDbContext>();
        var pageRepository = serviceScope.ServiceProvider.GetService<IPageProfileRepository>();
        var userProfileRepository = serviceScope.ServiceProvider.GetService<IUserProfileRepository>();
        var identityClient = serviceScope.ServiceProvider.GetService<CommonIdentityClient>();

        var existingPageProfileCount = await pageRepository.GetAllAsync();
        if (existingPageProfileCount.Count == 0)
        {
            IList<string> accountIds = await identityClient.GetAccountIds();
            IList<AccountWithDefaultProfile> accountWithDefaultProfiles = await identityClient.GetAccountsWithDefaultProfiles();

            var userProfiles = GenerateUserProfiles(accountWithDefaultProfiles);
            await userProfileRepository.CreateAsync(userProfiles);

            var pageProfiles = GeneratePageProfiles(numberOfPageProfiles, accountIds);
            await pageRepository.CreateAsync(pageProfiles);
        }
    }

    private static List<PageProfile> GeneratePageProfiles(int count, IList<string> accountIds)
    {
        var faker = new Faker<PageProfile>()
            .CustomInstantiator(f =>
            {
                var randomAccountId = Guid.Parse(f.PickRandom(accountIds));
                return new PageProfile
                {
                    Type = ProfileType.Page,
                    AccountId = randomAccountId,
                    CreatedBy = randomAccountId
                };
            })
            .RuleFor(p => p.AvatarId, f => Guid.NewGuid())
            .RuleFor(p => p.CoverId, f => Guid.NewGuid())
            .RuleFor(p => p.MobileNumber, f => f.Phone.PhoneNumber())
            .RuleFor(p => p.Location, f => f.Address.FullAddress())
            .RuleFor(p => p.Name, f => f.Company.CompanyName())
            .RuleFor(p => p.Description, f => f.Lorem.Sentence(30))
            .RuleFor(p => p.CreatedAt, f => f.Date.Recent(f.Random.Int(1, 365)));

        return faker.Generate(count);
    }

    private static IList<UserProfile> GenerateUserProfiles(
        IList<AccountWithDefaultProfile> accountWithDefaultProfileIds)
    {
        IList<UserProfile> userProfiles =  [];
        Faker faker = new ();

        foreach (var item in accountWithDefaultProfileIds)
        {

            UserProfile userProfile = new ()
            {
                Id = Guid.Parse(item.DefaultUserProfileId),
                Type = ProfileType.User,
                AccountId = Guid.Parse(item.Id),
                AvatarId = Guid.NewGuid(),
                CoverId = Guid.NewGuid(),
                CreatedBy = Guid.Parse(item.Id),
                Username = faker.Internet.UserName(),
                MobileNumber = faker.Phone.PhoneNumber(),
                Location = faker.Address.FullAddress(),
                FirstName = faker.Name.FirstName(),
                MiddleName = faker.Name.FirstName(),
                LastName = faker.Name.LastName(),
                Birthdate = GenerateRandomBirthDate(),
                Gender = faker.PickRandom<Gender>(),
                Bio = faker.Lorem.Sentence(50),
                CreatedAt = faker.Date.Recent(faker.Random.Int(1, 365))
            };
            userProfiles.Add(userProfile);

        }

        return userProfiles;
    }

    private static DateOnly GenerateRandomBirthDate()
    {
        // Calculate the latest allowed birthdate for someone who is 18 years old
        var today = DateOnly.FromDateTime(DateTime.Today);
        var latestBirthDate = today.AddYears(-18); // Subtract 18 years from today

        // Create a Faker for DateOnly, with a date range ensuring age > 18
        var faker = new Faker();

        var randomBirthDate = faker.Date
            .BetweenDateOnly(latestBirthDate.AddYears(-82), latestBirthDate); // Between 100 years ago and 18 years ago

        return randomBirthDate;
    }

}
