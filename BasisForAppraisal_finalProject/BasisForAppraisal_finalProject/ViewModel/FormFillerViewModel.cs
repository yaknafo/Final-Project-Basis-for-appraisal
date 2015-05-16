using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BasisForAppraisal_finalProject.DBML;
using BasisForAppraisal_finalProject.Models;

namespace BasisForAppraisal_finalProject.ViewModel
{
    public class FormFillerViewModel
    {
        public tbl_ConnectorFormFill Connector {set; get;}

        public tblForm From {set; get;}

        private List<tbl_Section> sections;

        public tbl_Section CurrentSection {set ;get;}

        private List<tbl_IntentionalQuestion> questions;

        private List<tbl_IntentionalQuestion> unAnswerQuestions = new List<tbl_IntentionalQuestion>();

        public List<tbl_ConnectorAnswer> answerQuestions =  new List<tbl_ConnectorAnswer>();

        // data manager for get data from DB
        private DataManager dm;

        public string FillOn { set; get; }
        public string FillBy { set; get; }

        public int CompanyId { set; get; }

        public int FormIdConnector { set; get; }

        public FormFillerViewModel(tblForm form, tbl_ConnectorFormFill connector)
	    {
            dm = new DataManager();
            this.From= form;
            this.sections = form.Sections;
            this.Connector = connector;
                CurrentSection = form.Sections.First();
            this.questions = CurrentSection.Questions;
            initQuestion();
            this.FillOn = connector.employeeOnId;
            this.FillBy = connector.employeeFillId;
            this.CompanyId = connector.companyId;
            this.FormIdConnector = connector.formId;

	    }

        public FormFillerViewModel()
        {

        }

       public DataManager DM
        {
            get
            {
                return dm ?? new DataManager();
            }
           set
            {
                dm = value;
            }
        }

       public List<tbl_Section> Sections
       {
           get
           {
               if (sections == null)
                   sections = new List<tbl_Section>();
               return sections;

           }
           set
           {
               sections = value;
           }

       }


       public List<tbl_IntentionalQuestion> Questions
       {
           get
           {
               if (questions == null)
                   questions = DM.IntentionalQuestion.Where(x => x.FormId == From.formId).ToList();
               return questions;

           }
           set
           {
               questions = value;
           }

       }


       public List<tbl_ConnectorAnswer> AnswerQuestions
       {
           get
           {

               return answerQuestions;

           }
           set
           {
               answerQuestions = value;
           }

       }

         private List<tbl_IntentionalQuestion> initQuestion()
       {
          var tempList= new List<tbl_IntentionalQuestion> ();

             foreach(tbl_IntentionalQuestion q in Questions)
             {
                 tempList.Add(dm.GetQuestionWithAnswers(From.formId, q.QuestionId));
             }

             Questions = tempList;
             return Questions;
       }

        public tbl_ConnectorAnswer GetSelectedAnswer(tbl_IntentionalQuestion question)
        {
            tbl_IntentionalAnswer selectedAnswer;

            if (question.QuestionType == "FreeText")
                selectedAnswer = question.Answers[0];

            else if (question.selectedAnswer == 0)
            {
               
                return null;
            }
            else
            {
                

                selectedAnswer = question.Answers.Where(x => x.AnswerId == question.selectedAnswer).FirstOrDefault();
            }

            
            var connectorAnswer= new tbl_ConnectorAnswer{ companyId = CompanyId,
                                                          employeeFillId = FillBy ,
                                                          employeeOnId =FillOn, 
                                                          formConnectorId= FormIdConnector, 
                                                          FormId= selectedAnswer.FormId, 
                                                          QuestionId= selectedAnswer.QuestionId,
                                                          SectionId = question.SectionId,
                                                          AnswerId = selectedAnswer.AnswerId
                                                          };



            return connectorAnswer;
        }


    }
}