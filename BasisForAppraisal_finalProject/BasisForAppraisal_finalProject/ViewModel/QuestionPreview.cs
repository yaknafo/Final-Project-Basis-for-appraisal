using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BasisForAppraisal_finalProject.DBML;

namespace BasisForAppraisal_finalProject.ViewModel
{
    public class QuestionPreview
    {
       public int numberOfQuestion;
        public tbl_IntentionalQuestion question;

        public QuestionPreview(tbl_IntentionalQuestion ques, int num)
        {
            question = ques;
            numberOfQuestion = num;
        }
    }
}