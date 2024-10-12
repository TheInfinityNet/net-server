using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System;
using Bogus;
using InfinityNetServer.Services.Identity.Domain.Entities;
using InfinityNetServer.Services.Identity.Application;
using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Commands;
using InfinityNetServer.BuildingBlocks.Presentation.Controllers;
using InfinityNetServer.BuildingBlocks.Application.Interfaces;
using MassTransit;
using InfinityNetServer.BuildingBlocks.Domain.Enums;
using InfinityNetServer.Services.Identity.Domain.Repositories;

namespace InfinityNetServer.Services.Identity.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : BaseApiController
    {
        private readonly IStringLocalizer<IdentitySharedResource> _localizer;

        private readonly IAccountRepository _accountRepository;

        private static readonly string[] Summaries = [
            "Freezing",
            "Bracing",
            "Chilly",
            "Cool",
            "Mild",
            "Warm",
            "Balmy",
            "Hot",
            "Sweltering",
            "Scorching"
        ];

        private readonly ILogger<WeatherForecastController> _logger;

        private readonly IPublishEndpoint _publishEndpoint;

        public WeatherForecastController(
            IAuthenticatedUserService authenticatedUserService,
            ILogger<WeatherForecastController> logger,
            IStringLocalizer<IdentitySharedResource> Localizer,
            IPublishEndpoint publishEndpoint,
            IAccountRepository accountRepository) : base(authenticatedUserService)
        {
            _logger = logger;
            _localizer = Localizer;
            _publishEndpoint = publishEndpoint;
            _accountRepository = accountRepository;
        }

        [Authorize]
        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            var user = User;
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
                .RuleFor(a => a.DefaultUserProfile, f => Guid.NewGuid());
            Account account = faker.Generate(1).First();
            account.AccountId = Guid.Parse("086101dd-b01b-474b-b710-82f90e044234");
            await _accountRepository.UpdateAccountAsync(account);

            _logger.LogInformation(CultureInfo.CurrentCulture.ToString());
            _logger.LogInformation(_localizer["msg_test", "Ben"]);
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("seed-data")]
        public async Task<IActionResult> SeedData()
        {
            var accounts = await _accountRepository.GetAllAccountsAsync();

            foreach (var account in accounts)
            {
                var command = new ProfileCreatedCommand(
                    account.AccountId.ToString(),
                    account.Email,
                    "John",
                    "Doe",
                    "Smith",
                    new Faker().Phone.PhoneNumber(),
                    new DateTime(1990, 1, 12),
                    Gender.Male
                    );

                await _publishEndpoint.Publish<IBaseContract<ProfileCreatedCommand>>(new
                {
                    RoutingKey = "app.info",
                    SendAt = DateTime.UtcNow,
                    AcceptLanguage = CultureInfo.CurrentCulture.ToString(),
                    Content = command
                });
            }

            return Ok(new { Message = "Seeded data successfully!" });
        }

    }
}
