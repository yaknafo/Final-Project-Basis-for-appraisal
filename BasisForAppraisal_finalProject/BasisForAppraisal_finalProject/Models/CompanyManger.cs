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
        // method add data from excel file to sql server db- workers to company
        public void upload_excelfile(string path, int idCompany)
        {
            var db = new DataMangerCompany();
            db.upload_excelfile(path, idCompany);
            //AddUnits(path, idCompany);
            //AddClasss(path, idCompany);



        }

        public void deleteWorker(String workerid, int companyNumber)
        {
            var db = new DataMangerCompany();
            db.deleteWorker(workerid, companyNumber);

        }

        public void AddUnits(string path, int idCompany)
         {

             Microsoft.Office.Interop.Excel.Application xlApp = new Excel.Application();

             try
             {
                 Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@path);
                 Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                 Excel.Range xlRange = xlWorksheet.UsedRange;
                 int rowCount = xlRange.Rows.Count;
                 int colCount = xlRange.Columns.Count;

                 var units= new List<tbl_Unit>();
                 for (int i = 1; i <= rowCount; i++)
                 {
                            //String[] data = new string[colCount];
                    
                             var stringNameUNit = (xlRange.Cells[i, 5] as Excel.Range).Value2.ToString();

                             var tempunit= new tbl_Unit{unitName = stringNameUNit,companyId= idCompany};
                       
                     units.Add(tempunit);
                   }

                 var DM= new DataMangerCompany();

                 units.ForEach(u => DM.AddUnit(u));}
                 catch (Exception ex)
             {

             }
             finally
             {
                 xlApp.Workbooks.Close();
            }
          }


        public void AddClasss(string path, int idCompany)
        {

            Microsoft.Office.Interop.Excel.Application xlApp = new Excel.Application();
            var DM = new DataMangerCompany();

            try
            {
                Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@path);
                Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                Excel.Range xlRange = xlWorksheet.UsedRange;
                int rowCount = xlRange.Rows.Count;
                int colCount = xlRange.Columns.Count;

                var classes = new List<tbl_Class>();
                for (int i = 1; i <= 4; i++)
                {
                    //String[] data = new string[colCount];
                    var stringNameUNit = (xlRange.Cells[i, 5] as Excel.Range).Value2.ToString();
                    var stringNameClass = (xlRange.Cells[i, 6] as Excel.Range).Value2.ToString();

                    var tempclass = new tbl_Class { unitName = stringNameUNit, companyId = idCompany, className = stringNameClass };

                    if (DM.getClassByName(stringNameClass, stringNameUNit, idCompany) == null)
                    classes.Add(tempclass);
                }




                DM.AddClass(classes);
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
         }
    }




//                     var emp = new tbl_Employee();
//                     emp.companyId = idCompany;
//                     emp.employeeId = data[0];
//                     emp.firstName = data[1];// change it to cto'r!!!
//                     emp.lastName = data[2];
//                     emp.Email = data[3];
//                     unitName = data[4];
//                     className = data[5];
//                     if (data[6].Equals("כן"))
//                         emp.IsManger = true;
//                     else
//                         emp.IsManger = false;

//                     // sreach for unit in Db
//                     var unitFormDB = getUnitByName(unitName, idCompany);

//                     if (unitFormDB == null)
//                     {
//                         unitFormDB = new tbl_Unit { companyId = idCompany, unitName = unitName };
//                         AddUnit(unitFormDB);
//                         emp.unitName = unitFormDB.unitName;
//                     }
//                     else
//                     {
//                         emp.unitName = unitFormDB.unitName;
//                     }

//                     // sreach for unit in Db
//                     // if unit not have a class ---> defult value will be Genral
//                     if (className == string.Empty)
//                         className = "Genral";

//                     var ClassFormDB = getClassByName(className, unitName, idCompany);

//                     try
//                     {
//                         if (ClassFormDB == null)
//                         {
//                             var newClassFormDB = new tbl_Class { companyId = idCompany, unitName = unitName, className = className };
//                             AddClass(newClassFormDB, unitFormDB);
//                             emp.className = newClassFormDB.className;
//                             emp.tbl_Class = newClassFormDB;
//                         }
//                         else
//                         {
//                             emp.className = ClassFormDB.className;
//                         }
//                     }
//                     catch (Exception ex)
//                     {

//                     }



//                     // addWorkerToDb(emp);
//                 }
//             }
//             catch (Exception ex)
//             {

//             }
//             finally
//             {
//                 xlApp.Workbooks.Close();
//                 File.Delete(path);
//             }
//         }
//    }
//}