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

        //----------------------------------------------- Excel Hellper ---------------------------------------------------//
        public void upload_excelfile(string path, int idCompany)
        {

            string unitName = string.Empty;

            string className = string.Empty;

            Excel.Application xlApp = new Excel.Application();
            try
            {
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
                        try
                        {
                        data[j - 1] = (xlRange.Cells[i, j] as Excel.Range).Value2.ToString();
                    }
                        catch
                        {
                            // the col of the unit and class
                            if(j==5 || j ==6)
                             data[j - 1] = string.Empty;
                        }
                    }
                    var emp = new tbl_Employee();
                    emp.companyId = idCompany;
                    emp.employeeId = data[0];
                    emp.firstName = data[1];// change it to cto'r!!!
                    emp.lastName = data[2];
                    emp.Email = data[3];
                    unitName = data[4];
                    className = data[5];
                    if (data[6].Equals("כן"))
                        emp.IsManger = true;
                    else
                        emp.IsManger = false;

                    // sreach for unit in Db
                    var unitFormDB = getUnitByName(unitName, idCompany);

                    if(unitFormDB == null)
                    {
                        unitFormDB = new tbl_Unit { companyId = idCompany, unitName = unitName };
                        AddUnit(unitFormDB);
                        emp.unitName = unitName;
                    }
                    else
                    {
                        emp.unitName = unitFormDB.unitName;
                    }

                    // sreach for unit in Db
                    // if unit not have a class ---> defult value will be Genral
                    if (className == string.Empty)
                        className = "Genral";

                    var ClassFormDB = getClassByName(className, unitName, idCompany);

                    if (ClassFormDB == null)
                    {
                        ClassFormDB = new tbl_Class { companyId = idCompany, unitName = unitName, className = className, tbl_Unit = unitFormDB };
                        AddClass(ClassFormDB);
                        emp.className = className;
                        emp.tbl_Class = ClassFormDB;
                    }
                    else
                    {
                        emp.className = ClassFormDB.className;
                    }



                   addWorkerToDb(emp);
                }
            }
            finally
            {
                xlApp.Workbooks.Close();
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


        //----------------------------------------------- add Methods ---------------------------------------------------//

        public void AddUnit(tbl_Unit unit)
        {

            manager.tbl_Units.InsertOnSubmit(unit);
            manager.SubmitChanges();
        }

        public void AddClass(tbl_Class cla)
        {

            manager.tbl_Classes.InsertOnSubmit(cla);
            manager.SubmitChanges();
        }
         public List<tbl_Employee> getEmployee(int idCompany,string unit,string cl)
         {
              return manager.tbl_Employees.Where(x => x.companyId == idCompany&&x.className.Equals(cl)&&x.unitName.Equals(unit)).ToList();
         }


        //----------------------------------------------- get Methods ---------------------------------------------------//

        public tbl_Unit getUnitByName(string unitName, int companyId)
        {
            return manager.tbl_Units.Where(x => x.unitName == unitName && x.companyId ==companyId).FirstOrDefault();
        }

        public tbl_Class getClassByName(string className ,string unitName, int companyId)
        {
            return manager.tbl_Classes.Where(x => x.className == className && x.unitName == unitName  && x.companyId == companyId).FirstOrDefault();
        }
        public List<tbl_Employee> getEmpForEmail(int id, string unit, string cl)
        {
            return manager.tbl_Employees.Where(x => x.companyId == id && x.unitName.Equals(unit) && x.className.Equals(cl)).ToList();
            
        }
    }


}