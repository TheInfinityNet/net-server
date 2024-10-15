using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using InfinityNetServer.Services.Identity.Domain.Entities;
using InfinityNetServer.Services.Identity.Domain.Repositories;
using InfinityNetServer.Services.Identity.Infrastructure.Data;
using System;

namespace InfinityNetServer.Services.Identity.Infrastructure.Repositories
{
    public class VerificationRepository : SqlRepository<Verification, Guid>, IVerificationRepository
    {

        public VerificationRepository(IdentityDbContext context) : base(context)
        { }

        

    }
}
