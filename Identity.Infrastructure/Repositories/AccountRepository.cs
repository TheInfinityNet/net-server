using InfinityNetServer.Services.Identity.Domain.Entities;
using InfinityNetServer.Services.Identity.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfinityNetServer.Services.Identity.Domain.Repositories;

namespace InfinityNetServer.Services.Identity.Infrastructure.Repositories
{
    public class AccountRepository : IAccountRepository
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
        public async Task<List<string>> GetAllAccountIdsAsync()
        {
            return await _context.Set<Account>()
                .Select(a => a.AccountId.ToString())  // Chỉ chọn Id
                .ToListAsync();
        }

        public async Task<List<Account>> GetAllAccountsAsync()
        {
            return await _context.Set<Account>().ToListAsync();
        }

        public async Task<Account> GetAccountByIdAsync(Guid id)
        {
            return await _context.Set<Account>().FindAsync(id);
        }

        public async Task<Account> GetAccountByEmailAsync(string email)
        {
            return await _context.Set<Account>().FirstOrDefaultAsync(a => a.Email == email);
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
            var existingAccount = await _context.Set<Account>().FindAsync(account.AccountId);
            if (existingAccount != null)
            {
                // Cập nhật thông tin
                _context.Entry(existingAccount).CurrentValues.SetValues(account);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Account with Id {account.AccountId} not found.");
            }
        }

    }
}
