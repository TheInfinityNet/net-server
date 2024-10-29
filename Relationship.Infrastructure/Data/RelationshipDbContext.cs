using InfinityNetServer.BuildingBlocks.Application.Services;
using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL;
using InfinityNetServer.Services.Relationship.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace InfinityNetServer.Services.Relationship.Infrastructure.Data
{
    public class RelationshipDbContext(
        DbContextOptions<RelationshipDbContext> options,
        IConfiguration configuration,
        IAuthenticatedUserService authenticatedUserService) 
        : PostreSqlDbContext<RelationshipDbContext>(options, configuration, authenticatedUserService)
    {

        public DbSet<Friendship> Friendships { get; set; }

        public DbSet<Interaction> Interactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Interaction>()
                .HasIndex(i => new { UserProfileId = i.ProfileId, i.RelateProfileId })
                .IsUnique();

            var friendship = modelBuilder.Entity<Friendship>();

            friendship.HasIndex(f => new { f.SenderId, f.ReceiverId }).IsUnique();
            friendship
                .HasOne(f => f.Interaction)
                .WithOne(i => i.Friendship)
                .HasForeignKey<Interaction>(i => i.FriendshipId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }

}
