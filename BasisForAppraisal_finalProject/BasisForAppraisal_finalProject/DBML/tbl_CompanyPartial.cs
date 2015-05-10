using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasisForAppraisal_finalProject.Models;
using System.ComponentModel.DataAnnotations;

namespace BasisForAppraisal_finalProject.DBML
{
    [MetadataType(typeof(CompanyMetadata))]
    public partial class tbl_Company
    {
        private List<tbl_Unit> units= new List<tbl_Unit>();
        public List<tbl_Unit> Units
        {
            get
            {
                
                loadUnits();
                return units;
            }
           
        }
        public void loadUnits()
        {
            var dManager = new DataMangerCompany();
            units = dManager.getUnitsForCompany(companyId);
        }
        private List<tbl_Employee> employees;
      

        public List<tbl_Employee> Employees
        {
            get
            {
               
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

    public class CompanyMetadata
    {
        [Display(Name="שם")]
        [DataType(DataType.Text)]
        public string comapnyName { get; set; }

        [Display(Name = "טלפון:")]
        [DataType(DataType.PhoneNumber)]
        public string comapnyPhone { get; set; }

        [Display(Name = "כתובת:")]
        [DataType(DataType.Text)]
        public string comapnyAddress { get; set;}

    }
}
