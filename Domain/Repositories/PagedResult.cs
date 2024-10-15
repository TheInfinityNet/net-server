using System;
using System.Collections.Generic;

namespace InfinityNetServer.BuildingBlocks.Domain.Repositories
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }

        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public bool HasPrevious => PageNumber > 1;
        public bool HasNext => PageNumber < TotalPages;

        // Liên kết đến các trang khác
        public string PreviousPageLink { get; set; }
        public string NextPageLink { get; set; }
    }

}
