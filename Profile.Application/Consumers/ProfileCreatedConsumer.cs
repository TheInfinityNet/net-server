using MassTransit;
using Microsoft.Extensions.Localization;
using System.Globalization;
using InfinityNetServer.BuildingBlocks.Application.Attributes;
using InfinityNetServer.BuildingBlocks.Application.Consumers;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Commands;
using InfinityNetServer.Services.Profile.Domain.Entities;
using System;
using InfinityNetServer.Services.Profile.Domain.Repositories;

namespace InfinityNetServer.Services.Profile.Application.Consumers
{
    [QueueName("profile-created")]
    public class ProfileCreatedConsumer : BaseConsumer<BaseCommand<ProfileCreatedPayload>>
    {

        private readonly ILogger<ProfileCreatedConsumer> _logger;

        private readonly IStringLocalizer<ProfileSharedResource> _localizer;

        private readonly IUserProfileRepository _userProfileRepository;

        public ProfileCreatedConsumer(
            ILogger<ProfileCreatedConsumer> logger,
            IStringLocalizer<ProfileSharedResource> localizer,
            IUserProfileRepository userProfileRepository)
        {
            _logger = logger;
            _localizer = localizer;
            _userProfileRepository = userProfileRepository;
        }

        public override async Task ConsumeMessage(ConsumeContext<BaseCommand<ProfileCreatedPayload>> context)
        {
            _logger.LogInformation(context.Message.AcceptLanguage);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(context.Message.AcceptLanguage);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(context.Message.AcceptLanguage);
            _logger.LogInformation(CultureInfo.CurrentCulture.Name);
            _logger.LogInformation(Thread.CurrentThread.CurrentCulture.Name);
            var message = context.Message;
            _logger.LogInformation(_localizer["Hello"].ToString());

            ProfileCreatedPayload payload = message.Payload;

            UserProfile userProfile = new UserProfile
            {
                Id = Guid.Parse(payload.ProfileId),
                AccountId = Guid.Parse(payload.AccountId),
                CreatedBy = payload.AccountId,
                UpdatedBy = payload.AccountId,
                Username = payload.Username,
                MobileNumber = payload.MobileNumber,
                FirstName = payload.FirstName,
                MiddleName = payload.MiddleName,
                LastName = payload.LastName,
                Birthdate = payload.Birthdate,
                Gender = payload.Gender
            };

            await _userProfileRepository.CreateUserProfileAsync(userProfile);

            _logger.LogInformation("Profile created !!!");

            await Task.CompletedTask;
        }

    }
}
