using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasisForAppraisal_finalProject.Models;

namespace BasisForAppraisal_finalProject.DBML
{
    public partial class tbl_IntentionalQuestion
    {

   
         private List<tbl_IntentionalAnswer> answers= new List<tbl_IntentionalAnswer>();

         public List<tbl_IntentionalAnswer> Answers
        {
        get
        {
            answers= GetAllAnswers();
            return answers;
        }
    }

        public List<tbl_IntentionalAnswer> GetAllAnswers()
         {
             var manager = new DataManager();
            return  manager.IntentionalAnswer.Where(a => a.QuestionId == this.QuestionId && a.FormId == this.FormId).ToList();
         }


     
    }

     public partial class tblForm
    {

   
         private List<tbl_IntentionalQuestion> questions= new List<tbl_IntentionalQuestion>();

         public List<tbl_IntentionalQuestion> Questions
         {
             get
             {
                 questions = GetAllQuestions();
                 return questions;
             }

         }

         public List<tbl_IntentionalQuestion> GetAllQuestions()
         {
             var manager = new DataManager();
             return manager.IntentionalQuestion.Where(x => x.FormId == this._FormId).ToList();
         }
    }
}
