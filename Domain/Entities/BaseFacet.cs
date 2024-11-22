using InfinityNetServer.BuildingBlocks.Domain.Enums;
using System.Text.Json.Serialization;

namespace InfinityNetServer.BuildingBlocks.Domain.Entities
{
    public class BaseFacet
    {

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public FacetType Type { get; set; }

        public int Start { get; set; }

        public int End { get; set; }

    }
}
