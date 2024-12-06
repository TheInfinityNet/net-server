using InfinityNetServer.BuildingBlocks.Domain.Enums;

namespace InfinityNetServer.BuildingBlocks.Application.Contracts.Messages
{
    public interface IMailCommand : IMessage
    {

        public string ToMail { get; set; }

        public VerificationType Type { get; set; }

        public string AcceptLanguage { get; set; }

    }
}
