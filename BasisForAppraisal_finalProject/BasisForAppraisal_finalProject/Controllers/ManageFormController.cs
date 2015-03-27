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


namespace BasisForAppraisal_finalProject.Controllers
{
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
            return RedirectToAction("IntentionalFormWorkshop", "MainFormCreator", new { id = id });
        }

       
        [HttpPost]
        public ActionResult DeleteForm(int id)
        {
            var fm = new FormManager();
            fm.deleteForm(id);
            return RedirectToAction("MainFormManagment");
        }

        [HttpPost]
        public ActionResult addNewForm()
        {
            var fm = new FormManager();
            var numberOfForm=fm.AddNewForm();
            return RedirectToAction("IntentionalFormWorkshop", "MainFormCreator", new { id = numberOfForm });

        }

        [HttpGet]
        public ActionResult a()
        {
            var forms = new ManageFormViewModel();
            return View(forms.Forms);
        }
	}
}