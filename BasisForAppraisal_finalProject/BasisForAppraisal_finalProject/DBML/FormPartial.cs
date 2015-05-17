﻿using BasisForAppraisal_finalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasisForAppraisal_finalProject.DBML
{
    public partial class tblForm
    {

        private List<tbl_Section> sections = new List<tbl_Section>();

        private List<tbl_Company> companies;



        public List<tbl_Section> Sections
        {
            get
            {
                sections = GetAllSections();
                return sections;
            }
            set
            {
                sections = value;
            }

        }

        public List<tbl_Company> Companies
        {
            get
            {
                return companies ?? GetAllCompanies();
            }
            set
            {
                companies = value;
            }

        }




        public List<tbl_Section> GetAllSections()
        {
            var manager = new DataManager();
            return manager.Sections.Where(x => x.FormId == this.formId).ToList();
        }


        public List<tbl_Company> GetAllCompanies()
        {
            var DMC = new DataMangerCompany();
            companies = DMC.ConnectorFormFill.Where(x => x.formId == formId).Select(c => c.tbl_Company).ToList();
            return companies;
        }

      


    }
}