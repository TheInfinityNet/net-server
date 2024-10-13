using System;
using System.Globalization;
using InfinityNetServer.BuildingBlocks.Application.Bus;

namespace InfinityNetServer.BuildingBlocks.Application.DTOs.Commands
{
    public sealed record BaseCommand<T> : IIntegrationEvent
    {

        public string RoutingKey { get; set; } = "app.info"; // Ví dụ: "app.info", "app.error"

        public DateTime SentAt { get; set; } = DateTime.Now;

        public string AcceptLanguage { get; set; } = CultureInfo.CurrentCulture.ToString(); // Ví dụ: "vi-VN", "en-US"

        public T Payload { get; set; }

    }
}
