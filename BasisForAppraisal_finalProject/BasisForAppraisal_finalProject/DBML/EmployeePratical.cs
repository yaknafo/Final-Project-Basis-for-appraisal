using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BasisForAppraisal_finalProject.Models;

namespace BasisForAppraisal_finalProject.DBML
{

    [MetadataType(typeof(EmployeeMetadata))]
    public partial class tbl_Employee
    {
        private List<tbl_ConnectorFormFill> fillOnMe = null;

        private List<tbl_ConnectorFormFill> fillOnThem = null;

        public List<tbl_ConnectorFormFill> FillOnMe
        {
            get
            {
                return fillOnMe ?? InitfillOnMe();
            }
            set
            {
                fillOnMe = value;
            }
        }


        public List<tbl_ConnectorFormFill> FillOnThem
        {
            get
            {
                return fillOnThem?? InitfillOnThem();
            }
            set
            {
                fillOnThem= value;
            }
        }



        private List<tbl_ConnectorFormFill> InitfillOnMe()
        {
            var DM = new DataMangerCompany();

            var dic = new List<tbl_ConnectorFormFill>();

            var conecctor = DM.Conecctors().ToList();

            foreach(tbl_ConnectorFormFill f in conecctor)
            {
                if(f.employeeOnId == this.employeeId)
                dic.Add(f);
            }

            fillOnMe = dic;

            return dic;
        }


        private List<tbl_ConnectorFormFill> InitfillOnThem()
        {
            var DM = new DataMangerCompany();

            var dic = new List<tbl_ConnectorFormFill>();

            var conecctor = DM.Conecctors().Where(x => x.employeeFillId == this.employeeId).ToList();

            foreach (tbl_ConnectorFormFill f in conecctor)
            {
                dic.Add(f);
            }

            fillOnThem = dic;

            return dic;
        }

      public void RefreshConecctors()
      {
          InitfillOnMe();
          InitfillOnThem();
      }
      
    }

    public class EmployeeMetadata
    {
        [StringLength(30, MinimumLength = 2)]
        [Required (ErrorMessage="שדה חובה")]
        [Display(Name = "מחלקה:")]
        public string unitName { get; set; }

        [Required(ErrorMessage = "שדה חובה")]
        [Display(Name = "מחזור:")]
        public string className { get; set; }
       [EmailAddress(ErrorMessage = "מייל לא תקין")]
        [Required(ErrorMessage = "שדה חובה")]
        [Display(Name = "מייל:")]
        public string Email { get; set; }

        [Required(ErrorMessage = "שדה חובה")]
        [Display(Name = "שם פרטי:")]
        public string firstName { get; set; }
        [StringLength(30, MinimumLength = 1)]
        [Required(ErrorMessage = "שדה חובה")]
        [Display(Name = "שם משפחה:")]
        public string lastName { get; set; }

        [StringLength(9, MinimumLength = 9, ErrorMessage = "תז -9 ספרות")]
        [Required(ErrorMessage = "שדה חובה")]
        [Display(Name = "תז:")]
        public string employeeId { get; set; }

        [Required(ErrorMessage = "שדה חובה")]
        [Display(Name = "חברה:")]
        public int companyId { get; set; }


        [Display(Name = "מנהל:")]
        public string IsManger { get; set; }




    }
}