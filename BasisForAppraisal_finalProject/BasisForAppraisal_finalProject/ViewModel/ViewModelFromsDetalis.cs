using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BasisForAppraisal_finalProject.DBML;
using BasisForAppraisal_finalProject.Models;

namespace BasisForAppraisal_finalProject.ViewModel
{
    public class ViewModelFromsDetalis
    {
        public List<tblForm> forms { set; get; }

        public ViewModelFromsDetalis(List<tblForm> forms)
        {
            this.forms = forms;
        }


      
    }
}