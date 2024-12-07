using InfinityNetServer.Services.Mail.Application;
using InfinityNetServer.Services.Mail.Application.IServices;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using MimeKit;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace InfinityNetServer.Services.Mail.Infrastructure.SmtpMail
{
    public class MailService : IMailService
    {
        private readonly IConfiguration _configuration;

        private readonly SmtpOptions _smtpOptions;

        private readonly ILogger<MailService> _logger;

        private readonly IStringLocalizer<MailSharedResource> _localizer;

        public MailService(IConfiguration configuration, ILogger<MailService> logger, IStringLocalizer<MailSharedResource> localizer)
        {
            _configuration = configuration;
            _smtpOptions = _configuration.GetSection("Smtp").Get<SmtpOptions>();
            _logger = logger;
            _localizer = localizer;
        }

        public async Task SendMailToResetPassword(string toEmail, string code)
        {
            await SendEmailAsync(toEmail, 
                "Subject.ResetPassword", 
                "Content.ResetPassword", 
                "SubContent.ResetPassword", 
                "Footer.ResetPassword", code);
        }

        public async Task SendMailToVerifyWithCode(string toEmail, string code)
        {
            await SendEmailAsync(toEmail,
                "Subject.VerifyEmail",
                "Content.VerifyEmailWithCode",
                "SubContent.VerifyEmail",
                "Footer.VerifyEmail", code);
        }

        public async Task SendMailToVerifyWithToken(string toEmail, string token)
        {
            await SendEmailAsync(toEmail,
                "Subject.VerifyEmail",
                "Content.VerifyEmailWithToken",
                "SubContent.VerifyEmail",
                "Footer.VerifyEmail", token);
        }

        private async Task SendEmailAsync
            (string toEmail, string subjectKey, string contentKey, string subContentKey, string footerKey, string secret)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Infinity Net", _smtpOptions.SenderEmail));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = _localizer[subjectKey].ToString();

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "Templates\\MailTemplate.html").Replace("\\bin\\Debug\\net8.0", "");

            var parameters = new Dictionary<string, string>
            {
                { "Subject", _localizer[subjectKey].ToString() },
                { "Dear", _localizer["Message.Dear", toEmail].ToString() },
                { "Content", _localizer[contentKey].ToString() },
                { "Secret", secret },
                { "SubContent", _localizer[subContentKey].ToString() },
                { "Thank", _localizer["Message.Thank"].ToString() },
                { "Footer", _localizer[footerKey].ToString() }
            };

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = GetTemplateContent(templatePath, parameters)
            };
            message.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(_smtpOptions.SmtpServer, _smtpOptions.Port, MailKit.Security.SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_smtpOptions.SenderEmail, _smtpOptions.SenderPassword);
                await client.SendAsync(message);
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }

        public static string GetTemplateContent(string templatePath, Dictionary<string, string> parameters)
        {
            var template = File.ReadAllText(templatePath);

            foreach (var param in parameters)
            {
                template = template.Replace($"{{{{{param.Key}}}}}", param.Value);
            }

            return template;
        }

    }
}
