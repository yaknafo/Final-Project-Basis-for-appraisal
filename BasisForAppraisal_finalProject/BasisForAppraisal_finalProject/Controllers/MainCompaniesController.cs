using BasisForAppraisal_finalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BasisForAppraisal_finalProject.DBML;
using BasisForAppraisal_finalProject.ViewModel;
using System.IO;
using BasisForAppraisal_finalProject.ViewModel.Company;
using System.Threading.Tasks;

namespace BasisForAppraisal_finalProject.Controllers
{
    
    public class MainCompaniesController : Controller
    {
       
        // method read and save the file upload
        [HttpPost]
        public ActionResult Index( HttpPostedFileBase file = null, CompanyViewModel c= null)
        {
           
            var id = Convert.ToInt32( Session["companyId"]);
            var taskUpLoadExcel= Task.Factory.StartNew(() => SendUploadExcel(file, id));
            taskUpLoadExcel.Wait();

            return RedirectToAction("CompanyUnit", new { id = id });
          }
           
           

          
        // method add excel data to db
        //private void upload_excelfile(string path, int idCompany)
        //{
        //    var db = new CompanyManger();
        //    db.upload_excelfile(path,idCompany);

        //}
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

        public ActionResult MainEmployee(string id = "301378242")
        {
            if(!string.IsNullOrEmpty(id))
            {
                var DM = new DataManager();

                var emp = DM.Employees.Where(e => e.employeeId == id).FirstOrDefault();

                return View(emp);
 
            }

            return View();
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
            var cm = new CompanyManger();
            cm.deleteWorker(idworker, companyNumber);
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
            var dmc = new DataMangerCompany();
            dmc.deleteCompany(companyid);
            return RedirectToAction("MainCompanies");

        }

        public  ActionResult deleteFolder(string className, string unitid, int companyid)
        {
            var dmc = new DataMangerCompany();
            dmc.deleteUnitAndClass(className, unitid, companyid);
            return RedirectToAction("CompanyUnit", new { id = companyid});
            
        }
       
	}
}