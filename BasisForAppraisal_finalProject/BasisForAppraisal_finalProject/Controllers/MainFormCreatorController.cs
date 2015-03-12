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

        [HttpPost]
        public ActionResult Index(tblForm form)
        {
            var formManager = new FormManager();
            Task taskA = Task.Factory.StartNew(() => formManager.SaveForm(form));
            taskA.Wait();
            return View(form);
        }

        [HttpPost]
        public ActionResult addNewQuestion(tblForm form)
        {
            
            var formManager = new FormManager();
            Task taskA = Task.Factory.StartNew(() => formManager.SaveQuestionToForm(form.NewQuestion));
            taskA.Wait();
            return RedirectToAction("Index", new { id = form.formId });

        }

      


        
       public void deleteQustion(int formID, int quesNumber)
        {
            var b = new FormManager();
            b.deleteQustion(formID, quesNumber);

        }




       
	}
}