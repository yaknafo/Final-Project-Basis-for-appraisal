using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasisForAppraisal_finalProject.DBML;

namespace BasisForAppraisal_finalProject.Models
{
    public class QuestionReport
    {
        public tbl_IntentionalQuestion Question { get; set; }

         public int selfAverage{get ;set;}

        public int colleagueAverage{get ;set;}

        public int colleagueCounter { get; set; }

         public int directorAverage{get ;set;}

         public int directorCounter { get; set; }

         private DataManager DM = new DataManager();

         public QuestionReport(string employee, int question)
         {
             CalculationQuestion(employee, question);
         }


         public void CalculationQuestion(string employee, int question)
         {
             Question = DM.Questions.Where(x => x.QuestionId == question).FirstOrDefault();
             var listOfAnswer = DM.ConnectorAnswers.Where(ca => ca.employeeOnId.Equals(employee) && ca.QuestionId == question).ToList() ;
             if(listOfAnswer == null)
                 return;
             foreach (tbl_ConnectorAnswer ca in listOfAnswer)
             {
                 if(ca.employeeFillId == ca.employeeOnId)
                 {
                     selfAverage = ca.tbl_IntentionalAnswer.MyScore;
                 }

                 else if(DM.IsManager(ca.employeeFillId))
                 {
                     directorCounter++;
                     directorAverage = directorAverage + ca.tbl_IntentionalAnswer.MyScore;
                 }

                 else
                 {
                     colleagueCounter++;
                     colleagueAverage = colleagueAverage + ca.tbl_IntentionalAnswer.MyScore;
                 }
             }

             if (directorCounter > 0)
             directorAverage = directorAverage / directorCounter;

             if (colleagueCounter > 0)
                 colleagueAverage = colleagueAverage / colleagueCounter;

         }



    }
}
