using BasisForAppraisal_finalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BasisForAppraisal_finalProject.Controllers
{
    public class testController : Controller
    {
        // GET: test
        public ActionResult About()
        {
            return View();
        }
        [WordDocument]
        public ActionResult AboutDocument()
        {
            ViewBag.WordDocumentFilename = "AboutMeDocument";
            return View("About");
        }
    }
}