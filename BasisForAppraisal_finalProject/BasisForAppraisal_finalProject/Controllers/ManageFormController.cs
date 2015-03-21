﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BasisForAppraisal_finalProject.ViewModel;
using BasisForAppraisal_finalProject.Models;


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

        [HttpGet]
        public ActionResult addNewForm()
        {
            var fm = new FormManager();
            var numberOfForm=fm.AddNewForm();
            return RedirectToAction("IntentionalFormWorkshop", "MainFormCreator", new { id = numberOfForm });

        }
	}
}