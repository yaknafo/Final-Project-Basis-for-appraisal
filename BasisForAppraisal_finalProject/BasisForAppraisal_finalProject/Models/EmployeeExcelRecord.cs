using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasisForAppraisal_finalProject.DBML;

namespace BasisForAppraisal_finalProject.Models
{
    public class EmployeeExcelRecord
    {
        public tbl_Employee Employee { set; get; }

        public List<tbl_ConnectorAnswer> ConnectorAnswer { set; get; }

    }
}
