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
    public class ArticlesController : Controller
    {
        private readonly BlogContext _context;

        public ArticlesController(BlogContext context)
        {
            _context = context;
        }

        // GET: Articles
        public async Task<IActionResult> Index()
        {
            var blogContext = _context.Articles.Include(a => a.Creator).Include(a => a.Image).Include(a => a.TopicModel);
            return View(await blogContext.ToListAsync());
        }

        // GET: Articles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articleModel = await _context.Articles
                .Include(a => a.Creator)
                .Include(a => a.Image)
                .Include(a => a.TopicModel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (articleModel == null)
            {
                return NotFound();
            }

            return View(articleModel);
        }

        // GET: Articles/Create
        public IActionResult Create()
        {
            ViewData["CreatorId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["ImageId"] = new SelectList(_context.Images, "Id", "Id");
            ViewData["TopicModelId"] = new SelectList(_context.Topics, "Id", "Description");
            return View();
        }

        // POST: Articles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Summary,Body,Status,Created,Updated,Slug,ImageId,TopicModelId,CreatorId")] ArticleModel articleModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(articleModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatorId"] = new SelectList(_context.Users, "Id", "Id", articleModel.CreatorId);
            ViewData["ImageId"] = new SelectList(_context.Images, "Id", "Id", articleModel.ImageId);
            ViewData["TopicModelId"] = new SelectList(_context.Topics, "Id", "Description", articleModel.TopicModelId);
            return View(articleModel);
        }

        // GET: Articles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articleModel = await _context.Articles.FindAsync(id);
            if (articleModel == null)
            {
                return NotFound();
            }
            ViewData["CreatorId"] = new SelectList(_context.Users, "Id", "Id", articleModel.CreatorId);
            ViewData["ImageId"] = new SelectList(_context.Images, "Id", "Id", articleModel.ImageId);
            ViewData["TopicModelId"] = new SelectList(_context.Topics, "Id", "Description", articleModel.TopicModelId);
            return View(articleModel);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Summary,Body,Status,Created,Updated,Slug,ImageId,TopicModelId,CreatorId")] ArticleModel articleModel)
        {
            if (id != articleModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(articleModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleModelExists(articleModel.Id))
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
            ViewData["CreatorId"] = new SelectList(_context.Users, "Id", "Id", articleModel.CreatorId);
            ViewData["ImageId"] = new SelectList(_context.Images, "Id", "Id", articleModel.ImageId);
            ViewData["TopicModelId"] = new SelectList(_context.Topics, "Id", "Description", articleModel.TopicModelId);
            return View(articleModel);
        }

        // GET: Articles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var articleModel = await _context.Articles
                .Include(a => a.Creator)
                .Include(a => a.Image)
                .Include(a => a.TopicModel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (articleModel == null)
            {
                return NotFound();
            }

            return View(articleModel);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var articleModel = await _context.Articles.FindAsync(id);
            _context.Articles.Remove(articleModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleModelExists(int id)
        {
            return _context.Articles.Any(e => e.Id == id);
        }
    }
}
