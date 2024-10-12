using System;

namespace InfinityNetServer.BuildingBlocks.Application.Contracts
{
    public interface IBaseContract<T>
    {
        string RoutingKey { get; } // Ví dụ: "app.info", "app.error"

        DateTime SentAt { get; }

        string AcceptLanguage { get; }

        T Content { get; }
    }
}
