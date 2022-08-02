using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using ToDoListMVC.Data;
using ToDoListMVC.Models;

namespace ToDoListMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult JavaScriptToDo()
        {
            return View();
        }

        public IActionResult MVCToDo(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return View();
            }

            var model = _context.Users.Include(u => u.Lists).ThenInclude(l => l.Items).FirstOrDefault(u => u.UserName == userName);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UserSubmission(string userName, bool newUser)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return RedirectToAction(nameof(MVCToDo));
            };

            if (newUser)
            {
                var user = new User
                {
                    UserName = userName
                };

                _context.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(MVCToDo), new { userName });
            }

            return RedirectToAction(nameof(MVCToDo), new { userName });
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem(string userName, ToDoItem item)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return RedirectToAction(nameof(MVCToDo));
            };

            item.CreatedDate = DateTimeOffset.Now;
            _context.Add(item);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(MVCToDo), new { userName });
        }

        [HttpPost]
        public async Task<IActionResult> CreateList(string userName, ToDoList list)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return RedirectToAction(nameof(MVCToDo));
            };

            _context.Add(list);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(MVCToDo), new { userName });
        }

        [HttpPost]
        public async Task<IActionResult> CompleteItem(string userName, int? id)
        {
            if (id is null)
            {
                return RedirectToAction(nameof(MVCToDo), new { userName });
            };
            if (string.IsNullOrEmpty(userName))
            {
                return RedirectToAction(nameof(MVCToDo));
            };

            var item = await _context.Items.FirstOrDefaultAsync(i => i.Id == id);
            item.IsComplete = !item.IsComplete;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MVCToDo), new { userName });

        }

        [HttpPost]
        public async Task<IActionResult> DeleteItem(int? id, string userName)
        {
            if(id is null)
            {
                return RedirectToAction(nameof(MVCToDo), new { userName });
            };
            if (string.IsNullOrEmpty(userName))
            {
                return RedirectToAction(nameof(MVCToDo));
            };

            var item = await _context.Items.FirstOrDefaultAsync(i => i.Id == id);
            _context.Remove(item);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MVCToDo), new { userName });

        }

        [HttpPost]
        public async Task<IActionResult> DeleteList(int? id, string userName)
        {
            if(id is null)
            {
                return RedirectToAction(nameof(MVCToDo), new { userName });
            };
            if (string.IsNullOrEmpty(userName))
            {
                return RedirectToAction(nameof(MVCToDo));
            };

            var list = await _context.Lists.FirstOrDefaultAsync(i => i.Id == id);
            _context.Remove(list);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MVCToDo), new { userName });

        }

        [HttpPost]
        public async Task<IActionResult> ClearLists(string userName)
        {
            if (string.IsNullOrEmpty(userName))
            {
                return RedirectToAction(nameof(MVCToDo));
            };

            var userId = (await _context.Users.FirstOrDefaultAsync(u => u.UserName == userName)).Id;
            var lists = await _context.Lists.Where(l => l.UserId == userId).ToListAsync();
            foreach (var list in lists)
            {
                _context.Remove(list);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(MVCToDo), new { userName });
        }

        [HttpPost]
        public async Task<IActionResult> ClearItems(int? id, string userName)
        {
            if (id is null || string.IsNullOrEmpty(userName))
            {
                return RedirectToAction(nameof(MVCToDo));
            };

            var listId = (await _context.Lists.FirstOrDefaultAsync(l => l.Id == id)).Id;

            var items = await _context.Items.Where(i => i.ListId == listId).ToListAsync();

            foreach (var item in items)
            {
                _context.Remove(item);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(MVCToDo), new { userName });
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
