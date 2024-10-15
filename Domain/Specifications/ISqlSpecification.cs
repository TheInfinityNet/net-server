using System.Collections.Generic;
using System.Linq.Expressions;
using System;

namespace InfinityNetServer.BuildingBlocks.Domain.Specifications
{
    public interface ISqlSpecification<TEntity> where TEntity : class
    {
        // Điều kiện WHERE
        Expression<Func<TEntity, bool>> Criteria { get; }

        // Các biểu thức để bao gồm các liên kết (Include)
        List<Expression<Func<TEntity, object>>> Includes { get; }

        // Sắp xếp
        Expression<Func<TEntity, object>> OrderBy { get; }

        Expression<Func<TEntity, object>> OrderByDescending { get; }

        // Phân trang
        int? Take { get; }

        int? Skip { get; }

        bool IsPagingEnabled { get; }

    }
}
