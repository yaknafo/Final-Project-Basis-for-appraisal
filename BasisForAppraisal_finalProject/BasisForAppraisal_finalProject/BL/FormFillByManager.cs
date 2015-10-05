using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasisForAppraisal_finalProject.DBML;

namespace BasisForAppraisal_finalProject.BL
{
    public class FormFillByManager
    {
        public tblForm Form { set; get; }

        public tbl_Employee Employee {set; get;}


        public override bool Equals(object obj)
        {
            if(obj is FormFillByManager )
            {
                return Form.formId == ((FormFillByManager)obj).Form.formId && Employee.employeeId == ((FormFillByManager)obj).Employee.employeeId;
            }
            return false;

        }



    }
}
