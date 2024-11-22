using InfinityNetServer.BuildingBlocks.Application.Services;
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

        public DbSet<GoogleProvider> GoogleProviders { get; set; }

        public DbSet<FacebookProvider> FacebookProviders { get; set; }

        public DbSet<LocalProvider> LocalProviders { get; set; }

        public DbSet<Verification> Verifications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Account>()
                .HasIndex(p => p.DefaultUserProfileId)
                .IsUnique();

            modelBuilder.Entity<LocalProvider>()
                .HasIndex(p => p.Email)
                .IsUnique();

            modelBuilder.Entity<AccountProvider>()
                .HasOne(p => p.Account)
                .WithMany(b => b.AccountProviders)
                .HasForeignKey(p => p.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Verification>()
                .HasOne(p => p.Account)
                .WithMany(b => b.Verifications)
                .HasForeignKey(p => p.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GoogleProvider>()
                .HasOne(p => p.AccountProvider)
                .WithOne(b => b.GoogleProvider)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FacebookProvider>()
                .HasOne(p => p.AccountProvider)
                .WithOne(b => b.FacebookProvider)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<LocalProvider>()
                .HasOne(p => p.AccountProvider)
                .WithOne(b => b.LocalProvider)
                .OnDelete(DeleteBehavior.Cascade);

        }

    }

}
