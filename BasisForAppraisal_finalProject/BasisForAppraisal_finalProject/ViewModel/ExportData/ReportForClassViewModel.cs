using BasisForAppraisal_finalProject.DBML;
using BasisForAppraisal_finalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasisForAppraisal_finalProject.ViewModel.ExportData
{
    public class ReportForClassViewModel
    {
        public ReportForClass ReportForClassModel { set; get; }

        public List<ReportForClassLine> ReportForClassLinesModel { set; get; }

        public ReportForClassLine[] ReportForClassLinesArrayModel { set; get; }

        public List<tbl_TypeQuestion> Categories { set; get; }

        public List<ReportForClassMultipleChoiceListAnswer> MultipleChoiceListAnswer { set; get; }

        public Dictionary<string, List<ReportForClassLine>> CategoriesDictionaryLines { set; get; }

        public int numberOfCharts { set; get; }

        public ReportForClassViewModel(ReportForClass report)
        {
            ReportForClassModel = report;
            ReportForClassLinesModel = report.ReportForClassLines.ToList();
            //eportForOrganiztionLinesArrayModel = ReportForOrganiztionLinesModel.ToArray();
            Categories = ReportForClassLinesModel.Select(x => x.tbl_TypeQuestion).Distinct().ToList();
            CategoriesDictionaryLines = new Dictionary<string, List<ReportForClassLine>>();
            foreach (tbl_TypeQuestion c in Categories)
            {
                var listOfLines = ReportForClassLinesModel.Where(x => x.TypeQuestionName == c.Name).ToList();
                CategoriesDictionaryLines.Add(c.Name, listOfLines);

            }
            var dm = new DataManager();
            MultipleChoiceListAnswer = dm.GetReportForClassMultipleChoiceListAnswers(report.FormId, report.companyId, report.unitName,report.className);


            numberOfCharts = ReportForClassLinesModel.Count + MultipleChoiceListAnswer.Count;
            var SammaryQuestionReport = Categories.Count(x => x.Name == "Socialstatus" || x.Name == "EmploymentStatus");
            numberOfCharts += SammaryQuestionReport;

        }
    }
}