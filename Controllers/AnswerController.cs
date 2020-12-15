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
    public class AnswerController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly ConnectionDBClass _db;

        public AnswerController(ConnectionDBClass db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            var results = _db.Answers.ToList();
            return View(results);
        }

        public IActionResult Create(int? id)
        {
            var results = _db.Questions.Where(x => x.questionnaire_id == id).ToList();
            var questionnaire = _db.Questionnaires.Where(x => x.id == id).ToList().First();
            ViewBag.topic_id = questionnaire.topic_id;
            ViewBag.questionaire_title = questionnaire.title;
            ViewBag.questions = results;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(AnswerClass ans)
        {
            if (ModelState.IsValid)
            {
                _db.Add(ans);
                await _db.SaveChangesAsync();
                //return RedirectToAction("Index");
                return RedirectToAction("Questionnaire", "Index", new { id = 2 });

            }
            return View(ans);
        }
    }
}
