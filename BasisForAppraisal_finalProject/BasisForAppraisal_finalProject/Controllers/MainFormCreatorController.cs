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
using BasisForAppraisal_finalProject.Authorize;

namespace BasisForAppraisal_finalProject.Controllers
{

    [CustomAuthorizeAttribute(Roles = "Admin")]
    public class MainFormCreatorController : Controller
    {

       public ActionResult IntentionalFormWorkshop(int id=0)
       {
           var b = new DataManager();
           var q = b.GetFormWithSections(id);
           var t = new ViewModel.FormViewModel(q);
           ViewBag.MovieType = GetDropDown();
           ViewData["ListData"] = GetDropDown();
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
         public ActionResult IntentionalFormWorkshop(string submit, FormViewModel formViewModel, string Dep ,string question)
       {
           ViewBag.MovieType = GetDropDown();
           ViewData["ListData"] = GetDropDown();

         

           var manager = new FormManager();
           ModelState.Clear();

           if(submit == null)
           submit = Dep;
           try
           {

               switch (submit)
               {

                   case "Exit": return backToMainForm();

                   case "AddAnswerToMulitiChoice": formViewModel.NewQuestionMultipleChoice.AddAnswerOption();
                       //TempData["AddAnswerToMulitiChoice"] = "Add";
                       break;

                   case "AddAnswerToMulitiChoiceList": formViewModel.NewQuestionMultipleChoiceList.AddAnswerOption();
                       //TempData["MultipleChoiceListQuestion"] = "Add";
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

                   case "AddCbxQuestion": formViewModel.AddQuestionCbx(formViewModel.NewQuestionCbx);
                       TempData["Success"] = "הוספה בוצעה בהצלחה!";
                       TempData["changes"] = "שינויים לא שמורים";
                       break;

                   case "AddMultipleChoiceListQuestion": formViewModel.AddQuestionMultipleChoice(formViewModel.NewQuestionMultipleChoiceList);
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

       public ActionResult CreateNewBook()
       {

           var bookViewModel = new tbl_IntentionalAnswer();

           return PartialView("~/Views/MainFormCreator/EditorTemplates/tbl_IntentionalAnswer.cshtml", bookViewModel);

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


       public ActionResult NewIntentionalFormWorkshop(int id = 0)
       {
             var dataManager = new DataManager();
           try
           {
               var q = dataManager.GetFormWithSections(id);
               var t = new ViewModel.FormViewModel(q);
               ViewBag.MovieType = GetDropDown();
               ViewData["ListData"] = GetDropDown();
               ViewBag.NV = "true";
               return View(t);
           }
           catch(Exception ex)
           {
               TempData[ResultOperationConstans.Failed] = ex.Message;
               return backToMainForm();

           }
       }


       [HttpPost]
       public ActionResult NewIntentionalFormWorkshop(string submit, FormViewModel formViewModel, string add, string question)
       {
           ViewData["ListData"] = GetDropDown();
           ViewBag.NV = "true";
           var manager = new FormManager();
           ModelState.Clear();

           if (question != null)
           {
               char[] splitQuestionNumberAction ={' '};
               var questionNum = question.Split(splitQuestionNumberAction).First();
               var action = question.Split(splitQuestionNumberAction).Last();

               if(action == "delete")
               formViewModel.DeleteQuestion(Int32.Parse(questionNum));
               //else if(action == "edit")

               else if (action == "AddAnswer")
                   formViewModel.AddAnswerOptionToQuestoin((Int32.Parse(questionNum)));

              
           }

           else if(submit != null)
           {
               switch (submit)
               {
                   case "Save":
                       try
                       {
                           manager.UpdateForm(formViewModel);
                           TempData["Success"] = "שמירה בוצעה בהצלחה!";
                       }
                       catch (Exception e)
                       {
                           TempData[ResultOperationConstans.Failed] = e.Message;
                       }
                       break;
               }
           }

           else if (add != string.Empty)
           {
               formViewModel.AddQuestion(add);
           }

           if(Request.IsAjaxRequest())
           {
               return PartialView("_QuestionsLists", formViewModel);
           }
              
           
           return View(formViewModel);
       }



       private static void AddQuestion(FormViewModel formViewModel, string add)
       {
           try
           {

               switch (add)
               {
                   case "addQustion": formViewModel.AddQuestion(formViewModel.NewQuestion);
                       break;

                   case "AddQustionFreeText": formViewModel.AddQuestion(formViewModel.NewQuestionFreeText);
                       break;
                   case "AddYesNoQuestion": formViewModel.AddQuestion(formViewModel.NewQuestionYesNo);
                       break;

                   case "AddScaleQuestion": formViewModel.AddQuestionScale(formViewModel.NewQuestionScale);
                       break;

                   case "AddMultipleChoiceQuestion": formViewModel.AddQuestionMultipleChoice(formViewModel.NewQuestionMultipleChoice);
                       break;

                   case "AddCbxQuestion": formViewModel.AddQuestionCbx(formViewModel.NewQuestionCbx);
                       break;

                   case "AddMultipleChoiceListQuestion": formViewModel.AddQuestionMultipleChoice(formViewModel.NewQuestionMultipleChoiceList);
                       break;

                   case "AddNewQuestionSocialStatus": formViewModel.AddQuestionMultipleChoice(formViewModel.NewQuestionsocialStatus);
                       break;

               }
           }
           catch
           {


           }
       }


        public void DoCommand()
       {

       }

       public List<SelectListItem> GetDropDown()
       {
           List<SelectListItem> items = new List<SelectListItem>();

           items.Add(new SelectListItem { Text = "בחר סוג שאלה", Value = "" , Selected = true});

           items.Add(new SelectListItem { Text = "מחוון", Value = "addQustion" });

           items.Add(new SelectListItem { Text = "טקסט חופשי", Value = "AddQustionFreeText" });

           items.Add(new SelectListItem { Text = "קנה מידה", Value = "AddScaleQuestion" });

           items.Add(new SelectListItem { Text = "בחירה מרשימה", Value = "AddMultipleChoiceListQuestion" });

           items.Add(new SelectListItem { Text = "בחירה בודדת", Value = "AddMultipleChoiceQuestion" });

           items.Add(new SelectListItem { Text = "כן לא", Value = "AddYesNoQuestion" });

           items.Add(new SelectListItem { Text = "מעמד חברתי", Value = "AddNewQuestionSocialStatus" });

          return items;

       }
	}
}