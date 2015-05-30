using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasisForAppraisal_finalProject.DBML
{
    public partial class tbl_ConnectorFormFill
    {
        public bool IfDelete { set; get; }

        public void FillAnswers(List<tbl_ConnectorAnswer> ca)
        {
        
            foreach(tbl_ConnectorAnswer c in ca)
            {
                var tempQuestion = tblForm.Sections[0].Questions.Where(x => x.QuestionId == c.QuestionId).FirstOrDefault();

                if (tempQuestion == null)
                    continue;

                if (c.tbl_IntentionalAnswer.tbl_IntentionalQuestion.QuestionType.Equals("MultipleChoiceList"))
                    c.tbl_IntentionalAnswer.AnswerOptionWrapper = true;

                else
                    tempQuestion.selectedAnswer = c.AnswerId;
            }
        }


    }
}