using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BasisForAppraisal_finalProject.DBML;
using System.Data.Linq;
using System.IO;
using Microsoft.Office.Interop;
using Excel = Microsoft.Office.Interop.Excel;

namespace BasisForAppraisal_finalProject.Models
{

    public class DataMangerCompany
    {
        private BFADataBasedbmlDataContext manager = new BFADataBasedbmlDataContext();
        public DataMangerCompany()
        {
            this.manager = DbmlBFADataContext.GetDataContextInstance();
            var cultureinfo = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentCulture = cultureinfo;
        }
        public void upload_excelfile(string path, int idCompany)
        {
            try
            {
                Excel.Application xlApp = new Excel.Application();
                Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@path);
                Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                Excel.Range xlRange = xlWorksheet.UsedRange;
                int rowCount = xlRange.Rows.Count;
                int colCount = xlRange.Columns.Count;

                for (int i = 1; i <= rowCount; i++)
                {
                    String[] data = new string[colCount];
                    for (int j = 1; j <= colCount; j++)
                    {
                        data[j - 1] = (xlRange.Cells[i, j] as Excel.Range).Value2.ToString();
                    }
                    var emp = new tbl_Employee();
                    emp.companyId = idCompany;
                    emp.employeeId = data[0];
                    emp.firstName = data[1];// change it to cto'r!!!
                    emp.lastName = data[2];
                    emp.Email = data[3];
                   // emp.Unit = data[4];
                   // emp.Class = data[5];
                    if (data[6].Equals("כן"))
                        emp.IsManger = true;
                    else
                        emp.IsManger = false;
                    this.addWorkerToDb(emp);
                }
                xlApp.Workbooks.Close();
            }
            finally
            {
                File.Delete(path);
            }
        }

        //-------------------------------------------------------------------------------company method--------------------------------------------------------

        public void addCompany(tbl_Company cmp)
        {

            this.manager.tbl_Companies.InsertOnSubmit(cmp);
            this.manager.SubmitChanges();
        }
        // add employee to db withought company
        // check if there is double! ( not checked!!!!)

        public void addWorkerToDb(tbl_Employee emp)
        {
            if (!manager.tbl_Employees.Contains(emp))
            {
                this.manager.tbl_Employees.InsertOnSubmit(emp);
                this.manager.SubmitChanges();
            }
        }


        public void deleteWorker(String workerid, int companyNumber)
        {
            // find the record to delete f
            var workerToDelete = manager.tbl_Employees.Where(a => a.employeeId == workerid && a.companyId == companyNumber).FirstOrDefault();
            manager.tbl_Employees.DeleteOnSubmit(workerToDelete);
            manager.SubmitChanges();

        }



    }


}