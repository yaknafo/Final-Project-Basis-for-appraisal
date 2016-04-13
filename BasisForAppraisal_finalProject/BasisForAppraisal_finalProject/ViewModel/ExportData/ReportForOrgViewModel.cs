using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BasisForAppraisal_finalProject.DBML;
using BasisForAppraisal_finalProject.Models;

namespace BasisForAppraisal_finalProject.ViewModel.ExportData
{
    public class ReportForOrgViewModel
    {
        public ReportForOrganiztion ReportForOrganiztionModel { set; get; }

        public List<ReportForOrganiztionLine> ReportForOrganiztionLinesModel { set; get; }

        public ReportForOrganiztionLine[] ReportForOrganiztionLinesArrayModel { set; get; }

        public List<tbl_TypeQuestion> Categories { set; get; }

        public List<ReportForCompanyMultipleChoiceListAnswer> MultipleChoiceListAnswer { set; get; }

        public Dictionary<string, List<ReportForOrganiztionLine>> CategoriesDictionaryLines { set; get; }

        


        public int numberOfCharts { set; get; }
        public ReportForOrgViewModel(ReportForOrganiztion report)
        {
            ReportForOrganiztionModel = report;
            ReportForOrganiztionLinesModel = report.ReportForOrganiztionLines.ToList();
            //eportForOrganiztionLinesArrayModel = ReportForOrganiztionLinesModel.ToArray();
            Categories = ReportForOrganiztionLinesModel.Select(x => x.tbl_TypeQuestion).Distinct().ToList();
            CategoriesDictionaryLines = new Dictionary<string, List<ReportForOrganiztionLine>>();
            foreach (tbl_TypeQuestion c in Categories)
            {
                var listOfLines = ReportForOrganiztionLinesModel.Where(x => x.TypeQuestionName == c.Name).ToList();
                CategoriesDictionaryLines.Add(c.Name, listOfLines);

            }
            var dm = new DataManager();
            MultipleChoiceListAnswer = dm.GetReportForCompanyMultipleChoiceListAnswers(report.FormId, report.CompanyId);


            numberOfCharts = ReportForOrganiztionLinesModel.Count + MultipleChoiceListAnswer.Count;
        }
    }
}