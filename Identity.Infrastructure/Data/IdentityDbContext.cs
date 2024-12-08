using InfinityNetServer.BuildingBlocks.Application.IServices;
using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL;
using InfinityNetServer.Services.Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace InfinityNetServer.Services.Identity.Infrastructure.Data
{
    public class IdentityDbContext(
            DbContextOptions<IdentityDbContext> options,
            IConfiguration configuration,
            IAuthenticatedUserService authenticatedUserService)
        : PostreSqlDbContext<IdentityDbContext, Guid>(options, configuration, authenticatedUserService)
    {

        public DbSet<Account> Accounts { get; set; }

        public DbSet<AccountProvider> AccountProviders { get; set; }

        public DbSet<ExternalProvider> GoogleProviders { get; set; }

        public DbSet<LocalProvider> LocalProviders { get; set; }

        public DbSet<Verification> Verifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            var account = modelBuilder.Entity<Account>();
            var accountProvider = modelBuilder.Entity<AccountProvider>();
            var verification = modelBuilder.Entity<Verification>();
            var localProvider = modelBuilder.Entity<LocalProvider>();
            var externalProvider = modelBuilder.Entity<ExternalProvider>();

            account
                .HasIndex(p => p.DefaultUserProfileId)
                .IsUnique();

            accountProvider
                .HasIndex(p => p.AccountId);

            accountProvider
                .HasOne(p => p.Account)
                .WithMany(b => b.AccountProviders)
                .HasForeignKey(p => p.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            externalProvider
                .HasIndex(p => p.UserId);

            externalProvider
                .HasIndex(p => p.ExternalName);

            externalProvider
                .HasOne(p => p.AccountProvider)
                .WithOne(b => b.GoogleProvider)
                .OnDelete(DeleteBehavior.Cascade);

            localProvider
                .HasIndex(p => p.Email)
                .IsUnique();

            localProvider
                .HasOne(p => p.AccountProvider)
                .WithOne(b => b.LocalProvider)
                .OnDelete(DeleteBehavior.Cascade);

            verification
                .HasIndex(p => p.Token)
                .IsUnique();

            verification
                .HasIndex(p => p.Code)
                .IsUnique();

            verification
                .HasIndex(p => p.AccountId);

            verification
                .HasOne(p => p.Account)
                .WithMany(b => b.Verifications)
                .HasForeignKey(p => p.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

        }

    }

}
