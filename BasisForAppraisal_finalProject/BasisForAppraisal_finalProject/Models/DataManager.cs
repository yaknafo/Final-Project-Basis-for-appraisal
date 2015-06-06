using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BasisForAppraisal_finalProject.DBML;
using System.Data.Linq;
using System.IO;
using Microsoft.Office.Interop;
using Excel = Microsoft.Office.Interop.Excel;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;


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
        /// 
        

        public Table<tbl_IntentionalAnswer> IntentionalAnswer
        {
            get { return manager.tbl_IntentionalAnswers; }
        }

        public Table<tblForm> Forms
        {
            get { return manager.tblForms; }
        }

        public Table<tbl_Section> Sections
        {
            get { return manager.tbl_Sections; }
        }
     
        public Table<tbl_IntentionalQuestion> IntentionalQuestion
        {
            get { return manager.tbl_IntentionalQuestions; }
        }

        public Table<tbl_Company> Companyies
        {
            get { return manager.tbl_Companies; }
        }

        public Table<tbl_Employee> Employees
        {
            get { return manager.tbl_Employees; }
        }

        public Table<tbl_TypeQuestion> TypeQuestions
        {
            get { return manager.tbl_TypeQuestions; }
        }

        //----------------------------------------------------------------------------------------------------------------------------------//


        public tbl_IntentionalQuestion GetQuestionWithAnswers(int fid, int qid)
        {
            var question = manager.tbl_IntentionalQuestions.Where(x => x.FormId == fid && x.QuestionId == qid).First();
            question.GetAllAnswers();
            return question;
        }

        public tblForm GetFormWithSections(int fid)
        {
            var form = manager.tblForms.Where(x => x.formId == fid).FirstOrDefault();

            if (form == null)
                throw new Exception("form with number number: " + fid + " mot exist in DB");

            form.GetAllSections().ForEach(q => q.GetAllQuestions().ForEach(a => a.GetAllAnswers()));
            return form;
        }

        //--------------------------------   Get Method by id -----------------------------

        public List<tbl_Employee> getEmployeesByCompanyId(int companyId)
        {
            return manager.tbl_Employees.Where(e => e.companyId == companyId).ToList();
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
            question.Answers.ForEach(x => x.SectionId = question.SectionId);
            question.Answers.ForEach(x => x.FormId = question.FormId);

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

        public int SaveSection(tbl_Section section)
        {
            manager.tbl_Sections.InsertOnSubmit(section);
            manager.SubmitChanges();
            return section.FormId;
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
        /// delete Answer from specfic form and specfic question
        /// 
        /// <param name="formID"></param> 
        /// <param name="quesNumber"></param>
        public void deleteAnswer(int formID, int quesNumber, int answerId)
        {
            // find the record to delete from the right form and right ques number
            var answerToDelete = manager.tbl_IntentionalAnswers.Where(a => a.AnswerId == answerId).FirstOrDefault();

            if (answerToDelete != null)
                manager.tbl_IntentionalAnswers.DeleteOnSubmit(answerToDelete);

            manager.SubmitChanges();

        }
        
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
        /// delete Section from specfic form and specfic question
        /// 
        /// <param name="formID"></param> 
        /// <param name="quesNumber"></param>
        public void deleteSection(int sectionId)
        {
            // find the record to delete from the right form and right ques number
            var SectionToDelete = manager.tbl_Sections.Where(a => a.SectionId == sectionId).FirstOrDefault();

            var QuestionToDelete = manager.tbl_IntentionalAnswers.Where(a => a.SectionId == sectionId);

            SectionToDelete.Questions.ForEach(q => deleteQustion(q.FormId, q.QuestionId));

            manager.tbl_Sections.DeleteOnSubmit(SectionToDelete);
            
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
            if (formForDelete == null)
                throw new Exception(string.Format("the form with number id {0} camt be delete becouse he cant be found in DB", formID));

            // check that from dosnt have any connector
            if(manager.tbl_ConnectorFormFills.Any(x => x.formId == formID))
                throw new Exception("טופס זה מיועד למילוי לכן אינו ניתן למחיקה אנא מחק את החיבורים לפני ניסיון המחיקה הבא");


            // delete all the section of the form
            formForDelete.Sections.ForEach(q => deleteSection(q.SectionId));

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
            if (manager.tblForms.Where(f => f.FormName == form.FormName).Count() > 1)
                throw new Exception("השם של טופס זה תפוס אנא בחר שם אחר");

            var formUpdate = manager.tblForms.Where(f => f.formId == form.formId).FirstOrDefault();
            formUpdate.FormName = form.FormName;
            formUpdate.lastChange = DateTime.Now;
            manager.SubmitChanges();
        }


        /// <summary>
        /// save form include qustions and answers to db
        /// </summary>
        /// <param name="form"></param>
        public void UpdateSectionsToDB(List<tbl_Section> sections)
        {
            foreach (tbl_Section s in sections)
            {
                var sectionUpdate = manager.tbl_Sections.Where(f => s.SectionId == f.SectionId).FirstOrDefault();

                sectionUpdate.Name = s.Name;
                sectionUpdate.HelpExplanation = s.HelpExplanation;
                manager.SubmitChanges();
                
            }
           
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
                    // Delete Answers in the question that was deleted by the user
                    foreach (tbl_IntentionalAnswer answer in updateQuestoin.Answers)
                    {
                        if (!q.Answers.Select(x => x.AnswerId).Contains(answer.AnswerId))
                            deleteAnswer(q.FormId, q.QuestionId, answer.AnswerId);
                    }

                    // setting the title of the question
                    updateQuestoin.Title = q.Title;

                    var answerAndNewAnswer = q.Answers.Zip(updateQuestoin.Answers, (o, n) => new { newAnswer = o, oldAnswer = n });

                    foreach (var nw in answerAndNewAnswer)
                    {
                        UpdateAnswerToDB(nw.oldAnswer, nw.newAnswer);
                        manager.SubmitChanges();
                    }

                    var answersToDelete= manager.tbl_IntentionalAnswers.Where(x => x.QuestionId == q.QuestionId);

                    manager.SubmitChanges();
                }
            }

            // --------------------------- Delete Question ---------------------------------------------
            if (questions.Any())
            {
                var deleteQuestions = manager.tbl_IntentionalQuestions.Where(x => x.FormId == questions.First().FormId && !questions.Contains(x)).ToList();

                // delete all his question
                deleteQuestions.ForEach(q => deleteQustion(q.FormId, q.QuestionId));
            }

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
            {
                //throw new Exception(string.Format("Answer id = {0} cant be update, the question does not exist in the dataBase", answer.QuestionId));
                return;
            }

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



       ////  ------------------------------- secutiry --------------------------------------------------------------//


       // public async void CreateRole(string roleName)
       // {
       //     var roleManager = new RoleManager<Microsoft.AspNet.Identity.EntityFramework.IdentityRole>(new RoleStore<IdentityRole>(new ApplicationDbContext()));

       //     if (!roleManager.RoleExists(roleName))
       //     {
       //         var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
       //         role.Name = roleName;
       //         roleManager.Create(role);

       //     }

       // }

       // public async void CreateUser(string userName, string password)
       // {
       //     var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())); 
       //     var user = new ApplicationUser() { UserName = userName ,};
       //     var result = await UserManager.CreateAsync(user, password);
       // }



    }
}
