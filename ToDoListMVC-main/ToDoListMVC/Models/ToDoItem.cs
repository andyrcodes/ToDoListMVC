using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoListMVC.Models
{
    public class ToDoItem
    {
        public int Id { get; set; }

        [Required]
        public string Task { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public DateTimeOffset DueDate { get; set; }

        public bool IsComplete { get; set; }

        public int ListId { get; set; }

        public virtual ToDoList List { get; set; }

    }
}
