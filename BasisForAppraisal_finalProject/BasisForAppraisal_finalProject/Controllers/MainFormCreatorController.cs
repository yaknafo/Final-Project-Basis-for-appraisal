using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BasisForAppraisal_finalProject.DBML;
using BasisForAppraisal_finalProject.ViewModel;
using BasisForAppraisal_finalProject.Models;
using System.Threading.Tasks;
using System.Globalization;

namespace BasisForAppraisal_finalProject.Controllers
{
    public class MainFormCreatorController : Controller
    {



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
       /// saving/addQustion/exit/ SaveAndClose the form with all his changes
        /// </summary>
        /// <param name="formViewModel"></param>
        /// <returns></returns>
       [HttpPost]
       public ActionResult IntentionalFormWorkshop(string submit, FormViewModel formViewModel)
       {

           var manager = new FormManager();
           ModelState.Clear();

           switch(submit)
           {
               case "exit":       return backToMainForm();

               case "addQustion": formViewModel.AddQuestion(formViewModel.NewQuestion);
                                  TempData["Success"] = "הוספה בוצעה בהצלחה!";
                                  TempData["changes"] = "add";
                                  break;

               case "Save":   manager.UpdateForm(formViewModel);
                              TempData["Success"] = "שמירה בוצעה בהצלחה!";
                               break;
                
               case "delete": formViewModel.DeleteQuestions();
                              TempData["Success"] = "מחיקה בוצעה בהצלחה!";
                              TempData["changes"] = "remove";
                              break;

               case "SaveAndClose": manager.UpdateForm(formViewModel);
                                   TempData["Success"] = "שמירה בוצעה בהצלחה!";
                                   return backToMainForm();

           }
           return View(formViewModel);
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