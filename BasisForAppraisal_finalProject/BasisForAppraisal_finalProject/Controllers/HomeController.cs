﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BasisForAppraisal_finalProject.Models;
using BasisForAppraisal_finalProject.Authorize;
using BasisForAppraisal_finalProject.Common.IIS;

namespace BasisForAppraisal_finalProject.Controllers
{
    [CustomAuthorizeAttribute(Roles = "Admin")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult LifeSaverForIIS()
        {
            LifeSaver.RecycleAppPools();
            return RedirectToAction("Index");
        }
    }
}