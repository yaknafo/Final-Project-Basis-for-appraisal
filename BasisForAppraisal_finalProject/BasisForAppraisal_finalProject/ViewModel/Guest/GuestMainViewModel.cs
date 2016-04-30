using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasisForAppraisal_finalProject.DBML;
using System.Web.Mvc;

namespace BasisForAppraisal_finalProject.ViewModel.Guest
{
    public class GuestMainViewModel
    {
        public tbl_Employee Emp  {set; get;}

        public List<tbl_ConnectorFormFill> Froms { set; get; }

        public List<ReportForIndividual> Reports { set; get; }

        public bool IsLeader
        {
            get
            {
                return Emp.IsManagerWrapper;
            }
        }
    }
}
