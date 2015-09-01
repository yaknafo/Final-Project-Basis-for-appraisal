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
            calculation.GetResultForForm();
            return View(calculation);
        }
	}
}