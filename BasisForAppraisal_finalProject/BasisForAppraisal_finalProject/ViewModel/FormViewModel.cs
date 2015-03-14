using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasisForAppraisal_finalProject.DBML;

namespace BasisForAppraisal_finalProject.ViewModel
{
   public class FormViewModel
    {
        public tblForm form{set; get;}

        public List<tbl_IntentionalQuestion> IntentionalQuestions {set; get;}

        private tbl_IntentionalQuestion newQuestion;
 
        public FormViewModel (tblForm form)
	{
            this.form= form;
            this.IntentionalQuestions= form.Questions;
	}

        public FormViewModel()
        {

        }

    

        public tbl_IntentionalQuestion NewQuestion
        {
            get
            {

                return newQuestion ?? new tbl_IntentionalQuestion(3, 1);

            }
            set
            {
                newQuestion = value;
            }

        }

    }
}
