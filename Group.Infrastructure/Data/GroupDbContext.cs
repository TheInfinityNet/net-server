using InfinityNetServer.BuildingBlocks.Application.Services;
using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL;
using InfinityNetServer.Services.Group.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace InfinityNetServer.Services.Group.Infrastructure.Data
{
    public class GroupDbContext : PostreSqlDbContext<GroupDbContext>
    {

        public DbSet<Domain.Entities.Group> Groups { get; set; }

        public DbSet<GroupMember> GroupMembers { get; set; }

        public GroupDbContext(
            DbContextOptions<GroupDbContext> options,
            IConfiguration configuration,
            IAuthenticatedUserService authenticatedUserService) : base(options, configuration, authenticatedUserService)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var groupMember = modelBuilder.Entity<GroupMember>();
            groupMember
                .HasIndex(i => new { i.GroupId, i.ProfileId })
                .IsUnique();
            groupMember
                .HasOne(gm => gm.Group)
                .WithMany(g => g.Members)
                .HasForeignKey(gm => gm.GroupId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }

}
