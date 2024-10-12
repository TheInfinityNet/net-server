using MassTransit;
using Microsoft.Extensions.Localization;
using System.Globalization;
using InfinityNetServer.BuildingBlocks.Application.Attributes;
using InfinityNetServer.BuildingBlocks.Application.Consumers;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using InfinityNetServer.BuildingBlocks.Application.Contracts;
using InfinityNetServer.BuildingBlocks.Application.DTOs.Requests;

namespace InfinityNetServer.Services.Mail.Application.Consumers
{
    [QueueName("mail-sent")]
    public class MailSentConsumer : BaseConsumer<IBaseContract<SendMailRequest>>
    {

        private readonly ILogger<MailSentConsumer> _logger;

        private readonly IStringLocalizer<MailSharedResource> _localizer;

        public MailSentConsumer(
            ILogger<MailSentConsumer> logger,
            IStringLocalizer<MailSharedResource> localizer) : base()
        {
            _logger = logger;
            _localizer = localizer;
        }

        public override async Task ConsumeMessage(ConsumeContext<IBaseContract<SendMailRequest>> context)
        {
            _logger.LogInformation(context.Message.AcceptLanguage);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(context.Message.AcceptLanguage);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(context.Message.AcceptLanguage);
            _logger.LogInformation(CultureInfo.CurrentCulture.Name);
            _logger.LogInformation(Thread.CurrentThread.CurrentCulture.Name);
            var message = context.Message;
            _logger.LogInformation(_localizer["Hello"].ToString());
            _logger.LogInformation($"Processing SendMailMessage: {message.Content.Email} at {message.Content.Type}");
            await Task.CompletedTask;
        }

    }
}
