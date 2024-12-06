using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging
{
    public class SpecificationWithCursor<TEntity>
    {

        public Expression<Func<TEntity, bool>> Criteria { get; set; }

        // List các điều kiện sắp xếp theo nhiều trường
        public IList<OrderField<TEntity>> OrderFields { get; set; } = [];

        public string Cursor { get; set; }

        public int Limit { get; set; }

    }
}
