using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BasisForAppraisal_finalProject.DBML;
using BasisForAppraisal_finalProject.Models;

namespace BasisForAppraisal_finalProject.Controllers
{
    public class MainFormCreatorController : Controller
    {
        //
        // GET: /MainFormCreator/
        public ActionResult Index()
        {
            var b = new DataManager();
            var q = b.GetFormWithQuestion(1);
            
          
            return View(q);
        }
	}
}