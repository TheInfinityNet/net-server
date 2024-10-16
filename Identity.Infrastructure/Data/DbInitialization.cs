using Bogus;
using InfinityNetServer.Services.Identity.Domain.Entities;
using InfinityNetServer.Services.Identity.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using MassTransit;
using InfinityNetServer.Services.Identity.Domain.Repositories;
using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.Services.Identity.Application.Helpers;

namespace InfinityNetServer.Services.Identity.Infrastructure.Data;

public static class DbInitialization
{

    public static void DropDatabase(this IServiceProvider serviceProvider)
    {
        using var serviceScope = serviceProvider.CreateScope();

        var dbContext = serviceScope.ServiceProvider.GetService<IdentityDbContext>();

        dbContext.Database.EnsureDeleted();
    }

    public static async void SeedEssentialData(this IServiceProvider serviceProvider, int numberOfAccounts)
    {
        using var serviceScope = serviceProvider.CreateScope();
        var dbContext = serviceScope.ServiceProvider.GetService<IdentityDbContext>();
        var accountRepository = serviceScope.ServiceProvider.GetService<IAccountRepository>();
        var accountProviderRepository = serviceScope.ServiceProvider.GetService<ILocalProviderRepository>();
        var verificationRepository = serviceScope.ServiceProvider.GetService<IVerificationRepository>();
        var _publishEndpoint = serviceScope.ServiceProvider.GetService<IPublishEndpoint>();

        var existingAccountCount = await accountRepository.GetAllAsync();
        if (existingAccountCount.Count == 0)
        {
            var accounts = GenerateAccounts(numberOfAccounts);
            await accountRepository.CreateAsync(accounts);

            foreach (var account in accounts)
            {
                var accountProviders = GenerateAccountProviders(account, 1); 
                await accountProviderRepository.CreateAsync(accountProviders);

                var verifications = GenerateVerifications(account, 2); 
                await verificationRepository.CreateAsync(verifications);
            }
        }
    }

    private static List<Account> GenerateAccounts(int count)
    {
        var faker = new Faker<Account>()
            .RuleFor(a => a.DefaultUserProfileId, f => Guid.NewGuid());

        return faker.Generate(count);
    }

    private static List<LocalProvider> GenerateAccountProviders(Account account, int count)
    {
        var faker = new Faker<LocalProvider>()
            .RuleFor(ap => ap.Account, account)
            .RuleFor(ap => ap.Email, f => f.Internet.Email())
            .RuleFor(ap => ap.PasswordHash, PasswordHelper.HashPassword("123456"))
            .RuleFor(ap => ap.Type, ProviderType.Local);

        return faker.Generate(count);
    }

    private static List<Verification> GenerateVerifications(Account account, int count)
    {
        var faker = new Faker<Verification>()
            .RuleFor(v => v.Account, account)
            .RuleFor(v => v.Token, f => f.Random.AlphaNumeric(32))
            .RuleFor(v => v.Code, f => f.Random.Number(100000, 999999).ToString())
            .RuleFor(v => v.ExpiresAt, f => f.Date.Future());

        return faker.Generate(count);
    }

}
