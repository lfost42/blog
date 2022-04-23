using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogLibrary.Data;
using BlogLibrary.Models;
using BlogLibrary.Databases.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace BlogUI.Controllers
{
    public class SeriesController : Controller
    {
        private readonly BlogContext _context;
        private readonly IImageService _imageService;
        private readonly UserManager<UserModel> _userManager;
        private readonly IConfiguration _config;

        public SeriesController(BlogContext context, IImageService imageService, UserManager<UserModel> userManager, IConfiguration config)
        {
            _context = context;
            _imageService = imageService;
            _userManager = userManager;
            _config = config;
        }

        // GET: Series
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var blogContext = _context.Series.Include(t => t.Creator).Include(t => t.Image);
            return View(await blogContext.ToListAsync());
        }

        // GET: Series/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seriesModel = await _context.Series
                .Include(t => t.Creator)
                .Include(t => t.Image)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (seriesModel == null)
            {
                return NotFound();
            }

            return View(seriesModel);
        }

        // GET: Series/Create
        [HttpGet]
        [Authorize(Roles ="Owner")]
        public IActionResult Create()
        {
            ViewData["CreatorId"] = new SelectList(_context.AppUsers, "Id", "Id");
            ViewData["Image"] = new SelectList(_context.Images, "Image.Photo", "Image.Photo");
            return View();
        }

        // POST: Series/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,Image")] SeriesModel seriesModel)
        {
            if (ModelState.IsValid)
            {
                
                seriesModel.Created = DateTime.Now;
                seriesModel.CreatorId = _userManager.GetUserId(User);

                if (seriesModel.Image is not null)
                {
                    seriesModel.Image.ImageData = await _imageService.EncodeImageAsync(seriesModel.Image.Photo);
                    seriesModel.Image.ImageExtension = _imageService.ContentType(seriesModel.Image.Photo);
                }
                else
                {
                    seriesModel.Image = new ImageModel();
                    seriesModel.Image.ImageData = await _imageService.EncodeImageAsync(_config["DefaultImage"]);
                    seriesModel.Image.ImageExtension = Path.GetExtension(_config["DefaultImage"]);
                }

                _context.Add(seriesModel);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatorId"] = new SelectList(_context.AppUsers, "Id", "Id", seriesModel.CreatorId);
            ViewData["Image"] = new SelectList(_context.Images, "Image.Photo", "Image.Photo", seriesModel.Image.Photo);
            return View(seriesModel);
        }

        // GET: Series/Edit/5
        [HttpGet]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //var seriesModel = await _context.Series.FindAsync(id);
            var seriesModel = await _context.Series
            .Include(t => t.Creator)
            .Include(t => t.Image)
            .FirstOrDefaultAsync(m => m.Id == id);

            if (seriesModel == null)
            {
                return NotFound();
            }
            return View(seriesModel);
        }

        // POST: Series/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description")] SeriesModel seriesModel, IFormFile newImage)
        {
            if (id != seriesModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var newSeries = await _context.Series
                                        .Include(t => t.Creator)
                                        .Include(t => t.Image)
                                        .FirstOrDefaultAsync(m => m.Id == id);

                    newSeries.Updated = DateTime.Now;
                    newSeries.Title = seriesModel.Title;
                    newSeries.Description = seriesModel.Description;

                    if (newImage is not null)
                    {
                        newSeries.Image.ImageData = await _imageService.EncodeImageAsync(newImage);
                        newSeries.Image.ImageExtension = _imageService.ContentType(newImage);
                    }
                    
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SeriesModelExists(seriesModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Series", new { id = seriesModel.Id });
            }
            ViewData["NewImage"] = new SelectList(_context.Images, "NewImage", "NewImage", seriesModel.Image.Photo);
            ViewData["CreatorId"] = new SelectList(_context.AppUsers, "Id", "Id", seriesModel.CreatorId);
            return RedirectToAction(nameof(Index)); 
        }

        // GET: Series/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seriesModel = await _context.Series
                .Include(t => t.Creator)
                .Include(t => t.Image)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (seriesModel == null)
            {
                return NotFound();
            }

            return View(seriesModel);
        }

        // POST: Series/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var seriesModel = await _context.Series.FindAsync(id);
            _context.Series.Remove(seriesModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SeriesModelExists(int id)
        {
            return _context.Series.Any(e => e.Id == id);
        }
    }
}
