using InfinityNetServer.BuildingBlocks.Application.Services;
using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL;
using InfinityNetServer.Services.Tag.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;

namespace InfinityNetServer.Services.Tag.Infrastructure.Data
{
    public class TagDbContext(
        DbContextOptions<TagDbContext> options,
        IConfiguration configuration,
        IAuthenticatedUserService authenticatedUserService) 
        : PostreSqlDbContext<TagDbContext, Guid>(options, configuration, authenticatedUserService)
    {

        public DbSet<PostTag> PostTags { get; set; }

        public DbSet<CommentTag> CommentTags { get; set; }

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
