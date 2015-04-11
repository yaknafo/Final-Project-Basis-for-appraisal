using BasisForAppraisal_finalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BasisForAppraisal_finalProject.DBML;
using BasisForAppraisal_finalProject.ViewModel;

namespace BasisForAppraisal_finalProject.Controllers
{
    public class MainCompaniesController : Controller
    {
        //
        // GET: /MainCompanies/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult MainCompanies()
        {
            var dManager = new DataManager();
            var companyies = dManager.Companyies;
            return View(companyies);

        }

        public ActionResult ManageCompany(int id)
        {
            var dManager = new DataManager();
            var companyies = dManager.Companyies.Where(c => c.companyId == id).First(); ;
            var companyView = new CompanyViewModel(companyies);
            return View(companyView);

        }
        public ActionResult addCompanie()
        {
            return View();
        }

        [HttpPost]
        public ActionResult addCompanie(tbl_Company company)
        {
            var cm = new CompanyManger();
            cm.addCompany(company);
            return RedirectToAction("MainCompanies");
        }
       
	}
}