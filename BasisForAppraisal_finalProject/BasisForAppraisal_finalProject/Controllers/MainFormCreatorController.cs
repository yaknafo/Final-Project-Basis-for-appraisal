using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BasisForAppraisal_finalProject.DBML;
using BasisForAppraisal_finalProject.Models;

namespace BasisForAppraisal_finalProject.Controllers
{
    public class MainFormCreatorController : Controller
    {
        //
        // GET: /MainFormCreator/
        public ActionResult Index()
        {
            var b = new DataManager();
            var q = b.GetFormWithQuestion(1);
            return View(q);
        }

        public ActionResult addQuestion(int id)
        {
            var b = new DataManager();
            var q = b.GetFormWithQuestion(id);
            var newQuestion = new tbl_IntentionalQuestion(3,id,3);
            newQuestion.createAnswersToQuestion(3);
        
            b.saveQuestionToDB(newQuestion);
            return RedirectToAction("Index", new { id = id });
            
        }

        [HttpPost]
        public ActionResult Index(int id)
        {
            var b = new DataManager();
            var q = b.GetFormWithQuestion(id);
            return View(q);
        }

	}
}