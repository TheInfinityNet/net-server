namespace InfinityNetServer.BuildingBlocks.Infrastructure.RabbitMQ
{
    public sealed record RabbitMqOptions
    {
        public string Host { get; set; }

        public string VirtualHost { get; set; } = "/";

        public string ExchangeName { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }
    }

}
