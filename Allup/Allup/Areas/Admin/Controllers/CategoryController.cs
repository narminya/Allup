using Allup.Areas.Admin.Constants;
using Allup.Areas.Admin.Utils;
using Allup.Areas.Admin.ViewModel;
using Allup.Models.DataAccessLayer;
using Allup.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Allup.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _db;
        public CategoryController(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            var result = await _db.categories.Where(c => c.IsMain && !c.IsDeleted).ToListAsync();
            return View(result);
        }

        public IActionResult Create()
        {
            ViewBag.Parents = new SelectList(_db.categories.Where(i => i.IsMain));
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryCreateViewModel model)
        {

            ViewBag.Parents = new SelectList(_db.categories.Where(i => i.IsMain));

            var result = await _db.categories.Where(c => c.IsMain && !c.IsDeleted).ToListAsync();


            if (!ModelState.IsValid)
            {
                return View();
            }
            if (model.IsMain)
            {
                if (!model.File.ContentType.Contains("image"))
                {
                    ModelState.AddModelError(nameof(model.File), "File is not supported");

                    return View();
                }
                if (model.File.Length > 1000 * 1024)
                {
                    ModelState.AddModelError(nameof(model.File), "File is not supported");
                    return View();
                }

                var image = FileUtil.FileCreate(model.File);
                Category category = new Category()
                {
                    Name = model.Name,
                    Image = image,
                    IsMain = model.IsMain
                };

                await _db.categories.AddAsync(category);
                
            }
            else
            {
                var parent = await _db.categories.FirstOrDefaultAsync(c=>c.IsMain && !c.IsDeleted && c.Id == model.ParentId);
                if (parent==null)
                {
                    ModelState.AddModelError("ParentId", "Doesn`t exist");
                    return View();
                }

                Category category = new Category
                {
                    Name = model.Name,
                    IsMain = false,
                    Parent = parent
                };
            }
            await _db.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
        }
    }
}
