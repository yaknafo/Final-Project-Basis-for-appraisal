using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BasisForAppraisal_finalProject.DBML;
using System.Data.Linq;
using System.IO;
using Microsoft.Office.Interop;
using Excel = Microsoft.Office.Interop.Excel;


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
            var formUpdate = manager.tblForms.Where(f => f.formId == form.formId).FirstOrDefault();
            formUpdate.FormName = form.FormName;
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



        //-----------------------------------------------------------------------------------output- input method-----------------------------------------------------------------
        public void upload_excelfile(string path,int idCompany)
        {
            try
            {
                Excel.Application xlApp = new Excel.Application();
                Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(@path);
                Excel._Worksheet xlWorksheet = xlWorkbook.Sheets[1];
                Excel.Range xlRange = xlWorksheet.UsedRange;
                int rowCount = xlRange.Rows.Count;
                int colCount = xlRange.Columns.Count;

                for (int i = 1; i <= rowCount; i++)
                {
                    String[] data = new string[colCount];
                    for (int j = 1; j <= colCount; j++)
                    {
                        data[j - 1] = (xlRange.Cells[i, j] as Excel.Range).Value2.ToString();
                    }
                    var emp = new tbl_Employee();
                    emp.companyId = idCompany;
                    emp.employeeId = data[0];
                    emp.firstName = data[1];// change it to cto'r!!!
                    emp.lastName = data[2];
                    emp.Email = data[3];
                    emp.Unit = data[4];
                    emp.Class = data[5];
                    if (data[6].Equals("כן"))
                        emp.IsManger = true;
                    else
                        emp.IsManger = false;
                    this.addWorkerToDb(emp);
                }
                xlApp.Workbooks.Close();
            }
            finally
            {
                File.Delete(path);
            }
        }



        //-------------------------------------------------------------------------------company method--------------------------------------------------------

        public void addCompany(tbl_Company cmp)
        {
           
            this.manager.tbl_Companies.InsertOnSubmit(cmp);
            this.manager.SubmitChanges();
        }
        // add employee to db withought company
        // check if there is double! ( not checked!!!!)

        public void addWorkerToDb(tbl_Employee emp)
        {
            if (!manager.tbl_Employees.Contains(emp))
            {
                this.manager.tbl_Employees.InsertOnSubmit(emp);
                this.manager.SubmitChanges();
            }
        }

       
         public void  deleteWorker(String workerid,int companyNumber)
        {
            // find the record to delete f
            var workerToDelete = manager.tbl_Employees.Where(a => a.employeeId == workerid && a.companyId == companyNumber).FirstOrDefault();
            manager.tbl_Employees.DeleteOnSubmit(workerToDelete);
            manager.SubmitChanges();

        }




    }
}
