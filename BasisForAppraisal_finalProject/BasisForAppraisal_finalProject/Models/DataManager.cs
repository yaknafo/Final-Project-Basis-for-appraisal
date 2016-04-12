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
using System.Data.Entity;


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

        public BFADataBasedbmlDataContext GetNewDataManager()
        {
            var manager = DbmlBFADataContext.GetDataContextInstance();
            var cultureinfo = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentCulture = cultureinfo;
            System.Threading.Thread.CurrentThread.CurrentUICulture = cultureinfo;
            return manager;
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

        public Table<tbl_ConnectorAnswer> ConnectorAnswers 
        {
            get { return manager.tbl_ConnectorAnswers; }
        }

        public Table<tbl_ConnectorFormFill> ConnectorForm
        {
            get { return manager.tbl_ConnectorFormFills; }
        }

        public Table<tbl_IntentionalQuestion> Questions
        {
            get { return manager.tbl_IntentionalQuestions; }
        }

        public Table<ReportForIndividual> ReportForIndividuals
        {
            get { return manager.ReportForIndividuals; }
        }

        public Table<ReportForIndividualLine> ReportForIndividualLines
        {
            get { return manager.ReportForIndividualLines; }
        }

        public Table<ReportForOrganiztion> ReportForOrganiztions
        {
            get { return manager.ReportForOrganiztions; }
        }

        public Table<ReportForOrganiztionLine> ReportForOrganiztionLines
        {
            get { return manager.ReportForOrganiztionLines; }
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
            // need todo something better here!!!
            if (question.QuestionType.Contains("Scale"))
                question.createAnswersToQuestion(question.NumberOfAnswers.Value);

            manager.tbl_IntentionalQuestions.InsertOnSubmit(question);
            manager.SubmitChanges();

            if (!question.QuestionType.Contains("Scale"))
            {
                question.Answers.ForEach(x => x.QuestionId = question.QuestionId);
                question.Answers.ForEach(x => x.SectionId = question.SectionId);
                question.Answers.ForEach(x => x.FormId = question.FormId);
                manager.tbl_IntentionalAnswers.InsertAllOnSubmit(question.Answers);
                manager.SubmitChanges();
            }

           
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
        public int AddForm(tblForm form, bool ifCopy = false)
        {
            manager.tblForms.InsertOnSubmit(form);

          if (ifCopy)
          {
           UpdateFormToDB(form);
            foreach(tbl_Section sec in form.Sections)
            {
            SaveSection(sec);
            SaveQuestionsToDB(sec.Questions);
            }
        }

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
            var questionToDelete = manager.tbl_IntentionalQuestions.Where(a=>  a.QuestionId == quesNumber).FirstOrDefault();
            var answersToDelete = manager.tbl_IntentionalAnswers.Where(a => a.QuestionId == quesNumber);
            if (answersToDelete != null)
            {
                manager.tbl_IntentionalAnswers.DeleteAllOnSubmit(answersToDelete);
                manager.SubmitChanges();
            }
            if (questionToDelete != null)
            {
                manager.tbl_IntentionalQuestions.DeleteOnSubmit(questionToDelete);
                manager.SubmitChanges();
            }

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
            try
            {
                if (manager.tblForms.Where(f => f.FormName == form.FormName).Count() > 1)
                    throw new Exception("השם של טופס זה תפוס אנא בחר שם אחר");

                var formUpdate = manager.tblForms.Where(f => f.formId == form.formId).FirstOrDefault();
                formUpdate.FormName = form.FormName;
                formUpdate.introduction = form.introduction;
                formUpdate.lastChange = DateTime.Now;
                manager.SubmitChanges();
            }
            catch
            {
                manager = DbmlBFADataContext.GetNewDataContextInstance();
            }
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

                if (sectionUpdate != null)
                {
                    sectionUpdate.Name = s.Name;
                    sectionUpdate.HelpExplanation = s.HelpExplanation;
                }
                else
                {
                    manager.tbl_Sections.InsertOnSubmit(s);
                }
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
                    updateQuestoin.HelpText = q.HelpText;
                    updateQuestoin.MandatoryQuestion = q.MandatoryQuestion;
                    updateQuestoin.ForManager = q.ForManager;
                    updateQuestoin.ForAccompanied = q.ForAccompanied;
                    updateQuestoin.ForSelf = q.ForSelf;
                    updateQuestoin.Forcolleagues = q.Forcolleagues;
                    updateQuestoin.ForReports = q.ForReports;

                    var answerAndNewAnswer = q.Answers.Zip(updateQuestoin.Answers, (o, n) => new { newAnswer = o, oldAnswer = n });

                    foreach (var nw in answerAndNewAnswer)
                    {
                        UpdateAnswerToDB(nw.oldAnswer, nw.newAnswer);
                        manager.SubmitChanges();
                    }

                    var answersToDelete= manager.tbl_IntentionalAnswers.Where(x => x.QuestionId == q.QuestionId);

                    // add new answer
                    var answersToAdd = q.Answers.Where(a => a.AnswerId == 0).ToList();
                    if (answersToAdd.Any())
                        UpdateForgienKeyQuestionToAnswer(q, answersToAdd);
                    manager.tbl_IntentionalAnswers.InsertAllOnSubmit(answersToAdd);

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


        public void UpdateForgienKeyQuestionToAnswer(tbl_IntentionalQuestion question, List<tbl_IntentionalAnswer> answers)
        {
            foreach (tbl_IntentionalAnswer answer in answers)
            {
                answer.FormId = question.FormId;
                answer.SectionId = question.SectionId;
                answer.QuestionId = question.QuestionId;
            }

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
                updateAnswer.Score = newAnswer.MyScore;
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

        ////  ------------------------------- Reports --------------------------------------------------------------//

        public void SaveReportForIndividual(ReportForIndividual report)
        {
            var reportFromDb = manager.ReportForIndividuals.SingleOrDefault(x => x.IndividualId == report.IndividualId && x.FormId == report.FormId);
            if (reportFromDb ==null)
            {
                manager.ReportForIndividuals.InsertOnSubmit(report);
                manager.SubmitChanges();
            }
            else{
                report.feedback = reportFromDb.feedback;
                report.IsClose = reportFromDb.IsClose;
            }
        }

        public void SaveReportForOrganiztion(ReportForOrganiztion report)
        {
            var reportFromDb = manager.ReportForOrganiztions.SingleOrDefault(x => x.CompanyId == report.CompanyId && x.FormId == report.FormId);
            if (reportFromDb == null)
            {
                report.CreationDate = DateTime.Now;
                report.LastCalculationDate = DateTime.Now; 
                manager.ReportForOrganiztions.InsertOnSubmit(report);
                manager.SubmitChanges();
            }
            else
            {
                reportFromDb.LastCalculationDate = DateTime.Now;
                manager.SubmitChanges();
            }
        }


        public void SaveReportForOrganiztionLines(ReportForOrganiztionLine line)
        {
            var lineFromDb = manager.ReportForOrganiztionLines.FirstOrDefault(x => x.CompanyId == line.CompanyId && x.FormId == line.FormId && line.QuestionId == x.QuestionId);
            if (lineFromDb == null)
            {
                manager.ReportForOrganiztionLines.InsertOnSubmit(line);
                manager.SubmitChanges();
            }
            else
            {
                manager.SubmitChanges();
            }
        }

        public void EditReportForIndividual(string text, bool isClose, string IndividualId, int FormId)
        {
            var report = manager.ReportForIndividuals.SingleOrDefault(x => x.IndividualId == IndividualId && x.FormId == FormId);
            if (report !=null)
            {
               report.feedback = text;
                report.IsClose = isClose;
                manager.SubmitChanges();
            }
        }

        public void SaveReportForIndividualLines(string IndividualId, int formId, int sectionId, int QuestionId, double SelfEvaluation, double collegesEvaluation, double SupervisorEvaluation, double AccompaniedEvaluation, int CountOfFormsFilled, double TotalAverage)
        {
            ReportForIndividualLine lineUpdate;

            if (manager.ReportForIndividualLines.Any(x => x.IndividualId == IndividualId && x.QuestionId == QuestionId))
            {
                lineUpdate = manager.ReportForIndividualLines.FirstOrDefault(x => x.IndividualId == IndividualId &&  x.QuestionId == QuestionId);
                if (lineUpdate != null)
                {
                    lineUpdate.SelfEvaluation = SelfEvaluation;
                    lineUpdate.collegesEvaluation = collegesEvaluation;
                    lineUpdate.SupervisorEvaluation = SupervisorEvaluation;
                    lineUpdate.AccompaniedEvaluation = AccompaniedEvaluation;
                    lineUpdate.CountOfFormsFilled = CountOfFormsFilled;
                    lineUpdate.TotalAverage = TotalAverage;
                    manager.SubmitChanges();
                }
                   
            }
            else
            {
                var newLine = new ReportForIndividualLine()
                {
                    QuestionId = QuestionId,
                    IndividualId = IndividualId,
                    QuestioFormId = formId,
                    FromId = formId,
                    SectionId = sectionId,
                    SelfEvaluation = SelfEvaluation,
                    collegesEvaluation = collegesEvaluation,
                    SupervisorEvaluation = SupervisorEvaluation,
                    AccompaniedEvaluation = AccompaniedEvaluation,
                    CountOfFormsFilled = CountOfFormsFilled,
                    TotalAverage = TotalAverage

                };

                manager.ReportForIndividualLines.InsertOnSubmit(newLine);
                manager.SubmitChanges();
              
            }
              //manager.SubmitChanges();
        }


        public ReportForIndividual GetReportForIndividual(string IndividualId, int formid )
        {
            return manager.ReportForIndividuals.FirstOrDefault(x => x.IndividualId == IndividualId && x.FormId == formid);
        }

        public bool Save()
        {
            try
            {
                manager.SubmitChanges();
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }


        public ReportForOrganiztionLine GetReportOrganiztionLine(int formId,int OrganiztionId, int questionId)
        {
            return manager.ReportForOrganiztionLines.FirstOrDefault(x => x.FormId == formId && x.CompanyId == OrganiztionId && x.QuestionId == questionId);
        }

        public List<ReportForIndividualLine> GetReportForIndividualLineForOrganiztion(int formId, int OrganiztionId)
        {
            return manager.ReportForIndividualLines.Where(x => x.FromId == formId && x.ReportForIndividual.CompanyId == OrganiztionId).ToList();
        }

       ////  ------------------------------- secutiry --------------------------------------------------------------//

        public bool IsManager(string id){
            return (bool)Employees.FirstOrDefault(x => x.employeeId == id).IsManger;
        }

        public bool IsAccompanied(string id)
        {
            return (bool)Employees.FirstOrDefault(x => x.employeeId == id).IsAccompanied;
        }

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
