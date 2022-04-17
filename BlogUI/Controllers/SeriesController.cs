using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogLibrary.Data;
using BlogLibrary.Models;

namespace BlogUI.Controllers
{
    public class SeriesController : Controller
    {
        private readonly BlogContext _context;

        public SeriesController(BlogContext context)
        {
            _context = context;
        }

        // GET: Series
        public async Task<IActionResult> Index()
        {
            var blogContext = _context.Series.Include(t => t.Creator).Include(t => t.Image);
            return View(await blogContext.ToListAsync());
        }

        // GET: Series/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seriesModel = await _context.Series
                .Include(t => t.Creator)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (seriesModel == null)
            {
                return NotFound();
            }

            return View(seriesModel);
        }

        // GET: Series/Create
        public IActionResult Create()
        {
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
                _context.Add(seriesModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatorId"] = new SelectList(_context.Users, "Id", "Id", seriesModel.CreatorId);
            return View(seriesModel);
        }

        // GET: Series/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var seriesModel = await _context.Series.FindAsync(id);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description, Image")] SeriesModel seriesModel)
        {
            if (id != seriesModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    seriesModel.Updated = DateTime.Now;
                    _context.Update(seriesModel);
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
                return RedirectToAction(nameof(Index));
            }
            return View(seriesModel);
        }

        // GET: Series/Delete/5
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
