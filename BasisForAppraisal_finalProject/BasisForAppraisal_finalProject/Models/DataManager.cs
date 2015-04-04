﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BasisForAppraisal_finalProject.DBML;
using System.Data.Linq;

namespace BasisForAppraisal_finalProject.Models
{
    public class DataManager
    {

        BFADataBasedbmlDataContext manager = new BFADataBasedbmlDataContext();

        public DataManager()
        {
            this.manager = DbmlBFADataContext.GetDataContextInstance();
            var cultureinfo = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentCulture = cultureinfo;
            System.Threading.Thread.CurrentThread.CurrentUICulture = cultureinfo;
        }

        /// <summary>
        /// get method for all tables
        /// </summary>
        public Table<tbl_IntentionalAnswer> IntentionalAnswer
        {
            get { return manager.tbl_IntentionalAnswers; }
        }

        public Table<tblForm> Forms
        {
            get { return manager.tblForms; }
        }

        public Table<tbl_IntentionalQuestion> IntentionalQuestion
        {
            get { return manager.tbl_IntentionalQuestions; }
        }

        public tbl_IntentionalQuestion GetQuestionWithAnswers(int fid, int qid)
        {
            var question = manager.tbl_IntentionalQuestions.Where(x => x.FormId == fid && x.QuestionId == qid).First();
            question.GetAllAnswers();
            return question;
        }

        public tblForm GetFormWithQuestion(int fid)
        {
            var form = manager.tblForms.Where(x => x.formId== fid).FirstOrDefault();

            if (form == null)
                throw new Exception("form with number number: " + fid + " mot exist in DB");

            form.GetAllQuestions().ForEach(q => q.GetAllAnswers());
            return form;
        }

        //--------------------------------   SAVE Method -----------------------------

        public void saveAnswerToDB(List<tbl_IntentionalAnswer> answers)
        {
            manager.tbl_IntentionalAnswers.InsertAllOnSubmit(answers);
            manager.SubmitChanges();
        }

        public void saveFormToDB(tblForm form)
        {
            var changeForm = manager.tblForms.Where(x => x.formId == form.formId).First();
            changeForm.FormName = form.FormName;
           
            manager.SubmitChanges();
        }


        public void saveQuestionToDB(tbl_IntentionalQuestion question)
        {
            manager.tbl_IntentionalQuestions.InsertOnSubmit(question);
            manager.SubmitChanges();
            question.Answers.ForEach(x => x.QuestionId = question.QuestionId);
            manager.tbl_IntentionalAnswers.InsertAllOnSubmit(question.Answers);
            manager.SubmitChanges();
        }


        public void SaveQuestionsToDB(List<tbl_IntentionalQuestion> questions)
        {
            // get all the new question from the list
            var newQuestios = questions.Where(x => !manager.tbl_IntentionalQuestions.Contains(x));

            // save all the answer of the question
            foreach (tbl_IntentionalQuestion q in newQuestios)
                saveAnswerToDB(q.Answers);

            // save all the new question
            manager.tbl_IntentionalQuestions.InsertAllOnSubmit(newQuestios);

        }

       
        /// <summary>
        /// save new form in data base
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public int AddForm(tblForm form)
        {
            manager.tblForms.InsertOnSubmit(form);
            manager.SubmitChanges();
            return form.formId;
        }
        //////////////////////////////////////////////////////////////////////////Delete method////////////////////////////////////////
        /// <summary>
        /// delte question from specfic form and specfic question
        /// 
        /// <param name="formID"></param> 
        /// <param name="quesNumber"></param>
        public void deleteQustion(int formID, int quesNumber)
        {
            // find the record to delete from the right form and right ques number
            var questionToDelete = manager.tbl_IntentionalQuestions.Where(a => a.FormId == formID && a.QuestionId == quesNumber).FirstOrDefault();
            var answersToDelete = manager.tbl_IntentionalAnswers.Where(a => a.FormId == formID && a.QuestionId == quesNumber);
            manager.tbl_IntentionalAnswers.DeleteAllOnSubmit(answersToDelete);
            manager.tbl_IntentionalQuestions.DeleteOnSubmit(questionToDelete);
            manager.SubmitChanges();

        }

        /// <summary>
        /// delte question from specfic form and specfic question
        /// 
        /// <param name="formID"></param> 
        /// <param name="quesNumber"></param>
        public void deleteForm(int formID)
        {
            // find the form
            var formForDelete = manager.tblForms.Where(x => x.formId == formID).FirstOrDefault();

            // check if exist
            if(formForDelete == null)
                throw new Exception(string.Format("the form with number id {0} camt be delete becouse he cant be found in DB",formID));

            // delete all his question
            formForDelete.Questions.ForEach(q => deleteQustion(q.FormId, q.QuestionId));

            // delete the form him self
            manager.tblForms.DeleteOnSubmit(formForDelete);

            manager.SubmitChanges();
        }


        //------------------------------------------------   Update Method -----------------------------------------------------------//

        /// <summary>
        /// save form include qustions and answers to db
        /// </summary>
        /// <param name="form"></param>
        public void UpdateFormToDB(tblForm form)
        {
            var formUpdate=manager.tblForms.Where(f => f.formId== form.formId).FirstOrDefault();
            formUpdate.FormName = form.FormName;
            manager.SubmitChanges();
        }


        /// <summary>
        /// update all the question in the form 
        /// </summary>
        /// <param name="questions"></param>
        public void UpdateQuestionsToDB(List<tbl_IntentionalQuestion> questions)
        {
            foreach (tbl_IntentionalQuestion q in questions)
            {
                // get pointer in DB
                var updateQuestoin = manager.tbl_IntentionalQuestions.Where(x => x.QuestionId == q.QuestionId).FirstOrDefault();

                // check if exist
                if (updateQuestoin == null)
                    saveQuestionToDB(q);
                else
                {
                    // setting the title of the question
                    updateQuestoin.Title = q.Title;

                    var answerAndNewAnswer = q.Answers.Zip(updateQuestoin.Answers, (o, n) => new { newAnswer = o, oldAnswer = n });

                    foreach (var nw in answerAndNewAnswer)
                    {
                        UpdateAnswerToDB(nw.oldAnswer, nw.newAnswer);
                        manager.SubmitChanges();
                    }

                    manager.SubmitChanges();
                }
            }

            // delete part
            var deleteQuestions = manager.tbl_IntentionalQuestions.Where(x => x.FormId == questions.First().FormId && !questions.Contains(x)).ToList();


            // delete all his question
            deleteQuestions.ForEach(q => deleteQustion(q.FormId, q.QuestionId));

            manager.SubmitChanges();
        }


        /// <summary>
        /// up date answer
        /// </summary>
        /// <param name="answer"></param>
        /// <param name="newAnswer"></param>
        public void UpdateAnswerToDB(tbl_IntentionalAnswer answer, tbl_IntentionalAnswer newAnswer)
        {
            // getting pointer to the dataBase
            var updateAnswer = manager.tbl_IntentionalAnswers.Where(x => x.AnswerId == answer.AnswerId).FirstOrDefault();
           
            // check if exist
            if (updateAnswer == null)
                throw new Exception(string.Format("Answer id = {0} cant be update, the question does not exist in the dataBase", answer.QuestionId));

            // setting the data for the new answer
            if (newAnswer != null)
            {
                updateAnswer.Text = newAnswer.Text;
                updateAnswer.Score = newAnswer.Score;
                updateAnswer.AnswerOption = newAnswer.AnswerOption;
            }
            else
            {
                updateAnswer.Text = string.Empty;
                updateAnswer.Score = 0;
                updateAnswer.AnswerOption = false;
            }

            // save change to db
            manager.SubmitChanges();
        }

    }
}