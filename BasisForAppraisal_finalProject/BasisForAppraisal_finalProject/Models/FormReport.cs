using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BasisForAppraisal_finalProject.DBML;
using System.Threading.Tasks;

namespace BasisForAppraisal_finalProject.Models
{
     public class FormReportPerEmployee
    {

         public tblForm Form{get; set;}

         public tbl_Employee Employee { get; set; }

         public List<QuestionReport> FormQiestopnReport { get; set; }

         

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
         }
        
    }
}
