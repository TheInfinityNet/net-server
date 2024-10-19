using InfinityNetServer.BuildingBlocks.Application.Services;
using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace InfinityNetServer.Services.Comment.Infrastructure.Data
{
    public class CommentDbContext : PostreSqlDbContext<CommentDbContext>
    {

        DbSet<Domain.Entities.Comment> comments { get; set; }

        public CommentDbContext(
            DbContextOptions<CommentDbContext> options,
            IConfiguration configuration,
            IAuthenticatedUserService authenticatedUserService) : base(options, configuration, authenticatedUserService)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var comment = modelBuilder.Entity<Domain.Entities.Comment>();

            comment
                .HasOne(c => c.ParentComment)
                .WithMany(c => c.repliesComments)
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.NoAction);
        }

    }

}
