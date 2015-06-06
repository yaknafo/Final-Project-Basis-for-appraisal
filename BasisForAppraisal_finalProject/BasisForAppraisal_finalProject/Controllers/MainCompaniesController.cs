using BasisForAppraisal_finalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BasisForAppraisal_finalProject.DBML;
using BasisForAppraisal_finalProject.ViewModel;
using BasisForAppraisal_finalProject.Common.Constans;
using System.IO;
using BasisForAppraisal_finalProject.ViewModel.Company;
using System.Threading.Tasks;
using BasisForAppraisal_finalProject.Authorize;

namespace BasisForAppraisal_finalProject.Controllers
{

    [CustomAuthorizeAttribute(Roles = "Admin")]
    public class MainCompaniesController : Controller
    {

       private DataMangerCompany DM = new DataMangerCompany();
       private DataManager DMO = new DataManager();
        // method read and save the file upload
        [HttpPost]
        public ActionResult Index( HttpPostedFileBase file = null, CompanyViewModel c= null)
        {
           
            var id = Convert.ToInt32( Session["companyId"]);
            var taskUpLoadExcel= Task.Factory.StartNew(() => SendUploadExcel(file, id));
            taskUpLoadExcel.Wait();

            return RedirectToAction("CompanyUnit", new { id = id });
          }



      
     


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

        public ActionResult ManageCompany(int id,string unit,string cl)
        {
            var dManager = new DataManager();
            var comapny = dManager.Companyies.Where(x => x.companyId == id).First();
            var myunit = comapny.Units.Where(x => x.unitName.Equals( unit)).FirstOrDefault();
            var myclass = myunit.tbl_Classes.Where(x => x.className.Equals(cl)).FirstOrDefault();
            var unitAndForm = new ClassUnitViewModel(myclass);
            comapny.LoadEmployees();
            return View(unitAndForm);

        }
        public ActionResult CompanyUnit(int id )
        {
            var dManager = new DataManager();
            ViewBag.Companyies = new SelectList(dManager.Companyies, "companyId", "comapnyName");
            var companyies = dManager.Companyies.Where(c => c.companyId == id).First();
            ViewBag.id = id;
            ViewBag.name = companyies.comapnyName;
            ViewBag.phone = companyies.comapnyPhone;
            ViewBag.adress = companyies.comapnyAddress;
            List<tbl_Class> list= new List<tbl_Class>();
            foreach (var unit in companyies.Units)
                foreach (var cl in unit.tbl_Classes)
                    list.Add(cl);
           

            // refresh Employees
            //companyies.LoadEmployees();
            ViewBag.name = companyies.comapnyName;
            return View(list);

        }

        [HttpPost]
        public ActionResult AddClass( tbl_Class cls)
        {
            var unit = new tbl_Unit { companyId = cls.companyId, unitName = cls.unitName };
            if (!string.IsNullOrEmpty(cls.unitName) && !string.IsNullOrEmpty(cls.className))
            {
                try
                {

                    var addUnit = Task.Factory.StartNew(() => DM.AddUnit(unit));
                    addUnit.Wait();
                    var addClass = Task.Factory.StartNew(() => DM.AddClass(cls));
                    addClass.Wait();
                    TempData[ResultOperationConstans.Success] = "הוסף בהצלחה";
                }
                catch (Exception e)
                {
                    TempData[ResultOperationConstans.Failed] = e.Message;
                }
            }
            return RedirectToAction("CompanyUnit", new { id = cls.companyId });
        }


        public ActionResult AddEmployee(tbl_Employee emp, int Companyies, string units , string clas )
        {
            if (Companyies == null || units == null || clas == null || string.IsNullOrEmpty(units) || string.IsNullOrEmpty(clas))
            {
                TempData[ResultOperationConstans.Failed] = "בחר כל הוספת עובד חברה מחלקה ומחזור";
                return RedirectToAction("CompanyUnit", new { id = Companyies });
            }
            emp.companyId = Companyies;
            emp.unitName = units;
            emp.className = clas;
            emp.IsManger = false;
            try
            {

                var addWorkerToDb = Task.Factory.StartNew(() => DM.addWorkerToDb(emp));
                addWorkerToDb.Wait();
                TempData[ResultOperationConstans.Success] = "הוסף בהצלחה";
            }
            catch(Exception e)
            {
                TempData[ResultOperationConstans.Failed] = e.Message;
            }
          return  RedirectToAction("CompanyUnit", new {id=Companyies });
        }

        [HttpGet]
        public ActionResult MainEmployee(string id = "301378242")
        {
            var DM = new DataManager();
            var emp = DM.Employees.Where(e => e.employeeId == id).FirstOrDefault();

            if(emp != null)
            {
                emp.RefreshConecctors();
                return View(emp);
 
            }

            TempData[ResultOperationConstans.Failed] = "ID not exist in the system";
            return new RedirectResult(Request.UrlReferrer.AbsoluteUri);
        }


        [HttpPost]
        public ActionResult MainEmployee(string submit, string iFillOnThem, string fromName, string ThemFillOnMe, string fromNameOnMe, tbl_Employee emp)
        {
            ModelState.Clear();
            var cm = new CompanyManger();
            bool res;
           
            if (submit.Equals("addThemfillOnMe"))
            {
                var formID = DMO.Forms.Where(f => f.FormName == fromNameOnMe).Select(f => f.formId).FirstOrDefault();

                // check if from and emp are exist
                res = CheckParam(formID, ThemFillOnMe);
                if (res)
                {
                    var taskAddCon = Task.Factory.StartNew(() => DM.AddConnector(ThemFillOnMe, emp.employeeId, emp.companyId, formID));
                    taskAddCon.Wait();
                }
            }

            else if (submit.Equals("addMEfillOnThem"))
            {
                var formID = DMO.Forms.Where(f => f.FormName == fromName).Select(f => f.formId).FirstOrDefault();

                // check if from and emp are exist
                res = CheckParam(formID, iFillOnThem);
                if (res)
                {
                    var taskAddCon = Task.Factory.StartNew(() => DM.AddConnector(emp.employeeId, iFillOnThem, emp.companyId, formID));
                    taskAddCon.Wait();
                }
            }
            else if (submit.Equals("DeleteMEfillOnThem"))
            {
                var conecntorsForDelete = emp.FillOnThem.Where(x => x.IfDelete).ToList();
                var taskAddCon = Task.Factory.StartNew(() => cm.DeleteConnectors(conecntorsForDelete));
                taskAddCon.Wait();
            }
            else if (submit.Equals("DeleteThemFillOnMe"))
            {
                var conecntorsForDelete = emp.FillOnMe.Where(x => x.IfDelete).ToList();
                var taskAddCon = Task.Factory.StartNew(() => cm.DeleteConnectors(conecntorsForDelete));
                taskAddCon.Wait();
            }

            emp.RefreshConecctors();

            if(Request.IsAjaxRequest())
            {
                return PartialView("_ConecctorTable", emp);
            }
           
            return View(emp);
        }

        private bool CheckParam(int formID, string employeeId)
        {
        
            if (DMO.Employees.Where(x => x.employeeId == employeeId).Any() && formID != 0)
                return true;
            TempData[ResultOperationConstans.Failed] = "froms or employee not exist";
            return false;

        }


        /// ---------------------------------- Ajax Part -----------------------------------------------------------//
        /// 



        public PartialViewResult GetUnitCascadeCompany(int id)
        {
            ViewBag.units = new SelectList(DM.getUnitsForCompany(id), "unitName", "unitName");

            ViewBag.clas = new SelectList(DM.Class.Where(x => x.unitName == "Not found"), "unitName", "unitName");

            return PartialView("_UnitsForCbx");


        }


        public PartialViewResult GetclassCascadeUnit(string id)
        {
            ViewBag.clas = new SelectList(DM.Class.Where(x => x.unitName == id), "className", "className");

            return PartialView("_ClassForCbx");


        }

        public ActionResult qa()
        {
            var dManager = new DataManager();
            ViewBag.Companyies = new SelectList(dManager.Companyies, "companyId", "comapnyName");
            return View();
        }
        [HttpPost]
        public JsonResult AutoCompleteCountry(string term)
        {
            
            return Json(null, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployee(string term)
        {
            var DM = new DataManager();

            var emp = DM.Employees.Where(x => x.employeeId.StartsWith(term)).Select(a=> a.employeeId).ToList();

            return Json(emp, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployees(string term)
        {
            var DM = new DataManager();

            var emp = DM.Employees.Where(x => x.employeeId.StartsWith(term)).Select(a => a.employeeId).ToList();

            return Json(emp, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetForms(string term)
       {
            var DM = new DataManager();

            var emp = DM.Forms.Where(x => x.FormName.StartsWith(term)).Select(a => a.FormName).Take(5).ToList();

            return Json(emp, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetFormss(string term)
        {
            var DM = new DataManager();

            var emp = DM.Forms.Where(x => x.FormName.StartsWith(term)).Select(a => a.FormName).Take(5).ToList();

            return Json(emp, JsonRequestBehavior.AllowGet);
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
        [HttpPost]
        public ActionResult deleteWorker(String idworker, int companyNumber, string unit, string cl)
        {
            try
            {
                var cm = new CompanyManger();
                cm.deleteWorker(idworker, companyNumber);             
                TempData[ResultOperationConstans.Success] = "עובד נמחק בהצלחה";           
               
            }
            catch (Exception ex)
            {
                TempData[ResultOperationConstans.Failed] = "לא ניתן למחוק עובד";
            }
            return RedirectToAction("ManageCompany", new { id = companyNumber, unit = unit, cl = cl });
        }
        public ActionResult test()
        {
            return View();
        }
       
        private void SendUploadExcel(HttpPostedFileBase file, int id)
        {
            var db = new CompanyManger();
            if (!Directory.Exists(Server.MapPath("~/App_Data/uploads")))
            {
                Directory.CreateDirectory(Server.MapPath("~/App_Data/uploads"));
            }

            try
            {
                string[] str = Path.GetFileName(file.FileName).Split('.');
                if (file.ContentLength > 0)
                    if (str[str.Count() - 1].Equals("xlsx") || str[str.Count() - 1].Equals("csv") || str[str.Count() - 1].Equals("xls"))
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), Guid.NewGuid() + fileName);
                        file.SaveAs(path);
                        db.UploadExcelFile(path, id);

                    }
            }
            catch (Exception ex) { }
        }
        public void intalizeCheckBox(bool workers, bool manager,bool onManger,bool onHimself)
        {
            //delete
            Session["workers"] = workers;
            Session["manager"] = manager;
            Session["onManger"] = onManger;
            Session["onHimself"] = onHimself;

        }
        [HttpPost]
        public ActionResult AddConnector(int companyid, string unit, string cl, int formId)
        {
            Boolean b = Boolean.Parse(Session["onHimself"].ToString());
            if (Boolean.Parse( Session["workers"].ToString()))
                addConectorToWorkersOnly(companyid, unit, cl, formId);
            if (Boolean.Parse(Session["manager"].ToString()))
                addConectorTomangerstoWorker(companyid, unit, cl, formId);
            if (Boolean.Parse(Session["onManger"].ToString()))
                addConectorworkersOnManager(companyid, unit, cl, formId);
            TempData[ResultOperationConstans.Success] = "טופס נוסף בהצלחה ליחידה זו";
           
            return RedirectToAction("ManageCompany", new { id = companyid, unit = unit, cl = cl });
            
        }
        //
        private void addConectorToWorkersOnly(int companyid, string unit, string cl, int formId)
        {
            var dmc = new DataMangerCompany();
            var employes = dmc.getEmployee(companyid, unit, cl);
            foreach (var e1 in employes)
                foreach (var e2 in employes)
                    if (!(e2.IsManger == true || e1.IsManger==true))
                        if ((e1 == e2 && Boolean.Parse(Session["onHimself"].ToString()))||e1!=e2)
                            dmc.AddConnector(e1.employeeId, e2.employeeId, companyid, formId);
                       


        }
        private void addConectorTomangerstoWorker(int companyid, string unit, string cl, int formId)
        {
            var dmc = new DataMangerCompany();
            var employes = dmc.getEmployee(companyid, unit, cl);
            foreach (var e1 in employes)
            {
                foreach (var e2 in employes)
                    if ((e1 == e2 && Boolean.Parse(Session["onHimself"].ToString())) || e1 != e2)
                        if (e1.IsManger == true)
                            dmc.AddConnector(e1.employeeId, e2.employeeId, companyid, formId);
                Boolean b = Boolean.Parse(Session["onHimself"].ToString());
            }
        }
        private void addConectorworkersOnManager(int companyid, string unit, string cl, int formId)
        {
            var dmc = new DataMangerCompany();
            var employes = dmc.getEmployee(companyid, unit, cl);
            foreach (var e1 in employes)
                foreach (var e2 in employes)
                  if ((e1 == e2 && Boolean.Parse(Session["onHimself"].ToString()))||e1!=e2)
                    if (e2.IsManger == true)
                        dmc.AddConnector(e1.employeeId, e2.employeeId, companyid, formId);

        }
        [HttpPost]
        public ActionResult deleteCompany(int companyid)
        {
            try
            {
                var dmc = new DataMangerCompany();
                dmc.deleteCompany(companyid);
                TempData[ResultOperationConstans.Success] = "חברה נמחקה בהצלחה";
               
                
            }
            catch(Exception ex)
            {
                TempData[ResultOperationConstans.Failed] = "לא ניתן למחוק חברה";
            }
            return RedirectToAction("MainCompanies");
        }

        public  ActionResult deleteFolder(string className, string unitid, int companyid)
        {
            try
            {
                var dmc = new DataMangerCompany();
                dmc.deleteUnitAndClass(className, unitid, companyid);
                TempData[ResultOperationConstans.Success] = "יחידה נמחקה בהצלחה";
            }
            catch (Exception ex)
            {
                TempData[ResultOperationConstans.Failed] = "לא ניתן למחוק יחידה";
            }
            return RedirectToAction("CompanyUnit", new { id = companyid});
            
        }
       
	}
}