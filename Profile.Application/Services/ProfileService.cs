using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
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
    public class ProfileService(
        IProfileRepository profileRepository,
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
            Domain.Entities.Profile existedProfile = await GetById(profile.Id.ToString())
                ?? throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            existedProfile.AvatarId = profile.AvatarId;
            existedProfile.CoverId = profile.CoverId;
            existedProfile.Location = profile.Location;
            existedProfile.MobileNumber = profile.MobileNumber;
            existedProfile.Status = profile.Status;

            await profileRepository.UpdateAsync(existedProfile);

            return existedProfile;
        }

    }
}
