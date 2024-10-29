using InfinityNetServer.BuildingBlocks.Application.Services;
using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

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
        }

    }

}
