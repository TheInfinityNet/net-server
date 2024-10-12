using InfinityNetServer.BuildingBlocks.Application.Interfaces;
using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL;
using InfinityNetServer.Services.Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace InfinityNetServer.Services.Identity.Infrastructure.Data
{
    public class IdentityDbContext : PostreSqlDbContext<IdentityDbContext>
    {

        public DbSet<Account> Accounts { get; set; }

        public DbSet<AccountProvider> AccountProviders { get; set; }

        public DbSet<Verification> Verifications { get; set; }


        public IdentityDbContext(
            DbContextOptions<IdentityDbContext> options,
            IConfiguration configuration,
            IAuthenticatedUserService authenticatedUserService) : base(options, configuration, authenticatedUserService)
        {

        }

    }

}
