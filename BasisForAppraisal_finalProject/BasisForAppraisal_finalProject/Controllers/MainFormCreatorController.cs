using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BasisForAppraisal_finalProject.DBML;
using BasisForAppraisal_finalProject.ViewModel;
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
        public ActionResult addNewQuestion(FormViewModel form)
        {
            
            var formManager = new FormManager();
            Task taskA = Task.Factory.StartNew(() => formManager.SaveQuestionToForm(form.NewQuestion));
            taskA.Wait();
            TempData["Success"] = "הוספה בוצעה בהצלחה!";
            return RedirectToAction("IntentionalFormWorkshop", new { id = form.formId });

        }



        public ActionResult deleteQustion(int formID, int quesNumber)
        {
            var b = new FormManager();
            b.deleteQustion(formID, quesNumber);
            return RedirectToAction("IntentionalFormWorkshop", new { id = formID });

        }

       public ActionResult IntentionalFormWorkshop(int id=0)
       {
           var b = new DataManager();
           var q = b.GetFormWithQuestion(id);
           var t = new ViewModel.FormViewModel(q);
           return View(t);
       }

        /// <summary>
        /// saving the form with all his changes
        /// </summary>
        /// <param name="formViewModel"></param>
        /// <returns></returns>
       [HttpPost]
       public ActionResult IntentionalFormWorkshop(FormViewModel formViewModel)
       {
           var manager = new FormManager();
           manager.UpdateForm(formViewModel);
         
               TempData["Success"] = "שמירה בוצעה בהצלחה!";
           return RedirectToAction("IntentionalFormWorkshop",formViewModel.formId);
       }

       [HttpGet]
       public ActionResult preview(int formid=1 )
       {
           var b = new DataManager();
           var q = b.GetFormWithQuestion(formid);
           var t = new ViewModel.FormViewModel(q);
           return View(t);
       }

       [HttpGet]
       public ActionResult backToMainForm()
       {
           return RedirectToAction("MainFormManagment", "ManageForm");
       }
       
	}
}