using InfinityNetServer.BuildingBlocks.Domain.Entities;
using InfinityNetServer.BuildingBlocks.Domain.Repositories;
using InfinityNetServer.BuildingBlocks.Domain.Specifications;
using InfinityNetServer.BuildingBlocks.Domain.Specifications.CursorPaging;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InfinityNetServer.BuildingBlocks.Infrastructure.PostgreSQL.Repositories
{
    public class SqlRepository<TEntity, TId>(DbContext context) : ISqlRepository<TEntity, TId> where TEntity : AuditEntity<TId>
    {

        protected readonly DbSet<TEntity> DbSet = context.Set<TEntity>();

        public async Task<List<TEntity>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<TEntity> GetByIdAsync(TId id)
        {
            return await DbSet.FindAsync(id);
        }

        public virtual async Task CreateAsync(IEnumerable<TEntity> items)
        {
            ArgumentNullException.ThrowIfNull(items);
            await DbSet.AddRangeAsync(items);
            await context.SaveChangesAsync();
        }

        public async Task<TEntity> CreateAsync(TEntity item)
        {
            ArgumentNullException.ThrowIfNull(item);
            await DbSet.AddAsync(item);
            await context.SaveChangesAsync();
            return item;
        }

        public async Task<TEntity> UpdateAsync(TEntity item)
        {
            ArgumentNullException.ThrowIfNull(item);
            DbSet.Attach(item);
            context.Entry(item).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return item;
        }

        public async Task<TEntity> DeleteAsync(TId id)
        {
            var entity = await GetByIdAsync(id) ?? throw new KeyNotFoundException($"Entity with id {id} not found.");
            DbSet.Remove(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> SoftDeleteAsync(TId id)
        {
            var entity = await GetByIdAsync(id) ?? throw new KeyNotFoundException($"Entity with id {id} not found.");
            entity.IsDeleted = true;
            DbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<List<TEntity>> FindAsync(Func<TEntity, bool> predicate)
        {
            ArgumentNullException.ThrowIfNull(predicate);
            return await Task.FromResult(DbSet.AsEnumerable().Where(predicate).ToList());
        }

        public async Task<bool> ExistsAsync(TId id)
        {
            var entity = await GetByIdAsync(id);
            return entity != null;
        }

        public async Task<int> CountAsync()
        {
            return await DbSet.CountAsync();
        }

        public async Task<PagedResult<TEntity>> GetPagedAsync(ISqlSpecification<TEntity> spec)
        {
            ArgumentNullException.ThrowIfNull(spec);

            IQueryable<TEntity> query = ApplySpecification(spec);

            var totalItems = await query.CountAsync();

            var items = await query
                .Skip(spec.Skip ?? 0)
                .Take(spec.Take ?? 10)
                .ToListAsync();

            var pageNumber = spec.Skip.HasValue && spec.Take.HasValue
                ? (spec.Skip.Value / spec.Take.Value) + 1
                : 1;

            return new PagedResult<TEntity>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = spec.Take ?? 10
            };
        }

        public async Task<CursorPagedResult<TEntity>> GetPagedAsync(SpecificationWithCursor<TEntity> spec)
        {
            // Giả sử _dbContext là DbContext để truy vấn dữ liệu.
            var query = context.Set<TEntity>().AsQueryable();

            // Áp dụng các điều kiện lọc nếu có
            if (spec.Criteria != null)
                query = query.Where(spec.Criteria);

            // Kiểm tra nếu không có điều kiện sắp xếp nào được truyền vào
            if (spec.OrderFields == null || !spec.OrderFields.Any())
            {
                query = query.OrderByDescending(x => x.CreatedAt).ThenByDescending(x => x.Id);
            }
            else
            {
                // Nếu có OrderFields, áp dụng các điều kiện sắp xếp đã truyền vào
                foreach (var orderField in spec.OrderFields)
                {
                    query = orderField.Direction == SortDirection.Descending
                        ? query.OrderByDescending(orderField.Field).ThenByDescending(x => x.Id)
                        : query.OrderBy(orderField.Field).ThenBy(x => x.Id);
                }
            }

            // Áp dụng điều kiện Cursor
            if (!string.IsNullOrEmpty(spec.Cursor))
            {
                var cursorParts = spec.Cursor.Split('|');
                var cursorDateTime = DateTime.Parse(cursorParts[0]);
                var cursorId = cursorParts[1];

                if (spec.OrderFields == null || spec.OrderFields.First().Direction == SortDirection.Descending)
                {
                    query = query.Where(x => x.CreatedAt < cursorDateTime
                                             || (x.CreatedAt == cursorDateTime && x.Id.ToString().CompareTo(cursorId) < 0));
                }
                else
                {
                    query = query.Where(x => x.CreatedAt > cursorDateTime
                                             || (x.CreatedAt == cursorDateTime && x.Id.ToString().CompareTo(cursorId) > 0));
                }
            }

            // Lấy dữ liệu phân trang
            var items = await query.Take(spec.PageSize).ToListAsync();

            // Xác định Cursor tiếp theo
            string nextCursor = items.Count == spec.PageSize
                ? $"{items.Last().CreatedAt:O}|{items.Last().Id}"
                : null;

            // Xác định Cursor trước
            string prevCursor = items.Count > 0 && !string.IsNullOrEmpty(spec.Cursor)
                ? $"{items.First().CreatedAt:O}|{items.First().Id}"
                : null;

            // Trả về kết quả
            return new CursorPagedResult<TEntity>
            {
                Items = items,
                NextCursor = nextCursor,
                PrevCursor = prevCursor
            };
        }

        private IQueryable<TEntity> ApplySpecification(ISqlSpecification<TEntity> spec)
        {
            IQueryable<TEntity> query = DbSet;

            // Apply Criteria
            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }

            // Apply Includes
            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));

            // Apply Sorting
            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            else if (spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }

            return query;
        }

    }

}
