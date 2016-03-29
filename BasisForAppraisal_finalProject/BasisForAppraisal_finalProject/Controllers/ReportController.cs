using BasisForAppraisal_finalProject.DBML;
using BasisForAppraisal_finalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BasisForAppraisal_finalProject.Controllers
{
    public class ReportController : Controller
    {
        //
        // GET: /Report/
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ReportPerEmployee(string employeeId, string forms)
        {
            DataManager DMO = new DataManager();

            var emp = DMO.Employees.Where(e => e.employeeId == employeeId).First();
            var form = DMO.Forms.Where(f => f.formId == Int32.Parse(forms)).FirstOrDefault();
            var calculation = new FormReportPerEmployee { Employee = emp, Form = form };
            var reportDb = DMO.GetReportForIndividual(employeeId, form.formId);
            if (!reportDb.IsClose)
            {
                var report = new ReportForIndividual() { IndividualId = employeeId, FormId = form.formId, createDate = DateTime.Now };
                DMO.SaveReportForIndividual(report);
                
                calculation.GetResultForForm();

                foreach (QuestionReport c in calculation.FormQiestopnReport)
                {
                    DMO.SaveReportForIndividualLines(employeeId, form.formId, form.Sections.First().SectionId, c.Question.QuestionId, c.selfAverage, c.colleagueAverage, c.directorAverage);
                }
            }
            return View(calculation);
        }
	}
}