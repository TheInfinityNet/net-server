using InfinityNetServer.BuildingBlocks.Application.Services;
using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL;
using InfinityNetServer.Services.Tag.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace InfinityNetServer.Services.File.Infrastructure.Data
{
    public class FileDbContext : PostreSqlDbContext<FileDbContext>
    {

        DbSet<PostTag> PostTags { get; set; }

        DbSet<CommentTag> CommentTags { get; set; }

        public FileDbContext(
            DbContextOptions<FileDbContext> options,
            IConfiguration configuration,
            IAuthenticatedUserService authenticatedUserService) : base(options, configuration, authenticatedUserService)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PostTag>()
                .HasIndex(pt => new { pt.PostId, pt.TaggedProfileId })
                .IsUnique();

            modelBuilder.Entity<CommentTag>()
                .HasIndex(ct => new { ct.CommentId, ct.TaggedProfileId })
                .IsUnique();
        }

    }

}
