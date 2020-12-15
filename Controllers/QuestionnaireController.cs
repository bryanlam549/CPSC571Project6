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
    public class QuestionnaireController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly ConnectionDBClass _db;

        public QuestionnaireController(ConnectionDBClass db)
        {
            _db = db;
        }

        public IActionResult Index(int? id)
        {
            var results = _db.Questionnaires.Where(x => x.topic_id == id).ToList();
            ViewBag.topic_id = results.First().topic_id;
            return View(results);
        }

        /*public IActionResult Questionnaire()
        {
            var results = _db.Questionnaires.ToList();
            //results.Insert(0, new TopicClass { rowno = 0, subject_Name = "--Select Topic Name--" });
            return View(results);
        }*/

        public IActionResult Analyze(int? id)
        {
            var results = _db.Questionnaires.ToList();
            return View(results);
        }

        public IActionResult Create(int? tID)
        {
            ViewBag.tID = tID;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(QuestionnaireClass newTitle)
        {
            if (ModelState.IsValid)
            {
                _db.Add(newTitle);
                await _db.SaveChangesAsync();
                //return RedirectToAction("Topics");
                //return RedirectToAction("Index");
                return RedirectToAction("Index", new { id = newTitle.topic_id });

            }
            return View(newTitle);
        }


    }
}
