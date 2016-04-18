using BasisForAppraisal_finalProject.Authorize;
using BasisForAppraisal_finalProject.DBML;
using BasisForAppraisal_finalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BasisForAppraisal_finalProject.ViewModel;
using BasisForAppraisal_finalProject.ViewModel.ExportData;
using System.Web.Helpers;
namespace BasisForAppraisal_finalProject.Controllers
{

    [CustomAuthorizeAttribute(Roles = "Admin")]
    public class ReportController : Controller
    {
        //
        // GET: /Report/
        public ActionResult Index()
        {
            return View();
        }


        
        public ActionResult ReportPerEmployee(string employeeId, string forms,FormReportPerEmployee formReport = null)
        {
            DataManager DMO = new DataManager();

            if (formReport != null && (formReport.GroupLeaderSummry != null || formReport.IsClose))
            {
                DMO.EditReportForIndividual(formReport.GroupLeaderSummry, formReport.IsClose, formReport.Employee.employeeId, formReport.Form.formId);
                employeeId = formReport.Employee.employeeId;
                forms = formReport.Form.formId.ToString();
            }

            var emp = DMO.Employees.Where(e => e.employeeId == employeeId).First();
            
            var form = DMO.Forms.Where(f => f.formId == Int32.Parse(forms)).FirstOrDefault();
            var calculation = new FormReportPerEmployee { Employee = emp, Form = form };
            var reportDb = DMO.GetReportForIndividual(employeeId, form.formId);
            if (reportDb == null || !reportDb.IsClose)
            {
                var report = new ReportForIndividual() { IndividualId = employeeId, FormId = form.formId, createDate = DateTime.Now, CompanyId = emp.companyId };
                DMO.SaveReportForIndividual(report);
                
                calculation.GetResultForForm();
                calculation.GroupLeaderSummry = report.feedback;
                calculation.IsClose = report.IsClose;
                foreach (QuestionReport c in calculation.FormQiestopnReport)
                {
                    DMO.SaveReportForIndividualLines(employeeId, form.formId, form.Sections.First().SectionId, c.Question.QuestionId, c.selfAverage, c.colleagueAverage, c.directorAverage,c.AccompaniedAverage, c.TotalCounter,c.TotalAverage);
                }
            }
            else
            {
                foreach (ReportForIndividualLine v in reportDb.ReportForIndividualLines)
                {
                    calculation.FormQiestopnReport.Add(new QuestionReport(v.SelfEvaluation, v.collegesEvaluation, v.SupervisorEvaluation, v.AccompaniedEvaluation, v.CountOfFormsFilled, v.TotalAverage, v.tbl_IntentionalQuestion));
                }
                calculation.FormQiestopnReport.ForEach(x => x.CalculationLinesForCloseReport());
                calculation.GroupLeaderSummry = reportDb.feedback;
                calculation.IsClose = reportDb.IsClose;
            }
            return View(calculation);
        }


        public ActionResult ReportPerOrganiztion(int companyId, int forms)
        {
            var dm = new DataManager();
            var reportForOrganiztion = dm.ReportForOrganiztions.SingleOrDefault(x => x.CompanyId == companyId && x.FormId == forms);
            if (reportForOrganiztion == null)
                return View();
            var viewModel = new ReportForOrgViewModel(reportForOrganiztion);
            return View(viewModel);
        }


        public ActionResult ReportPerClass(int companyId, int forms,string unit , string cls)
        {
            var dm = new DataManager();
            var reportForClass = dm.ReportForClasses.SingleOrDefault(x => x.companyId == companyId && x.FormId == forms && x.unitName == unit && x.className == cls);
            if (reportForClass == null)
                return View();
            var viewModel = new ReportForClassViewModel(reportForClass);
            return View(viewModel);
        }


        public JsonResult GetReportsLines(int companyId, int form )
        {

            var dm = new DataManager();
            var reportForOrganiztion = dm.ReportForOrganiztions.SingleOrDefault(x => x.CompanyId == companyId && x.FormId == form);
             var data = new List<ReportForOrganiztionLinesViewModel>();
            var listData = reportForOrganiztion.ReportForOrganiztionLines.ToArray();
            foreach(ReportForOrganiztionLine r in listData)
            {
                var tempLine = new ReportForOrganiztionLinesViewModel() { QuestionId = r.QuestionId, TypeQuestion = r.TypeQuestionName, HighScore = r.HighScore, MidScore = r.MidScore, LowScore = r.LowScore,TitleQuestion=r.tbl_IntentionalQuestion.Title };
                data.Add(tempLine);
            }
            var answers = dm.GetReportForCompanyMultipleChoiceListAnswers(form, companyId);
            foreach(ReportForCompanyMultipleChoiceListAnswer r in answers)
            {
                var answerReport = new ReportForOrganiztionLinesViewModel() { QuestionId = r.AnswerId, HighScore = r.numberOfMarket, MidScore = 0, LowScore = r.numberOfShowTotal - r.numberOfMarket, TitleQuestion = r.tbl_IntentionalAnswer.Text };
                data.Add(answerReport);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetReportsLinesForClass(int companyId, int form, string unit, string cls)
        {

            var dm = new DataManager();
            var reportForOrganiztion = dm.ReportForClasses.SingleOrDefault(x => x.companyId == companyId && x.FormId == form && x.unitName == unit && x.className == cls);
            var data = new List<ReportForOrganiztionLinesViewModel>();
            var listData = reportForOrganiztion.ReportForClassLines.ToArray();
            foreach (ReportForClassLine r in listData)
            {
                var tempLine = new ReportForOrganiztionLinesViewModel() { QuestionId = r.QuestionId, TypeQuestion = r.TypeQuestionName, HighScore = r.HighScore, MidScore = r.MidScore, LowScore = r.LowScore, TitleQuestion = r.tbl_IntentionalQuestion.Title };
                data.Add(tempLine);
            }
            var answers = dm.GetReportForClassMultipleChoiceListAnswers(form, companyId,unit,cls);
            foreach (ReportForClassMultipleChoiceListAnswer r in answers)
            {
                var answerReport = new ReportForOrganiztionLinesViewModel() { QuestionId = r.AnswerId, HighScore = r.numberOfMarket, MidScore = 0, LowScore = r.numberOfShowTotal - r.numberOfMarket, TitleQuestion = r.tbl_IntentionalAnswer.Text };
                data.Add(answerReport);
            }
            return Json(data, JsonRequestBehavior.AllowGet);
        }

	}
}