using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BasisForAppraisal_finalProject.DBML;
using BasisForAppraisal_finalProject.Models;
using BasisForAppraisal_finalProject.ViewModel;
using System.Threading.Tasks;

namespace BasisForAppraisal_finalProject.Controllers
{
  
    public class FormFillerController : Controller
    {
        private DataManager DM = new DataManager();
        private DataMangerCompany DMC = new DataMangerCompany();
        private FromFillerManager ffm = new FromFillerManager();
        //
        // GET: /FormFiller/
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult FormFill()
        {
            var xconnector = TempData["con"] as tbl_ConnectorFormFill;

            // this will be deleted after yair finsish to work on it!
            if (xconnector == null)
            xconnector = DMC.Conecctors().Where(x => x.employeeFillId.Equals("301378240")).FirstOrDefault();
            ////////////////////////////////////////////////////////////////////////////////

            var fillerViewModel = new FormFillerViewModel(xconnector.tblForm, xconnector);

            return View(fillerViewModel);
        }

        [HttpPost]
        public  ActionResult FormFill(FormFillerViewModel connector)
        {
            var xconnector = DMC.Conecctors().Where(x => x.employeeFillId.Equals("301378240")).FirstOrDefault();

            List<tbl_ConnectorAnswer> selectedAnswer = new List<tbl_ConnectorAnswer>();

            foreach(tbl_IntentionalQuestion q in connector.Questions)
            {
                selectedAnswer.Add(connector.GetSelectedAnswer(q));
            }

            var taskUpLoadExcel= Task.Factory.StartNew(() => ffm.SaveConnectorAnswers(selectedAnswer));
            taskUpLoadExcel.Wait();

            
            var fillerViewModel = new FormFillerViewModel(xconnector.tblForm, xconnector);

            return View(fillerViewModel);
        }
	}
}