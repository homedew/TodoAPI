using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoAPI.Dtos
{
    public class TodoDto
    {
        public int Id {get; set;}
        public string Title {get; set;} = string.Empty;
        public string? Description {get; set;}
        public bool IsCompleted {get; set;} = false;
    }
}