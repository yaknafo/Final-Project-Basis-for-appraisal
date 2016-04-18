using BasisForAppraisal_finalProject.DBML;
using BasisForAppraisal_finalProject.Helpr;
using BasisForAppraisal_finalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasisForAppraisal_finalProject.BL
{
    public class ReportBL
    {
        public void ReportPerEmployee(string employeeId, int forms, FormReportPerEmployee formReport = null)
        {
            DataManager DMO = new DataManager();

            if (formReport != null && (formReport.GroupLeaderSummry != null || formReport.IsClose))
            {
                DMO.EditReportForIndividual(formReport.GroupLeaderSummry, formReport.IsClose, formReport.Employee.employeeId, formReport.Form.formId);
                employeeId = formReport.Employee.employeeId;
            }

            var emp = DMO.Employees.Where(e => e.employeeId == employeeId).First();
            var form = DMO.Forms.Where(f => f.formId == forms).FirstOrDefault();
            var calculation = new FormReportPerEmployee(){ Employee = emp, Form = form };
            var reportDb = DMO.GetReportForIndividual(employeeId, form.formId);
            if (reportDb == null || !reportDb.IsClose)
            {
                var report = new ReportForIndividual() { IndividualId = employeeId, FormId = form.formId, createDate = DateTime.Now,CompanyId = emp.companyId};
                DMO.SaveReportForIndividual(report);

                calculation.GetResultForForm();
                calculation.GroupLeaderSummry = report.feedback;
                calculation.IsClose = report.IsClose;
                foreach (QuestionReport c in calculation.FormQiestopnReport)
                {
                    DMO.SaveReportForIndividualLines(employeeId, form.formId, form.Sections.First().SectionId, c.Question.QuestionId, c.selfAverage, c.colleagueAverage, c.directorAverage, c.AccompaniedAverage, c.TotalCounter, c.TotalAverage);
                }
            }
            else
            {
                foreach (ReportForIndividualLine v in reportDb.ReportForIndividualLines)
                {
                    calculation.FormQiestopnReport.Add(new QuestionReport(v.SelfEvaluation, v.collegesEvaluation, v.SupervisorEvaluation, v.AccompaniedEvaluation, v.CountOfFormsFilled, v.TotalAverage, v.tbl_IntentionalQuestion));
                }
                calculation.FormQiestopnReport.ForEach(x => x.CalculationLinesForCloseReport());
                calculation.GroupLeaderSummry = reportDb.feedback;
                calculation.IsClose = reportDb.IsClose;
            }
        }

        public void CalculateReportForOrganiztion(int companyId, int formId)
        {
            DataManager DMO = new DataManager();
            var organiztion = DMO.Companyies.FirstOrDefault(x => x.companyId == companyId);
            if (organiztion == null)
                return;
            var OrganiztionEmployessId = DMO.ConnectorAnswers.Where(x => x.companyId == organiztion.companyId && x.FormId == formId).Select(x => x.employeeOnId).Distinct().ToList();


            // create report for invidual for all the employee in the company
            //TODO: this is so slow we need to with that something
           OrganiztionEmployessId.ForEach(x => ReportPerEmployee(x, formId));

            var report = new ReportForOrganiztion() { CompanyId = organiztion.companyId, FormId = formId };
            DMO.SaveReportForOrganiztion(report);

          

            var form = DMO.Forms.FirstOrDefault(x => x.formId == formId);
            if (form == null)
                return;


            // Create list of the queestions in the form
            var listOfReportLInes = new List<ReportForOrganiztionLine>();
             var listOfReportLInesAnswers = new List<ReportForCompanyMultipleChoiceListAnswer>();
            var allQuestions = form.Sections.First().Questions;
 
               foreach (tbl_IntentionalQuestion question in allQuestions)
            {
              

                var line = DMO.GetReportOrganiztionLine(report.FormId, report.CompanyId,question.QuestionId);
                if(line == null)
                {
                      line = new ReportForOrganiztionLine() {FormId= report.FormId, CompanyId = report.CompanyId,
                                                                            QuestioFormId = question.FormId, SectionId = question.SectionId,
                                                                             QuestionId = question.QuestionId,TypeQuestionName = question.QuestionType};
                }

                   listOfReportLInes.Add(line);
               }


            // init All counters 
            foreach(ReportForOrganiztionLine line in listOfReportLInes)
            {
                line.HighScore = 0;
                line.LowScore = 0;
                line.MidScore = 0;
            }
            // get all types of questions that show in report for invidual
            var questionsInRepostForIndividual = allQuestions.Where(x => x.ForReports).ToList();

            //   ------------- Report individual for Organiztion use--------------------------------------------
            var individualReportLines = DMO.GetReportForIndividualLineForOrganiztion(report.FormId, report.CompanyId).ToList();
            
            foreach(ReportForIndividualLine r in individualReportLines)
            {
                var line = listOfReportLInes.FirstOrDefault(x => x.QuestionId == r.QuestionId);
                if (line == null)
                    continue;

                CountScoreForReportForOrganiztionLines(r, line);

            }

            //   -------------End Report individual for Organiztion use--------------------------------------------

            //   ------------- Report Muliti choice for Organiztion use--------------------------------------------
            var questionsNotInRepostForIndividual = allQuestions.Where(x => !x.ForReports).ToList();


            var MulitiChoiceListQuestion = questionsNotInRepostForIndividual.Where(x => x.QuestionType == "MultipleChoiceList").ToList();



            foreach(tbl_IntentionalQuestion q in MulitiChoiceListQuestion)
            {
                var AnswerForQuestion = DMO.ConnectorAnswers.Where(x => x.QuestionId == q.QuestionId && x.companyId == companyId).ToList();
                var groupByEmp = AnswerForQuestion.GroupBy(x => x.employeeOnId).ToList();
                var answerGroupOrg = AnswerForQuestion.GroupBy(x => x.AnswerId).ToList();
                var line = listOfReportLInes.FirstOrDefault(x => x.QuestionId == q.QuestionId);
                foreach(var i in groupByEmp)
                {
                    CountScoreForReportForOrganiztionLinesMulitiChoice(i.Count(), line);
                }

                int numberQuestionsOfFiller = 0;
                if (OrganiztionEmployessId.Count != 0)
                    numberQuestionsOfFiller = AnswerForQuestion.Count / OrganiztionEmployessId.Count;

                foreach (var i in q.tbl_IntentionalAnswers)
                {
                    var answerLine = DMO.GetSingleLineReportForClassMultipleChoiceListAnswer(formId,companyId,i.AnswerId);
                    if(answerLine == null)
                    {
                        answerLine = new ReportForCompanyMultipleChoiceListAnswer() { AnswerId = i.AnswerId, companyId = companyId, FormId = formId, QuestionId = q.QuestionId, SectionId = q.SectionId };

                    }
                    var listtOfAnswer = answerGroupOrg.FirstOrDefault(x => x.Key == i.AnswerId);
                    int numberOfAnswer = 0;
                    if (listtOfAnswer != null)
                    {
                        numberOfAnswer = listtOfAnswer.Count();
                    }
                    answerLine.numberOfMarket = numberOfAnswer;
                    answerLine.numberOfShowTotal = numberQuestionsOfFiller;
                    DMO.SaveReportForCompanyMultipleChoiceListAnswer(answerLine);
                }

            }

            var notMulitiChoiceListQuestion = questionsNotInRepostForIndividual.Where(x => x.QuestionType != "MultipleChoiceList").ToList();
            var notMulitiChoiceAnswerForQuestion = DMO.ConnectorAnswers.Where(x => x.tbl_IntentionalAnswer.tbl_IntentionalQuestion.QuestionType != "MultipleChoiceList" && x.companyId == companyId && x.FormId == formId && !x.tbl_IntentionalAnswer.tbl_IntentionalQuestion.ForReports).ToList();

            foreach (tbl_IntentionalQuestion q in notMulitiChoiceListQuestion)
            {
                var answerForQuestion = notMulitiChoiceAnswerForQuestion.Where(x => x.QuestionId == q.QuestionId).ToList();
                var line = listOfReportLInes.FirstOrDefault(x => x.QuestionId == q.QuestionId);
                answerForQuestion.ForEach(x => CountScoreForReportForOrganiztionLinesSpecialCatgory(x.tbl_IntentionalAnswer.MyScore, line));
            }

            //   -------------End Report Muliti choice for Organiztion use--------------------------------------------

            // ------------ Save all lines  for report For Organiztion
            listOfReportLInes.ForEach(x => DMO.SaveReportForOrganiztionLines(x));
            //   -------------End Report individual for Organiztion use--------------------------------------------
         
        }


        /// <summary>
        /// this method should calcaulte and save all Org report with given Level
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="formId"></param>
        /// <param name="unit"></param>
        /// <param name="cls"></param>
        public void CalculateReportForClass(int companyId, int formId, string unit,string cls, List<string> empIds)
        {
            DataManager DMO = new DataManager();


            // create report for invidual for all the employee in the company
            //TODO: this is so slow we need to with that something
            empIds.ForEach(x => ReportPerEmployee(x, formId));

            /// todo: here we need to create Maper for all level
            var report = new ReportForClass() { companyId = companyId, FormId = formId, className =cls,unitName = unit };
            DMO.SaveReportForClass(report);



            var form = DMO.Forms.FirstOrDefault(x => x.formId == formId);
            if (form == null)
                return;


            // Create list of the queestions in the form
            /// todo: here we need to create Maper for all level
            var listOfReportLInes = new List<ReportForClassLine>();
            var listOfReportLInesAnswers = new List<ReportForClassMultipleChoiceListAnswer>();
            var allQuestions = form.Sections.First().Questions;

            foreach (tbl_IntentionalQuestion question in allQuestions)
            {


                var line = DMO.GetReportClassLine(report.FormId, report.companyId,report.unitName,report.className, question.QuestionId);
                if (line == null)
                {
                    line = new ReportForClassLine()
                    {
                        FormId = report.FormId,
                        companyId = report.companyId,
                        unitName = report.unitName,
                        className = report.className,
                        QuestioFormId = question.FormId,
                        SectionId = question.SectionId,
                        QuestionId = question.QuestionId,
                        TypeQuestionName = question.QuestionType
                    };
                }

                listOfReportLInes.Add(line);
            }


            // init All counters 
            foreach (ReportForClassLine line in listOfReportLInes)
            {
                line.HighScore = 0;
                line.LowScore = 0;
                line.MidScore = 0;
            }

            // get all types of questions that show in report for invidual
            var questionsInRepostForIndividual = allQuestions.Where(x => x.ForReports).ToList();

            //   ------------- Report individual for Organiztion use--------------------------------------------
            var individualReportLines = DMO.GetReportForIndividualLineForOrganiztion(report.FormId, report.companyId).Where(x =>empIds.Contains(x.IndividualId)).ToList();

            foreach (ReportForIndividualLine r in individualReportLines)
            {
                var line = listOfReportLInes.FirstOrDefault(x => x.QuestionId == r.QuestionId);
                if (line == null)
                    continue;

                CountScoreForReportForClassLines(r, line);

            }

            //   -------------End Report individual for Organiztion use--------------------------------------------

            //   ------------- Report Muliti choice for Organiztion use--------------------------------------------
            var questionsNotInRepostForIndividual = allQuestions.Where(x => !x.ForReports).ToList();


            var MulitiChoiceListQuestion = questionsNotInRepostForIndividual.Where(x => x.QuestionType == "MultipleChoiceList").ToList();



            foreach (tbl_IntentionalQuestion q in MulitiChoiceListQuestion)
            {
                var AnswerForQuestion = DMO.ConnectorAnswers.Where(x => x.QuestionId == q.QuestionId && x.companyId == companyId && x.tbl_ConnectorFormFill.tbl_Employee.unitName == unit && x.tbl_ConnectorFormFill.tbl_Employee.className == cls).ToList();
                var groupByEmp = AnswerForQuestion.GroupBy(x => x.employeeOnId).ToList();
                var answerGroupOrg = AnswerForQuestion.GroupBy(x => x.AnswerId).ToList();
                var line = listOfReportLInes.FirstOrDefault(x => x.QuestionId == q.QuestionId);
                foreach (var i in groupByEmp)
                {
                    CountScoreForReportForClassLinesMulitiChoice(i.Count(), line);
                }

                int numberQuestionsOfFiller = 0;
                if (empIds.Count != 0)
                    numberQuestionsOfFiller = AnswerForQuestion.Count / empIds.Count;

                foreach (var i in q.tbl_IntentionalAnswers)
                {
                    var answerLine = DMO.GetSingleLineReportForClassMultipleChoiceListAnswer(formId, companyId,unit,cls, i.AnswerId);
                    if (answerLine == null)
                    {
                        answerLine = new ReportForClassMultipleChoiceListAnswer() { AnswerId = i.AnswerId, companyId = companyId,unitName = unit,className =cls, FormId = formId, QuestionId = q.QuestionId, SectionId = q.SectionId };

                    }
                    var listtOfAnswer = answerGroupOrg.FirstOrDefault(x => x.Key == i.AnswerId);
                    int numberOfAnswer = 0;
                    if (listtOfAnswer != null)
                    {
                        numberOfAnswer = listtOfAnswer.Count();
                    }
                    answerLine.numberOfMarket = numberOfAnswer;
                    answerLine.numberOfShowTotal = empIds.Count;
                    DMO.SaveReportForClassMultipleChoiceListAnswer(answerLine);
                }

            }

            var notMulitiChoiceListQuestion = questionsNotInRepostForIndividual.Where(x => x.QuestionType != "MultipleChoiceList").ToList();
            var notMulitiChoiceAnswerForQuestion = DMO.ConnectorAnswers.Where(x => x.tbl_IntentionalAnswer.tbl_IntentionalQuestion.QuestionType != "MultipleChoiceList" && x.companyId == companyId && x.FormId == formId && !x.tbl_IntentionalAnswer.tbl_IntentionalQuestion.ForReports && empIds.Contains(x.employeeOnId)).ToList();

            foreach (tbl_IntentionalQuestion q in notMulitiChoiceListQuestion)
            {
                var answerForQuestion = notMulitiChoiceAnswerForQuestion.Where(x => x.QuestionId == q.QuestionId).ToList();
                var line = listOfReportLInes.FirstOrDefault(x => x.QuestionId == q.QuestionId);
                answerForQuestion.ForEach(x => CountScoreForReportForOrganiztionLinesSpecialCatgory(x.tbl_IntentionalAnswer.MyScore, line));
            }

            //   -------------End Report Muliti choice for Organiztion use--------------------------------------------

            // ------------ Save all lines  for report For Organiztion
            listOfReportLInes.ForEach(x => DMO.SaveReportForClassLines(x));
            //   -------------End Report individual for Organiztion use--------------------------------------------

        }


        //---------------------------- Company --------------------------------------------------------------
        public void CountScoreForReportForOrganiztionLinesMulitiChoice(int NumberOfTime, ReportForOrganiztionLine organiztion)
        {
            if (NumberOfTime >= 4)
                organiztion.HighScore++;
            else if (NumberOfTime >= 2)
                organiztion.MidScore++;
            else
             organiztion.LowScore++;
        }


        public void CountScoreForReportForOrganiztionLinesSpecialCatgory(int NumberOfTime, ReportForOrganiztionLine organiztion)
        {
            var helper = new ConvertAnswerHelperScore();
            var ConvertScore = helper.ConvertAnswerHelperScoreForReport(NumberOfTime);
            if (ConvertScore >= 2.5)
                organiztion.HighScore++;
            else if (ConvertScore >= 1.5)
                organiztion.MidScore++;
            else
                organiztion.LowScore++;
        }

        public void CountScoreForReportForOrganiztionLines(ReportForIndividualLine individual, ReportForOrganiztionLine organiztion)
        {
            if (individual.TotalAverage >= 2.5)
                organiztion.HighScore++;
            else if (individual.TotalAverage >= 1.5)
                organiztion.MidScore++;
            else
                organiztion.LowScore++;
        }


        //---------------------------- Class ----------------------------------------------------------------------

        public void CountScoreForReportForClassLines(ReportForIndividualLine individual, ReportForClassLine organiztion)
        {
            if (individual.TotalAverage >= 2.5)
                organiztion.HighScore++;
            else if (individual.TotalAverage >= 1.5)
                organiztion.MidScore++;
            else
                organiztion.LowScore++;
        }


        public void CountScoreForReportForClassLinesMulitiChoice(int NumberOfTime, ReportForClassLine organiztion)
        {
            if (NumberOfTime >= 4)
                organiztion.HighScore++;
            else if (NumberOfTime >= 2)
                organiztion.MidScore++;
            else
                organiztion.LowScore++;
        }

        public void CountScoreForReportForOrganiztionLinesSpecialCatgory(int NumberOfTime, ReportForClassLine organiztion)
        {
            var helper = new ConvertAnswerHelperScore();
            var ConvertScore = helper.ConvertAnswerHelperScoreForReport(NumberOfTime);
            if (ConvertScore >= 2.5)
                organiztion.HighScore++;
            else if (ConvertScore >= 1.5)
                organiztion.MidScore++;
            else
                organiztion.LowScore++;
        }
       
    }
}