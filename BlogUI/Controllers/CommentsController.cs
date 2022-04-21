using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BlogLibrary.Data;
using BlogLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace BlogUI.Controllers
{
    public class CommentsController : Controller
    {
        private readonly BlogContext _context;
        private readonly UserManager<UserModel> _userManager;

        public CommentsController(BlogContext context, UserManager<UserModel> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Comments
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> OriginalIndex()
        {
            var originalComments = await _context.Comments.ToListAsync();
            return View("Index", originalComments);
        }
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> ModeratedIndex()
        {
            var moderatedComments = await _context.Comments.Where(c => c.Moderated != null).ToListAsync();
            return View("Index", moderatedComments);
        }

        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Index()
        {
            var allComments = await _context.Comments.ToListAsync();
            return View(allComments);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ArticleId,Comment")] CommentModel commentModel, string articleSlug)
        {
            if (ModelState.IsValid)
            {
                commentModel.CreatorId = _userManager.GetUserId(User);
                commentModel.Created = DateTime.Now;
                _context.Add(commentModel);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", "Articles", new { slug = articleSlug }, "commentSection");
            }
            ViewData["CreatorId"] = new SelectList(_context.AppUsers, "Id", "Id");
            ViewData["ArticleId"] = new SelectList(_context.Articles, "Id", "Title");
            return View(commentModel);
        }

        // GET: Comments/Edit/5
        [Authorize(Roles = "Administrator")]
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
            ViewData["CreatorId"] = new SelectList(_context.AppUsers, "Id", "Id", commentModel.CreatorId);
            ViewData["ArticleId"] = new SelectList(_context.Articles, "Id", "Title", commentModel.Article);
            return View(commentModel);
        }

        // POST: Comments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Body")] CommentModel commentModel)
        {
            if (id != commentModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var newComment = await _context.Comments.Include(c => c.Article).FirstOrDefaultAsync(c => c.Id == commentModel.Id);
                try
                {
                    newComment.Comment = commentModel.Comment;
                    newComment.Updated = DateTime.Now;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(commentModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Posts", new { slug = newComment.Article.Slug }, "commentSection");
            }
            ViewData["CreatorId"] = new SelectList(_context.AppUsers, "Id", "Id", commentModel.CreatorId);
            ViewData["ArticleId"] = new SelectList(_context.Articles, "Id", "Title", commentModel.ArticleId);
            return View(commentModel);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Moderate(int id, [Bind("Id,Body,ModeratedBody,ModerationType")] CommentModel commentModel)
        {
            if (id != commentModel.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                var newComment = await _context.Comments.Include(c => c.Article).FirstOrDefaultAsync(c => c.Id == commentModel.Id);
                try
                {
                    newComment.ModeratedComment = commentModel.ModeratedComment;
                    newComment.Moderated = DateTime.Now;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(commentModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Posts", new { slug = newComment.Article.Slug }, "commentSection");
            }
            return View(commentModel);
        }


        // GET: Comments/Delete/5
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _context.Comments
                .Include(c => c.Creator)
                .Include(c => c.Article)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, string articleSlug)
        {
            var comment = await _context.Comments.FindAsync(id);
            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Posts", new { slug = articleSlug }, "commentSection");
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.Id == id);
        }
    }
}
