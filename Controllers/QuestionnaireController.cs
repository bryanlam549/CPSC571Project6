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
            return View(results);
        }

        public IActionResult Analyze(int? id)
        {
            var results = _db.Questionnaires.ToList();
            return View(results);
        }

        public IActionResult Select(int? id)
        {
            int count = _db.Answers.Where(x => x.questionnaire_Id == id).Count();
            if (count != 0)
            {
                var questionnaire = _db.Questionnaires.Where(x => x.id == id).ToList().First();
                ViewBag.topic_id = questionnaire.topic_id;
                return View("Attempted");
            }

            return RedirectToAction("Index", "Questions", new { id = id });

        }

        public IActionResult Edit(int? id)
        {
            return RedirectToAction("Index", "Questions", new { id = id });

        }

        public async Task<IActionResult> Delete(int? id)
        {
            var questionnaire = _db.Questionnaires.Where(x => x.id == id).ToList().First();
            ViewBag.topic_id = questionnaire.topic_id;
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            var toDelete = await _db.Questionnaires.FindAsync(id);
            return View(toDelete);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var questionnaire = _db.Questionnaires.Where(x => x.id == id).ToList().First();
            var topic_id = questionnaire.topic_id;

            var toDelete = await _db.Questionnaires.FindAsync(id);
            _db.Questionnaires.Remove(toDelete);

            await _db.SaveChangesAsync();

            return RedirectToAction("Index", new { id = topic_id });
        }
    }
}
