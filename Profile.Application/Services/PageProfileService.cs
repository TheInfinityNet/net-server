using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Domain.Specifications;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.Services.Profile.Application.IServices;
using InfinityNetServer.Services.Profile.Domain.Entities;
using InfinityNetServer.Services.Profile.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Profile.Application.Services
{
    public class PageProfileService(
        IPageProfileRepository pageProfileRepository,
        ILogger<PageProfileService> logger,
        CommonRelationshipClient relationshipClient
        ) : IPageProfileService
    {
        public async Task<CursorPagedResult<PageProfile>> GetBlockedList(string profileId, string cursor, int pageSize)
        {
            var profile = await GetById(profileId);
            IList<string> blockeeIds = await relationshipClient.GetAllBlockeeIds(profileId.ToString());

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
                Limit = pageSize
            };
            return await pageProfileRepository.GetPagedAsync(specification);
        }

        public async Task<CursorPagedResult<PageProfile>> GetFollowedList(string profileId, string cursor, int pageSize)
        {
            var profile = await GetById(profileId);
            IList<string> followerIds = await relationshipClient.GetAllFollowerIds(profileId);

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
                Limit = pageSize
            };
            return await pageProfileRepository.GetPagedAsync(specification);
        }

        public async Task<CursorPagedResult<PageProfile>> GetFollowingList(string profileId, string cursor, int pageSize)
        {
            var profile = await GetById(profileId);
            IList<string> followeeIds = await relationshipClient.GetAllFolloweeIds(profileId);

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
                Limit = pageSize
            };
            return await pageProfileRepository.GetPagedAsync(specification);
        }

        public async Task<PageProfile> GetByAccountId(string id)
            => await pageProfileRepository.GetByAccountIdAsync(Guid.Parse(id));

        public async Task<PageProfile> GetById(string id)
            => await pageProfileRepository.GetByIdAsync(Guid.Parse(id));

        public async Task<IList<PageProfile>> GetAllByIds(IList<string> ids)
            => await pageProfileRepository.GetAllByIdsAsync(ids.Select(Guid.Parse).ToList());

        public async Task<PageProfile> Update(PageProfile pageProfile)
        {
            //PageProfile existedProfile = await GetById(pageProfile.Id.ToString())
            //    ?? throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            //existedProfile.AvatarId = pageProfile.AvatarId;
            //existedProfile.CoverId = pageProfile.CoverId;
            //existedProfile.Location = pageProfile.Location;
            //existedProfile.MobileNumber = pageProfile.MobileNumber;
            //existedProfile.Status = pageProfile.Status;

            //existedProfile.Name = pageProfile.Name;
            //existedProfile.Description = pageProfile.Description;
            logger.LogInformation("Update page profile");
            return await pageProfileRepository.UpdateAsync(pageProfile);
        }
    }
}
