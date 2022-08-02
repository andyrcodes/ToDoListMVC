using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDoListMVC.Data;

namespace ToDoListMVC.Services
{
    public class DataService
    {
        private readonly ApplicationDbContext _context;

        public DataService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task ManageDataAsync()
        {
            //Task 0: Make sure the DB is present by running through the migrations
            await _context.Database.MigrateAsync();

        }
    }
}
