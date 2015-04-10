﻿using BasisForAppraisal_finalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BasisForAppraisal_finalProject.DBML;
using BasisForAppraisal_finalProject.ViewModel;
using System.IO;

namespace BasisForAppraisal_finalProject.Controllers
{
    public class MainCompaniesController : Controller
    {
        // method read and save the file upload
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            string[] str = Path.GetFileName(file.FileName).Split('.');
            if (file.ContentLength > 0)
                if (str[str.Count() - 1].Equals("xlsx") || str[str.Count() - 1].Equals("csv") || str[str.Count() - 1].Equals("xls"))
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                    file.SaveAs(path);
                    upload_excelfile(path,fileName);
                }
            return RedirectToAction("ManageCompany");
        }
        // method add excel data to db
        private void upload_excelfile(string path, string filename)
        {
            var db = new CompanyManger();
            db.upload_excelfile(path,filename);

        }
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