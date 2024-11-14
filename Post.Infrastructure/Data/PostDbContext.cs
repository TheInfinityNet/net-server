using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.BuildingBlocks.Application.Contracts.Commands;
using InfinityNetServer.BuildingBlocks.Application.Services;
using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL;
using InfinityNetServer.Services.Post.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Post.Infrastructure.Data
{
    public class PostDbContext(
            DbContextOptions<PostDbContext> options,
            IConfiguration configuration,
            IAuthenticatedUserService authenticatedUserService,
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
                .OnDelete(DeleteBehavior.NoAction);

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
                .WithMany(post => post.PostPrivacies)
                .HasForeignKey(p => p.PostId)
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
            var entries = ChangeTracker.Entries<Domain.Entities.Post>();

            int result = await base.SaveChangesAsync(cancellationToken);

            if (result > 0)
            {
                foreach (var entry in entries)
                {
                    if (entry.State == EntityState.Added || (entry.State == EntityState.Modified && entry.Entity.IsDeleted))
                    {
                        Guid id = entry.Entity.Id;
                        Guid ownerId = entry.Entity.OwnerId;
                        DateTime createdAt = entry.Entity.CreatedAt;

                        // tagged in comment
                        PostContent content = entry.Entity.Content;
                        if (content.TagFacets.Count > 0)
                        {
                            foreach (var tag in content.TagFacets)
                            {
                                Guid taggedProfileId = tag.ProfileId;
                                await messageBus.Publish(new DomainCommand.PostNotificationCommand
                                {
                                    Id = Guid.NewGuid(),
                                    TriggeredBy = ownerId.ToString(),
                                    RelatedProfileId = taggedProfileId,
                                    PostId = id,
                                    Type = BuildingBlocks.Domain.Enums.NotificationType.TaggedInPost,
                                    CreatedAt = createdAt
                                });
                            }
                        }
                    }

                }
            }
            return result;
        }
    }

}
