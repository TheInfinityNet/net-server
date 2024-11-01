using System.Text.Json;
using InfinityNetServer.BuildingBlocks.Application.Services;
using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL;
using InfinityNetServer.Services.Post.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;

namespace InfinityNetServer.Services.Post.Infrastructure.Data
{
    public class PostDbContext : PostreSqlDbContext<PostDbContext>
    {

        DbSet<Domain.Entities.Post> Posts { get; set; }

        public PostDbContext(
            DbContextOptions<PostDbContext> options,
            IConfiguration configuration,
            IAuthenticatedUserService authenticatedUserService) : base(options, configuration, authenticatedUserService)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var post = modelBuilder.Entity<Domain.Entities.Post>();
            var postPrivacy = modelBuilder.Entity<PostPrivacy>();

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


        }

    }

}
