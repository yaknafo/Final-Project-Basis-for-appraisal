using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BasisForAppraisal_finalProject.Models;
using BasisForAppraisal_finalProject.DBML;
using Microsoft.AspNet.Identity;
using BasisForAppraisal_finalProject.BL;
using BasisForAppraisal_finalProject.ViewModel.Guest;
using BasisForAppraisal_finalProject.Authorize;
using BasisForAppraisal_finalProject.Common.Constans;

namespace BasisForAppraisal_finalProject.Controllers
{
    [CustomAuthorizeAttribute(Roles = "Guest")]
    public class GuestController : Controller
    {

        private DataManager MD = new DataManager();
        private DataMangerCompany MDC = new DataMangerCompany();
        //
        // GET: /Guest/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Guest/
        [HttpGet]
        public ActionResult Guestlogin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Guestlogin(string email, string id)
        {
            var emp = MD.Employees.Where(x => x.employeeId == id && x.Email.Equals(email)).FirstOrDefault();
            if (emp == null)
            {
                TempData["Error"] = "problem";
                return View();
            }
            return RedirectToAction("GuestMain", "Guest", new { id=emp.employeeId });
        }

        public ActionResult GuestMain(string id)
        {
            //var c= MD.Employees.Where(c => c.check)

            var empa = MD.Employees.Where(x => x.employeeId == id).FirstOrDefault();
            empa.RefreshConecctors();
            empa.FillOnThem=empa.FillOnThem.OrderBy(x => x.done).ToList();

            List<tbl_ConnectorFormFill> froms;
            List<ReportForIndividual> reports;
            var reportBl = new ReportBL();
            var viewModelGuest = new GuestMainViewModel();
            viewModelGuest.Emp = empa;
            // Get Reports
            if(empa.IsManagerWrapper)
            {
                viewModelGuest.Froms = MD.ConnectorForm.Where(x => x.employeeFillId == empa.employeeId).ToList();
              
            }
            else
            {
                viewModelGuest.Reports = MD.ReportForIndividuals.Where(x => x.IndividualId == empa.employeeId && x.IsClose).ToList();
            }

            return View(viewModelGuest);
           
        }

        [HttpPost]
        public ActionResult GetFromForFill(int companyId, string employeeFillId, string employeeOnId, int formId)
        {
            var con = MDC.Conecctors().Where(x => x.formId == formId && x.companyId == companyId && x.employeeFillId == employeeFillId && x.employeeOnId == employeeOnId).FirstOrDefault();
            TempData["con"] = con;
            return RedirectToAction("FormFill", "FormFiller");
        }


        [HttpPost]
        public JsonResult GetRreports(string empId, string formId)
        {
            List<tbl_Employee> reports;
            List<SelectListGroup> items = new List<SelectListGroup>();
            if (MD.IsManager(empId))
            {
                reports = MD.ConnectorForm.Where(x => x.employeeFillId == empId && x.formId.ToString() == formId).Select(x => x.tbl_Employee).Distinct().ToList();

            }
            else
            {
                reports = MD.ConnectorForm.Where(x => x.employeeOnId == empId && x.formId.ToString() == formId).Select(x => x.tbl_Employee1).Distinct().ToList();
            }

            return Json(reports, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReportPerEmployee(string employeeId, string forms,bool isLeader, FormReportPerEmployee formReport = null)
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
                    DMO.SaveReportForIndividualLines(employeeId, form.formId, form.Sections.First().SectionId, c.Question.QuestionId, c.selfAverage, c.colleagueAverage, c.directorAverage, c.AccompaniedAverage, c.TotalCounter, c.TotalAverage);
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

            calculation.IsLeader = isLeader;
            return View(calculation);
        }


 
	}
}