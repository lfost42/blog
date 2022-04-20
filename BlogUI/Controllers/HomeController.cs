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
        public async Task<IActionResult> Index()
        {
            var series = await _context.Series
                .Include(b => b.Creator)
                .ToListAsync();

            return View(series);
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
