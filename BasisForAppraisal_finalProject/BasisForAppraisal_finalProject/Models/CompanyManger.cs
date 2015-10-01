using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BasisForAppraisal_finalProject.DBML;
using System.Data.OleDb;
using System.IO;
using System.Data;
using Excel;

namespace BasisForAppraisal_finalProject.Models
{
    public class CompanyManger
    {

        private static string defultUnit = "Genral";

        private static string defultClass = "Genral";

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

        public void DeleteConnectors(List<tbl_ConnectorFormFill> Connectors)
        {
             var db = new DataMangerCompany();
             for (int i = 0; i < Connectors.Count; i++)
                 db.DeleteConnector(Connectors[i].employeeFillId, Connectors[i].employeeOnId, Connectors[i].companyId, Connectors[i].formId);
            
        }


        // method add data from excel file to sql server db- workers to company
        public void UploadExcelFile(string path, int idCompany, string fileName)
        {
            try{
            FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read);

            ////1. Reading from a binary Excel file ('97-2003 format; *.xls)
            //IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream);
            //...
            //2. Reading from a OpenXml Excel file (2007 format; *.xlsx)
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
            //...
            //3. DataSet - The result of each spreadsheet will be created in the result.Tables
            DataSet result = excelReader.AsDataSet();
            //...
            ////4. DataSet - Create column names from first row
            //excelReader.IsFirstRowAsColumnNames = true;
            //DataSet result = excelReader.AsDataSet();
            var unitName = string.Empty;
            string className = string.Empty;
            var DM = new DataMangerCompany();
            //5. Data Reader methods
            for (int i = 0; i < result.Tables[0].Rows.Count; i++)
            {
                if (result.Tables[0].Rows[i][0] != string.Empty)
                {
                    tbl_Employee emp = GetEmployeeFromRow(result.Tables[0].Rows[i], result.Tables[0].Columns.Count, idCompany);
                    unitName = emp.unitName;
                    className = emp.className;
                    var inputStatus = validationRowInput(emp, ref unitName, ref className);

                    if (inputStatus)
                    {
                        unitName = SetUnitTOEmployee(idCompany, DM, unitName, emp);


                        className = SetClassToEmployee(idCompany, DM, unitName, className, emp);

                        DM.addWorkerToDb(emp);
                    }
                }
            }

             
            

            //6. Free resources (IExcelDataReader is IDisposable)
            excelReader.Close();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                //xlApp.Workbooks.Close();
                File.Delete(path);
            }
        }

        private tbl_Employee GetEmployeeFromRow(DataRow dataRow, int numberOfCol, int idCompany)
        {
            if (numberOfCol < 7)
                throw new Exception("הטבלה אינה תקינה מכילה פחות מ7 עמודות");
            var emp = new tbl_Employee();
            emp.companyId = idCompany;
            emp.employeeId = dataRow[0].ToString();
            emp.firstName = dataRow[1].ToString();
            emp.lastName = dataRow[2].ToString();
            emp.Email = dataRow[3].ToString();
            emp.className = dataRow[4].ToString();
            emp.unitName = dataRow[5].ToString();
            emp.IsManagerWrapper = (dataRow[6] == "כן");
            return emp;
        }


        //------------------------------ Private method of Excel upload method --------------------------------------------------------------- //
        private static string SetClassToEmployee(int idCompany, DataMangerCompany DM, string unitName, string className, tbl_Employee emp)
        {
            // sreach for unit in Db
            // if unit not have a class ---> defult value will be Genral
            if (className == string.Empty)
                className = defultClass;

            var ClassFormDB = DM.getClassByName(className, unitName, idCompany);

            try
            {
                if (ClassFormDB == null)
                {
                    var newClassFormDB = new tbl_Class { companyId = idCompany, unitName = unitName, className = className };
                    DM.AddClass(newClassFormDB);
                    // emp.className = newClassFormDB.className;
                    emp.className = newClassFormDB.className;

                    if (ClassFormDB != null && ClassFormDB.unitName != null)
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
                unitName = defultUnit;

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

        private bool validationRowInput(tbl_Employee employee, ref string unitName, ref string  className)
        {
            if (!employee.employeeId.All(char.IsDigit) || !(employee.employeeId.Length == 9))
                return false;

            if (string.IsNullOrEmpty(employee.firstName) || string.IsNullOrEmpty(employee.lastName))
                return false;

            if (string.IsNullOrEmpty(className))
               className = defultClass;

            if (string.IsNullOrEmpty(unitName))
                unitName = defultUnit;

            return true;
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



    }
}