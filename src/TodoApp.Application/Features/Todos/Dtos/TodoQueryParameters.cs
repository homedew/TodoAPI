using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApp.Application.Dto
{
    public class TodoQueryParameters
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string? SortBy { get; set; }
        public string? SortOrder { get; set; }

        // Filter fields
        public string? Title { get; set; }
        public string? Status { get; set; }
        public string? Searching { get; set; }

    }
}