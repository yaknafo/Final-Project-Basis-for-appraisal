using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasisForAppraisal_finalProject.Models;

namespace BasisForAppraisal_finalProject.DBML
{
    public partial class tbl_Company
    {

        private List<tbl_Employee> employees;

        public List<tbl_Employee> Employees
        {
            get
            {
                if (employees == null)
                    LoadEmployees();
                return employees;
            }
            set
            {
                employees = value;
            }
        }

        // init all the employee of the company 
        public void LoadEmployees()
        {
            var dManager = new DataManager();
            Employees= dManager.getEmployeesByCompanyId(companyId);
        }
    }
}
