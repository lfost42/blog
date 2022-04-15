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
    public class CommentsController : Controller
    {
        private readonly BlogContext _context;

        public CommentsController(BlogContext context)
        {
            _context = context;
        }

        // GET: Comments
        public async Task<IActionResult> Index()
        {
            var blogContext = _context.Comments.Include(c => c.FileModel).Include(c => c.Post);
            return View(await blogContext.ToListAsync());
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commentModel = await _context.Comments
                .Include(c => c.FileModel)
                .Include(c => c.Post)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (commentModel == null)
            {
                return NotFound();
            }

            return View(commentModel);
        }

        // GET: Comments/Create
        public IActionResult Create()
        {
            ViewData["FileModelId"] = new SelectList(_context.Files, "Id", "Id");
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "PostBody");
            return View();
        }

        // POST: Comments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Subject,Comment,Created,Updated,Moderated,Deleted,FileModelId,PostId,CreatorId,ModeratedComment,Type")] CommentModel commentModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(commentModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FileModelId"] = new SelectList(_context.Files, "Id", "Id", commentModel.FileModelId);
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "PostBody", commentModel.PostId);
            return View(commentModel);
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commentModel = await _context.Comments.FindAsync(id);
            if (commentModel == null)
            {
                return NotFound();
            }
            ViewData["FileModelId"] = new SelectList(_context.Files, "Id", "Id", commentModel.FileModelId);
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "PostBody", commentModel.PostId);
            return View(commentModel);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Subject,Comment,Created,Updated,Moderated,Deleted,FileModelId,PostId,CreatorId,ModeratedComment,Type")] CommentModel commentModel)
        {
            if (id != commentModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(commentModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentModelExists(commentModel.Id))
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
            ViewData["FileModelId"] = new SelectList(_context.Files, "Id", "Id", commentModel.FileModelId);
            ViewData["PostId"] = new SelectList(_context.Posts, "Id", "PostBody", commentModel.PostId);
            return View(commentModel);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commentModel = await _context.Comments
                .Include(c => c.FileModel)
                .Include(c => c.Post)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (commentModel == null)
            {
                return NotFound();
            }

            return View(commentModel);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var commentModel = await _context.Comments.FindAsync(id);
            _context.Comments.Remove(commentModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommentModelExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}
