using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BasisForAppraisal_finalProject.ViewModel;
using BasisForAppraisal_finalProject.Models;
using PagedList;
using PagedList;
using BasisForAppraisal_finalProject.DBML;
using System.Net;
using System.Text;
using System.IO;
using BasisForAppraisal_finalProject.Common.Constans;
using BasisForAppraisal_finalProject.Authorize;
using BasisForAppraisal_finalProject.BL;


namespace BasisForAppraisal_finalProject.Controllers
{

    [CustomAuthorizeAttribute(Roles = "Admin")]
    public class ManageFormController : Controller
    {
        //
        // GET: /ManageForm/
        public ActionResult MainFormManagment()
        {
            var forms = new ManageFormViewModel();
            return View(forms.Forms);
        }


        public ActionResult Edit(int id)
        {
            return RedirectToAction("NewIntentionalFormWorkshop", "MainFormCreator", new { id = id });
        }

        public ActionResult CopyForm(int id)
        {
            var CopyService = new CopyFormService();
            var newForm = CopyService.CopyFormById(id);
            return RedirectToAction("NewIntentionalFormWorkshop", "MainFormCreator", new { id = newForm });
        }



       
        [HttpPost]
        public ActionResult DeleteForm(int id)
        {
            var fm = new FormManager();
            try
            {
                fm.deleteForm(id);
            }catch(Exception e)
            {
                TempData[ResultOperationConstans.Failed] = e.Message;
            }
            return RedirectToAction("MainFormManagment");
        }

        [HttpPost]
        public ActionResult addNewForm()
        {
            var fm = new FormManager();
            var numberOfForm=fm.AddNewForm();
            return RedirectToAction("NewIntentionalFormWorkshop", "MainFormCreator", new { id = numberOfForm });

        }

        [HttpGet]
        public ActionResult a()
        {
            var forms = new ManageFormViewModel();
            return View(forms.Forms);
        }
        [WordDocument]
        public ActionResult AboutDocument()
        {
            ViewBag.WordDocumentFilename = "AboutMeDocument";
            return View("About");
        }
  
	}
}