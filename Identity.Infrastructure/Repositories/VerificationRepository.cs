using InfinityNetServer.Services.Identity.Domain.Entities;
using InfinityNetServer.Services.Identity.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Identity.Infrastructure.Repositories
{
    public class VerificationRepository
    {
        private readonly IdentityDbContext _context;

        public VerificationRepository(IdentityDbContext context)
        {
            _context = context;
        }

        // Thêm nhiều Verification
        public async Task CreateVerificationsAsync(IEnumerable<Verification> verifications)
        {
            await _context.Set<Verification>().AddRangeAsync(verifications);
            await _context.SaveChangesAsync();
        }

        // Lấy tất cả Verifications
        public async Task<List<Verification>> GetAllVerificationsAsync()
        {
            return await _context.Set<Verification>()
                .Include(v => v.Account)
                .ToListAsync();
        }

    }
}
