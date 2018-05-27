using System.Collections.Generic;

namespace WebApi.Entities
{
    public class SearchResults<TDataItem>
    {
        public int PageNum { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public ICollection<TDataItem> Items { get; set; }
    }
}
