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
    public class PostsController : Controller
    {
        private readonly BlogContext _context;

        public PostsController(BlogContext context)
        {
            _context = context;
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            var blogContext = _context.Posts.Include(p => p.BlogModel).Include(p => p.Creator).Include(p => p.FileModel);
            return View(await blogContext.ToListAsync());
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postModel = await _context.Posts
                .Include(p => p.BlogModel)
                .Include(p => p.Creator)
                .Include(p => p.FileModel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (postModel == null)
            {
                return NotFound();
            }

            return View(postModel);
        }

        // GET: Posts/Create
        public IActionResult Create()
        {
            ViewData["BlogModelId"] = new SelectList(_context.Blogs, "Id", "Description");
            ViewData["CreatorId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["FileModelId"] = new SelectList(_context.Files, "Id", "Id");
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Summary,PostBody,Status,Created,Updated,Slug,FileModelId,BlogModelId,CreatorId")] PostModel postModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(postModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BlogModelId"] = new SelectList(_context.Blogs, "Id", "Description", postModel.BlogModelId);
            ViewData["CreatorId"] = new SelectList(_context.Users, "Id", "Id", postModel.CreatorId);
            ViewData["FileModelId"] = new SelectList(_context.Files, "Id", "Id", postModel.FileModelId);
            return View(postModel);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postModel = await _context.Posts.FindAsync(id);
            if (postModel == null)
            {
                return NotFound();
            }
            ViewData["BlogModelId"] = new SelectList(_context.Blogs, "Id", "Description", postModel.BlogModelId);
            ViewData["CreatorId"] = new SelectList(_context.Users, "Id", "Id", postModel.CreatorId);
            ViewData["FileModelId"] = new SelectList(_context.Files, "Id", "Id", postModel.FileModelId);
            return View(postModel);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Summary,PostBody,Status,Created,Updated,Slug,FileModelId,BlogModelId,CreatorId")] PostModel postModel)
        {
            if (id != postModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(postModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostModelExists(postModel.Id))
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
            ViewData["BlogModelId"] = new SelectList(_context.Blogs, "Id", "Description", postModel.BlogModelId);
            ViewData["CreatorId"] = new SelectList(_context.Users, "Id", "Id", postModel.CreatorId);
            ViewData["FileModelId"] = new SelectList(_context.Files, "Id", "Id", postModel.FileModelId);
            return View(postModel);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postModel = await _context.Posts
                .Include(p => p.BlogModel)
                .Include(p => p.Creator)
                .Include(p => p.FileModel)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (postModel == null)
            {
                return NotFound();
            }

            return View(postModel);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var postModel = await _context.Posts.FindAsync(id);
            _context.Posts.Remove(postModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostModelExists(int id)
        {
            return _context.Posts.Any(e => e.Id == id);
        }
    }
}
