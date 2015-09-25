using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BasisForAppraisal_finalProject.DBML;
using BasisForAppraisal_finalProject.Models;
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
	}
}