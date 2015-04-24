using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasisForAppraisal_finalProject.Models;
using System.Data.Linq;
using BasisForAppraisal_finalProject.DBML.Interface;

namespace BasisForAppraisal_finalProject.DBML
{
    /// <summary>
    /// partial class tbl_IntentionalQuestion 
    /// </summary>
    public partial class tbl_IntentionalQuestion
    {
      /// <summary>
      /// which answer has been selected
      /// </summary>
        public int selectedAnswer { set; get; }

        /// <summary>
        /// if to delete the question 
        /// </summary>
        public bool deleteQuestion { set; get; }
       
        /// <summary>
        /// counstrator costume 
        /// </summary>
        /// <param name="numberOfAnswers"></param>
        /// <param name="formId"></param>
        /// <param name="numberOfQuestoin"></param>
        public tbl_IntentionalQuestion(int numberOfAnswers, int formId, int sectionId, tbl_TypeQuestion type)
            : base()
        {
             
            this.FormId = formId;
            this.SectionId = sectionId;
            this._tbl_IntentionalAnswers = new EntitySet<tbl_IntentionalAnswer>(new Action<tbl_IntentionalAnswer>(this.attach_tbl_IntentionalAnswers), new Action<tbl_IntentionalAnswer>(this.detach_tbl_IntentionalAnswers));
            // this.tbl_Section = default(EntityRef<tbl_Section);
            createAnswersToQuestion(numberOfAnswers);
            this.QuestionType = type.Name;
        }


        public bool IfDelete()
        {
            return deleteQuestion;
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

                    FormId = this.FormId,
                    QuestionId = this.QuestionId,
                    SectionId = this.SectionId,
                    Text = string.Empty,
                    tbl_IntentionalQuestion = this,
                    AnswerOption = false,
                };
               
               
               tempList.Add(temp);
            }

            this.Answers = tempList;
        }

        /// <summary>
        /// Add answer option to question will be use in muliti choice Question and choose from a list question
        /// </summary>
        public void AddAnswerOption()
        {
            var answerToAdd = new tbl_IntentionalAnswer() {FormId= this.FormId,QuestionId= this.QuestionId,SectionId = this.SectionId,Text= string.Empty,
                                                           tbl_IntentionalQuestion= this, AnswerOption= false };
            Answers.Add(answerToAdd);
        }
   
        private List<tbl_IntentionalAnswer> answers = new List<tbl_IntentionalAnswer>();

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
            this.Answers = manager.IntentionalAnswer.Where(a => a.QuestionId == this.QuestionId && a.FormId == this.FormId).ToList();
         }


     
    }


    public partial class tbl_IntentionalAnswer
    {
        private bool answerOptionWrapper = false;

        public bool AnswerOptionWrapper
        {
            get
            {
                if (AnswerOption != null)
                return AnswerOption.Value;
                return false;
            }
            set
            {
                answerOptionWrapper = value;
                AnswerOption = value;
            }
        }
    }


}
