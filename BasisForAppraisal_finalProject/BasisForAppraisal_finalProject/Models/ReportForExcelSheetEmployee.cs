using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BasisForAppraisal_finalProject.Models;

namespace BasisForAppraisal_finalProject.Models
{
    public class ReportForExcelSheetEmployee
    {
        public List<EmployeeExcelRecord> EmployeeExcelRecordList { set; get; }

        public void GetAllEmployee(int companyId, string cls, string unit)
        {
            var DM = new DataManager();
            var employeeList = DM.getEmployeesByCompanyId(companyId).Where(x => x.className == cls && x.unitName == unit).ToList();
        }
    }
}