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

            var friendship = modelBuilder.Entity<Friendship>();
            var interaction = modelBuilder.Entity<Interaction>();

            friendship.HasIndex(f => new { f.SenderId, f.ReceiverId }).IsUnique();

            friendship.HasIndex(i => i.Status);

            friendship.HasIndex(i => i.CreatedAt);

            friendship
                .HasOne(f => f.Interaction)
                .WithOne(i => i.Friendship)
                .HasForeignKey<Interaction>(i => i.FriendshipId)
                .OnDelete(DeleteBehavior.Cascade);

            interaction
                .HasIndex(i => new { i.ProfileId, i.RelateProfileId })
                .IsUnique();

            interaction.HasIndex(i => i.FriendshipId);

            interaction.HasIndex(i => i.Type);

            interaction.HasIndex(i => i.CreatedAt);
        }

    }

}
