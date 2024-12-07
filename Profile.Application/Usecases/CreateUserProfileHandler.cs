using InfinityNetServer.BuildingBlocks.Application.Contracts.Commands;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.Services.Profile.Application.IServices;
using InfinityNetServer.Services.Profile.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Profile.Application.Usecases
{
    public class CreateUserProfileHandler(
        IUserProfileService userProfileService,
        ILogger<CreateUserProfileHandler> logger) : IRequestHandler<DomainCommand.CreateUserProfileCommand>
    {

        public async Task Handle(DomainCommand.CreateUserProfileCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation("CreateUserProfileHandler: Processing CreateUserProfile");

            UserProfile userProfile = new ()
            {
                Id = request.ProfileId,
                CreatedBy = request.AccountId,
                AccountId = request.AccountId,
                Username = $"{request.FirstName} {request.LastName}",
                MobileNumber = request.MobileNumber,
                FirstName = request.FirstName,
                MiddleName = request.MiddleName,
                LastName = request.LastName,
                Birthdate = request.Birthdate,
                Gender = request.Gender,
                Status = request.IsActive ? ProfileStatus.Active : ProfileStatus.Locked
            };

            await userProfileService.Create(userProfile);

            logger.LogInformation($"Processing CreateUserProfile: {userProfile.Username}");
        }

    }
}
