using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BasisForAppraisal_finalProject.DBML;
using BasisForAppraisal_finalProject.Models;
using BasisForAppraisal_finalProject.ViewModel.ExportData;
using System.IO;
using System.Web.UI;

namespace BasisForAppraisal_finalProject.Controllers
{
    public class ExportDataController : Controller
    {

        public DataManager DMO = new DataManager();
        //
        // GET: /ExportData/
        public ActionResult Index()
        {
            var dManager = new DataManager();
            ViewData["ListData"] = GetDropDownCompany(dManager.Companyies.ToList());
            return View();
        }

        //------------------------------------ Combo Box Area----------------------------------------------------------------
        public PartialViewResult GetUnitCascadeCompany(int id)
        {
            var dManager = new DataMangerCompany();
            ViewData["ListDataUnit"] = GetDropDownUnit(dManager.getUnitsForCompany(id).ToList());
            return PartialView("_UnitsForCbx",id.ToString());
        }

        public PartialViewResult GetclassCascadeUnit(string id, string unit)
        {
            var dManager = new DataMangerCompany();
            ViewData["ListDataClass"] = GetDropDownClass(dManager.Class.Where(x => x.unitName == id).ToList());
            return PartialView("_ClassForCbx",new string[]{unit ,id});
        }

        public PartialViewResult GetFromsForCls(string id, string com, string unit)
        {
            var dManager = new DataMangerCompany();
            var unitFromDb = dManager.getUnitByName(id, 1);
            //var res = dManager.ConnectorFormFill.Where(x => x.tbl_Employee1.className == id).Select(s => s.tblForm).Distinct().ToList();
            var res = dManager.ConnectorFormFill.Where(x => x.companyId == Int32.Parse(com) && x.tbl_Employee1.className == id && x.tbl_Employee1.unitName.StartsWith(unit)).Select(s => s.tblForm).Distinct().ToList();


            var formCheckBoxList = new List<FormCheckBoxViewModel>();

            res.ForEach(x => formCheckBoxList.Add(new FormCheckBoxViewModel { IsSelected = false,FormId = x.formId , FormName = x.FormName} ));
            return PartialView("_FormList", formCheckBoxList);
        }
        //------------------------------------ End Combo Box Area----------------------------------------------------------------

        [HttpPost]
        public ActionResult Index(int Companyiesa, string units, string cls, List<FormCheckBoxViewModel> formCheckList)
        {
            if (formCheckList.First().FormId == null)
                Redirect("Index");
            var dManager = new DataMangerCompany();
            var res = dManager.ConnectorFormFill.Where(x => x.companyId == Companyiesa && x.tbl_Employee1.className == cls && x.tbl_Employee1.unitName == units).Select(s => s.tblForm).Distinct().ToList();
           
            return View();
        }

       public void ExportToCSV()
        {
            var Employies = DMO.Employees.ToList();

            StringWriter sw = new StringWriter();

            sw.WriteLine("\"ID\",\"First Name\",\"Last Name\",\"Company\",\"Unit\",\"Class\",\"Email\"");

            Response.ClearContent();
            Response.AddHeader("content-disposition", "attachment;filename=ExportEmployee" + DateTime.Now + ".csv");
            Response.ContentType = "text/csv";

            foreach (var line in Employies)
            {
                sw.WriteLine(string.Format("\"{0}\",\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\",\"{6}\"",
                                           line.employeeId,
                                           line.firstName,
                                           line.lastName,
                                           line.companyId,
                                           line.unitName,
                                           line.className,
                                           line.Email));
            }

            Response.Write(sw.ToString());

            Response.End();
        }

       public void ExportClientsListToExcel()
       {
           var grid = new System.Web.UI.WebControls.GridView();
           var ClientsList = DMO.Employees.ToList();

           grid.DataSource = 
                            from d in ClientsList
                             select new
                             {
                                 ID = d.employeeId,
                                 FirstName = d.firstName,
                                 LastName = d.lastName,
                                 Email = d.Email

                             };

           grid.DataBind();

           Response.ClearContent();
           Response.AddHeader("content-disposition", "attachment; filename=Exported_Diners.xls");
           Response.ContentType = "application/excel";
           Response.ContentEncoding = System.Text.Encoding.Unicode;
           Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
           StringWriter sw = new StringWriter();
           HtmlTextWriter htw = new HtmlTextWriter(sw);

           grid.RenderControl(htw);

           Response.Write(sw.ToString());

           Response.End();

       }

       public void ExportClientsListToExcelForYair(List<tbl_ConnectorAnswer> a)
       {
           var grid = new System.Web.UI.WebControls.GridView();
           var ClientsList = DMO.Employees.ToList();

           grid.DataSource =
                            from d in ClientsList
                            select new
                            {
                                ID = d.employeeId,
                                FirstName = d.firstName,
                                LastName = d.lastName,
                                Email = d.Email

                            };

           grid.DataBind();

           Response.ClearContent();
           Response.AddHeader("content-disposition", "attachment; filename=Exported_Diners.xls");
           Response.ContentType = "application/excel";
           Response.ContentEncoding = System.Text.Encoding.Unicode;
           Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
           StringWriter sw = new StringWriter();
           HtmlTextWriter htw = new HtmlTextWriter(sw);

           grid.RenderControl(htw);

           Response.Write(sw.ToString());

           Response.End();

       }

       public List<SelectListItem> GetDropDownCompany(List<tbl_Company> com)
       {
           List<SelectListItem> items = new List<SelectListItem>();
           com.ForEach(x => items.Add(new SelectListItem { Text = x.comapnyName, Value = x.companyId.ToString() }));
           items.Add(new SelectListItem { Text = ".....", Value = "", Selected = true });
           return items;
       }

       public List<SelectListItem> GetDropDownUnit(List<tbl_Unit> com)
       {
           List<SelectListItem> items = new List<SelectListItem>();
           com.ForEach(x => items.Add(new SelectListItem { Text = x.unitName, Value = x.unitName }));
           items.Add(new SelectListItem { Text = ".....", Value = "", Selected = true });
           return items;
       }


       public List<SelectListItem> GetDropDownClass(List<tbl_Class> com)
       {
           List<SelectListItem> items = new List<SelectListItem>();
           com.ForEach(x => items.Add(new SelectListItem { Text = x.className, Value = x.className }));
           items.Add(new SelectListItem { Text = ".....", Value = "", Selected = true });
           return items;
       }
	}
}