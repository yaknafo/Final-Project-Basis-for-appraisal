using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BasisForAppraisal_finalProject.DBML;
using BasisForAppraisal_finalProject.Models;
using BasisForAppraisal_finalProject.ViewModel;
using System.Threading.Tasks;

namespace BasisForAppraisal_finalProject.Controllers
{
  
    public class FormFillerController : Controller
    {
        private DataManager DM = new DataManager();
        private DataMangerCompany DMC = new DataMangerCompany();
        private FromFillerManager ffm = new FromFillerManager();
        //
        // GET: /FormFiller/
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult FormFill()
        {
            try
            {
                var xconnector = TempData["con"] as tbl_ConnectorFormFill;

                // this will be deleted after yair finsish to work on it!
                if (xconnector == null)
                    xconnector = DMC.Conecctors().Where(x => x.employeeFillId.Equals("301378240")).FirstOrDefault();
                ////////////////////////////////////////////////////////////////////////////////

                var fillerViewModel = new FormFillerViewModel(xconnector.tblForm, xconnector, false);

                return View(fillerViewModel);
            }catch
            {
              return  RedirectToAction("Login", "Account");
            }
        }

        [HttpPost]
        public  ActionResult FormFill(FormFillerViewModel connector)
        {
           // var xconnector = DMC.Conecctors().Where(x => x.employeeFillId.Equals("301378240")).FirstOrDefault();

            List<tbl_IntentionalQuestion> tempAnswerQuestions;

            List<tbl_ConnectorAnswer> selectedAnswer = new List<tbl_ConnectorAnswer>();

             tempAnswerQuestions = new List<tbl_IntentionalQuestion>(connector.Questions);


             foreach (tbl_IntentionalQuestion q in tempAnswerQuestions)
             {
                 if (q.QuestionType.Equals("MultipleChoiceList"))
                     selectedAnswer.AddRange(connector.GetSelectedAnswersFromMulitiChoiceQuestion(q));
                 else
                 {
                     if (q.selectedAnswer != -1)
                     selectedAnswer.Add(connector.GetSelectedAnswer(q));
                 }
             }



            if (selectedAnswer.Where(x => x == null).Any())
            {
                TempData["UnAnswerQuestions"] = "exist";
                return View(connector);
            }

            var taskSaveAnswer = Task.Factory.StartNew(() => ffm.SaveConnectorAnswers(selectedAnswer));
            taskSaveAnswer.Wait();

            
         //   var fillerViewModel = new FormFillerViewModel(connector.tblForm, xconnector);

            // go back to main guest after fill  in the form
            return RedirectToAction("GuestMain", "Guest", new { id = connector.FillBy });
        }


          public ActionResult ShowAnswers(int companyId ,int formId, string empOn, string empFill)
        {
            var dmc = new DataMangerCompany();
            var connector = dmc.ConnectorFormFill.Where(x => x.companyId == companyId && x.employeeFillId == empFill
                                                                        && x.employeeOnId == empOn && x.formId == formId).FirstOrDefault();
              var answers = dmc.ConnectorAnswer.Where(x => x.companyId == companyId && x.employeeFillId == empFill
                                                                        && x.employeeOnId == empOn && x.formConnectorId == formId).ToList();


              var fillerViewModel = new FormFillerViewModel(connector.tblForm, connector, false);

              fillerViewModel.fillMyAnswer(answers);

              return View(fillerViewModel);
        }


	}
}