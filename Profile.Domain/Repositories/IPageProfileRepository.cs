using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using InfinityNetServer.Services.Profile.Domain.Entities;

namespace InfinityNetServer.Services.Profile.Domain.Repositories
{
    public interface IPageProfileRepository
    {

        Task CreatePageProfilesAsync(IEnumerable<PageProfile> pageProfiles);

        Task<List<Guid>> GetAllPageProfileIdsAsync();

        Task<PageProfile> CreatePageProfileAsync(PageProfile pageProfile);

        Task UpdatePageProfileAsync(PageProfile pageProfile);

    }
}
