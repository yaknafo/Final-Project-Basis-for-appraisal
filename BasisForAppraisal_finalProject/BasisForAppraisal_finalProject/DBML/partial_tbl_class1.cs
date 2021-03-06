﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BasisForAppraisal_finalProject.Models;
using System.ComponentModel.DataAnnotations;

namespace BasisForAppraisal_finalProject.DBML
{
    [MetadataType(typeof(ClassMetadata))]
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

    public class ClassMetadata
    {
        [Required(ErrorMessage = "שדה חובה")]
        [Display(Name = "מחלקה:")]
        public string unitName { get; set; }

        [Required(ErrorMessage = "שדה חובה")]
        [Display(Name = "מחזור:")]
        public string className { get; set; }
    }
}