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

        public Dictionary<tbl_Employee, tbl_ConnectorAnswer> DictionaryMangerPoint { get; set; }
         public int selfAverage{get ;set;}

         public int selfCounter { get; set; }


        public double colleagueAverage{get ;set;}

        public int colleagueCounter { get; set; }

         public double directorAverage{get ;set;}

         public int directorCounter { get; set; }

         private DataManager DM = new DataManager();

         public QuestionReport(string employee, int question)
         {
             CalculationQuestion(employee, question);
         }

         public QuestionReport()
         {
         } 


         public void CalculationQuestion(string employee, int question, List<tbl_ConnectorAnswer> ConnectorAnswers = null , List<tbl_IntentionalQuestion> allQuestions = null) 
         {
             List<tbl_ConnectorAnswer> listOfAnswer = null;
             if(allQuestions == null)
             Question = DM.Questions.Where(x => x.QuestionId == question).FirstOrDefault();
             else
                 Question = allQuestions.Where(x => x.QuestionId == question).FirstOrDefault();
             if(ConnectorAnswers == null)
               listOfAnswer = DM.ConnectorAnswers.Where(ca => ca.employeeOnId.Equals(employee) && ca.QuestionId == question).ToList() ;
             else
                 listOfAnswer = ConnectorAnswers.Where(ca => ca.employeeOnId.Equals(employee) && ca.QuestionId == question).ToList();


             if(listOfAnswer == null)
                 return;
             foreach (tbl_ConnectorAnswer ca in listOfAnswer)
             {
                 if(ca.employeeFillId == ca.employeeOnId)
                 {
                     selfCounter++;
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
             else
             {

             }

             if (colleagueCounter > 0)
                 colleagueAverage = colleagueAverage / colleagueCounter;

         }


         public double CalculationQuestionFoeType(string employee, int question, List<tbl_ConnectorAnswer> ConnectorAnswers = null, List<tbl_IntentionalQuestion> allQuestions = null)
         {
             List<tbl_ConnectorAnswer> listOfAnswer = null;
             if (allQuestions == null)
                 Question = DM.Questions.Where(x => x.QuestionId == question).FirstOrDefault();
             else
                 Question = allQuestions.Where(x => x.QuestionId == question).FirstOrDefault();
             if (ConnectorAnswers == null)
                 listOfAnswer = DM.ConnectorAnswers.Where(ca => ca.employeeOnId.Equals(employee) && ca.QuestionId == question).ToList();
             else
                 listOfAnswer = ConnectorAnswers.Where(ca => ca.employeeOnId.Equals(employee) && ca.QuestionId == question).ToList();


             if (listOfAnswer == null)
                 return -1;
             foreach (tbl_ConnectorAnswer ca in listOfAnswer)
             {
                 if (ca.employeeFillId == ca.employeeOnId)
                 {
                     ConnectorAnswers.Remove(ca);
                   return ca.tbl_IntentionalAnswer.MyScore;
                 }

                 else if (DM.IsManager(ca.employeeFillId))
                 {
                     ConnectorAnswers.Remove(ca);
                     return ca.tbl_IntentionalAnswer.MyScore;
                 }

                 else
                 {
                     colleagueCounter++;
                     colleagueAverage = colleagueAverage + ca.tbl_IntentionalAnswer.MyScore;
                     ConnectorAnswers.Remove(ca);
                 }
             }
             if(colleagueCounter > 0)
             return colleagueAverage;
             return -1;

         }


         public void CalculationQuestionForManagers(string employee, int question, List<tbl_ConnectorAnswer> ConnectorAnswers = null, List<tbl_IntentionalQuestion> allQuestions = null)
         {
            

         }




    }
}
