using InfinityNetServer.BuildingBlocks.Application.Interfaces;
using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL;
using InfinityNetServer.Services.Profile.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace InfinityNetServer.Services.Profile.Infrastructure.Data
{
    public class ProfileDbContext : PostreSqlDbContext<ProfileDbContext>
    {

        public DbSet<Domain.Entities.Profile> profiles { get; set; }

        public DbSet<UserProfile> userProfiles { get; set; }

        public DbSet<PageProfile> pageProfiles { get; set; }


        public ProfileDbContext(
            DbContextOptions<ProfileDbContext> options,
            IConfiguration configuration,
            IAuthenticatedUserService authenticatedUserService) : base(options, configuration, authenticatedUserService)
        {

        }

    }

}
