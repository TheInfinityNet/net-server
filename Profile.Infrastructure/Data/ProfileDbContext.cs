﻿using InfinityNetServer.BuildingBlocks.Application.Services;
using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL;
using InfinityNetServer.Services.Profile.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace InfinityNetServer.Services.Profile.Infrastructure.Data
{
    public class ProfileDbContext : PostreSqlDbContext<ProfileDbContext>
    {

        public DbSet<Domain.Entities.Profile> Profiles { get; set; }

        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<PageProfile> PageProfiles { get; set; }


        public ProfileDbContext(
            DbContextOptions<ProfileDbContext> options,
            IConfiguration configuration,
            IAuthenticatedUserService authenticatedUserService) : base(options, configuration, authenticatedUserService)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var userProfile = modelBuilder.Entity<UserProfile>();
            var profile = modelBuilder.Entity<Domain.Entities.Profile>();
            var pageProfile = modelBuilder.Entity<PageProfile>();

            profile.HasIndex(p => p.MobileNumber).IsUnique();
            profile.HasIndex(p => p.AccountId);
            profile.HasIndex(p => p.Type);
            profile.HasIndex(p => p.Status);

            userProfile.HasIndex(p => p.Username).IsUnique();
            //userProfile.HasIndex(p => new { p.Username, p.MobileNumber }).IsUnique();

            userProfile
                .HasOne(p => p.Profile)
                .WithOne(b => b.UserProfile)
                .OnDelete(DeleteBehavior.Cascade);

            pageProfile.HasIndex(p => p.Name).IsUnique();

            pageProfile
                .HasOne(p => p.Profile)
                .WithOne(b => b.PageProfile)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }

}
