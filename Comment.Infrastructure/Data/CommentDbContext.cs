using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.BuildingBlocks.Application.Contracts.Commands;
using InfinityNetServer.BuildingBlocks.Application.Services;
using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL;
using InfinityNetServer.Services.Comment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Comment.Infrastructure.Data
{
    public class CommentDbContext(
        DbContextOptions<CommentDbContext> options,
        IConfiguration configuration,
        IAuthenticatedUserService authenticatedUserService,
        IMessageBus messageBus)
        : PostreSqlDbContext<CommentDbContext, Guid>(options, configuration, authenticatedUserService)
    {

        public DbSet<Domain.Entities.Comment> Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var comment = modelBuilder.Entity<Domain.Entities.Comment>();

            comment
                .HasOne(c => c.ParentComment)
                .WithMany(c => c.RepliesComments)
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.NoAction);

            var converter = new ValueConverter<CommentContent, string>(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null), // Chuyển đối tượng MyData thành chuỗi JSON
                v => JsonSerializer.Deserialize<CommentContent>(v, (JsonSerializerOptions)null) // Chuyển chuỗi JSON thành đối tượng MyData
            );

            comment.Property(e => e.Content).HasConversion(converter);

            comment.HasIndex(c => c.ProfileId);
            comment.HasIndex(c => c.PostId);
            comment.HasIndex(c => c.ParentId);
            comment.HasIndex(c => c.CreatedAt);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<Domain.Entities.Comment>();

            int result = await base.SaveChangesAsync(cancellationToken);

            if (result > 0)
            {
                foreach (var entry in entries)
                {
                    Guid id = entry.Entity.Id;
                    Guid profileId = entry.Entity.ProfileId;
                    DateTime createdAt = entry.Entity.CreatedAt;

                    // tagged in comment
                    CommentContent content = entry.Entity.Content;
                    if (content.TagFacets.Count > 0)
                    {
                        foreach (var tag in content.TagFacets)
                        {
                            Guid taggedProfileId = tag.ProfileId;
                            await messageBus.Publish(new DomainCommand.CreateCommentNotificationCommand
                            {
                                TriggeredBy = profileId.ToString(),
                                TargetProfileId = taggedProfileId,
                                CommentId = id,
                                Type = BuildingBlocks.Domain.Enums.NotificationType.TaggedInComment,
                                CreatedAt = createdAt
                            });
                        }
                    }

                    // reply to comment
                    if (entry.Entity.ParentId != null)
                    {
                        Guid parentCommentId = entry.Entity.ParentId.Value;
                        Domain.Entities.Comment parentComment = await Comments.FindAsync(parentCommentId);
                        await messageBus.Publish(new DomainCommand.CreateCommentNotificationCommand
                        {
                            TriggeredBy = profileId.ToString(),
                            TargetProfileId = parentComment.ProfileId,
                            CommentId = id,
                            Type = BuildingBlocks.Domain.Enums.NotificationType.ReplyToComment,
                            CreatedAt = createdAt
                        });
                    }
                }
            }
            return result;
        }

    }

}
