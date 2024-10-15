using InfinityNetServer.BuildingBlocks.Application.Services;
using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace InfinityNetServer.Services.Relationship.Infrastructure.Data
{
    public class RelationshipDbContext : PostreSqlDbContext<RelationshipDbContext>
    {

        

        public RelationshipDbContext(
            DbContextOptions<RelationshipDbContext> options,
            IConfiguration configuration,
            IAuthenticatedUserService authenticatedUserService) : base(options, configuration, authenticatedUserService)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }

    }

}
