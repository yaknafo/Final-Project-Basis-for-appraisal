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
using BasisForAppraisal_finalProject.Common.Constans;

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
            return PartialView(h);
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
        /// 
         [HttpPost]
         public ActionResult AddAnswerToQuestion(int id, tbl_IntentionalQuestion q)
        {

            
            var h = new tbl_IntentionalAnswer();
             return PartialView("_MultipleChoiceQuestion", h);
        }
       [HttpPost]
       public ActionResult IntentionalFormWorkshop(string submit, FormViewModel formViewModel)
       {

           var manager = new FormManager();
           ModelState.Clear();

           // delete answer
           if (submit.All(char.IsDigit))
               formViewModel.DeleteAnswer(Convert.ToInt32(submit));
           try
           {

               switch (submit)
               {
                   case "Exit": return backToMainForm();

                   case "AddAnswerToMulitiChoice": formViewModel.NewQuestionMultipleChoice.AddAnswerOption();
                       TempData["AddAnswerToMulitiChoice"] = "Add";
                       break;

                   case "addQustion": formViewModel.AddQuestion(formViewModel.NewQuestion);
                       TempData["Success"] = "הוספה בוצעה בהצלחה!";
                       TempData["changes"] = "שינויים לא שמורים";
                       break;


                   case "AddQustionFreeText": formViewModel.AddQuestion(formViewModel.NewQuestionFreeText);
                       TempData["Success"] = "הוספה בוצעה בהצלחה!";
                       TempData["changes"] = "שינויים לא שמורים";
                       break;
                   case "AddYesNoQuestion": formViewModel.AddQuestion(formViewModel.NewQuestionYesNo);
                       TempData["Success"] = "הוספה בוצעה בהצלחה!";
                       TempData["changes"] = "שינויים לא שמורים";
                       break;

                   case "AddScaleQuestion": formViewModel.AddQuestionScale(formViewModel.NewQuestionScale);
                       TempData["Success"] = "הוספה בוצעה בהצלחה!";
                       TempData["changes"] = "שינויים לא שמורים";
                       break;

                   case "AddMultipleChoiceQuestion": formViewModel.AddQuestionMultipleChoice(formViewModel.NewQuestionMultipleChoice);
                       TempData["Success"] = "הוספה בוצעה בהצלחה!";
                       TempData["changes"] = "שינויים לא שמורים";
                       break;

                   case "Save":
                       try
                       {
                           manager.UpdateForm(formViewModel);
                           TempData["Success"] = "שמירה בוצעה בהצלחה!";
                       }catch(Exception e)
                       {
                           TempData[ResultOperationConstans.Failed] = e.Message;
                       }
                       break;

                   case "Delete": formViewModel.DeleteQuestions();
                       TempData["Success"] = "מחיקה בוצעה בהצלחה!";
                       TempData["changes"] = "שינויים לא שמורים";
                       break;

                   case "SaveAndClose": manager.UpdateForm(formViewModel);
                       TempData["Success"] = "שמירה בוצעה בהצלחה!";
                       return backToMainForm();

               }
           }catch(Exception e)
           {
               TempData["Failed"] = "פעולה נכשלה";
           }
           if(Request.IsAjaxRequest())
           {
               return PartialView("_QuestionsLists", formViewModel);
           }
           return View(formViewModel);
       }



       [HttpGet]
       public ActionResult FormPreview(int formid=1 )
       {
           var DM= new DataManager();
           var formPreview = DM.GetFormWithSections(formid);
           return View(formPreview);
       }

       [HttpGet]
       public ActionResult backToMainForm()
       {
           return RedirectToAction("MainFormManagment", "ManageForm");
       }
       
	}
}