using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BasisForAppraisal_finalProject.DBML;
using System.Data.Linq;

namespace BasisForAppraisal_finalProject.Models
{
    public class DataManager
    {

      BFADataBasedbmlDataContext manager = new BFADataBasedbmlDataContext();

         public DataManager()
        {
            this.manager = DbmlBFADataContext.GetDataContextInstance();
            var cultureinfo = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentCulture = cultureinfo;
            System.Threading.Thread.CurrentThread.CurrentUICulture = cultureinfo;
        }

         /// <summary>
         /// get method for all tables
         /// </summary>
         public Table<tbl_IntentionalAnswer> IntentionalAnswer
         {
             get { return manager.tbl_IntentionalAnswers; }
         }

         public Table<tblForm> Forms
         {
             get { return manager.tblForms; }
         }

         public Table<tbl_IntentionalQuestion> IntentionalQuestion
         {
             get { return manager.tbl_IntentionalQuestions; }
         }

        public tbl_IntentionalQuestion GetQuestionWithAnswers(int fid, int qid)
         {
             var question = manager.tbl_IntentionalQuestions.Where(x => x.FormId == fid && x.QuestionId == qid).First();
             question.GetAllAnswers();
             return question;
         }

        public tblForm GetFormWithQuestion(int fid)
        {
            var form = manager.tblForms.Where(x => x.formId == fid).First();
            form.GetAllQuestions().ForEach(q => q.GetAllAnswers());
            return form;
        }

        //--------------------------------   SAVE Method -----------------------------

        public void saveAnswerToDB(List<tbl_IntentionalAnswer> answers)
        {
            manager.tbl_IntentionalAnswers.InsertAllOnSubmit(answers);
            manager.SubmitChanges();
        }

        public void saveFormToDB(tblForm form)
        {
            var changeForm = manager.tblForms.Where(x => x.formId == form.formId).First();
            changeForm.FormName = form.FormName;
           
            manager.SubmitChanges();
        }


        public void saveQuestionToDB(tbl_IntentionalQuestion question)
        {
            manager.tbl_IntentionalQuestions.InsertOnSubmit(question);
            manager.SubmitChanges();
            question.Answers.ForEach(x => x.QuestionId = question.QuestionId);
            manager.tbl_IntentionalAnswers.InsertAllOnSubmit(question.Answers);
            manager.SubmitChanges();
        }


        private void SaveQuestionToDB(List<tbl_IntentionalQuestion> questions)
        {
            // get all the new question from the list
            var newQuestios = questions.Where(x => !manager.tbl_IntentionalQuestions.Contains(x));

            // save all the answer of the question
            foreach (tbl_IntentionalQuestion q in newQuestios)
                saveAnswerToDB(q.Answers);

            // save all the new question
            manager.tbl_IntentionalQuestions.InsertAllOnSubmit(newQuestios);
        
        }

        /// <summary>
        /// sace form include qustions and answers to db
        /// </summary>
        /// <param name="form"></param>
        public void SaveFormToDB(tblForm form)
        {
            manager.tblForms.Attach(form);
            manager.SubmitChanges();
        }


        //////////////////////////////////////////////////////////////////////////Delete method////////////////////////////////////////
        /// <summary>
        /// delte question from specfic form and specfic question
        /// 
        /// <param name="formID"></param> 
        /// <param name="quesNumber"></param>
        public void deleteQustion(int formID, int quesNumber)
        {
            // find the record to delete from the right form and right ques number
            var questionToDelete = manager.tbl_IntentionalQuestions.Where(a => a.FormId == formID && a.FormId == quesNumber).FirstOrDefault();
            var answersToDelete = manager.tbl_IntentionalAnswers.Where(a => a.FormId == formID && a.FormId == quesNumber);
            manager.tbl_IntentionalQuestions.DeleteOnSubmit(questionToDelete);
            manager.tbl_IntentionalAnswers.DeleteAllOnSubmit(answersToDelete);
            manager.SubmitChanges();

        }
    }
}