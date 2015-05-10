using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BasisForAppraisal_finalProject.Models;
using BasisForAppraisal_finalProject.DBML;
using Microsoft.AspNet.Identity;

namespace BasisForAppraisal_finalProject.Controllers
{
    public class GuestController : Controller
    {

        private DataManager MD = new DataManager();
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
            empa.FillOnThem.OrderBy(x => x.done);
            return View(empa);
           
        }

        public ActionResult GetFromForFill(int companyId, string employeeFillId, string employeeOnId, int formId)
       {
           return View();
       }
	}
}