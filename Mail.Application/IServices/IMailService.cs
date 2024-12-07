using System.Threading.Tasks;

namespace InfinityNetServer.Services.Mail.Application.IServices
{
    public interface IMailService
    {

        public Task SendMailToVerifyWithToken(string toEmail, string token);

        public Task SendMailToVerifyWithCode(string toEmail, string code);

        public Task SendMailToResetPassword(string toEmail, string code);

    }
}
