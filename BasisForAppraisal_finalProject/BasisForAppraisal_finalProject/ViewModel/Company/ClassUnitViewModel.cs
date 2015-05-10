using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BasisForAppraisal_finalProject.DBML;
using BasisForAppraisal_finalProject.Models;

namespace BasisForAppraisal_finalProject.ViewModel.Company
{
    public class ClassUnitViewModel
    {
        private tbl_Class myClass;

        private List<tblForm> forms;
        public bool onlymanger{set;get;}
        public bool onlyworkers { set; get; }
        public bool workersAndManger { set; get; }


        public ClassUnitViewModel()
        {

        }

        public ClassUnitViewModel(tbl_Class myClass)
        {
            this.myClass = myClass;
        }

        public tbl_Class MyClass
        {
            get
            {
                return myClass;
            }
            set
            {
                myClass = value;
            }
        }

        public List<tblForm> Forms
        {
            get
            {
                return forms ?? InitForms();
            }

            set
            {
                forms = value;
            }
        }


        public List<tblForm> InitForms()
        {
            var DB= new DataManager();
            forms = DB.Forms.ToList();
            return forms;
        }
    }
}