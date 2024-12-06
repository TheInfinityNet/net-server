using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.Services.Profile.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Profile.Application.IServices
{
    public interface IPageProfileService
    {

        Task<PageProfile> GetById(string id);

        Task<PageProfile> GetByAccountId(string id);

        Task<IList<PageProfile>> GetAllByIds(IList<string> ids);

        Task<PageProfile> Update(PageProfile userProfile);

        Task<CursorPagedResult<PageProfile>> GetFollowingList(string profileId, string cursor, int pageSize);

        Task<CursorPagedResult<PageProfile>> GetFollowedList(string profileId, string cursor, int pageSize);

    }
}
