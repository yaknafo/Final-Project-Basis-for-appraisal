using BasisForAppraisal_finalProject.DBML;
using BasisForAppraisal_finalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasisForAppraisal_finalProject.ViewModel.ExportData
{
    public class ReportForUnitViewModel
    {
        public ReportForUnit ReportForUnitModel { set; get; }

        public List<ReportForUnitLine> ReportForUnitLinesModel { set; get; }

        public ReportForUnitLine[] ReportForUnitLinesArrayModel { set; get; }

        public List<tbl_TypeQuestion> Categories { set; get; }

        public List<ReportForUnitMultipleChoiceListAnswer> MultipleChoiceListAnswer { set; get; }

        public Dictionary<string, List<ReportForUnitLine>> CategoriesDictionaryLines { set; get; }

        public int numberOfCharts { set; get; }

        public ReportForUnitViewModel(ReportForUnit report)
        {
            ReportForUnitModel = report;
            ReportForUnitLinesModel = report.ReportForUnitLines.ToList();
            Categories = ReportForUnitLinesModel.Select(x => x.tbl_TypeQuestion).Distinct().ToList();
            CategoriesDictionaryLines = new Dictionary<string, List<ReportForUnitLine>>();

            foreach (tbl_TypeQuestion c in Categories)
            {
                var listOfLines = ReportForUnitLinesModel.Where(x => x.TypeQuestionName == c.Name).ToList();
                CategoriesDictionaryLines.Add(c.Name, listOfLines);
            }

            var dm = new DataManager();
            MultipleChoiceListAnswer = dm.GetReportForUnitMultipleChoiceListAnswers(report.FormId, report.companyId, report.unitName);

            numberOfCharts = ReportForUnitLinesModel.Count + MultipleChoiceListAnswer.Count;
            var SammaryQuestionReport = Categories.Count(x => x.Name == "Socialstatus" || x.Name == "EmploymentStatus");
            numberOfCharts += SammaryQuestionReport;
        }
    }
}