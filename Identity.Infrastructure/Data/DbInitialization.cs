using Bogus;
using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.Services.Identity.Domain.Entities;
using InfinityNetServer.Services.Identity.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Globalization;
using MassTransit;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Commands;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.Services.Identity.Domain.Repositories;

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
        var accountProviderRepository = serviceScope.ServiceProvider.GetService<IAccountProviderRepository>();
        var verificationRepository = serviceScope.ServiceProvider.GetService<IVerificationRepository>();
        var _publishEndpoint = serviceScope.ServiceProvider.GetService<IPublishEndpoint>();

        var existingAccountCount = await accountRepository.GetAllAccountIdsAsync();
        if (existingAccountCount.Count == 0)
        {
            var accounts = GenerateAccounts(numberOfAccounts);
            await accountRepository.CreateAccountsAsync(accounts);

            foreach (var account in accounts)
            {
                var accountProviders = GenerateAccountProviders(account, 2); 
                await accountProviderRepository.CreateAccountProvidersAsync(accountProviders);

                var verifications = GenerateVerifications(account, 2); 
                await verificationRepository.CreateVerificationsAsync(verifications);
            }
        }
    }

    private static List<Account> GenerateAccounts(int count)
    {
        var faker = new Faker<Account>()
            .RuleFor(a => a.DefaultUserProfile, f => Guid.NewGuid())
            .RuleFor(ap => ap.Email, f => f.Internet.Email())
            .RuleFor(ap => ap.Password, f => f.Internet.Password());

        return faker.Generate(count);
    }

    private static List<AccountProvider> GenerateAccountProviders(Account account, int count)
    {
        var faker = new Faker<AccountProvider>()
            .RuleFor(ap => ap.Account, account)
            .RuleFor(ap => ap.Type, f => f.PickRandom<ProviderType>());

        return faker.Generate(count);
    }

    private static List<Verification> GenerateVerifications(Account account, int count)
    {
        var faker = new Faker<Verification>()
            .RuleFor(v => v.Account, account)
            .RuleFor(v => v.Token, f => f.Random.AlphaNumeric(32))
            .RuleFor(v => v.OtpCode, f => f.Random.Number(100000, 999999).ToString())
            .RuleFor(v => v.Status, f => f.PickRandom<VerificationStatus>())
            .RuleFor(v => v.ExpiresAt, f => f.Date.Future());

        return faker.Generate(count);
    }

}
