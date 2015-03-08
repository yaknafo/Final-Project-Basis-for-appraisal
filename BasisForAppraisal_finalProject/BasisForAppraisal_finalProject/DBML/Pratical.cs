using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasisForAppraisal_finalProject.Models;
using System.Data.Linq;

namespace BasisForAppraisal_finalProject.DBML
{
    /// <summary>
    /// partial class tbl_IntentionalQuestion 
    /// </summary>
    public partial class tbl_IntentionalQuestion
    {
        /// <summary>
        /// counstrator costume 
        /// </summary>
        /// <param name="numberOfAnswers"></param>
        /// <param name="formId"></param>
        /// <param name="numberOfQuestoin"></param>
        public tbl_IntentionalQuestion(int numberOfAnswers, int formId,int numberOfQuestoin):base()
        {
          
            this.FormId = formId;
            this.QuestionId = numberOfQuestoin;
            this._tbl_IntentionalAnswers = new EntitySet<tbl_IntentionalAnswer>(new Action<tbl_IntentionalAnswer>(this.attach_tbl_IntentionalAnswers), new Action<tbl_IntentionalAnswer>(this.detach_tbl_IntentionalAnswers));
            this._tblForm = default(EntityRef<tblForm>);
            createAnswersToQuestion(numberOfAnswers);
        }

        /// <summary>
        /// create how many answer options that the user like (at the moment our defult is 3)
        /// </summary>
        /// <param name="numberOfQuestoin"></param>
        public void createAnswersToQuestion(int numberOfQuestoin)
        {
            List<tbl_IntentionalAnswer> tempList = new List<tbl_IntentionalAnswer>(); ;

            for (int i = 0; i < numberOfQuestoin; i++)
            {
               var temp = new tbl_IntentionalAnswer
                {
                    AnswerId = i,
                    QuestionId = this.QuestionId,
                    FormId = this.FormId,
                    Text = string.Empty,
                    Score = 0,
                    AnswerOption = false,
                };
               
               
               tempList.Add(temp);
            }

            this.Answers = tempList;
        }
   
         private List<tbl_IntentionalAnswer> answers= new List<tbl_IntentionalAnswer>();

         public List<tbl_IntentionalAnswer> Answers
        {
        get
        {
            return answers;
        }
            set
            {
                answers = value;
            }
    }
        /// <summary>
        /// gat al the answer of th question from tbl_intenational answers
        /// </summary>
        public void GetAllAnswers()
         {
             var manager = new DataManager();
           this.Answers= manager.IntentionalAnswer.Where(a => a.QuestionId == this.QuestionId && a.FormId == this.FormId).ToList();
         }


     
    }

    /// <summary>
    /// partial class tblForm
    /// </summary>
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

        // public string Title { get; set; }

         public int formId { get; set; }

      

         public List<tbl_IntentionalQuestion> GetAllQuestions()
         {
             var manager = new DataManager();
             return manager.IntentionalQuestion.Where(x => x.FormId == this._FormId).ToList();
         }
    }
}
