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
using Microsoft.AspNetCore.Identity;
using CPSC571Project6.Areas.Identity.Data;

namespace CPSC571Project6.Controllers
{
    public class QuestionnaireController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly ConnectionDBClass _db;
        private readonly UserManager<ApplicationUser> _userManager;
        public QuestionnaireController(ConnectionDBClass db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Index(int? id)
        {
            var results = _db.Questionnaires.Where(x => x.topic_id == id).ToList();
            ViewBag.tID = results.First().topic_id;

            return View(results);
        }

        public IActionResult Analyze(int? id)
        {
            var results = _db.Answers.Where(x => x.questionnaire_Id == id).ToList();
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
                return RedirectToAction("Index", new { id = newTitle.topic_id });

            }
            return View(newTitle);
        }
        public IActionResult Select(int? id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            int count = _db.Answers.Where(x => x.questionnaire_Id == id && x.user_Id == userId).Count();
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

        public async Task<IActionResult> EditTitle(int? id)
        {
            var results = _db.Questions.Where(x => x.questionnaire_id == id).ToList();
            var questionnaire = _db.Questionnaires.Where(x => x.id == id).ToList().First();
            ViewBag.questionnaire_id = questionnaire.id;
            if (id == null)
            {
                return RedirectToAction("Index", "Questions", new { id = ViewBag.questionnaire_id });
            }

            var toEdit = await _db.Questionnaires.FindAsync(id);
            return View(toEdit);
        }

        [HttpPost]
        public async Task<IActionResult> EditTitle(QuestionnaireClass editQuestionnaire)
        {
            if (ModelState.IsValid)
            {
                _db.Update(editQuestionnaire);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index", "Questions", new { id = editQuestionnaire.id });
            }
            return View(editQuestionnaire);

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
