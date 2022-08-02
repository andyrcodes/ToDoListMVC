using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoListMVC.Models
{
    public class ToDoList
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Name { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<ToDoItem> Items { get; set; } = new List<ToDoItem>();

    }
}
