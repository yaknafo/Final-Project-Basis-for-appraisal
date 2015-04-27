using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BasisForAppraisal_finalProject.DBML;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;

namespace BasisForAppraisal_finalProject.Models
{
    public class CompanyManger
    {

        public void addCompany(tbl_Company cmp)
        {
            var manger = new DataMangerCompany();
            manger.addCompany(cmp);
        }
        


        public void deleteWorker(String workerid, int companyNumber)
        {
            var db = new DataMangerCompany();
            db.deleteWorker(workerid, companyNumber);

        }

        // method add data from excel file to sql server db- workers to company
        public void UploadExcelFile(string path, int idCompany)
        {
            var DM = new DataMangerCompany();

            string unitName = string.Empty;

            string className = string.Empty;

            Excel.Application xlApp = new Excel.Application();

            var dictionary = new Dictionary<string , int>();

            dictionary.Add("Add", 0);
            dictionary.Add("Not Add", 0);

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

                    // getting the data of every row to data array string
                    GatDataOfRow(xlRange, colCount, i, data);

                    // setting all the valuues from the row in the excel into the new emoloyee
                    var emp = SetValuesInEmployee(idCompany, ref unitName, ref className, data);


                    unitName = SetUnitTOEmployee(idCompany, DM, unitName, emp);


                    className = SetClassToEmployee(idCompany, DM, unitName, className, emp);

                    DM.addWorkerToDb(emp);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                xlApp.Workbooks.Close();
                File.Delete(path);
            }
        }

        private static string SetClassToEmployee(int idCompany, DataMangerCompany DM, string unitName, string className, tbl_Employee emp)
        {
            // sreach for unit in Db
            // if unit not have a class ---> defult value will be Genral
            if (className == string.Empty)
                className = "Genral";

            var ClassFormDB = DM.getClassByName(className, unitName, idCompany);

            try
            {
                if (ClassFormDB == null)
                {
                    var newClassFormDB = new tbl_Class { companyId = idCompany, unitName = unitName, className = className };
                    DM.AddClass(newClassFormDB);
                    // emp.className = newClassFormDB.className;
                    emp.className = newClassFormDB.className;
                    if (ClassFormDB.unitName != null)

                        emp.unitName = ClassFormDB.unitName;
                    else
                        emp.unitName = unitName;

                }
                else
                {
                    emp.className = ClassFormDB.className;
                    emp.unitName = ClassFormDB.unitName;
                }
            }
            catch (Exception ex)
            {

            }
            return className;
        }

        private static string SetUnitTOEmployee(int idCompany, DataMangerCompany DM, string unitName, tbl_Employee emp)
        {
            // sreach for unit in Db
            // if unit not have a class ---> defult value will be Genral
            if (unitName == string.Empty)
                unitName = "Genral";

            // sreach for unit in Db
            var unitFormDB = DM.getUnitByName(unitName, idCompany);

            if (unitFormDB == null)
            {
                unitFormDB = new tbl_Unit { companyId = idCompany, unitName = unitName };
                DM.AddUnit(unitFormDB);
                emp.unitName = unitFormDB.unitName;
            }
            return unitName;
        }

        private static tbl_Employee SetValuesInEmployee(int idCompany, ref string unitName, ref string className, String[] data)
        {
            var emp = new tbl_Employee();
            emp.companyId = idCompany;
            emp.employeeId = data[0];
            emp.firstName = data[1];// change it to cto'r!!!!
            emp.lastName = data[2];
            emp.Email = data[3];
            unitName = data[4];
            className = data[5];
            if (data[6].Equals("כן"))
                emp.IsManger = true;
            else
                emp.IsManger = false;
            return emp;
        }

        private static void GatDataOfRow(Excel.Range xlRange, int colCount, int i, String[] data)
        {
            for (int j = 1; j <= colCount; j++)
            {
                try
                {
                    data[j - 1] = (xlRange.Cells[i, j] as Excel.Range).Value2.ToString();
                }
                catch
                {
                    // the col of the unit and class
                    if (j == 5 || j == 6)
                        data[j - 1] = string.Empty;
                }
            }
        }


    }
}