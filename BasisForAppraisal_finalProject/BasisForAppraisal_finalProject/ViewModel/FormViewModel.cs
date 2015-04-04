using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasisForAppraisal_finalProject.DBML;
using BasisForAppraisal_finalProject.Models;

namespace BasisForAppraisal_finalProject.ViewModel
{
   public class FormViewModel
    {
        public tblForm form{set; get;}

        public List<tbl_IntentionalQuestion> IntentionalQuestions {set; get;}

        private tbl_IntentionalQuestion newQuestion;

        public int formId { set; get; }

        public FormViewModel (tblForm form)
	{
            this.form= form;
            this.IntentionalQuestions= form.Questions;
            this.formId = form.formId;
	}

        public FormViewModel()
        {

        }

    

        public tbl_IntentionalQuestion NewQuestion
        {
            get
            {

                return newQuestion ?? new tbl_IntentionalQuestion(3, formId);

            }
            set
            {
                newQuestion = value;
            }

        }

       public void AddQuestion(tbl_IntentionalQuestion question)
        {
            if (question.FormId == 0)
            {
                question.FormId = form.formId;
                question.Answers.ForEach(a => a.FormId = form.formId);
            }

            IntentionalQuestions.Add(question);
        }

       public void DeleteQuestions()
       {
           var deleteQuestion=IntentionalQuestions.Where(q => q.deleteQuestion);

           IntentionalQuestions.RemoveAll(x => x.deleteQuestion);
           
       }


     

    }
}
