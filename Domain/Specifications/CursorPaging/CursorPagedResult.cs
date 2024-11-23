using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging
{
    public sealed record CursorPagedResult<TEntity>
    {

        public IList<TEntity> Items { get; set; } = [];

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string NextCursor { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string PrevCursor { get; set; }

    }
}
