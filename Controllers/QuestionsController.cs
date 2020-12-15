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
using System.Data.SqlClient;
using Microsoft.IdentityModel.Protocols;
using System.Configuration;
using System.Data;
using Microsoft.AspNetCore.Http;
using CPSC571Project6.Areas.Identity.Data;

namespace CPSC571Project6.Controllers
{
    public class QuestionsController : Controller
    {
        //private readonly ILogger<HomeController> _logger;
        private readonly ConnectionDBClass _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public QuestionsController(ConnectionDBClass db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Index(int? id)
        {
            var results = _db.Questions.Where(x => x.questionnaire_id == id).ToList();
            var questionnaire = _db.Questionnaires.Where(x => x.id == id).ToList().First();
            ViewBag.topic_id = questionnaire.topic_id;
            ViewBag.questionnaire_title = questionnaire.title;
            ViewBag.questionnaire_id = questionnaire.id;
            
            return View(results);

        }

        

        [HttpPost]
        public IActionResult Submit(IFormCollection iformCollection, int? id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);

            string[] questionsId = iformCollection["questionId"];
            foreach (var questionId in questionsId)
            {
                int answer = int.Parse(iformCollection[questionId]);

                string connString = "Data Source=cpsc571server.database.windows.net;Initial Catalog=CPSC571Database;User ID=admin123;Password=Applepie1";
                SqlConnection sqlcon = new SqlConnection(connString);
                String pname = "AnswersDataInsert";
                sqlcon.Open();
                SqlCommand com = new SqlCommand(pname, sqlcon);
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.Parameters.AddWithValue("@user_id", userId);
                com.Parameters.AddWithValue("@questionnaire_id", id);
                com.Parameters.AddWithValue("@question_id", questionId);
                com.Parameters.AddWithValue("@answer", answer);
                com.ExecuteNonQuery();
                sqlcon.Close();
            }

            //var questionnaire = _db.Questionnaires.Where(x => x.id == id).ToList().First();
            //ViewBag.topic_id = questionnaire.topic_id;
            //Should redirect to...thanks for taking the questionnaire and then that should redirect to home page?

            var questionnaire = _db.Questionnaires.Where(x => x.id == id).ToList().First();
            ViewBag.topic_id = questionnaire.topic_id;

            return View("Result");



        }


        //create
        public IActionResult Create(int? qid)
        {
            ViewBag.questionnaire_id = qid;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(QuestionClass newQuestion)
        {
            if (ModelState.IsValid)
            {
                _db.Add(newQuestion);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index", new { id = newQuestion.questionnaire_id});

            }
            return View(newQuestion);
        }

        //Edit
        public async Task<IActionResult> Edit(int? id)
        {
            var question = _db.Questions.Where(x => x.id == id).ToList().First();
            var questionnaire_id = question.questionnaire_id;
            if (id == null)
            {
                return RedirectToAction("Index", new { id = questionnaire_id });
            }

            var toEdit = await _db.Questions.FindAsync(id);
            return View(toEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(QuestionClass editQuestion)
        {
            if (ModelState.IsValid)
            {
                var x = editQuestion.questionnaire_id;
                _db.Update(editQuestion);
                await _db.SaveChangesAsync();
                return RedirectToAction("Index", new { id = editQuestion.questionnaire_id });
            }
            return View(editQuestion);

        }

        //Delete
        public async Task<IActionResult> Delete(int? id)
        {
            var question = _db.Questions.Where(x => x.id == id).ToList().First();
            var questionnaire_id = question.questionnaire_id;
            if (id == null)
            {
                return RedirectToAction("Index", new { id = questionnaire_id });
            }

            var toDelete = await _db.Questions.FindAsync(id);
            return View(toDelete);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var question = _db.Questions.Where(x => x.id == id).ToList().First();
            var questionnaire_id = question.questionnaire_id;

            var toDelete = await _db.Questions.FindAsync(id);
            _db.Questions.Remove(toDelete);

            await _db.SaveChangesAsync();

            return RedirectToAction("Index", new { id = questionnaire_id });
        }

    }
}
