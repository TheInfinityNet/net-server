using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System;
using Bogus;
using InfinityNetServer.Services.Identity.Domain.Entities;
using InfinityNetServer.Services.Identity.Application;
using InfinityNetServer.BuildingBlocks.Presentation.Controllers;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.Services.Identity.Domain.Repositories;
using InfinityNetServer.BuildingBlocks.Application.Services;
using InfinityNetServer.BuildingBlocks.Application.Contracts;

namespace InfinityNetServer.Services.Identity.Presentation.Controllers
{
    [ApiController]
    [Route("test")]
    public class TestController : BaseApiController
    {
        private readonly IStringLocalizer<IdentitySharedResource> _localizer;

        private readonly IAccountRepository _accountRepository;

        private readonly ILogger<TestController> _logger;

        private readonly IMessageBus _messageBus;

        public TestController(
            IAuthenticatedUserService authenticatedUserService,
            ILogger<TestController> logger,
            IStringLocalizer<IdentitySharedResource> Localizer,
            IMessageBus messageBus,
            IAccountRepository accountRepository) : base(authenticatedUserService)
        {
            _logger = logger;
            _localizer = Localizer;
            _messageBus = messageBus;
            _accountRepository = accountRepository;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Test()
        {
            /*var user = User;
            if (user == null)
            {
                _logger.LogError("User is null");
            }
            else
            {
                _logger.LogInformation($"User is authenticated: {user.Identity?.IsAuthenticated}");
                foreach (var claim in user.Claims)
                {
                    _logger.LogInformation($"Claim: {claim.Type} = {claim.Value}");
                }
            }
            _logger.LogInformation(User.FindFirst("sub")?.Value);
            var faker = new Faker<Account>()
                .RuleFor(a => a.DefaultUserProfileId, f => Guid.NewGuid());
            Account account = faker.Generate(1).First();
            account.Id = Guid.Parse("086101dd-b01b-474b-b710-82f90e044234");
            await _accountRepository.UpdateAsync(account);

            _logger.LogInformation(CultureInfo.CurrentCulture.ToString());
            return Ok(new { Message = _localizer["msg_test", "Ben"].ToString() });*/
            return Ok(new { Message = GetCurrentUserId().ToString() });
        }

        /*[HttpGet("seed-data")]
        public async Task<IActionResult> SeedData()
        {
            var accounts = await _accountRepository.GetAllAsync();
            var faker = new Faker();
            foreach (var account in accounts)
            {
                var payload = new ProfileCreatedPayload (
                    account.Id.ToString(),
                    account.DefaultUserProfileId.ToString(),
                    faker.Name.FullName(),
                    faker.Name.FirstName(),
                    string.Empty,
                    faker.Name.LastName(),
                    new Faker().Phone.PhoneNumber(),
                    new DateOnly(1990, 1, 12),
                    faker.PickRandom<Gender>()
                    );

                await _messageBus.Publish(new BaseCommand<ProfileCreatedPayload>
                {
                    Payload = payload
                });
            }

            return Ok(new { Message = "Seeded data successfully!" });
        }*/

    }
}
