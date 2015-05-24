using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BasisForAppraisal_finalProject.DBML;
using System.Data.Linq;
using System.IO;
using Microsoft.Office.Interop;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using BasisForAppraisal_finalProject.Common.Constans;

namespace BasisForAppraisal_finalProject.Models
{

    public class DataMangerCompany
    {
        private BFADataBasedbmlDataContext manager; 
        public DataMangerCompany()
        {
            manager= new BFADataBasedbmlDataContext();
            this.manager = DbmlBFADataContext.GetDataContextInstance();
            var cultureinfo = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentCulture = cultureinfo;
        }

       //-------------------------------------------------- Get Method --------------------------------------------------

        public Table<tbl_ConnectorAnswer> ConnectorAnswer
        {
            get { return manager.tbl_ConnectorAnswers; }
        }

        public Table<tbl_ConnectorFormFill> ConnectorFormFill
        {
            get { return manager.tbl_ConnectorFormFills; }
        }

        public List<tbl_ConnectorFormFill> Conecctors()
        {
            return manager.tbl_ConnectorFormFills.ToList();
        }
        public Table<tbl_Unit> Units
        {
            get { return manager.tbl_Units; }
        }
        public Table<tbl_Class> Class
        {
            get { return manager.tbl_Classes; }
        }
      

        public List<tbl_Unit> getUnitsForCompany(int id)
        {
            return this.manager.tbl_Units.Where(x => x.companyId == id).ToList();
        }
        //--------------------------------------------------Delete Method --------------------------------------------------
       

        public void deleteWorker(String workerid, int companyNumber)
        {

            // get connectorsAnswers cant delete employe before delete all connectorsAnswers that his id is key there
            var connecAnswer = manager.tbl_ConnectorAnswers.Where(x => x.employeeFillId == workerid || x.employeeOnId == workerid).ToList();
            manager.tbl_ConnectorAnswers.DeleteAllOnSubmit(connecAnswer);

            //get connectors cant delete employe before delete all connectors that his id is key there
            var connector = manager.tbl_ConnectorFormFills.Where(x => x.employeeFillId == workerid || x.employeeOnId == workerid).ToList();
            manager.tbl_ConnectorFormFills.DeleteAllOnSubmit(connector);

         
            // // delete worker --> the worker has been deleted from the DB.
            var workerToDelete = manager.tbl_Employees.Where(a => a.employeeId == workerid && a.companyId == companyNumber).FirstOrDefault();
            manager.tbl_Employees.DeleteOnSubmit(workerToDelete);

            // delete Identity --> the user cant login to the system any more
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var userExiset = UserManager.FindByName(workerid);
            UserManager.Delete(userExiset);

            manager.SubmitChanges();

        }
        private void deleteConectForWorker(int companyid , List<tbl_Employee> myList)
        {
            var listcomapny= manager.tbl_ConnectorFormFills.Where(x => x.companyId == companyid).ToList();
            foreach(var person in myList)
            {
                var delete= listcomapny.Where(x=>x.employeeFillId==person.employeeId||x.employeeOnId==person.employeeId).ToList();
                manager.tbl_ConnectorFormFills.DeleteAllOnSubmit(delete);
               

            }
        }
             public void deleteUnitAndClass(string className, string unitid, int companyid)
        {
            // find the record to delete f
            var unitToDelete = manager.tbl_Units.Where(a => a.companyId == companyid && a.unitName.Equals(unitid)).FirstOrDefault();
            var classToDelete = manager.tbl_Classes.Where(x => x.companyId == companyid && x.tbl_Unit.unitName == unitToDelete.unitName&&x.className.Equals(className)).FirstOrDefault();
            var workersToDelete = manager.tbl_Employees.Where(x => x.companyId == companyid&&x.className.Equals(className)&&x.unitName.Equals(unitid)).ToList();
            deleteConectForWorker(companyid, workersToDelete);
            manager.SubmitChanges();
            manager.tbl_Employees.DeleteAllOnSubmit(workersToDelete);
            manager.SubmitChanges();
            var workersAfterDelete = manager.tbl_Employees.Where(x => x.className.Equals(unitid)).ToList();
            // if somone use this class-> dont delete it!
            if (!(workersAfterDelete.Count() > 0))
                {
                    manager.tbl_Classes.DeleteOnSubmit(classToDelete);
                    manager.SubmitChanges();
                }
            workersAfterDelete = manager.tbl_Employees.Where(x => x.unitName.Equals(unitid)).ToList();
            manager.SubmitChanges();
            // if somone use this unit-> dont delete it!
            if (!(workersAfterDelete.Count() > 0))
            {
                manager.tbl_Units.DeleteOnSubmit(unitToDelete);
                manager.SubmitChanges();
            }
          
        }

         public void deleteCompany(int companyid)
        {
            var conectionToDelete = manager.tbl_ConnectorFormFills.Where(x => x.companyId == companyid).ToList();
            manager.tbl_ConnectorFormFills.DeleteAllOnSubmit(conectionToDelete);
            var workersToDelete = manager.tbl_Employees.Where(x => x.companyId == companyid).ToList();
            manager.tbl_Employees.DeleteAllOnSubmit(workersToDelete);
            var classToDelete = manager.tbl_Classes.Where(x => x.companyId == companyid).ToList();
            manager.tbl_Classes.DeleteAllOnSubmit(classToDelete);
            var unitsToDelete = manager.tbl_Units.Where(x => x.companyId == companyid).ToList();
            manager.tbl_Units.DeleteAllOnSubmit(unitsToDelete);
            var companyToDelete = manager.tbl_Companies.Where(x => x.companyId == companyid).ToList();
            manager.tbl_Companies.DeleteAllOnSubmit(companyToDelete);
            manager.SubmitChanges();

                
        }

        public void DeleteConnectorAnswers(tbl_ConnectorAnswer ca)
         {
             var caForDelete = ConnectorAnswer.Where(x => x.Equals(ca)).FirstOrDefault();

             if (caForDelete == null)
                 return;

             ConnectorAnswer.DeleteOnSubmit(caForDelete);
             manager.SubmitChanges();

         }


        //----------------------------------------------- add Methods ---------------------------------------------------//
        public void addCompany(tbl_Company cmp)
        {

            this.manager.tbl_Companies.InsertOnSubmit(cmp);
            this.manager.SubmitChanges();
        }
        // add employee to db withought company
        // check if there is double! ( not checked!!!!)

        public async void addWorkerToDb(tbl_Employee emp)
        {
            if (!manager.tbl_Employees.Contains(emp))
            {
                CreateUserWithRole(emp.employeeId, emp.employeeId, RolesConstans.Guest);

                this.manager.tbl_Employees.InsertOnSubmit(emp);
                
                this.manager.SubmitChanges();

            }
        }


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


            var tempconnector = new tbl_ConnectorFormFill { employeeFillId = employeeFillID, employeeOnId = employeeOnId, companyId = companyId, formId = formID };
            if (manager.tbl_ConnectorFormFills.Contains(tempconnector))
                return;
            manager.tbl_ConnectorFormFills.InsertOnSubmit(tempconnector);
            manager.SubmitChanges();


        }

        public void AddConnectorAnswers(tbl_ConnectorAnswer ca)
        {
            if (ca == null || ca.AnswerId == 0 || ca.AnswerId == null || ConnectorAnswer.Contains(ca))
                return;

            ConnectorAnswer.InsertOnSubmit(ca);

           

            manager.SubmitChanges();

        }

        public void AddConnectorAnswers(List<tbl_ConnectorAnswer> ca)
        {

            ca.ForEach(AddConnectorAnswers);

            // flag done should be true after saving all the answer of the from
            if(ca.Any())
            {
             var connectorFalgDone = manager.tbl_ConnectorFormFills.Where(x => x.companyId == ca[0].companyId && x.employeeFillId == ca[0].employeeFillId
                                                                        && x.employeeOnId == ca[0].employeeOnId && x.formId == ca[0].FormId).FirstOrDefault();

             connectorFalgDone.done = true;

             manager.SubmitChanges();
            }

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

        //----------------------------------------------- Delete Method ---------------------------------------------------//

        public void DeleteConnector(string employeeFillID, string employeeOnId, int companyId, int formID)
        {
            if (string.IsNullOrEmpty(employeeFillID) || string.IsNullOrEmpty(employeeOnId))
                throw new Exception("employee id is missing");

            if (manager.tbl_Companies.Where(x => x.companyId == companyId).FirstOrDefault() == null)
                throw new Exception("Company not exit in the DB");

            if (manager.tblForms.Where(x => x.formId == formID).FirstOrDefault() == null)
                throw new Exception("form not exit in the DB");


          //var tempconnector = new tbl_ConnectorFormFill { employeeFillId = employeeFillID, employeeOnId = employeeOnId, companyId = 1, formId = formID };

            var tempconnector = manager.tbl_ConnectorFormFills.Where(x => x.companyId == companyId && x.employeeFillId == employeeFillID
                                                                     && x.employeeOnId == employeeOnId && x.formId == formID).FirstOrDefault();

            if (tempconnector == null)
                return;
            manager.tbl_ConnectorFormFills.DeleteOnSubmit(tempconnector);
            manager.SubmitChanges();

        }


        public void DeleteConnectors(List<tbl_ConnectorFormFill> Connectors)
        {
           
            manager.tbl_ConnectorFormFills.DeleteAllOnSubmit(Connectors);
            manager.SubmitChanges();

        }


        //  ------------------------------- secutiry --------------------------------------------------------------//


        public async void CreateRole(string roleName)
        {
            var roleManager = new RoleManager<Microsoft.AspNet.Identity.EntityFramework.IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

            if (!roleManager.RoleExists(roleName))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = roleName;
                roleManager.Create(role);

            }

        }


        public async Task<string> getRoleById(string roleId)
        {
            var roleManager = new RoleManager<Microsoft.AspNet.Identity.EntityFramework.IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

            var role = roleManager.Roles.Where(x => x.Id == roleId).FirstOrDefault();

            return role.Name;
        }

        public async void CreateUserWithRole(string userName, string password, string roleName)
        {
            var roleManager = new RoleManager<Microsoft.AspNet.Identity.EntityFramework.IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));
            var role = roleManager.FindByNameAsync(roleName);

            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = new ApplicationUser() { UserName = userName };

            var result = await UserManager.CreateAsync(user, password);

            var userExiset = UserManager.FindByName(userName);

            var roleExiset = roleManager.FindByName(roleName);
            if (roleExiset == null)
            {
                CreateRole(roleName);
                roleExiset = roleManager.FindByName(roleName);
            }

            if (result.Succeeded && userExiset != null && roleExiset != null)
            {
                await UserManager.AddToRoleAsync(userExiset.Id, roleName);
            }


        }


    }


    


}