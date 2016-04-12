using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BasisForAppraisal_finalProject.DBML;

namespace BasisForAppraisal_finalProject.ViewModel.ExportData
{
    public class ReportForOrgViewModel
    {
        public ReportForOrganiztion ReportForOrganiztionModel { set; get; }

        public List<ReportForOrganiztionLine> ReportForOrganiztionLinesModel { set; get; }

        public ReportForOrganiztionLine[] ReportForOrganiztionLinesArrayModel { set; get; }

        public ReportForOrgViewModel(ReportForOrganiztion report)
        {
            ReportForOrganiztionModel = report;
            ReportForOrganiztionLinesModel = report.ReportForOrganiztionLines.ToList();
            ReportForOrganiztionLinesArrayModel = ReportForOrganiztionLinesModel.ToArray();
        }
    }
}