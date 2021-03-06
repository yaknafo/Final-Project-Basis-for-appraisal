﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BasisForAppraisal_finalProject.DBML;
using BasisForAppraisal_finalProject.Models;
using BasisForAppraisal_finalProject.ViewModel.ExportData;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using BasisForAppraisal_finalProject.BL;
using BasisForAppraisal_finalProject.Authorize;

namespace BasisForAppraisal_finalProject.Controllers
{
    [CustomAuthorizeAttribute(Roles = "Admin")]
    public class ExportDataController : Controller
    {

        public DataManager DMO = new DataManager();

        //
        // GET: /ExportData/
        public ActionResult MainData()
        {
            
            return View();
        }


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
            ViewData["ListDataUnit"] = GetDropDownUnit(dManager.getUnitsForCompany(id).Distinct().ToList());
            return PartialView("_UnitsForCbx",id.ToString());
        }

        public PartialViewResult GetclassCascadeUnit(string id, string unit)
        {
            var dManager = new DataMangerCompany();
            ViewData["ListDataClass"] = GetDropDownClass(dManager.Class.Where(x => x.unitName == id && x.companyId.ToString() == unit).Distinct().ToList());
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

        //-------------------------- for student report----------------------------------------------

        public PartialViewResult GetUnitCascadeCompanyEmp(int id)
        {
            var dManager = new DataMangerCompany();
            ViewData["ListDataUnit"] = GetDropDownUnit(dManager.getUnitsForCompany(id).Distinct().ToList());
            return PartialView("_UnitsForEmpCbx", id.ToString());
        }

        public PartialViewResult GetclassCascadeUnitEmp(string id, string unit)
        {
            var dManager = new DataMangerCompany();
            ViewData["ListDataClass"] = GetDropDownClass(dManager.Class.Where(x => x.unitName == id && x.companyId.ToString() == unit).Distinct().ToList());
            return PartialView("_ClassForCbxEmp", new string[] { unit, id });
        }
        public PartialViewResult GetEmpCascadeUnit(string id, string unit, string classanme)
        {
            var dManager = new DataManager();
            ViewData["ListDataClass"] = GetDropDownClass(dManager.Employees.Where(x => x.className == id && x.unitName.ToString() == unit && x.companyId.ToString() == classanme).Distinct().ToList());
            return PartialView("_EmpForCbxEmp", new string[] { unit, id });
        }

        public PartialViewResult GetFromsForEmp(string id, string empId)
        {
            var dManager = new DataMangerCompany();
            var unitFromDb = dManager.getUnitByName(id, 1);
            var res = dManager.ConnectorFormFill.Where(x => x.employeeOnId == id).Select(s => s.tblForm).Distinct().ToList();
            ViewBag.emp = id;

            var formCheckBoxList = new List<FormCheckBoxViewModel>();

            res.ForEach(x => formCheckBoxList.Add(new FormCheckBoxViewModel { IsSelected = false, FormId = x.formId, FormName = x.FormName }));
            return PartialView("_FormList", formCheckBoxList);
        }
        //------------------------------------ End Combo Box Area----------------------------------------------------------------

        [HttpPost]
        public ActionResult Index(int Companyiesa, string units, string cls, List<FormCheckBoxViewModel> formCheckList)
        {
            if (formCheckList.First().FormId == null)
                Redirect("Index");

            var formSelected = formCheckList.Where(x => x.IsSelected).Select(f => f.FormId).ToList();
            var dManager = new DataMangerCompany();
            var res = dManager.ConnectorFormFill.Where(x => x.companyId == Companyiesa && x.tbl_Employee1.className == cls && x.tbl_Employee1.unitName == units && formSelected.Contains(x.formId)).Select(s => s.tblForm).Distinct().ToList();

           
            
            var EmployeeOfTheUnit = dManager.getEmployee(Companyiesa, units,cls);

            ExportToCSV(res, EmployeeOfTheUnit);
            return View();
        }

        public void ExportToCSV(List<DBML.tblForm> res, List<DBML.tbl_Employee> EmployeeOfTheUnit)
        {
           
            var excelService = new ExcelExportDataService();
            var grid = excelService.ExportToCSV(res, EmployeeOfTheUnit);
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=MyExcelFile.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            Response.ContentEncoding = System.Text.Encoding.Unicode;
            Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            grid.RenderControl(htw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();

        }

       
        //------------------------------------------------------- For Org Report cascade ------------------------------///
        public PartialViewResult GetUnitCascadeCompanyOrg(int id)
        {
            var dManager = new DataMangerCompany();
            ViewData["ListDataUnit"] = GetDropDownUnit(dManager.getUnitsForCompany(id).Distinct().ToList());
            return PartialView("_UnitsForOrgCbx", id.ToString());
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


       public List<SelectListItem> GetDropDownCompany(List<tbl_Company> com)
       {
           List<SelectListItem> items = new List<SelectListItem>();
           com.ForEach(x => items.Add(new SelectListItem { Text = x.comapnyName, Value = x.companyId.ToString() }));
           items.Add(new SelectListItem { Text = ".....", Value = "", Selected = true });
           return items;
       }


       public List<SelectListItem> GetLevelInOrg()
       {
           List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "ארגון", Value = "1" , Selected = true});
           items.Add(new SelectListItem { Text = "שנה" ,Value = "2" });
           items.Add(new SelectListItem { Text = "מחזור" ,Value = "3" });
           
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

       public List<SelectListItem> GetDropDownClass(List<tbl_Employee> emp)
       {
           List<SelectListItem> items = new List<SelectListItem>();
           emp.ForEach(x => items.Add(new SelectListItem { Text = x.firstName+" "+x.lastName, Value = x.employeeId }));
           items.Add(new SelectListItem { Text = "שגריר", Value = "", Selected = true });
           return items;
       }

        [HttpGet]
       public ActionResult ReportForStudent()
       {
           var dManager = new DataManager();
           ViewData["ListData"] = GetDropDownCompany(dManager.Companyies.ToList());
           return View();
       }


        [HttpPost]
        public ActionResult ReportForStudent( string emp, List<FormCheckBoxViewModel> formCheckList)
        {
            var dManager = new DataManager();
            var formSelected = formCheckList.Where(x => x.IsSelected).Select(f => f.FormId).ToList();
            if(formSelected.Count> 0)
                return RedirectToAction("ReportPerEmployee", "Report", new { employeeId = emp, forms = formSelected.First().ToString() });
            return View();
        }

        [HttpGet]
        public ActionResult ReportForOrganiztion()
        {
            var dManager = new DataManager();
            ViewData["ListData"] = GetDropDownCompany(dManager.Companyies.ToList());
            ViewData["Levels"] = GetLevelInOrg();
            return View();
        }

        [HttpPost]
        public ActionResult ReportForOrganiztion(int Companyiesa, List<FormCheckBoxViewModel> formCheckList,int levels,string units="", string cls ="", bool noCalculation = true)
        {
            var dManager = new DataManager();
            var reportBL = new ReportBL();
            var form = formCheckList.FirstOrDefault(x => x.IsSelected);
            if (form == null)
            {
                ViewData["ListData"] = GetDropDownCompany(dManager.Companyies.ToList());
                return View();
            }
            if (!noCalculation)
                if(levels == 1)
                {
                    var empIds = dManager.Employees.Where(x => x.companyId == Companyiesa  && !x.IsAccompanied && !x.IsManger.Value).Select(x => x.employeeId).ToList();
                    var empIdsFillter = dManager.ConnectorAnswers.Where(x => empIds.Contains(x.employeeOnId)).Select(x => x.employeeOnId).Distinct().ToList();
                    reportBL.CalculateReportForOrganiztion(Companyiesa, form.FormId, empIdsFillter);
                }
                // for Unit
                else if (levels == 2)
                {
                    var empIds = dManager.Employees.Where(x => x.companyId == Companyiesa && x.unitName == units && !x.IsAccompanied && !x.IsManger.Value).Select(x => x.employeeId).ToList();
                    var empIdsFillter = dManager.ConnectorAnswers.Where(x => empIds.Contains(x.employeeOnId)).Select(x => x.employeeOnId).Distinct().ToList();
                    reportBL.CalculateReportForUnit(Companyiesa, form.FormId, units, empIdsFillter);
                    return RedirectToAction("ReportPerUnit", "Report", new { companyId = Companyiesa, forms = form.FormId, unit = units});
                }
                // for Class
                else if (levels == 3)
                {
                    var empIds = dManager.Employees.Where(x => x.companyId == Companyiesa && x.unitName == units && x.className == cls &&  !x.IsAccompanied && !x.IsManger.Value).Select(x => x.employeeId).ToList();
                    var empIdsFillter = dManager.ConnectorAnswers.Where(x => empIds.Contains(x.employeeOnId)).Select(x => x.employeeOnId).Distinct().ToList();
                    reportBL.CalculateReportForClass(Companyiesa, form.FormId, units, cls, empIdsFillter);
                    return RedirectToAction("ReportPerClass", "Report", new { companyId = Companyiesa, forms = form.FormId, unit= units, cls=cls });
                }

            return RedirectToAction("ReportPerOrganiztion", "Report", new { companyId = Companyiesa, forms = form.FormId });
        }

        /// <summary>
        /// Get Froms For Organiztion
        /// </summary>
        /// <param name="organiztionid"></param>
        /// <returns></returns>
        public PartialViewResult GetFromsForOrganiztion(int organiztionid)
        {
            var dManager = new DataMangerCompany();
            var res = dManager.ConnectorAnswer.Where(x => x.companyId == organiztionid).Select(s => s.tbl_ConnectorFormFill.tblForm).Distinct().ToList();

            var formCheckBoxList = new List<FormCheckBoxViewModel>();

            res.ForEach(x => formCheckBoxList.Add(new FormCheckBoxViewModel { IsSelected = false, FormId = x.formId, FormName = x.FormName }));
            return PartialView("_FormList", formCheckBoxList);
        }

        public ActionResult chartimage()
        {
            return View();
        }
	}
}