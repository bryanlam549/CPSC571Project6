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


    }
}
