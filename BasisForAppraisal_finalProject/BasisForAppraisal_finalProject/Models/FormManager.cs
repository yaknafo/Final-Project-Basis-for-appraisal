using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BasisForAppraisal_finalProject.DBML;

namespace BasisForAppraisal_finalProject.Models
{
    /// <summary>
    /// this class is reponsable about the logic for add and remove question from the form
    /// </summary>
    public class FormManager
    {
        /// <summary>
        /// adding question to the form
        /// </summary>
        /// <param name="question"></param>
        public void addQuestionToForm(int formId)
        {
            var manager = new DataManager();
            var form = manager.GetFormWithQuestion(formId);
            var numberOfTheQuestion = form.Questions.Count()+1;
            var newQuestion = new tbl_IntentionalQuestion(3, formId, numberOfTheQuestion);
            manager.saveQuestionToDB(newQuestion);
        }
    }
}