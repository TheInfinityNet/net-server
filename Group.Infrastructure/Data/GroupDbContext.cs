using InfinityNetServer.BuildingBlocks.Application.IServices;
using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL;
using InfinityNetServer.Services.Group.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace InfinityNetServer.Services.Group.Infrastructure.Data
{
    public class GroupDbContext(
        DbContextOptions<GroupDbContext> options,
        IConfiguration configuration,
        IAuthenticatedUserService authenticatedUserService)
        : PostreSqlDbContext<GroupDbContext, Guid>(options, configuration, authenticatedUserService)
    {

        public DbSet<Domain.Entities.Group> Groups { get; set; }

        public DbSet<GroupMember> GroupMembers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var groupMember = modelBuilder.Entity<GroupMember>();
            groupMember
                .HasIndex(i => new { i.GroupId, i.UserProfileId, i.CreatedBy })
                .IsUnique();
            groupMember
                .HasOne(gm => gm.Group)
                .WithMany(g => g.Members)
                .HasForeignKey(gm => gm.GroupId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }

}
