using InfinityNetServer.Services.Identity.Domain.Entities;
using InfinityNetServer.Services.Identity.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Identity.Infrastructure.Repositories
{
    public class AccountProviderRepository
    {
        private readonly IdentityDbContext _context;

        public AccountProviderRepository(IdentityDbContext context)
        {
            _context = context;
        }

        // Thêm nhiều AccountProvider
        public async Task CreateAccountProvidersAsync(IEnumerable<AccountProvider> accountProviders)
        {
            await _context.Set<AccountProvider>().AddRangeAsync(accountProviders);
            await _context.SaveChangesAsync();
        }

        // Lấy tất cả AccountProviders
        public async Task<List<AccountProvider>> GetAllAccountProvidersAsync()
        {
            return await _context.Set<AccountProvider>()
                .Include(ap => ap.Account)
                .ToListAsync();
        }

    }
}
