using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasisForAppraisal_finalProject.DBML;
using BasisForAppraisal_finalProject.Models;

namespace BasisForAppraisal_finalProject.ViewModel
{
    public class CompanyViewModel
    {
        public tbl_Company Company { set; get; }

        public List<tbl_Employee> Employees { get; set; }

        public CompanyViewModel(tbl_Company Company)
        {
            this.Company = Company;

            this.Employees = Company.Employees;

        }

       
    }
}
