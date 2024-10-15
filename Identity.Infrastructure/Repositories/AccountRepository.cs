using InfinityNetServer.Services.Identity.Domain.Entities;
using InfinityNetServer.Services.Identity.Infrastructure.Data;
using System;
using InfinityNetServer.Services.Identity.Domain.Repositories;
using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;

namespace InfinityNetServer.Services.Identity.Infrastructure.Repositories
{
    public class AccountRepository : SqlRepository<Account, Guid>, IAccountRepository
    {

        public AccountRepository(IdentityDbContext context) : base(context)
        { }



    }
}
