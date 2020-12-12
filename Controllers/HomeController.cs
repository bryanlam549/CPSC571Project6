using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CPSC571Project6.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CPSC571Project6.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly ConnectionDBClass _db;

        public HomeController(ConnectionDBClass db)
        {
            _db = db;
        }

        public IActionResult Topics()
        {
            var results = _db.Topics.ToList();
            //results.Insert(0, new TopicClass { rowno = 0, subject_Name = "--Select Topic Name--" });
            return View(results);
        }

        public IActionResult Create()
        {

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> Create(TopicClass newTopic)
        {
            if (ModelState.IsValid)
            {
                _db.Add(newTopic);
                await _db.SaveChangesAsync();
                return RedirectToAction("Topics");

            }
            return View(newTopic);
        }
    }
}
