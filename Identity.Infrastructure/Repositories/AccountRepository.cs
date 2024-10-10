using InfinityNetServer.Services.Identity.Domain.Entities;
using InfinityNetServer.Services.Identity.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Identity.Infrastructure.Repositories
{
    public class AccountRepository
    {
        private readonly IdentityDbContext _context;

        public AccountRepository(IdentityDbContext context)
        {
            _context = context;
        }

        // Thêm nhiều Account
        public async Task CreateAccountsAsync(IEnumerable<Account> accounts)
        {
            await _context.Set<Account>().AddRangeAsync(accounts);
            await _context.SaveChangesAsync();
        }

        // Lấy tất cả Account Id
        public async Task<List<Guid>> GetAllAccountIdsAsync()
        {
            return await _context.Set<Account>()
                .Select(a => a.Id)  // Chỉ chọn Id
                .ToListAsync();
        }

        // Thêm một Account
        public async Task<Account> CreateAccountAsync(Account account)
        {
            await _context.Set<Account>().AddAsync(account);
            await _context.SaveChangesAsync();
            return account; // Trả về account đã được thêm
        }

        // Cập nhật một Account
        public async Task UpdateAccountAsync(Account account)
        {
            // Kiểm tra xem account có tồn tại trong DbContext không
            var existingAccount = await _context.Set<Account>().FindAsync(account.Id);
            if (existingAccount != null)
            {
                // Cập nhật thông tin
                _context.Entry(existingAccount).CurrentValues.SetValues(account);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Account with Id {account.Id} not found.");
            }
        }

    }
}
