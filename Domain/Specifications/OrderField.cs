using System;
using System.Linq.Expressions;

namespace InfinityNetServer.BuildingBlocks.Domain.Specifications
{
    public class OrderField<TEntity>
    {

        public Expression<Func<TEntity, object>> Field { get; set; }

        public SortDirection Direction { get; set; }

    }
}
