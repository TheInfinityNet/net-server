using InfinityNetServer.BuildingBlocks.Application.Contracts.Commands;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Mail.Application.Usecases
{
    public class SendMailWithCodeHandler
        (ILogger<SendMailWithCodeHandler> logger,
        IStringLocalizer<MailSharedResource> localizer) : IRequestHandler<DomainCommand.SendMailWithCodeCommand>
    {

        public async Task Handle(DomainCommand.SendMailWithCodeCommand request, CancellationToken cancellationToken)
        {
            logger.LogInformation(request.AcceptLanguage);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(request.AcceptLanguage);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(request.AcceptLanguage);
            logger.LogInformation(CultureInfo.CurrentCulture.Name);
            logger.LogInformation(Thread.CurrentThread.CurrentCulture.Name);
            var message = request;
            logger.LogInformation(localizer["Hello"].ToString());
            logger.LogInformation($"Processing SendMailMessage: {message.ToMail} at {message.Type} with code {message.Code}");
            await Task.CompletedTask;
        }

    }
}
