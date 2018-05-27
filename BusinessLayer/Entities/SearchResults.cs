using System.Collections.Generic;

namespace BusinessLayer.Entities
{
    public class SearchResults<TItem>
    {
        public int PageNum { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public ICollection<TItem> Items { get; set; }
    }
}
