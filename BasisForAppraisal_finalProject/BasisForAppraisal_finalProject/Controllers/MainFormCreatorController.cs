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

       public ActionResult IntentionalFormWorkshop(int id=0)
       {
           var b = new DataManager();
           var q = b.GetFormWithSections(id);
           var t = new ViewModel.FormViewModel(q);
           return View(t);
       }


        //---------------!!!@@@@ play ground @@@@@!!!!!!------------------------
        [HttpGet]
       public ActionResult Int(int id = 0)
       {
           var b = new DataManager();
           var q = b.GetFormWithSections(2021);
           var t = new ViewModel.FormViewModel(q);
           if(Request.IsAjaxRequest())
           {
               //var c = new tbl_Section { SectionId = 1 };
               //formViewModel.CurrentSection = c;
               //_intetionalQusestionPartialForAdd
               return PartialView("_intetionalQusestionPartialForAdd", t.NewQuestionScale);
           }
          
           return View(t.NewQuestionFreeText);
       }

        [HttpGet]
        public PartialViewResult changeQuestion()
        {
            var h = new tbl_IntentionalQuestion() { HelpText = "yari knafo" };
            return PartialView("_example", h);
        }
       // [HttpPost]
       //public ActionResult Int(FormViewModel formViewModel)
       //{

       //    var c = new tbl_Section { SectionId = 1 };
       //    formViewModel.CurrentSection = c;
       //    //_intetionalQusestionPartialForAdd
       //    return PartialView("_intetionalQusestionPartialForAdd", formViewModel.NewQuestionScale);
       //}

        //---------------!!!@@@@ play ground @@@@@!!!!!!------------------------
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
               case "Exit":       return backToMainForm();

               case "addQustion": formViewModel.AddQuestion(formViewModel.NewQuestion);
                                  TempData["Success"] = "הוספה בוצעה בהצלחה!";
                                  TempData["changes"] = "add";
                                  break;

               case "AddQustionFreeText": formViewModel.AddQuestion(formViewModel.NewQuestionFreeText);
                                          TempData["Success"] = "הוספה בוצעה בהצלחה!";
                                          TempData["changes"] = "add";
                                          break;

               case "AddScaleQuestion": formViewModel.AddQuestionScale(formViewModel.NewQuestionScale);
                                        TempData["Success"] = "הוספה בוצעה בהצלחה!";
                                        TempData["changes"] = "add";
                                        break;

               case "Save":   manager.UpdateForm(formViewModel);
                              TempData["Success"] = "שמירה בוצעה בהצלחה!";
                               break;
                
               case "Delete": formViewModel.DeleteQuestions();
                              TempData["Success"] = "מחיקה בוצעה בהצלחה!";
                              TempData["changes"] = "remove";
                              break;

               case "SaveAndClose": manager.UpdateForm(formViewModel);
                                   TempData["Success"] = "שמירה בוצעה בהצלחה!";
                                   return backToMainForm();

           }
           if(Request.IsAjaxRequest())
           {
               return PartialView("_QuestionsLists", formViewModel);
           }
           return View(formViewModel);
       }



       [HttpGet]
       public ActionResult preview(int formid=1 )
       {
           var b = new DataManager();
           var q = b.GetFormWithSections(formid);
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