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
using System.IO;
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

        public async Task<IActionResult> Details(int id )
        {
            var result = await _db.categories.Include(c => c.Children).FirstOrDefaultAsync(c => c.Id == id);
            if (result==null)
            {
                return NotFound();
            }

            return View(result);
        }

    

        public async Task<IActionResult> Delete(int id)
        {
            var category = await _db.categories.Include(c=>c.Children).FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
            if (category.IsMain)
            {
                category.IsDeleted = true;

                foreach (var item in category.Children)
                {
                    item.IsDeleted = true;
                }
            }
            else
            {
                category.IsDeleted = true;
            }

            await _db.SaveChangesAsync();
            return Json(category);

        }

        public async Task<IActionResult> Update(int? id)
        {
            ViewBag.Parents = new SelectList(_db.categories.Where(i => i.IsMain));

            if (!id.HasValue || id.Value < 1)
            {
                return BadRequest();
            }
            var result = await _db.categories.FirstOrDefaultAsync(c => c.Id == id);

            if (result == null)
            {
                return NotFound();
            }


            CategoryCreateViewModel model = new CategoryCreateViewModel()
            {
                Name = result.Name
            };

           
       
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id, CategoryCreateViewModel model)
        {
                ViewBag.Parents = new SelectList(_db.categories.Where(i => i.IsMain));

                var category = await _db.categories.Include(c => c.Children).FirstOrDefaultAsync(c => c.Id == id);


            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (model.IsMain)
            {
                if (!model.File.ContentType.Contains("image"))
                {
                    ModelState.AddModelError(nameof(CategoryCreateViewModel.File), "File is not supported");

                    return View();
                }
                if (model.File.Length > 1000 * 1024)
                {
                    ModelState.AddModelError(nameof(CategoryCreateViewModel.File), "File is not supported");
                    return View();
                }

                string path = Path.Combine(FileConstants.ImagePath, category.Image);
                FileUtil.FileDelete(path);
                var image = FileUtil.FileCreate(model.File);


             

                category.Image = image;
                category.Name = model.Name;

                 _db.categories.Update(category);

            }
            else
            {
                var parent = await _db.categories.FirstOrDefaultAsync(c => c.IsMain && !c.IsDeleted && c.Id == model.ParentId);
                if (parent == null)
                {
                    ModelState.AddModelError("ParentId", "Doesn`t exist");
                    return View();
                }
                category.Name = model.Name;
                category.Parent = parent;
                _db.categories.Update(category);
            }

            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        
        }
    }
}
