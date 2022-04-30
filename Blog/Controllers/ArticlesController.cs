using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Blog.Data.Data;
using Blog.Data.Models;
using Microsoft.AspNetCore.Identity;
using Blog.Data.Databases.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Extensions.Configuration;
using Blog.Data.Models.Enum;
using X.PagedList;

namespace BlogUI.Controllers
{
    public class ArticlesController : Controller
    {
        private readonly BlogContext _context;
        private readonly IImageService _imageService;
        private readonly UserManager<UserModel> _userManager;
        private readonly ISlugService _slugService;
        private readonly IConfiguration _config;
        private readonly ISearchService _searchService;

        public ArticlesController(BlogContext context, ISlugService slugService, IImageService imageService, UserManager<UserModel> userManager, IConfiguration config, ISearchService searchService)
        {
            _context = context;
            _slugService = slugService;
            _imageService = imageService;
            _userManager = userManager;
            _config = config;
            _searchService = searchService;
        }

        [HttpPost]
        public async Task<IActionResult> SearchIndex(int? page, string searchTerm)
        {
            ViewData["SearchTerm"] = searchTerm;

            var pageNumber = page ?? 1;
            var pageSize = 6;

            var articles = _searchService.Search(searchTerm)
                .Include(a => a.Creator)
                .Include(a => a.Image)
                .Include(a => a.Tags);

            return View(await articles.ToPagedListAsync(pageNumber, pageSize));
        }

        // GET: Articles
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var blogContext = _context.Articles.Include(a => a.SeriesModel)
                .Include(a => a.Image)
                .Include(a => a.SeriesModel);
            return View(await blogContext.ToListAsync());
        }

        public async Task<IActionResult> TagIndex(int? id, int? page, string tag)
        {
            if (id is null)
            {
                return NotFound();
            }

            var pageNumber = page ?? 1;
            var pageSize = 5;

            var articleModels = await _context.Articles.Where(p => p.Tags.Any(t => t.Tag.ToLower() == tag) && p.Status == Status.Published)
                .Include(a => a.SeriesModel)
                .Include(a => a.Image)
                .OrderByDescending(p => p.Created)
                .ToPagedListAsync(pageNumber, pageSize);

            return View("ArticleIndex", articleModels);
        }

        public async Task<IActionResult> ArticleIndex(int? id, int? page)
        {
            if(id is null)
            {
                return NotFound();
            }

            var pageNumber = page ?? 1;
            var pageSize = 5;

            var articles = await _context.Articles
                .Where(a => a.SeriesModelId == id && a.Status == Status.Published)
                .Include(s => s.SeriesModel)
                .Include(s => s.Creator)
                .Include(s => s.Image)
                .OrderByDescending(s => s.Created)
                .ToPagedListAsync(pageNumber, pageSize);

            return View(articles);
        }

        // GET: Articles/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return NotFound();
            }

            var articleModel = await _context.Articles
                .Include(a => a.Creator)
                .Include(a => a.Image)
                .Include(a => a.Tags)
                .Include(a => a.SeriesModel)
                .Include(a => a.Comments).ThenInclude(c => c.Creator)
                .FirstOrDefaultAsync(m => m.Slug == slug);

            if (articleModel == null)
            {
                return NotFound();
            }

            return View(articleModel);
        }

        // GET: Articles/Create
        [HttpGet]
        [Authorize(Roles = "Owner")]
        public IActionResult Create()
        {
            ViewData["SeriesModelId"] = new SelectList(_context.Series, "Id", "Title");
            ViewData["ArticleId"] = new SelectList(_context.Articles, "Id", "Title");
            ViewData["Image"] = new SelectList(_context.Images, "Image.Photo", "Image.Photo");
            ViewData["CreatorId"] = new SelectList(_context.AppUsers, "Id", "Id");
            return View();
        }

        // POST: Articles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Summary,Body,Status,Image,SeriesModelId")] ArticleModel articleModel, List<string> tagValues)
        {
            if (ModelState.IsValid)
            {
                articleModel.Created = DateTime.Now;
                articleModel.CreatorId = _userManager.GetUserId(User);

                if (articleModel.Image is not null)
                {
                    articleModel.Image.ImageData = await _imageService.EncodeImageAsync(articleModel.Image.Photo);
                    articleModel.Image.ImageExtension = _imageService.ContentType(articleModel.Image.Photo);
                }
                else
                {
                    articleModel.Image = new ImageModel();
                    articleModel.Image.ImageData = await _imageService.EncodeImageAsync(_config["DefaultImage"]);
                    articleModel.Image.ImageExtension = Path.GetExtension(_config["DefaultImage"]);

                }

                var slug = _slugService.UrlRoute(articleModel.Title);

                if (!_slugService.IsUnique(slug))
                {
                    articleModel.Slug = slug + "_1";
                }

                articleModel.Slug = slug;

                _context.Add(articleModel);
                await _context.SaveChangesAsync();

                foreach (var tag in tagValues)
                {
                    _context.Add(new TagModel()
                    {
                        ArticleModelId = articleModel.Id,
                        CreatorId = articleModel.CreatorId,
                        Tag = tag
                    });
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TagValues"] = string.Join(",", tagValues);
            ViewData["CreatorId"] = new SelectList(_context.AppUsers, "Id", "Id", articleModel.CreatorId);
            ViewData["SeriesModelId"] = new SelectList(_context.Series, "Id", "Title", articleModel.SeriesModelId);
            return View(articleModel);
        }

        // GET: Articles/Edit/5
        [HttpGet]
        [Authorize(Roles = "Owner")]
        public async Task<IActionResult> Edit(string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return NotFound();
            }

            var articleModel = await _context.Articles
                .Include(a => a.Creator)
                .Include(a => a.Image)
                .Include(a => a.Tags)
                .Include(a => a.SeriesModel)
                .FirstOrDefaultAsync(m => m.Slug == slug);

            if (articleModel == null)
            {
                return NotFound();
            }

            ViewData["NewImage"] = new SelectList(_context.Images, "NewImage", "NewImage", articleModel.Image.Photo);
            ViewData["SeriesModelId"] = new SelectList(_context.Series, "Id", "Title", articleModel.SeriesModelId);
            ViewData["CreatorId"] = new SelectList(_context.AppUsers, "Id", "Id", articleModel.CreatorId);
            ViewData["TagValues"] = string.Join(",", articleModel.Tags.Select(t => t.Tag));
            return View(articleModel);
        }

        // POST: Articles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SeriesModelId,Title,Summary,Body,Status")] ArticleModel articleModel, IFormFile newImage, List<string> tagValues)
        {
            if (id != articleModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var newArticle = await _context.Articles
                                    .Include(a => a.Creator)
                                    .Include(a => a.Image)
                                    .Include(a => a.Tags)
                                    .Include(a => a.SeriesModel)
                                    .FirstOrDefaultAsync(m => m.Id == id);

                    newArticle.SeriesModelId = articleModel.SeriesModelId;
                    newArticle.Updated = DateTime.Now;
                    newArticle.Title = articleModel.Title;
                    newArticle.Summary = articleModel.Summary;
                    newArticle.Body = articleModel.Body;
                    newArticle.Status = articleModel.Status;

                    if (newImage is not null)
                    {
                        newArticle.Image.ImageData = await _imageService.EncodeImageAsync(newImage);
                        newArticle.Image.ImageExtension = _imageService.ContentType(newImage);
                    }

                    var newSlug = _slugService.UrlRoute(articleModel.Title);

                    if (newSlug != newArticle.Slug)
                    {
                        if (_slugService.IsUnique(newSlug))
                        {
                            newArticle.Title = articleModel.Title;
                            newArticle.Slug = newSlug;
                        }
                        else
                        {
                            newArticle.Slug = newSlug + "_i";
                        }
                    }

                    if(articleModel.Tags != null)
                    {
                        _context.Tags.RemoveRange(newArticle.Tags);
                        foreach (var tag in tagValues)
                        {
                            _context.Add(new TagModel()
                            {
                                ArticleModelId = articleModel.Id,
                                CreatorId = articleModel.CreatorId,
                                Tag = tag
                            });
                        }
                    }
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleModelExists(articleModel.Slug))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("Details", "Articles", new { slug = _slugService.UrlRoute(articleModel.Title) } );
     
            }

            ViewData["CreatorId"] = new SelectList(_context.AppUsers, "Id", "Id", articleModel.CreatorId);
            ViewData["Image"] = new SelectList(_context.Series, "Image.Photo", "Image.Photo", articleModel.Image.Id);
            ViewData["SeriesModelId"] = new SelectList(_context.Series, "Id", "Title", articleModel.SeriesModelId);
            return View(articleModel);
        }

        // GET: Articles/Delete/5
        [HttpGet]
        public async Task<IActionResult> Delete(string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return NotFound();
            }

            var articleModel = await _context.Articles
                .Include(a => a.SeriesModel)
                .Include(a => a.Image)
                .Include(a => a.Tags)
                .FirstOrDefaultAsync(m => m.Slug == slug);
            if (articleModel == null)
            {
                return NotFound();
            }

            return View(articleModel);
        }

        // POST: Articles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string slug)
        {
            var articleModel = await _context.Articles.FindAsync(slug);
            _context.Articles.Remove(articleModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleModelExists(string slug)
        {
            return _context.Articles.Any(e => e.Slug == slug);
        }
    }
}
