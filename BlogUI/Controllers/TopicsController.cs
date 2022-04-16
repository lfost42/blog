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
    public class TopicsController : Controller
    {
        private readonly BlogContext _context;

        public TopicsController(BlogContext context)
        {
            _context = context;
        }

        // GET: Topics
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var blogContext = _context.Topics.Include(t => t.Creator);
            return View(await blogContext.ToListAsync());
        }

        // GET: Topics/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topicModel = await _context.Topics
                .Include(t => t.Creator)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (topicModel == null)
            {
                return NotFound();
            }

            return View(topicModel);
        }

        // GET: Topics/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["CreatorId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: Topics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Image")] TopicModel topicModel)
        {
            if (ModelState.IsValid)
            {
                topicModel.Created = DateTime.Now;
                _context.Add(topicModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CreatorId"] = new SelectList(_context.Users, "Id", "Id", topicModel.CreatorId);
            return View(topicModel);
        }

        // GET: Topics/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topicModel = await _context.Topics.FindAsync(id);
            if (topicModel == null)
            {
                return NotFound();
            }
            return View(topicModel);
        }

        // POST: Topics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Image.Image")] TopicModel topicModel)
        {
            if (id != topicModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(topicModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TopicModelExists(topicModel.Id))
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
            ViewData["CreatorId"] = new SelectList(_context.Users, "Id", "Id", topicModel.CreatorId);
            return View(topicModel);
        }

        // GET: Topics/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var topicModel = await _context.Topics
                .Include(t => t.Creator)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (topicModel == null)
            {
                return NotFound();
            }

            return View(topicModel);
        }

        // POST: Topics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var topicModel = await _context.Topics.FindAsync(id);
            _context.Topics.Remove(topicModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TopicModelExists(int id)
        {
            return _context.Topics.Any(e => e.Id == id);
        }
    }
}
