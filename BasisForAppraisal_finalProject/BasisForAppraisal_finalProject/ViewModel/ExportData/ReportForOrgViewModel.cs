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

        public List<string> Categories { set; get; }

        public Dictionary<string, List<ReportForOrganiztionLine>> CategoriesDictionaryLines { set; get; }

        public ReportForOrgViewModel(ReportForOrganiztion report)
        {
            ReportForOrganiztionModel = report;
            ReportForOrganiztionLinesModel = report.ReportForOrganiztionLines.ToList();
            //eportForOrganiztionLinesArrayModel = ReportForOrganiztionLinesModel.ToArray();
            Categories = ReportForOrganiztionLinesModel.Select(x => x.TypeQuestionName).Distinct().ToList();
            Categories.ForEach(x => x.Replace(" ", ""));
            CategoriesDictionaryLines = new Dictionary<string, List<ReportForOrganiztionLine>>();
            foreach(string c in Categories)
            {
                var listOfLines = ReportForOrganiztionLinesModel.Where(x => x.TypeQuestionName == c).ToList();
                CategoriesDictionaryLines.Add(c, listOfLines);

            }

        }
    }
}