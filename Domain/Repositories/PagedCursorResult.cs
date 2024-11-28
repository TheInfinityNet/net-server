using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinityNetServer.BuildingBlocks.Domain.Repositories
{
    public class PagedCursorResult<TEntity>
    {
        public Task<IList<TEntity>> Results { get; set; }
        public string NextCursor { get; set; }
        public string PreviousCursor { get; set; }
        public bool HasNext { get; set; }
        public bool HasPrevious { get; set; }
    }
}
