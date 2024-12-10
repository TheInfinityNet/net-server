using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.BuildingBlocks.Application.Contracts.Events;
using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.BuildingBlocks.Application.GrpcClients;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using InfinityNetServer.Services.Profile.Application.IServices;
using InfinityNetServer.Services.Profile.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Profile.Application.Services
{
    public class ProfileService(
        IProfileRepository profileRepository,
        CommonRelationshipClient relationshipClient,
        IMessageBus messageBus,
        ILogger<ProfileService> logger) : IProfileService
    {

        public async Task<Domain.Entities.Profile> GetById(string id)
            => await profileRepository.GetByIdAsync(Guid.Parse(id));

        public async Task<IList<Domain.Entities.Profile>> GetAllByIds(IList<string> ids)
            => await profileRepository.GetAllByIdsAsync(ids.Select(Guid.Parse));

        public async Task<IList<Domain.Entities.Profile>> GetAll()
            => await profileRepository.GetAllAsync();

        public Task<IList<Domain.Entities.Profile>> GetAllByType(ProfileType type)
            => profileRepository.GetAllByTypeAsync(type);

        public Task<IList<Domain.Entities.Profile>> GetPotentialByLocation(string location, int? limit)
            => profileRepository.GetPotentialByLocationAsync(location, limit.Value);

        public async Task<Domain.Entities.Profile> Update(Domain.Entities.Profile profile)
        {
            //Domain.Entities.Profile existedProfile = await GetById(profile.Id.ToString())
            //    ?? throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            //existedProfile.AvatarId = profile.AvatarId;
            //existedProfile.CoverId = profile.CoverId;
            //existedProfile.Location = profile.Location;
            //existedProfile.MobileNumber = profile.MobileNumber;
            //existedProfile.Status = profile.Status;
            return await profileRepository.UpdateAsync(profile);
        }

        public async Task ConfirmSave(string id, string fileMetadataId, bool isAvatar)
        {
            Domain.Entities.Profile profile = await GetById(id)
                ?? throw new BaseException(BaseError.POST_NOT_FOUND, StatusCodes.Status404NotFound);

            Guid? fileMetadataGuid = (isAvatar ? profile.AvatarId : profile.CoverId) 
                ?? throw new BaseException(BaseError.FILE_NOT_FOUND, StatusCodes.Status404NotFound);

            await messageBus.Publish(new DomainEvent.CreatePhotoMetadataEvent
            {
                FileMetadataId = fileMetadataGuid.Value,
                TempId = Guid.Parse(fileMetadataId),
                OwnerId = Guid.Parse(id),
                OwnerType = FileOwnerType.Post,
                UpdatedAt = DateTime.Now,
                UpdatedBy = profile.Id
            });
            
        }

        public async Task<CursorPagedResult<Domain.Entities.Profile>> Search(string keywords, string profileId, string cursor, int limit)
        {

            IList<string> blockerIds = await relationshipClient.GetAllBlockerIds(profileId.ToString());
            IList<string> blockeeIds = await relationshipClient.GetAllBlockeeIds(profileId.ToString());

            var specification = new SpecificationWithCursor<Domain.Entities.Profile>
            {
                Criteria = userProfile =>
                        (string.IsNullOrEmpty(keywords) ||
                         userProfile.UserProfile.FirstName.Contains(keywords, StringComparison.CurrentCultureIgnoreCase) ||
                         userProfile.UserProfile.LastName.Contains(keywords, StringComparison.CurrentCultureIgnoreCase) ||
                         userProfile.UserProfile.Username.Contains(keywords, StringComparison.CurrentCultureIgnoreCase) ||
                         userProfile.PageProfile.Name.Contains(keywords, StringComparison.CurrentCultureIgnoreCase))
                        & !blockerIds.Concat(blockeeIds).Contains(userProfile.Id.ToString()),
                Cursor = cursor,
                Limit = limit
            };
            return await profileRepository.GetPagedAsync(specification);
        }

    }
}
