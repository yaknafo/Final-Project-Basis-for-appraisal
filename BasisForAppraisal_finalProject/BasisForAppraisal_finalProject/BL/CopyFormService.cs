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
          // get source form from DB
          var sourceForm = DM.GetFormWithSections(formID);
          var fm = new FormManager();

          // Create new form
          var numberOfForm = fm.AddNewForm(false);
          var newForm = DM.GetFormWithSections(numberOfForm);

          //Copy Form And Form Sections only from Source form
          CopyFormWithSectionOnly(sourceForm, newForm);

          //Save in data base
          DM.UpdateFormToDB(newForm);
          DM.UpdateSectionsToDB(newForm.Sections);

          // in this part we copy all the questions from source form and save it to data base
          int counter =0;
          foreach(tbl_Section sec in sourceForm.Sections)
          {
             var tempQuestions = CopyQuestionPerSection(sec.Questions, newForm.formId, newForm.Sections.ToArray()[counter].SectionId);
             DM.UpdateQuestionsToDB(tempQuestions);
              counter++;
          }

          return newForm.formId;
      }


      private tblForm CopyFormWithSectionOnly(tblForm form, tblForm newForm)
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

              newSection.Add(tempSection);

          }
         return newSection;
      }

      private List<tbl_IntentionalQuestion> CopyQuestionPerSection(List<tbl_IntentionalQuestion> sourceQuestions, int formId ,int secId)
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
              tempQuestion.FormId = formId;
              tempQuestion.SectionId = secId;

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