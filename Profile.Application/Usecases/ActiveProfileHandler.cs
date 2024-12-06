using InfinityNetServer.BuildingBlocks.Application.Contracts.Commands;
using InfinityNetServer.BuildingBlocks.Application.Exceptions;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.Services.Profile.Application.IServices;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Profile.Application.Usecases
{
    public class ActiveProfileHandler(
        IProfileService profileService,
        ILogger<ActiveProfileHandler> logger) : IRequestHandler<DomainCommand.ActiveProfileCommand>
    {

        public async Task Handle(DomainCommand.ActiveProfileCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("ActiveProfileHandler: Processing CreateUserProfile");

            Domain.Entities.Profile profile = await profileService.GetById(request.ProfileId.ToString()) 
                ?? throw new BaseException(BaseError.PROFILE_NOT_FOUND, StatusCodes.Status404NotFound);

            profile.Status = ProfileStatus.Active;

            await profileService.Update(profile);

            logger.LogInformation($"Processing ActiveProfile: {profile.Id}");
        }

    }
}
