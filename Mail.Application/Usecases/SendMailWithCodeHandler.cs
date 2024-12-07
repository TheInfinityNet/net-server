using InfinityNetServer.BuildingBlocks.Application.Contracts.Commands;
using InfinityNetServer.Services.Mail.Application.IServices;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Mail.Application.Usecases
{
    public class SendMailWithCodeHandler(
        IMailService mailService,
        ILogger<SendMailWithCodeHandler> logger,
        IStringLocalizer<MailSharedResource> localizer) : IRequestHandler<DomainCommand.SendMailWithCodeCommand>
    {

        public async Task Handle(DomainCommand.SendMailWithCodeCommand request, CancellationToken cancellationToken)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo(request.AcceptLanguage);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(request.AcceptLanguage);
            //logger.LogInformation(CultureInfo.CurrentCulture.Name);
            //logger.LogInformation(Thread.CurrentThread.CurrentCulture.Name);
            var message = request;
            //logger.LogInformation(localizer["Hello"].ToString());
            //logger.LogInformation($"Processing SendMailMessage: {message.ToMail} at {message.Type} with code {message.Code}");

            await mailService.SendMailToVerifyWithCode(message.ToMail, message.Code);

            await Task.CompletedTask;
        }

    }
}
