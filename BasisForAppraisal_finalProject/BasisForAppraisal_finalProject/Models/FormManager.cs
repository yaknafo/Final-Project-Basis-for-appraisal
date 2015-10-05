using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BasisForAppraisal_finalProject.DBML;
using BasisForAppraisal_finalProject.ViewModel;
using System.Data.Linq;

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
            //var manager = new DataManager();
            //var form = manager.GetFormWithSections(formId);
            //var numberOfTheQuestion = form.Questions.Count()+1;
            //var newQuestion = new tbl_IntentionalQuestion(3, formId);
            //manager.saveQuestionToDB(newQuestion);
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

       

        /// <summary>
        /// update form with all his change that had been made in the workshope
        /// </summary>
        /// <param name="form"></param>
        public void UpdateForm(FormViewModel form)
        {
            var manager = new DataManager();
            manager.UpdateFormToDB(form.form);

            //// can be that we still dont have question in ur form
            if (form.CurrentSection != null)
                manager.UpdateSectionsToDB(new List<tbl_Section>{form.CurrentSection});

            //// can be that we still dont have question in ur form
           if(form.Questions != null)
               manager.UpdateQuestionsToDB(form.Questions);

        }

        /// <summary>
        ///  add new form to DB
        /// </summary>
        /// <returns></returns>
        public int AddNewForm(bool withSection = true)
        {
            var manager = new DataManager();
            var newform= new tblForm{ FormName="שאלון חדש"};
            var section = new tbl_Section { Name= "חלק חדש"};
           section.FormId = manager.AddForm(newform);
           if (withSection)
           return manager.SaveSection(section);

           return newform.formId;
        }

    }
}