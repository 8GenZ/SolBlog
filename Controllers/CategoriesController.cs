using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SolBlog.Data;
using SolBlog.Models;
using SolBlog.Services;
using SolBlog.Services.Interfaces;


namespace SolBlog.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IBlogService _blogService;
        private readonly IImageService _imageService;

        public CategoriesController(ApplicationDbContext context,IBlogService blogService, IImageService imageService)
        {
            _context = context;
            _blogService = blogService;
            _imageService = imageService;
        }

        public async Task<IActionResult> Index()
        {
            IEnumerable<Category> categories = await _blogService.GetCategoriesAsync(5);
            return View(categories);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _blogService.GetCategoryAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,ImageFile")] Category category)
        {
            if (ModelState.IsValid)
            {
                if (category.ImageFile != null)
                {
                    category.ImageData = await _imageService.ConvertFileToByteArrayAsync(category.ImageFile);
                    category.ImageType = category.ImageFile.ContentType;
                }
                await _blogService.AddCategoryAsync(category);
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var category = await _blogService.GetCategoryAsync(id);

            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,ImageFile")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (category.ImageFile != null)
                    {
                        category.ImageData = await _imageService.ConvertFileToByteArrayAsync(category.ImageFile);
                        category.ImageType = category.ImageFile.ContentType;
                    }
                    await _blogService.AddCategoryAsync(category);
                }
                catch (Exception)
                {
                    if (!CategoryExists(category.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _blogService.GetCategoryAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _blogService.DeleteCategoryAsync(id);
            
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
          return (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
