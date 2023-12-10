using Ispit.Todo_GoranKolesar.Data;
using Ispit.Todo_GoranKolesar.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Ispit.Todo_GoranKolesar.Controllers
{
    public class HomeController : Controller
    {
        private readonly ToDoContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ToDoContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> IndexTodo(string id)
        {
            var filters = new Filters(id);
            ViewBag.Filters = filters;

            var loggedInUser = await _userManager.GetUserAsync(User);

            if (loggedInUser != null)
            {
                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.Statuses = _context.Statuses.ToList();
                ViewBag.DueFilters = Filters.DueFilterValues;

                IQueryable<ToDo> query = _context.ToDos
                    .Include(t => t.Category)
                    .Include(t => t.Status)
                    .Where(t => t.UserId == loggedInUser.Id); 

                if (filters.HasCategory)
                {
                    query = query.Where(t => t.CategoryId == filters.CategoryId);
                }

                if (filters.HasStatus)
                {
                    query = query.Where(t => t.StatusId == filters.StatusId);
                }

                if (filters.HasDue)
                {
                    var today = DateTime.Today;
                    if (filters.IsPast)
                    {
                        query = query.Where(t => t.DueDate < today);
                    }
                    else if (filters.IsFuture)
                    {
                        query = query.Where(t => t.DueDate > today);
                    }
                    else if (filters.IsToday)
                    {
                        query = query.Where(t => t.DueDate == today);
                    }
                }

                var tasks = query.OrderBy(t => t.DueDate).ToList();

                return View(tasks);
            }
            else
            {
                return RedirectToAction("Login"); 
            }
        }


        [HttpGet]
        public IActionResult Add()
        {
            
            ViewBag.Categories = _context.Categories.ToList();
            ViewBag.Statuses = _context.Statuses.ToList();
            var task = new ToDo { StatusId = "open" };

            return View(task);
        }

        [HttpPost]
        public async Task<IActionResult> Add(ToDo task)
        {
            if (ModelState.IsValid)
            {
                var loggedInUser = await _userManager.GetUserAsync(User);

                if (loggedInUser != null)
                {
                    task.UserId  = loggedInUser.Id;
                    
                    _context.ToDos.Add(task);
                    _context.SaveChanges();
                    return RedirectToAction("IndexTodo");
                }
                else
                {
                    return RedirectToAction("Login"); 
                }
            }
            else
            {
                ViewBag.Categories = _context.Categories.ToList();
                ViewBag.Statuses = _context.Statuses.ToList();
                return View(task);
            }
        }


        [HttpPost]
        public IActionResult Filter(string[] filter)
        {
            string id = string.Join('-', filter);
            return RedirectToAction("IndexTodo", new { ID = id });
        }

        [HttpPost]
        public IActionResult MarkComplete([FromRoute]string id, ToDo selected)
        {
            selected = _context.ToDos.Find(selected.Id)!;

            if(selected != null) 
            {
                selected.StatusId = "closed";
                _context.SaveChanges();
            }
            return RedirectToAction("IndexTodo", new {ID = id});
        }

        [HttpPost]
        public IActionResult DeleteComplete(string id) 
        {
            var toDelete = _context.ToDos.Where(t => t.StatusId == "closed").ToList();

            foreach (var task in toDelete)
            {
                _context.ToDos.Remove(task);
            }
            _context.SaveChanges();

            return RedirectToAction("IndexTodo", new {ID = id});
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}