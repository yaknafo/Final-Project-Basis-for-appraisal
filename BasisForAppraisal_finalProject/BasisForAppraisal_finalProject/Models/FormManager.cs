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
            var newQuestion = new tbl_IntentionalQuestion(3, formId);
            manager.saveQuestionToDB(newQuestion);
        }

        /// <summary>
        /// this method attach question to from without save the question in the DB
        /// </summary>
        /// <param name="formId"></param>
        /// <param name="numberOfTheQuestion"></param>
        /// <returns></returns>
        public tblForm GetQuestionToForm(int formId, tbl_IntentionalQuestion question)
        {

            var manager = new DataManager();
            var form = manager.GetFormWithQuestion(formId);
            form.Questions.Add(question);
            return form;
          
        }

        public void SaveQuestionToForm(tbl_IntentionalQuestion question)
        {
            var manager = new DataManager();
            manager.saveQuestionToDB(question);
            question.tblForm.AddedNewQuestion();
        }

        public void SaveForm(tblForm form)
        {
            var manager = new DataManager();

            manager.saveFormToDB(form);
        }

   
        public void deleteQustion(int formID, int quesNumber)
        {
            DataManager db =  new DataManager();
            db.deleteQustion(formID, quesNumber);
        }

        public void UpdateQuestionsToForm(List<tbl_IntentionalQuestion> questions)
        {
            var manager = new DataManager();
            manager.UpdateQuestionsToDB(questions);
           
        }

    }
}