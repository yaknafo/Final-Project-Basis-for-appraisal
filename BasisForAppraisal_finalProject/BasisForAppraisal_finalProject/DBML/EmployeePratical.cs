using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BasisForAppraisal_finalProject.DBML
{

    [MetadataType(typeof(EmployeeMetadata))]
    public partial class tbl_Employee
    {
        private Dictionary<tblForm, tbl_Employee> fillOnMe;

        private Dictionary<tblForm, tbl_Employee> fillOnThem;

      
    }

    public class EmployeeMetadata
    {
       
        [Display(Name = "מחלקה:")]
        [DataType(DataType.Text)]
        public string unitName { get; set; }

        [Display(Name = "מחזור:")]
        [DataType(DataType.Text)]
        public string className { get; set; }

        [Display(Name = "מייל:")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "שם פרטי:")]
        [DataType(DataType.Text)]
        public string firstName { get; set; }

        [Display(Name = "שם פרטי:")]
        [DataType(DataType.Text)]
        public string lastName { get; set; }


        [Display(Name = "תז:")]
        [DataType(DataType.Text)]
        public string employeeId { get; set; }

        [Display(Name = "מנהל:")]
        [DataType(DataType.Text)]
        public string IsManger { get; set; }




    }
}