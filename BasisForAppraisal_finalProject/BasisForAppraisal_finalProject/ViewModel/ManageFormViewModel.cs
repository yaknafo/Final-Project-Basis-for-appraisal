using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BasisForAppraisal_finalProject.DBML;
using BasisForAppraisal_finalProject.Models;

namespace BasisForAppraisal_finalProject.ViewModel
{
    public class ManageFormViewModel
    {
        public List<tblForm> Forms {get; set;}


      public ManageFormViewModel ()
	{
          var manager= new DataManager();
          Forms = manager.Forms.ToList();
	}
    }
}