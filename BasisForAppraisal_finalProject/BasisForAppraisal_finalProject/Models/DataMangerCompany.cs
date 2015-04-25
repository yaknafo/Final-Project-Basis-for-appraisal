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

       //-------------------------------------------------- Get Method --------------------------------------------------
        public List<tbl_ConnectorFormFill> getConecctors()
        {
            return manager.tbl_ConnectorFormFills.ToList();
        }


        //-------------------------------------------------- Add Method --------------------------------------------------
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
            if (!manager.tbl_Units.Contains(unit))
            {
                var compantForUnit = manager.tbl_Companies.Where(x => x.companyId == unit.companyId).FirstOrDefault();
                if (compantForUnit != null)
                {
                    unit.tbl_Classes = null;
                    manager.tbl_Units.InsertOnSubmit(unit);
                    unit.tbl_Classes = null;
                    manager.SubmitChanges();
                }
            }
        }


        public void AddClass(List<tbl_Class> cla)
        {
            manager.tbl_Classes.InsertAllOnSubmit(cla);
            manager.SubmitChanges();
        }

        public void AddClass(tbl_Class cla)
        {

            if (!manager.tbl_Classes.Contains(cla))
            {
               var c = manager.tbl_Units.Where(s => s.unitName == cla.unitName && s.companyId == cla.companyId).First();
               cla.tbl_Unit = c;
                manager.tbl_Classes.InsertOnSubmit(cla);
                manager.SubmitChanges();
            }
        }

        public void AddConnector(string employeeFillID, string employeeOnId, int companyId, int formID)
        {
            if (string.IsNullOrEmpty(employeeFillID) || string.IsNullOrEmpty(employeeOnId))
                throw new Exception("employee id is missing");

            if(manager.tbl_Companies.Where(x=> x.companyId ==companyId).FirstOrDefault() == null)
                throw new Exception("Company not exit in the DB");

            if (manager.tblForms.Where(x => x.formId == formID).FirstOrDefault() == null)
                throw new Exception("form not exit in the DB");


            var tempconnector = new tbl_ConnectorFormFill { employeeFillId = employeeFillID, employeeOnId = employeeOnId, companyId = 1, formId = formID };
            if (manager.tbl_ConnectorFormFills.Contains(tempconnector))
                return;
            manager.tbl_ConnectorFormFills.InsertOnSubmit(tempconnector);
            manager.SubmitChanges();

        }

        //----------------------------------------------- get Methods ---------------------------------------------------//

         public List<tbl_Employee> getEmployee(int idCompany,string unit,string cl)
         {
              return manager.tbl_Employees.Where(x => x.companyId == idCompany&&x.className.Equals(cl)&&x.unitName.Equals(unit)).ToList();
         }


       

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