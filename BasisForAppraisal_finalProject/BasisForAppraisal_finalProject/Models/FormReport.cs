using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BasisForAppraisal_finalProject.DBML;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace BasisForAppraisal_finalProject.Models
{
     public class FormReportPerEmployee
    {

         public tblForm Form{get; set;}

         public tbl_Employee Employee { get; set; }

         public List<QuestionReport> FormQiestopnReport { get; set; }

         public int Over { get; set; }
         public int Tie { get; set; }
         public int Below { get; set; }

         public double SelfAvg { get; set; }
         public double ColAvg { get; set; }
         public double SupAvg { get; set; }

         public int SelfCounter { get; set; }
         public int ColCounter { get; set; }
         public int SupCounter { get; set; }

         [DataType(DataType.MultilineText)]
         public string GroupLeaderSummry { get; set; }

         public bool IsClose { get; set; }
         

         public FormReportPerEmployee()
         {
             FormQiestopnReport =  new List<QuestionReport>();
         }

         public void GetResultForForm()
         {
             foreach(tbl_IntentionalQuestion question in Form.tbl_Sections.FirstOrDefault().tbl_IntentionalQuestions)
             {
                 FormQiestopnReport.Add(new QuestionReport(Employee.employeeId, question.QuestionId));
             }

             foreach(QuestionReport q in FormQiestopnReport)
             {
                 if (q.colleagueAverage > q.selfAverage)
                     Below++;
                 else if (q.colleagueAverage < q.selfAverage)
                     Over++;
                 else
                     Tie++;


                 if(q.selfCounter > 0)
                 {
                     SelfCounter++;
                     SelfAvg += q.selfAverage;
                 }
                 if (q.colleagueCounter > 0)
                 {
                     ColCounter++;
                     ColAvg += q.colleagueAverage;
                 }
                 if (q.directorCounter > 0)
                 {
                     SupCounter++;
                     SupAvg += q.directorAverage;
                 }
             }

             if (SelfCounter > 0) SelfAvg = SelfAvg/SelfCounter; else SelfAvg = -1;

             if (ColCounter > 0) ColAvg = ColAvg / ColCounter; else ColAvg = -1;

             if(SupCounter > 0) SupAvg=SupAvg/SupCounter; else SupAvg = -1;
             
         }

        
    }
}
