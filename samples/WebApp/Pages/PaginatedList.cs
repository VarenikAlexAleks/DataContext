using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApp.Pages
{
    public class PaginatedList<T> : List<T>
    {
        public long PageIndex { get; private set; }
        public long TotalPages { get; private set; }

        public PaginatedList(IEnumerable<T> items, long totalItems, long currentPage)
        {
            PageIndex = currentPage;
            TotalPages = totalItems;

            this.AddRange(items);
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }
    }
}