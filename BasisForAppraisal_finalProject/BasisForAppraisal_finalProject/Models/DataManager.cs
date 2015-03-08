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
            var form = manager.tblForms.Where(x => x.FormId == fid).First();
            form.GetAllQuestions().ForEach(q => q.GetAllAnswers());
            return form;
        }
    }
}