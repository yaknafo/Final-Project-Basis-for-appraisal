using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BasisForAppraisal_finalProject.DBML;
using BasisForAppraisal_finalProject.Models;
using System.Threading.Tasks;

namespace BasisForAppraisal_finalProject.Controllers
{
    public class MainFormCreatorController : Controller
    {
        //
        // GET: /MainFormCreator/
        [HttpGet]
        public ActionResult Index()
        {
            var b = new DataManager();
            var q = b.GetFormWithQuestion(1);
            return View(q);
        }

        public ActionResult addQuestion(int id)
        {
            var formManager = new FormManager();
            Task taskA = Task.Factory.StartNew(() => formManager.addQuestionToForm(id));
            taskA.Wait();
            return RedirectToAction("Index", new { id = id });
            
        }

        [HttpPost]
        public ActionResult Index(int id)
        {
            var b = new DataManager();
            var q = b.GetFormWithQuestion(id);
            return View(q);
        }
        
       public void deleteQustion(int formID, int quesNumber)
        {
            var b = new FormManager();
            b.deleteQustion(formID, quesNumber);

        }




       
	}
}