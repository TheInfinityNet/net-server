using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.BuildingBlocks.Application.Contracts.Commands;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Application.Services;
using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL;
using InfinityNetServer.Services.Post.Domain.Entities;
using InfinityNetServer.Services.Post.Domain.Enums;
using MassTransit.Initializers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Post.Infrastructure.Data
{
    public class PostDbContext(
            DbContextOptions<PostDbContext> options,
            IConfiguration configuration,
            IAuthenticatedUserService authenticatedUserService,
            CommonRelationshipClient relationshipClient,
            IMessageBus messageBus) : PostreSqlDbContext<PostDbContext>(options, configuration, authenticatedUserService)
    {

        public DbSet<Domain.Entities.Post> Posts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var post = modelBuilder.Entity<Domain.Entities.Post>();
            var postPrivacy = modelBuilder.Entity<PostPrivacy>();
            var postPrivacyInclude = modelBuilder.Entity<PostPrivacyInclude>();
            var postPrivacyExclude = modelBuilder.Entity<PostPrivacyExclude>();

            post
                .HasOne(p => p.Parent)
                .WithMany(post => post.SharedPosts)
                .HasForeignKey(p => p.ParentId)
                .OnDelete(DeleteBehavior.SetNull);

            post
                .HasOne(p => p.Presentation)
                .WithMany(post => post.SubPosts)
                .HasForeignKey(p => p.PresentationId)
                .OnDelete(DeleteBehavior.Cascade);

            var converter = new ValueConverter<PostContent, string>(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null), // Chuyển đối tượng MyData thành chuỗi JSON
                v => JsonSerializer.Deserialize<PostContent>(v, (JsonSerializerOptions)null) // Chuyển chuỗi JSON thành đối tượng MyData
            );

            post.Property(e => e.Content).HasConversion(converter);

            post.HasIndex(p => p.OwnerId);
            post.HasIndex(p => p.GroupId);
            post.HasIndex(p => p.Type);
            post.HasIndex(p => p.CreatedAt);

            postPrivacy
                .HasOne(p => p.Post)
                .WithOne(post => post.Privacy)
                .OnDelete(DeleteBehavior.Cascade);

            postPrivacy.HasIndex(pp => pp.PostId);
            postPrivacy.HasIndex(pp => pp.Type);

            postPrivacyInclude
                .HasOne(p => p.PostPrivacy)
                .WithMany(pp => pp.PostPrivacyIncludes)
                .HasForeignKey(p => p.PostPrivacyId)
                .OnDelete(DeleteBehavior.Cascade);

            postPrivacyInclude.HasIndex(pp => pp.PostPrivacyId);
            postPrivacyInclude.HasIndex(pp => pp.ProfileId);

            postPrivacyExclude
                .HasOne(p => p.PostPrivacy)
                .WithMany(pp => pp.PostPrivacyExcludes)
                .HasForeignKey(p => p.PostPrivacyId)
                .OnDelete(DeleteBehavior.Cascade);

            postPrivacyExclude.HasIndex(pp => pp.PostPrivacyId);
            postPrivacyExclude.HasIndex(pp => pp.ProfileId);

        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var postEntries = ChangeTracker.Entries<Domain.Entities.Post>();
            var postPrivacyEntries = ChangeTracker.Entries<PostPrivacy>();

            int result = await base.SaveChangesAsync(cancellationToken);

            if (result > 0)
            {
                foreach (var entry in postEntries)
                {
                    await PublishPostNotificationCommands(entry.Entity);
                }

                foreach (var entry in postPrivacyEntries)
                {
                    var post = await Posts.FindAsync(entry.Entity.PostId);
                    await PublishUserTimelineCommand(post);
                }
            }
            return result;
        }

        private async Task PublishPostNotificationCommands(Domain.Entities.Post entity)
        {
            Guid id = entity.Id;
            Guid ownerId = entity.OwnerId;
            DateTime createdAt = entity.CreatedAt;
            PostContent content = entity.Content;

            if (content.TagFacets.Count > 0)
            {
                foreach (var tag in content.TagFacets)
                {
                    Guid taggedProfileId = tag.ProfileId;

                    var notificationCommand = new DomainCommand.CreatePostNotificationCommand
                    {
                        Id = Guid.NewGuid(),
                        TriggeredBy = ownerId.ToString(),
                        TargetProfileId = taggedProfileId,
                        PostId = id,
                        Type = BuildingBlocks.Domain.Enums.NotificationType.TaggedInPost,
                        CreatedAt = createdAt
                    };

                    await messageBus.Publish(notificationCommand);
                }
            }
        }

        private async Task PublishUserTimelineCommand(Domain.Entities.Post entity)
        {
            Guid id = entity.Id;
            Guid ownerId = entity.OwnerId;
            PostPrivacy privacy = entity.Privacy;
            DateTime createdAt = entity.CreatedAt;

            IList<string> followerIds = await relationshipClient.GetFollowerIds(ownerId.ToString());
            IList<string> friendIds = await relationshipClient.GetFriendIds(ownerId.ToString());
            IList<string> blockerIds = await relationshipClient.GetBlockerIds(ownerId.ToString());
            IList<string> blockeeIds = await relationshipClient.GetBlockeeIds(ownerId.ToString());
            IList<string> includeIds = privacy.PostPrivacyIncludes.Select(i => i.ProfileId.ToString()).ToList();
            IList<string> excludeIds = privacy.PostPrivacyExcludes.Select(i => i.ProfileId.ToString()).ToList();

            //foreach (var profileId in followerIds.Concat(friendIds).Concat(includeIds).Concat(excludeIds).Concat(blockerIds).Concat(blockeeIds).Distinct())
            //{
            //    Console.WriteLine(profileId);
            //}

            IList<Guid> profileIds = [];

            switch (privacy.Type)
            {
                case PostPrivacyType.Public:
                    profileIds = (followerIds.Any() ? followerIds.Select(Guid.Parse) : [])
                        .Concat(friendIds.Any() ? friendIds.Select(Guid.Parse) : [])
                        .ToList();
                    break;

                case PostPrivacyType.Friends:
                    profileIds = friendIds.Any() ? friendIds.Select(Guid.Parse).ToList() : [];
                    break;

                case PostPrivacyType.OnlyMe:
                    profileIds = [ ownerId ];
                    break;
            }

            profileIds = profileIds
             .Concat(includeIds.Any() ? includeIds.Select(Guid.Parse) : [])
             .Except(excludeIds.Any() ? excludeIds.Select(Guid.Parse) : [])
             .Except(blockerIds.Any() ? blockerIds.Select(Guid.Parse) : [])
             .Except(blockeeIds.Any() ? blockeeIds.Select(Guid.Parse) : [])
             .Distinct()
             .ToList();

            foreach (var profileId in profileIds)
            {
                var timelineCommand = new DomainCommand.UpdateUserTimelineCommand
                {
                    ProfileId = profileId,
                    PostId = id,
                    CreatedAt = createdAt
                };

                await messageBus.Publish(timelineCommand);
            }
        }

    }

}
