using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BasisForAppraisal_finalProject.DBML;
using System.Data.OleDb;
using System.IO;
using System.Data;

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

            var DM = new DataMangerCompany();

            string unitName = string.Empty;
            DataSet ds = new DataSet();
            string className = string.Empty;
              string excelConnectionString = string.Empty;

              excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
              path + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
              //connection String for xls file format.
              if (fileName == ".xls")
              {
                  excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                  path + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
              }
              //connection String for xlsx file format.
              else if (fileName == ".xlsx")
              {
                  excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
                  path + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
              }

              //Create Connection to Excel work book and add oledb namespace
              OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
              excelConnection.Open();
              DataTable dt = new DataTable();

              dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
              

              String[] excelSheets = new String[dt.Rows.Count];
              int t = 0;
              //excel data saves in temp file here.
              foreach (DataRow row in dt.Rows)
              {
                  excelSheets[t] = row["TABLE_NAME"].ToString();
                  t++;
              }
              OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);


              string query = string.Format("Select * from [{0}]", excelSheets[0]);
              using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
              {
                  dataAdapter.Fill(ds);
              }

              for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
              {

                  tbl_Employee emp = GetEmployeeFromRow(ds.Tables[0].Rows[i], ds.Tables[0].Columns.Count, idCompany);
                  string querty = "Insert into Person(Name,Email,Mobile) Values('" +
                  ds.Tables[0].Rows[i][0].ToString() + "','" + ds.Tables[0].Rows[i][1].ToString() +
                  "','" + ds.Tables[0].Rows[i][2].ToString() + "')";
                  if (i == 0)
                      throw new Exception(emp.firstName + " " + emp.lastName);
                 
              }

            //Excel.Application xlApp = new Excel.Application();

            //var dictionary = new Dictionary<string , int>();

            //dictionary.Add("Add", 0);
            //dictionary.Add("Not Add", 0);

            try
            {
            //    Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@path);
            //    Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
            //    Excel.Range xlRange = xlWorksheet.UsedRange;
            //    int rowCount = xlRange.Rows.Count;
            //    int colCount = xlRange.Columns.Count;

            //    for (int i = 1; i <= rowCount; i++)
            //    {
            //        String[] data = new string[colCount];

            //        // getting the data of every row to data array string
            //        GatDataOfRow(xlRange, colCount, i, data);

            //        // setting all the valuues from the row in the excel into the new emoloyee
            //        var emp = SetValuesInEmployee(idCompany, ref unitName, ref className, data);

            //        // check stauts of the input of the current row
            //        var inputStatus = validationRowInput(emp, ref unitName, ref className);

            //        if (inputStatus)
            //        {
            //            unitName = SetUnitTOEmployee(idCompany, DM, unitName, emp);


            //            className = SetClassToEmployee(idCompany, DM, unitName, className, emp);

            //            DM.addWorkerToDb(emp);
            //        }
            //    }
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