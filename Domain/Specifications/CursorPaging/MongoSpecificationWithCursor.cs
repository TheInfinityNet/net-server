using System;
using System.Linq.Expressions;

namespace InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging
{
    public class MongoSpecificationWithCursor<TEntity, TCursor>
    {

        public Expression<Func<TEntity, bool>> Criteria { get; set; }

        public Expression<Func<TEntity, object>> OrderBy { get; set; }

        public Expression<Func<TEntity, object>> OrderByDescending { get; set; }

        public TCursor Cursor { get; set; }

        public int PageSize { get; set; } = 10;

    }
}
