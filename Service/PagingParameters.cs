using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class PagingParameters
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchQuery { get; set; } = string.Empty;

        public List<Filter> Filters { get; set; } = new List<Filter>();
    }

    public class FilterOption
    {
        public required string Label { get; set; }
        public int Value { get; set; }
    }

    public class Filter
    {
        public required string FilterName { get; set; }
        public List<FilterOption> FilterOptions { get; set; } = new List<FilterOption>();
    }
}
