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

            var converter = new ValueConverter<Privacy, string>(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null), // Chuyển đối tượng MyData thành chuỗi JSON
                v => JsonSerializer.Deserialize<Privacy>(v, (JsonSerializerOptions)null) // Chuyển chuỗi JSON thành đối tượng MyData
            );

            post.Property(e => e.Privacy)
                .HasConversion(converter);
        }

    }

}
