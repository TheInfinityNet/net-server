using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.BuildingBlocks.Application.Contracts.Commands;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Application.IServices;
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
            IMessageBus messageBus) : PostreSqlDbContext<PostDbContext, Guid>(options, configuration, authenticatedUserService)
    {

        public DbSet<Domain.Entities.Post> Posts { get; set; }

        public DbSet<PostAudience> PostAudiences { get; set; }

        public DbSet<PostAudienceInclude> PostAudienceIncludes { get; set; }

        public DbSet<PostAudienceExclude> PostAudienceExcludes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var post = modelBuilder.Entity<Domain.Entities.Post>();
            var postAudience = modelBuilder.Entity<PostAudience>();
            var postAudienceInclude = modelBuilder.Entity<PostAudienceInclude>();
            var postAudienceExclude = modelBuilder.Entity<PostAudienceExclude>();

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

            postAudience
                .HasOne(p => p.Post)
                .WithOne(post => post.Audience)
                .OnDelete(DeleteBehavior.Cascade);

            postAudience.HasIndex(pp => pp.PostId);
            postAudience.HasIndex(pp => pp.Type);

            postAudienceInclude
                .HasOne(p => p.Audience)
                .WithMany(pp => pp.Includes)
                .HasForeignKey(p => p.AudienceId)
                .OnDelete(DeleteBehavior.Cascade);

            postAudienceInclude.HasIndex(pp => pp.AudienceId);
            postAudienceInclude.HasIndex(pp => pp.ProfileId);

            postAudienceExclude
                .HasOne(p => p.Audience)
                .WithMany(pp => pp.Excludes)
                .HasForeignKey(p => p.AudienceId)
                .OnDelete(DeleteBehavior.Cascade);

            postAudienceExclude.HasIndex(pp => pp.AudienceId);
            postAudienceExclude.HasIndex(pp => pp.ProfileId);

        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var postEntries = ChangeTracker.Entries<Domain.Entities.Post>();
            var postAudienceEntries = ChangeTracker.Entries<PostAudience>();

            int result = await base.SaveChangesAsync(cancellationToken);

            if (result > 0)
            {
                foreach (var entry in postEntries)
                    await PublishPostNotificationCommands(entry.Entity);

                //foreach (var entry in postPrivacyEntries)
                //{
                //    var post = await Posts.FindAsync(entry.Entity.PostId);
                //    await PublishUserTimelineCommand(post);
                //}
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
            PostAudience privacy = entity.Audience;
            DateTime createdAt = entity.CreatedAt;

            IList<string> followerIds = await relationshipClient.GetAllFollowerIds(ownerId.ToString());
            IList<string> friendIds = await relationshipClient.GetAllFriendIds(ownerId.ToString());
            IList<string> blockerIds = await relationshipClient.GetAllBlockerIds(ownerId.ToString());
            IList<string> blockeeIds = await relationshipClient.GetAllBlockeeIds(ownerId.ToString());
            IList<string> includeIds = privacy.Includes.Select(i => i.ProfileId.ToString()).ToList();
            IList<string> excludeIds = privacy.Excludes.Select(i => i.ProfileId.ToString()).ToList();

            //foreach (var profileId in followerIds.Concat(friendIds).Concat(includeIds).Concat(excludeIds).Concat(blockerIds).Concat(blockeeIds).Distinct())
            //{
            //    Console.WriteLine(profileId);
            //}

            IList<Guid> whoCanSee = [];

            switch (privacy.Type)
            {
                case PostAudienceType.Include:
                    whoCanSee = includeIds.Select(Guid.Parse).ToList();
                    break;

                case PostAudienceType.Exclude:
                    whoCanSee = friendIds.Except(excludeIds).Select(Guid.Parse).ToList();
                    break;

                case PostAudienceType.Custom:
                    whoCanSee = friendIds.Concat(includeIds).Except(excludeIds).Select(Guid.Parse).ToList();
                    break;

                case PostAudienceType.Friends:
                    whoCanSee = friendIds.Select(Guid.Parse).ToList();
                    break;

                case PostAudienceType.OnlyMe:
                    whoCanSee = [ownerId];
                    break;
            }

            whoCanSee = whoCanSee
             .Except(blockerIds.Select(Guid.Parse))
             .Except(blockeeIds.Select(Guid.Parse))
             .Distinct()
             .ToList();

            foreach (var profileId in whoCanSee)
            {
                var timelineCommand = new DomainCommand.PushPostToTimelineCommand
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
