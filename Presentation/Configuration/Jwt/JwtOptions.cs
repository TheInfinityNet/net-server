namespace InfinityNetServer.BuildingBlocks.Presentation.Configuration.Jwt;

public class JwtOptions
{

    public string AccessKey { get; set; }
    public string? RefreshKey { get; set; }
    public string Issuer { get; set; }
    public string[] Audiences { get; set; }
    public int? ValidDuration { get; set; }
    public long? RefreshDuration { get; set; }

}
