using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BlogUI.Models;
using BlogLibrary.Databases.Interfaces;
using BlogLibrary.Databases;
using BlogLibrary.Data;
using Microsoft.EntityFrameworkCore;
using BlogLibrary.Models.Enum;
using X.PagedList;

namespace BlogUI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogEmailService _emailSender;
        private readonly BlogContext _context;

        public HomeController(ILogger<HomeController> logger, IBlogEmailService emailSender, BlogContext context)
        {
            _logger = logger;
            _emailSender = emailSender;
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 5;

            var articles = _context.Articles.Where(a => a.Status == Status.Published)
                .Include(s => s.SeriesModel)
                .Include(s => s.Creator)
                .Include(s => s.Image)
                .OrderByDescending(s => s.Created)
                .ToPagedListAsync(pageNumber, pageSize);

            return View(await articles);
        }

        [HttpGet]
        public async Task<IActionResult> SeriesIndex(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 5;

            var series = _context.Series.Where(
                s => s.Articles.Any(a => a.Status == Status.Published))
                .Include(s => s.Creator)
                .Include(s => s.Image)
                .OrderByDescending(s => s.Created)
                .ToPagedListAsync(pageNumber, pageSize);

            return View(await series);
        }

        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ContactSettings model)
        {
            await _emailSender.SendContactEmailAsync(model.Email, model.Name, model.Subject, model.Message);
            return RedirectToAction("Index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
