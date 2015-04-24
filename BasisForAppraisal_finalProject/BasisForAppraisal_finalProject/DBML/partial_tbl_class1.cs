using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BasisForAppraisal_finalProject.Models;

namespace BasisForAppraisal_finalProject.DBML
{

    public partial class tbl_Class
    {
        public List<tbl_Employee> employees;
        public List<tbl_Employee> Employees
        {
            get
            {
                LoadEmployees();
                return employees;
            }

        }



        public void LoadEmployees()
        {
             DataMangerCompany dmc= new DataMangerCompany();
             employees = dmc.getEmployee(this._companyId, this.unitName, this.className);
         }

    }
}