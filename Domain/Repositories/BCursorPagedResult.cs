using System.Collections.Generic;

namespace InfinityNetServer.BuildingBlocks.Domain.Repositories
{
    public sealed record BCursorPagedResult<TEntity, TCursor>
    {

        public IList<TEntity> Items { get; set; } = [];

        public TCursor NextCursor { get; set; }

        public bool HasNext { get; set; }

    }
}
