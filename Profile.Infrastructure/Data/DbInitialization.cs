using Bogus;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.Services.Profile.Domain.Entities;
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
        var identityClient = serviceScope.ServiceProvider.GetService<IdentityClient>();

        var existingPageProfileCount = await pageRepository.GetAllAsync();
        if (existingPageProfileCount.Count == 0)
        {
            var profiles = await GeneratePageProfiles(numberOfProfiles, identityClient);
            await pageRepository.CreateAsync(profiles);
        }
    }

    private static async Task<List<PageProfile>> GeneratePageProfiles(int count, IdentityClient identityClient)
    {
        var accountIds = await identityClient.GetAccountIds();
        var faker = new Faker<PageProfile>()
            .RuleFor(a => a.Picture, f => f.Image.PicsumUrl())
            .RuleFor(a => a.CoverPicture, f => f.Image.PicsumUrl())
            .RuleFor(ap => ap.Name, f => f.Company.CompanyName())
            .RuleFor(ap => ap.OwnerId, f => Guid.Parse(f.PickRandom(accountIds)))
            .RuleFor(ap => ap.CreatedBy, f => f.PickRandom(accountIds))
            .RuleFor(ap => ap.UpdatedBy, f => f.PickRandom(accountIds))
            .RuleFor(ap => ap.Description, f => f.Lorem.Sentence());

        return faker.Generate(count);
    }

}
