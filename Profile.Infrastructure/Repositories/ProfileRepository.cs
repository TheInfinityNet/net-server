using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories;
using InfinityNetServer.Services.Profile.Domain.Repositories;
using InfinityNetServer.Services.Profile.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Profile.Infrastructure.Repositories
{
    public class ProfileRepository(ProfileDbContext context) : SqlRepository<Domain.Entities.Profile, Guid>(context), IProfileRepository
    {

        public async Task<IList<Domain.Entities.Profile>> GetAllByTypeAsync(ProfileType type)
            => await context.Profiles.Where(p => p.Type == type).ToListAsync();

        public async Task<IList<Domain.Entities.Profile>> GetAllByIdsAsync(IEnumerable<Guid> ids)
            => await context.Profiles.Where(p => ids.Contains(p.Id)).ToListAsync();

        public async Task<IList<Domain.Entities.Profile>> GetPotentialByLocationAsync(string location, int limit = 100)
        {
            var sevenDaysAgo = DateTime.Now.AddDays(-7);  // Lấy thời gian cách đây 7 ngày
            var searchPattern = "%" + location + "%";  // Tạo mẫu chuỗi để tìm kiếm gần đúng

            return await context.Profiles
                .Where(p => EF.Functions.Like(p.Location, searchPattern) || p.CreatedAt >= sevenDaysAgo)
                .OrderByDescending(p => p.CreatedAt)  // Sắp xếp theo 'created_at' giảm dần
                .Take(limit)  // Giới hạn kết quả chỉ 100 bản ghi
                .ToListAsync();
        }


    }
}
