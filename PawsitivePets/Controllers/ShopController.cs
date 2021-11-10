using Microsoft.AspNetCore.Mvc;
using PawsitivePets.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PawsitivePets.Controllers
{
    public class ShopController : Controller
    {
        // db connection instance
        private readonly ApplicationDbContext _context;

        // constructor - every instance of this class requires a DbContext object
        public ShopController(ApplicationDbContext context)
        {
            _context = context; // make db connection public in this class so we can use it in any method
        }

        public IActionResult Index()
        {
            // pass a list of Categories to the view so the user can select one
            var categories = _context.Categories.OrderBy(c => c.Name).ToList();
            return View(categories);
        }
    }
}
