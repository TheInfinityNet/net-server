using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Domain.Specifications;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.Services.Profile.Application.Services;
using InfinityNetServer.Services.Profile.Domain.Entities;
using InfinityNetServer.Services.Profile.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Profile.Presentation.Services
{
    public class PageProfileService(
        IPageProfileRepository _pageProfileRepository,
        ILogger<PageProfileService> _logger,
        CommonRelationshipClient relationshipClient
        ) : IPageProfileService
    {
        public async Task<CursorPagedResult<PageProfile>> GetBlockedList(string profileId, string cursor, int pageSize)
        {
            var profile = await GetById(profileId);
            IList<string> blockeeIds = await relationshipClient.GetBlockeeIds(profileId.ToString());

            var specification = new SpecificationWithCursor<PageProfile>
            {
                Criteria = userProfile =>
                        blockeeIds.Contains(userProfile.Id.ToString()),
                OrderFields = [
                        new OrderField<PageProfile>
                        {
                            Field = x => x.CreatedAt,
                            Direction = SortDirection.Descending
                        }
                    ],
                Cursor = cursor,
                PageSize = pageSize
            };
            return await _pageProfileRepository.GetPagedAsync(specification);
        }

        public async Task<CursorPagedResult<PageProfile>> GetFollowedList(string profileId, string cursor, int pageSize)
        {
            var profile = await GetById(profileId);
            IList<string> followerIds = await relationshipClient.GetFollowerIds(profileId);

            var specification = new SpecificationWithCursor<PageProfile>
            {
                Criteria = userProfile =>
                        followerIds.Contains(userProfile.Id.ToString()),
                OrderFields = [
                        new OrderField<PageProfile>
                        {
                            Field = x => x.CreatedAt,
                            Direction = SortDirection.Descending
                        }
                    ],
                Cursor = cursor,
                PageSize = pageSize
            };
            return await _pageProfileRepository.GetPagedAsync(specification);
        }

        public async Task<CursorPagedResult<PageProfile>> GetFollowingList(string profileId, string cursor, int pageSize)
        {
            var profile = await GetById(profileId);
            IList<string> followeeIds = await relationshipClient.GetFolloweeIds(profileId);

            var specification = new SpecificationWithCursor<PageProfile>
            {
                Criteria = userProfile =>
                        followeeIds.Contains(userProfile.Id.ToString()),
                OrderFields = [
                        new OrderField<PageProfile>
                        {
                            Field = x => x.CreatedAt,
                            Direction = SortDirection.Descending
                        }
                    ],
                Cursor = cursor,
                PageSize = pageSize
            };
            return await _pageProfileRepository.GetPagedAsync(specification);
        }
       
        public async Task<PageProfile> GetByAccountId(string id)
            => await _pageProfileRepository.GetByAccountIdAsync(Guid.Parse(id));

        public async Task<PageProfile> GetById(string id)
            => await _pageProfileRepository.GetByIdAsync(Guid.Parse(id));

        public async Task<IList<PageProfile>> GetAllByIds(IList<string> ids)
            => await _pageProfileRepository.GetAllByIdsAsync(ids.Select(Guid.Parse).ToList());

        public async Task<PageProfile> Update(PageProfile pageProfile)
        {
            PageProfile existedProfile = await GetById(pageProfile.Id.ToString()) 
                ?? throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            existedProfile.Name = pageProfile.Name;
            existedProfile.Description = pageProfile.Description;
            existedProfile.MobileNumber = pageProfile.MobileNumber;

            await _pageProfileRepository.UpdateAsync(existedProfile);

            return existedProfile;
        }
    }
}
