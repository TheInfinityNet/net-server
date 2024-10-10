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
using InfinityNetServer.Services.Identity.Infrastructure.Repositories;
using InfinityNetServer.Services.Identity.Domain.Entities;
using InfinityNetServer.Services.Identity.Application;

namespace InfinityNetServer.Services.Identity.Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IStringLocalizer<IdentitySharedResource> _localizer;

        private readonly AccountRepository _accountRepository;

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

        public WeatherForecastController(
            ILogger<WeatherForecastController> logger,
            IStringLocalizer<IdentitySharedResource> Localizer,
            AccountRepository accountRepository)
        {
            _logger = logger;
            _localizer = Localizer;
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
            account.Id = Guid.Parse("086101dd-b01b-474b-b710-82f90e044234");
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
    }
}
