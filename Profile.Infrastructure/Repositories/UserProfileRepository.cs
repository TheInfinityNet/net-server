using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfinityNetServer.Services.Profile.Infrastructure.Data;
using InfinityNetServer.Services.Profile.Domain.Entities;
using InfinityNetServer.Services.Profile.Domain.Repositories;

namespace InfinityNetServer.Services.Profile.Infrastructure.Repositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly ProfileDbContext _context;

        public UserProfileRepository(ProfileDbContext context)
        {
            _context = context;
        }

        // Thêm nhiều Account
        public async Task CreateUserProfilesAsync(IEnumerable<UserProfile> userProfiles)
        {
            await _context.Set<UserProfile>().AddRangeAsync(userProfiles);
            await _context.SaveChangesAsync();
        }

        // Lấy tất cả Account Id
        public async Task<List<Guid>> GetAllUserProfileIdsAsync()
        {
            return await _context.Set<UserProfile>()
                .Select(a => a.Id)  // Chỉ chọn Id
                .ToListAsync();
        }

        public async Task<UserProfile> GetUserProfileByIdAsync(Guid id)
        {
            return await _context.Set<UserProfile>().FindAsync(id);
        }

        // Thêm một Account
        public async Task<UserProfile> CreateUserProfileAsync(UserProfile userProfile)
        {
            await _context.Set<UserProfile>().AddAsync(userProfile);
            await _context.SaveChangesAsync();
            return userProfile; // Trả về account đã được thêm
        }

        // Cập nhật một Account
        public async Task UpdateUserProfileAsync(UserProfile userProfile)
        {
            // Kiểm tra xem account có tồn tại trong DbContext không
            var existingAccount = await _context.Set<PageProfile>().FindAsync(userProfile.Id);
            if (existingAccount != null)
            {
                // Cập nhật thông tin
                _context.Entry(existingAccount).CurrentValues.SetValues(userProfile);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Account with Id {userProfile.Id} not found.");
            }
        }

    }
}
