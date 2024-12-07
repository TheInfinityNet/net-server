namespace InfinityNetServer.Services.Identity.Application.IServices
{
    public sealed record SocialOauth2Options
    {

        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string[] Scopes { get; set; }

        public string RedirectUri { get; set; }

        public string AuthUri { get; set; }

        public string TokenUri { get; set; }

        public string UserInfoUri { get; set; }

    }
}
