using Bogus;
using InfinityNetServer.Services.Identity.Domain.Entities;
using InfinityNetServer.Services.Identity.Domain.Enums;
using InfinityNetServer.Services.Identity.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

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
        var accountRepository = serviceScope.ServiceProvider.GetService<AccountRepository>();
        var accountProviderRepository = serviceScope.ServiceProvider.GetService<AccountProviderRepository>();
        var verificationRepository = serviceScope.ServiceProvider.GetService<VerificationRepository>();

        var existingAccountCount = await accountRepository.GetAllAccountIdsAsync();
        if (existingAccountCount.Count == 0)
        {
            var accounts = GenerateAccounts(numberOfAccounts);
            await accountRepository.CreateAccountsAsync(accounts);

            foreach (var account in accounts)
            {
                var accountProviders = GenerateAccountProviders(account.Id, 3); // Giả định mỗi account có 3 providers
                await accountProviderRepository.CreateAccountProvidersAsync(accountProviders);

                var verifications = GenerateVerifications(account.Id, 2); // Giả định mỗi account có 2 verifications
                await verificationRepository.CreateVerificationsAsync(verifications);
            }
        }
    }

    private static List<Account> GenerateAccounts(int count)
    {
        var faker = new Faker<Account>()
            .RuleFor(a => a.DefaultUserProfile, f => Guid.NewGuid());

        return faker.Generate(count);
    }

    private static List<AccountProvider> GenerateAccountProviders(Guid accountId, int count)
    {
        var faker = new Faker<AccountProvider>()
            .RuleFor(ap => ap.AccountId, accountId)
            .RuleFor(ap => ap.Type, f => f.PickRandom<ProviderType>()) // Giả định bạn đã định nghĩa enum AccountProviderType
            .RuleFor(ap => ap.Email, f => f.Internet.Email())
            .RuleFor(ap => ap.Password, f => f.Internet.Password());

        return faker.Generate(count);
    }

    private static List<Verification> GenerateVerifications(Guid accountId, int count)
    {
        var faker = new Faker<Verification>()
            .RuleFor(v => v.AccountId, accountId)
            .RuleFor(v => v.Token, f => f.Random.AlphaNumeric(32))
            .RuleFor(v => v.OtpCode, f => f.Random.Number(100000, 999999).ToString())
            .RuleFor(v => v.Status, f => f.PickRandom<VerificationStatus>()) // Giả định bạn đã định nghĩa enum VerificationStatus
            .RuleFor(v => v.ExpiresAt, f => f.Date.Future());

        return faker.Generate(count);
    }

}
