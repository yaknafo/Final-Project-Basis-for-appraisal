using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BasisForAppraisal_finalProject.DBML;
using BasisForAppraisal_finalProject.ViewModel;

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

        /// <summary>
        /// save new question in form  
        /// </summary>
        /// <param name="question"></param>
        public void SaveQuestionToForm(tbl_IntentionalQuestion question)
        {
            var manager = new DataManager();
            manager.saveQuestionToDB(question);
        }

        /// <summary>
        /// save form with all is change that been made in the workshope
        /// </summary>
        /// <param name="form"></param>
        public void SaveForm(tblForm form)
        {
            var manager = new DataManager();

            manager.saveFormToDB(form);
        }

        /// <summary>
        /// delete question
        /// </summary>
        /// <param name="formID"></param>
        /// <param name="quesNumber"></param>
        public void deleteQustion(int formID, int quesNumber)
        {
            DataManager db =  new DataManager();
            db.deleteQustion(formID, quesNumber);
        }

        /// <summary>
        /// delete question
        /// </summary>
        /// <param name="formID"></param>
        /// <param name="quesNumber"></param>
        public void deleteForm(int formId)
        {
            DataManager db = new DataManager();
            db.deleteForm(formId);
        }

        public void upload_excelfile(string path)
        {
            DataManager db = new DataManager();
            db.upload_excelfile(path);

        }
       

        /// <summary>
        /// update form with all his change that had been made in the workshope
        /// </summary>
        /// <param name="form"></param>
        public void UpdateForm(FormViewModel form)
        {
            var manager = new DataManager();
            manager.UpdateFormToDB(form.form);

            // can be that we still dont have question in ur form
            if(form.IntentionalQuestions != null)
            manager.UpdateQuestionsToDB(form.IntentionalQuestions);

        }

        /// <summary>
        ///  add new form to DB
        /// </summary>
        /// <returns></returns>
        public int AddNewForm()
        {
            var manager = new DataManager();
            var newform= new tblForm{ FormName="טופס חדש"};
           return manager.AddForm(newform);
        }

    }
}