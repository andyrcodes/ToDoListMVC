using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoListMVC.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 2)]
        public string UserName { get; set; }

        public virtual ICollection<ToDoList> Lists { get; set; } = new List<ToDoList>();

    }
}
