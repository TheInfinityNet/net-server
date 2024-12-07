namespace InfinityNetServer.Services.Mail.Infrastructure.SmtpMail
{
    public sealed record SmtpOptions
    {

        public string SmtpServer { get; set; }

        public int Port { get; set; }

        public string SenderEmail { get; set; }

        public string SenderPassword { get; set; }

    }
}
