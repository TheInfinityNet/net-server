using System.Collections.Generic;

namespace InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging
{
    public sealed record BCursorPagedResult<TEntity>
    {

        public IList<TEntity> Items { get; set; } = [];

        public string? NextCursor { get; set; }

        public string? PrevCursor { get; set; }

    }
}
