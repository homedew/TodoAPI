using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoAPI.Entity
{
    public class TodoItem
    {
        public int Id {get; set;}
        public required string Title {get; set;}
        public string? Description {get; set;} 
        public bool IsCompleted {get; set;}
        public DateTime Created {get;set;} = DateTime.UtcNow;
    }
}