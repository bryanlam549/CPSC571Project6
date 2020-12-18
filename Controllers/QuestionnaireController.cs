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
using Cotur.DataMining.Association;

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
            var topic = _db.Topics.Where(x => x.rowno == id).ToList();
            ViewBag.tID = topic.First().rowno;
            

            return View(results);
        }

        public IActionResult Analyze(int? id, float? support, float? confidence)
        {
            var taken = _db.Answers.Where(x => x.questionnaire_Id == id).Select(c => c.user_Id).Distinct().ToList();
            var questionnaire = _db.Questionnaires.Where(x => x.id == id).ToList().First();
            var answers = _db.Answers.Where(x => x.questionnaire_Id == id).ToList();
            var results = _db.Analyze.Where(x => x.questionnaire_Id == id).ToList();
            var sp = support;
            var cf = confidence;

            ViewBag.taken_Count = taken.Count();
            ViewBag.title = questionnaire.title;
            ViewBag.topic_id = questionnaire.topic_id;  
            Apriori ap = PopulateApriori(id);

            
            if (sp == null)
            {
                sp = 0.5f;
            }

            if (cf == null)
            {
                cf = 0.5f;
            }
            ViewBag.sp = sp;
            ViewBag.cf = cf;
            ViewBag.ap = ap;
            
            if(answers.Count() != 0) { 
                ap.CalculateCNodes((float)sp);
                ViewBag.Rules = ap.Rules.Where(x => x.Confidence >= cf);
            }



            return View(results);
        }


        public Apriori PopulateApriori(int? id)
        {
            //Users who took this questionnaire will be used to populate transactions list
            var taken = _db.Answers.Where(x => x.questionnaire_Id == id).Select(c => c.user_Id).Distinct().ToList();
            
            //This will be used to populate fieldnames
            var questions = _db.Questions.Where(x => x.questionnaire_id == id).ToList();



            List<string> fieldnames = new List<string>();

            for(int i = 0; i < questions.Count(); i++)
            {
                fieldnames.Add("Question: " + (i + 1) + " Value: " + 1);
                fieldnames.Add("Question: " + (i + 1) + " Value: " + 2);
                fieldnames.Add("Question: " + (i + 1) + " Value: " + 3);
                fieldnames.Add("Question: " + (i + 1) + " Value: " + 4);
                fieldnames.Add("Question: " + (i + 1) + " Value: " + 5);
            }


            List<List<int>> Transactions = new List<List<int>>();
            foreach (var uid in taken)
            {
                var answers = _db.Answers.Where(x => x.user_Id == uid && x.questionnaire_Id == id).ToList();
                List<int> transaction = new List<int>();

                for(int i = 0; i < answers.Count(); i++)
                {
                    var t2 = answers[i].answer;
                    var t = (answers[i].answer - 1) + (i * 5);
                    
                    transaction.Add((answers[i].answer - 1) + (i*5));
                }
                Transactions.Add(transaction);
            }

            Apriori myApriori = new Apriori(
                new DataFields(fieldnames.Count(), Transactions, fieldnames)
                );

            return myApriori;
        }

        [HttpPost]
        public async Task<IActionResult> Analyze(float? sp, float? conf)
        {
            return RedirectToAction("Analyze", new { support = sp, confidence = conf });
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
