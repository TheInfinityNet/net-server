using InfinityNetServer.BuildingBlocks.Application.Contracts.Events;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace InfinityNetServer.Services.Mail.Application.Usecases
{
    public class SendMailWithCodeEventHandler
        (ILogger<SendMailWithCodeEventHandler> logger, 
        IStringLocalizer<MailSharedResource> localizer) : IRequestHandler<DomainEvent.SendMailWithCodeEvent>
    {

        public async Task Handle(DomainEvent.SendMailWithCodeEvent request, CancellationToken cancellationToken)
        {
            logger.LogInformation(request.AcceptLanguage);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(request.AcceptLanguage);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(request.AcceptLanguage);
            logger.LogInformation(CultureInfo.CurrentCulture.Name);
            logger.LogInformation(Thread.CurrentThread.CurrentCulture.Name);
            var message = request;
            logger.LogInformation(localizer["Hello"].ToString());
            logger.LogInformation($"Processing SendMailMessage: {message.ToMail} at {message.Type}");
            await Task.CompletedTask;
        }

    }
}
