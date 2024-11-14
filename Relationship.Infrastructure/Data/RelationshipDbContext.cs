using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.BuildingBlocks.Application.Contracts.Commands;
using InfinityNetServer.BuildingBlocks.Application.Services;
using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL;
using InfinityNetServer.Services.Relationship.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Relationship.Infrastructure.Data
{
    public class RelationshipDbContext(
        DbContextOptions<RelationshipDbContext> options,
        IConfiguration configuration,
        IAuthenticatedUserService authenticatedUserService,
        IMessageBus messageBus) 
        : PostreSqlDbContext<RelationshipDbContext>(options, configuration, authenticatedUserService)
    {

        public DbSet<Friendship> Friendships { get; set; }

        public DbSet<ProfileFollow> ProfileFollows { get; set; }

        public DbSet<ProfileBlock> ProfileBlocks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            var friendship = modelBuilder.Entity<Friendship>();
            var profileFollowers = modelBuilder.Entity<ProfileFollow>();
            var profileBlocks = modelBuilder.Entity<ProfileBlock>();

            friendship.HasIndex(f => new { f.SenderId, f.ReceiverId }).IsUnique();

            friendship.HasIndex(i => i.Status);

            friendship.HasIndex(i => i.CreatedAt);

            profileFollowers
                .HasIndex(i => new { i.FollowerId, i.FolloweeId })
                .IsUnique();

            profileFollowers.HasIndex(i => i.CreatedAt);

            profileBlocks
                .HasIndex(i => new { i.BlockerId, i.BlockeeId })
                .IsUnique();

            profileBlocks.HasIndex(i => i.CreatedAt);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var friendshipEntries = ChangeTracker.Entries<Friendship>();

            var profileFollowEntries = ChangeTracker.Entries<ProfileFollow>();

            int result = await base.SaveChangesAsync(cancellationToken);

            if (result > 0)
            {
                foreach (var entry in friendshipEntries)
                {
                    if (entry.Entity.UpdatedAt == null && entry.Entity.DeletedAt == null)
                    {
                        Guid id = entry.Entity.Id;
                        Guid senderId = entry.Entity.SenderId;
                        Guid receiverId = entry.Entity.ReceiverId;
                        DateTime createdAt = entry.Entity.CreatedAt;

                        // Friend invitation 
                        await messageBus.Publish(new DomainCommand.FriendshipNotificationCommand
                        {
                            Id = Guid.NewGuid(),
                            TriggeredBy = senderId.ToString(),
                            RelatedProfileId = receiverId,
                            FriendshipId = id,
                            Type = BuildingBlocks.Domain.Enums.NotificationType.FriendInvitation,
                            CreatedAt = createdAt
                        });

                    }
                }

                foreach (var entry in profileFollowEntries)
                {
                    if (entry.State == EntityState.Added)
                    {
                        Guid id = entry.Entity.Id;
                        Guid followerId = entry.Entity.FollowerId;
                        Guid followeeId = entry.Entity.FolloweeId;
                        DateTime createdAt = entry.Entity.CreatedAt;

                        // Friend invitation 
                        await messageBus.Publish(new DomainCommand.ProfileFollowNotificationCommand
                        {
                            Id = Guid.NewGuid(),
                            TriggeredBy = followerId.ToString(),
                            RelatedProfileId = followeeId,
                            ProfileFollowId = id,
                            Type = BuildingBlocks.Domain.Enums.NotificationType.NewProfileFollower,
                            CreatedAt = createdAt
                        });

                    }
                }
            }

            return result;
        }

    }

}
