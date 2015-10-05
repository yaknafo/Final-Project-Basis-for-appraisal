using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BasisForAppraisal_finalProject.DBML;
using BasisForAppraisal_finalProject.Models;
namespace BasisForAppraisal_finalProject.BL
{
    public class CopyFormService
    {
      private  DataManager DM = new DataManager();


      public int CopyFormById(int formID)
      {
          var sourceForm = DM.GetFormWithSections(formID);
          var fm = new FormManager();
          var numberOfForm = fm.AddNewForm(false);
          var newForm = DM.GetFormWithSections(numberOfForm);
          CopyFormFull(sourceForm, newForm);
          SaveCopyOfForm(newForm);
          return newForm.formId;
      }

      private void SaveCopyOfForm(tblForm newForm)
      {
          DM.UpdateFormToDB(newForm);
          DM.UpdateSectionsToDB(newForm.Sections);
          foreach (tbl_Section sec in newForm.Sections)
          {
              sec.Questions.ForEach(ques => ques.SectionId = sec.SectionId);
              sec.Questions.ForEach(ques => ques.FormId = sec.FormId);

              DM.UpdateQuestionsToDB(sec.Questions);
          }
      }

      private tblForm CopyFormFull(tblForm form, tblForm newForm)
      {
          newForm.FormName = form.FormName + " העתק " + DateTime.Now;
          newForm.lastChange = DateTime.Now;

          copySectionFromForm(form, newForm).ForEach(newForm.Sections.Add);

          return newForm;
      }

      private List<tbl_Section> copySectionFromForm(tblForm form, tblForm newForm)
      {
          var newSection =  new List<tbl_Section>();
          var sourceSectoins = form.Sections;
        

          foreach(tbl_Section sec in sourceSectoins)
          {
              var tempSection = new tbl_Section();
              tempSection.Name = sec.Name;
              tempSection.HelpExplanation = sec.HelpExplanation;
              tempSection.FormId = newForm.formId;
          

              //----- copy question ------//
              tempSection.Questions = CopyQuestionPerSection(sec.Questions);
              newSection.Add(tempSection);
          }
         return newSection;
      }

      private List<tbl_IntentionalQuestion> CopyQuestionPerSection(List<tbl_IntentionalQuestion> sourceQuestions)
      {
          var newQuestions = new List<tbl_IntentionalQuestion>();
          foreach(tbl_IntentionalQuestion sourceQuestion in sourceQuestions)
          {
              var tempQuestion = new tbl_IntentionalQuestion();

              tempQuestion.HelpText = sourceQuestion.HelpText;
              tempQuestion.MandatoryQuestion = sourceQuestion.MandatoryQuestion;
              tempQuestion.NumberOfAnswers = sourceQuestion.NumberOfAnswers;
              tempQuestion.QuestionNumberInForm = sourceQuestion.QuestionNumberInForm;
              tempQuestion.QuestionType = sourceQuestion.QuestionType;
              tempQuestion.Title = sourceQuestion.Title;

             //----- copy Answers ------//
              tempQuestion.Answers = CopyAnswerPerQuestion(sourceQuestion.Answers);
             newQuestions.Add(tempQuestion);

          }

          return newQuestions;
      }

      private List<tbl_IntentionalAnswer> CopyAnswerPerQuestion(List<tbl_IntentionalAnswer> sourceAnswers)
      {
           var newAnswers = new List<tbl_IntentionalAnswer>();
           foreach (tbl_IntentionalAnswer sourceAnswer in sourceAnswers)
           {
               var tempAnswer = new tbl_IntentionalAnswer();

               tempAnswer.MyScore = sourceAnswer.MyScore;
               tempAnswer.Score = sourceAnswer.Score;
               tempAnswer.Text = sourceAnswer.Text;
               newAnswers.Add(tempAnswer);
           }

          return newAnswers;
      }
       
    }
}