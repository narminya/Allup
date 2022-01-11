using Allup.Models;
using Allup.Models.DataAccessLayer;
using Allup.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Allup.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _dbContext;

        public HomeController(AppDbContext dbContext)
        {
            _dbContext = dbContext;

        }

        public async Task<IActionResult> Index()
        {
            var categories = await _dbContext.categories.ToListAsync();
            var homeVm = new HomeViewModel
            {
                Categories = categories
            };
            return View(homeVm);
        }

    }
}
