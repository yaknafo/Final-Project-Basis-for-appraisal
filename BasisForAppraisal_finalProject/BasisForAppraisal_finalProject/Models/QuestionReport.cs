﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasisForAppraisal_finalProject.DBML;
using BasisForAppraisal_finalProject.Helpr;

namespace BasisForAppraisal_finalProject.Models
{
    public class QuestionReport
    {
        public tbl_IntentionalQuestion Question { get; set; }

        private ConvertAnswerHelperScore helper { get; set; }

        /// <summary>
        /// Self
        /// </summary>
        public Dictionary<tbl_Employee, tbl_ConnectorAnswer> DictionaryMangerPoint { get; set; }
         public int selfAverage{get ;set;}

         public int selfCounter { get; set; }

         public string selfAverageTtitle { get; set; }

        /// <summary>
        /// colleague
        /// </summary>
        public double colleagueAverage{get ;set;}

        public int colleagueCounter { get; set; }

        public string colleagueAverageTitle { get; set; }

        /// <summary>
        /// director
        /// </summary>
         public double directorAverage{get ;set;}

         public int directorCounter { get; set; }

         public string directorAverageTitle { get; set; }

        /// <summary>
         /// Accompanied
        /// </summary>
        public double AccompaniedAverage { get; set; }

        public int AccompaniedCounter { get; set; }
        public string AccompaniedAverageTitle { get; set; }

        /// <summary>
        /// Total
        /// </summary>
        public int TotalCounter { get; set; }
        public double TotalAverage { get; set; }

        /// <summary>
        /// line Color
        /// </summary>
        public string ColorOfLine { get; set; }

        /// <summary>
        /// Over Under
        /// </summary>
        public int UnderOver { get; set; }
        

         private DataManager DM = new DataManager();

         public QuestionReport(string employee, int question)
         {
             CalculationQuestion(employee, question);
         }

         public QuestionReport()
         {
         }
         public QuestionReport(double SelfEvaluation, double collegesEvaluation, double SupervisorEvaluation, double AccompaniedEvaluation, int CountOfFormsFilled, double TotalAverage, tbl_IntentionalQuestion question)
         {
             this.selfAverage = (int)SelfEvaluation;
             this.colleagueAverage =collegesEvaluation;
             this.directorAverage = SupervisorEvaluation;
             this.AccompaniedAverage = AccompaniedEvaluation;
             this.TotalCounter =CountOfFormsFilled;
             this.TotalAverage = TotalAverage;
             this.Question = question;
             helper = new ConvertAnswerHelperScore();
         } 



         public void CalculationQuestion(string employee, int question, List<tbl_ConnectorAnswer> ConnectorAnswers = null , List<tbl_IntentionalQuestion> allQuestions = null) 
         {
             helper = new ConvertAnswerHelperScore();

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
                 int questionScore = helper.ConvertAnswerHelperScoreForReport(ca.tbl_IntentionalAnswer.MyScore);
                 if (ca.tbl_IntentionalAnswer.tbl_IntentionalQuestion.QuestionType.ToLower().Contains("multiplechoicelist"))
                 {
                     continue;
                 }
                     
                 if(ca.employeeFillId == ca.employeeOnId)
                 {
                     selfCounter++;
                     selfAverage = questionScore;
                 }

                 else if(DM.IsAccompanied(ca.employeeFillId))
                 {
                     AccompaniedCounter++;
                     AccompaniedAverage = AccompaniedAverage + questionScore;
                 }

                 else if (DM.IsManager(ca.employeeFillId))
                 {
                     directorCounter++;
                     directorAverage = directorAverage + questionScore;
                 }

                 else
                 {
                     colleagueCounter++;
                     colleagueAverage = colleagueAverage + questionScore;
                 }
             }


             //--------- Calculate the total:`
             if (colleagueCounter > 0)
                 TotalCounter++;
             if (AccompaniedCounter > 0)
                 TotalCounter++;
             if (directorCounter > 0)
                 TotalCounter++;
             if (selfCounter > 0)
                 TotalCounter++;

             TotalAverage = colleagueAverage + AccompaniedAverage + directorAverage + selfAverage;

             if (TotalCounter>0)
             {
             TotalAverage = TotalAverage / TotalCounter;

             ColorOfLine = helper.AverageToColor(TotalAverage);
             }

             //---------- Calcluate All ---------------------------
             if (directorCounter > 0)
             {
             directorAverage = directorAverage / directorCounter;
             directorAverageTitle = helper.ScoreTitle(directorAverage);
             }

             if (AccompaniedCounter > 0)
             {
                 AccompaniedAverage = AccompaniedAverage / AccompaniedCounter;
                 AccompaniedAverageTitle = helper.ScoreTitle(AccompaniedAverage);
             }

             if (colleagueCounter > 0)
             {
                 colleagueAverage = colleagueAverage / colleagueCounter;
                 colleagueAverageTitle = helper.ScoreTitle(colleagueAverage);
             }

              if (selfCounter > 0)
             {
                 selfAverageTtitle = helper.ScoreTitle(selfAverage);
             }

             

             //---------Over under Calculate--------------------------------
             var AvrageOther = (colleagueAverage + AccompaniedAverage + directorAverage) / (colleagueCounter + AccompaniedCounter + directorCounter);
             
             UnderOver = helper.OverUnder(selfAverage,AvrageOther);

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

                 else if (DM.IsAccompanied(ca.employeeFillId))
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


        public void CalculationLinesForCloseReport()
         {
            directorAverageTitle = helper.ScoreTitle(directorAverage);
            selfAverageTtitle = helper.ScoreTitle(selfAverage);
            colleagueAverageTitle = helper.ScoreTitle(colleagueAverage);
            AccompaniedAverageTitle = helper.ScoreTitle(AccompaniedAverage);
             ColorOfLine = helper.AverageToColor(TotalAverage);
             var AvrageOther = TotalAverage - selfAverage;
             UnderOver = helper.OverUnder(selfAverage, AvrageOther);

         }

         public void CalculationQuestionForManagers(string employee, int question, List<tbl_ConnectorAnswer> ConnectorAnswers = null, List<tbl_IntentionalQuestion> allQuestions = null)
         {
            

         }




    }
}
