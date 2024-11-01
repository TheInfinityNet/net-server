using InfinityNetServer.BuildingBlocks.Application.Services;
using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL;
using InfinityNetServer.Services.Comment.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace InfinityNetServer.Services.Comment.Infrastructure.Data
{
    public class CommentDbContext(
        DbContextOptions<CommentDbContext> options,
        IConfiguration configuration,
        IAuthenticatedUserService authenticatedUserService) 
        : PostreSqlDbContext<CommentDbContext>(options, configuration, authenticatedUserService)
    {

        DbSet<Domain.Entities.Comment> Comments { get; set; }

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

    }

}
