namespace InfinityNetServer.BuildingBlocks.Presentation.Configuration.Metric;

public class MetricOptions
{

    public bool StandAloneKestrelServerEnabled { get; set; }
    public ushort Port { get; set; }
    public string Url { get; set; }
    public string Hostname { get; set; }
    public bool HttpMetricsEnabled { get; set; }
    public bool SuppressDefaultMetrics { get; set; }

}
