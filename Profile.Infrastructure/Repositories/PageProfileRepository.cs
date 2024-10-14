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
    public class PageProfileRepository : IPageProfileRepository
    {
        private readonly ProfileDbContext _context;

        public PageProfileRepository(ProfileDbContext context)
        {
            _context = context;
        }

        public async Task CreatePageProfilesAsync(IEnumerable<PageProfile> pageProfiles)
        {
            await _context.Set<PageProfile>().AddRangeAsync(pageProfiles);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Guid>> GetAllPageProfileIdsAsync()
        {
            return await _context.Set<PageProfile>()
                .Select(a => a.Id)  // Chỉ chọn Id
                .ToListAsync();
        }

        public async Task<PageProfile> CreatePageProfileAsync(PageProfile pageProfile)
        {
            await _context.Set<PageProfile>().AddAsync(pageProfile);
            await _context.SaveChangesAsync();
            return pageProfile;
        }

        public async Task UpdatePageProfileAsync(PageProfile pageProfile)
        {
            var existingAccount = await _context.Set<PageProfile>().FindAsync(pageProfile.Id);
            if (existingAccount != null)
            {
                _context.Entry(existingAccount).CurrentValues.SetValues(pageProfile);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Account with Id {pageProfile.Id} not found.");
            }
        }

    }
}
