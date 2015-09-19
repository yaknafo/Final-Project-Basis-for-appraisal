using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BasisForAppraisal_finalProject.DBML;
using BasisForAppraisal_finalProject.Models;
using System.Globalization;

namespace BasisForAppraisal_finalProject.ViewModel
{
   public class FormViewModel
    {
        public tblForm form{set; get;}

        public tbl_Section CurrentSection { set; get; }

        private List<tbl_Section> sections; 

       // the new questions
        private tbl_IntentionalQuestion newQuestion;
        private tbl_IntentionalQuestion newQuestionFreeText;
        private tbl_IntentionalQuestion newQuestionScale;
        private tbl_IntentionalQuestion newQuestionMultipleChoice;
        private tbl_IntentionalQuestion newQuestionYesNo;
        private tbl_IntentionalQuestion newQuestionCbx;
        private tbl_IntentionalQuestion newQuestionMultipleChoiceList;

       // type of new Question
        private tbl_TypeQuestion intentionalType;
        private tbl_TypeQuestion freeTextType;
        private tbl_TypeQuestion scaleType;
        private tbl_TypeQuestion multipleChoiceType;
        private tbl_TypeQuestion yesNoType;
        private tbl_TypeQuestion cbxType;
        private tbl_TypeQuestion multipleChoiceListType;

       // data manager for get data from DB
        private DataManager dm;

       // List of question in the current Section
        private List<tbl_IntentionalQuestion> questions;


        public int formId { set; get; }

       


        public FormViewModel (tblForm form,int section=0)
	{
            dm = new DataManager();
            this.form= form;
            this.Sections = form.Sections;
            this.formId = form.formId;
            if (section == 0)
                CurrentSection = form.Sections.First();
            this.questions = CurrentSection.Questions;

            var firstQuestionToEdit = Questions.FirstOrDefault();

            if (firstQuestionToEdit != null)
                firstQuestionToEdit.EditQuestion = true;

            for(int i =0 ; i< Questions.Count ; i++)
            {
                Questions[i].QuestionNumberInForm = i;
            }
            

	}

        public FormViewModel()
        {

        }

       public DataManager DM
        {
            get
            {
                return dm ?? new DataManager();
            }
           set
            {
                dm = value;
            }
        }

       public List<tbl_Section> Sections
       {
           get
           {
               if (sections == null)
                   sections = new List<tbl_Section>();
               return sections;

           }
           set
           {
               sections = value;
           }

       }


       public List<tbl_IntentionalQuestion> Questions
       {
           get
           {
               if (questions == null)
                   questions = new List<tbl_IntentionalQuestion>();
               return questions;

           }
           set
           {
               questions = value;
           }

       }

       //------------- The Type Question Area 
        public tbl_TypeQuestion IntentionalType
        {
            get
            {
                return intentionalType ?? DM.TypeQuestions.Where(x => x.Name.Contains("Intentional")).FirstOrDefault();
            }
            set
            {
                intentionalType = value;
            }

        }

        public tbl_TypeQuestion FreeTextType
        {
            get
            {
                return freeTextType ?? DM.TypeQuestions.Where(x => x.Name.Contains("FreeText")).FirstOrDefault();
            }
            set
            {
                freeTextType = value;
            }

        }

        public tbl_TypeQuestion ScaleType
        {
            get
            {
                return scaleType ?? DM.TypeQuestions.Where(x => x.Name.Contains("Scale")).FirstOrDefault();
            }
            set
            {
                scaleType = value;
            }

        }

        public tbl_TypeQuestion MultipleChoiceType
        {
            get
            {
                return multipleChoiceType ?? DM.TypeQuestions.Where(x => x.Name.Contains("MultipleChoice")).FirstOrDefault();
            }
            set
            {
                multipleChoiceType = value;
            }

        }

        public tbl_TypeQuestion CbxType
        {
            get
            {
                return multipleChoiceType ?? DM.TypeQuestions.Where(x => x.Name.Contains("Cbx")).FirstOrDefault();
            }
            set
            {
               cbxType = value;
            }

        }

        public tbl_TypeQuestion YesNoType
        {
            get
            {
                return yesNoType ?? DM.TypeQuestions.Where(x => x.Name.Contains("YesNo")).FirstOrDefault();
            }
            set
            {
                yesNoType = value;
            }
        }

        public tbl_TypeQuestion MultipleChoiceListType
        {
            get
            {
                return multipleChoiceListType ?? DM.TypeQuestions.Where(x => x.Name.Equals("MultipleChoiceList")).FirstOrDefault();
            }
            set
            {
                multipleChoiceListType = value;
            }

        }

        //-------------End The Type Question Area 

       //------------- The new Question Area 
        public tbl_IntentionalQuestion NewQuestion
        {
            get
            {

                return newQuestion ?? new tbl_IntentionalQuestion(5, formId, CurrentSection.SectionId, IntentionalType);

            }
            set
            {
                newQuestion = value;
            }

        }

        public tbl_IntentionalQuestion NewQuestionFreeText
        {
            get
            {

                return newQuestionFreeText ?? InitNewQuestionFreeText();

            }
            set
            {
                newQuestionFreeText = value;
            }

        }

        public tbl_IntentionalQuestion NewQuestionScale
        {
            get
            {

                return newQuestionScale ?? InitNewQuestionScale();

            }
            set
            {
                newQuestionScale = value;
            }

        }

        public tbl_IntentionalQuestion NewQuestionMultipleChoice
        {
            get
            {

                return newQuestionMultipleChoice ?? InitnewQuestionMultipleChoice();

            }
            set
            {
                newQuestionMultipleChoice = value;
            }

        }

        public tbl_IntentionalQuestion NewQuestionYesNo
        {
            get
            {

                return newQuestionYesNo ?? InitnewQuestionYesNo();

            }
            set
            {
                newQuestionYesNo = value;
            }

        }

        public tbl_IntentionalQuestion NewQuestionCbx
        {
            get
            {

                return newQuestionCbx ?? InitnewQuestionCbx();

            }
            set
            {
                newQuestionCbx = value;
            }

        }


        public tbl_IntentionalQuestion NewQuestionMultipleChoiceList
        {
            get
            {

                return newQuestionMultipleChoiceList ?? InitnewQuestionMultipleChoiceList();

            }
            set
            {
                newQuestionMultipleChoiceList = value;
            }

        }

       
        //-------------End The new Question Area 

       //--------------- Init New question
        private tbl_IntentionalQuestion InitNewQuestionScale()
        {
            NewQuestionScale = new tbl_IntentionalQuestion(2, formId, CurrentSection.SectionId, ScaleType);
            NewQuestionScale.NumberOfAnswers = 2;
            return NewQuestionScale;
        }

        private tbl_IntentionalQuestion InitnewQuestionMultipleChoice()
        {
            NewQuestionMultipleChoice = new tbl_IntentionalQuestion(1, formId, CurrentSection.SectionId, MultipleChoiceType);
            NewQuestionMultipleChoice.NumberOfAnswers = 1;
            return NewQuestionMultipleChoice;
        }

       private tbl_IntentionalQuestion InitNewQuestionFreeText()
        {
            NewQuestionFreeText = new tbl_IntentionalQuestion(1, formId, CurrentSection.SectionId, FreeTextType);
            return NewQuestionFreeText;
        }

       private tbl_IntentionalQuestion InitnewQuestionYesNo()
       {
           NewQuestionYesNo = new tbl_IntentionalQuestion(2, formId, CurrentSection.SectionId, YesNoType);
           return NewQuestionYesNo;
       }

       private tbl_IntentionalQuestion InitnewQuestionCbx()
       {
           newQuestionCbx= new tbl_IntentionalQuestion(8, formId, CurrentSection.SectionId, CbxType);
           return newQuestionCbx;
       }

       private tbl_IntentionalQuestion InitnewQuestionMultipleChoiceList()
       {
           NewQuestionMultipleChoiceList = new tbl_IntentionalQuestion(1, formId, CurrentSection.SectionId, MultipleChoiceListType);
           NewQuestionMultipleChoiceList.NumberOfAnswers = 1;
           return NewQuestionMultipleChoiceList;
       }


       //--------------- End  Init New question

       public void AddQuestion(tbl_IntentionalQuestion question)
        {
             question.FormId = form.formId;
                //question.SectionId = form.Sections.First().SectionId;
                question.SectionId = CurrentSection.SectionId;
                question.Answers.ForEach(a => a.FormId = form.formId);
                //question.Answers.ForEach(a => a.SectionId = form.Sections.First().SectionId);

                question.Answers.ForEach(a => a.SectionId = CurrentSection.SectionId);
           

            Questions.Add(question);

            // stratup new question מחוון
            NewQuestion = null;

        }


       public void AddQuestionFreeText(tbl_IntentionalQuestion question)
       {
           if (question.FormId == 0 || question.SectionId == 0)
           {
               question.FormId = form.formId;
               question.SectionId = CurrentSection.SectionId;
               question.Answers.ForEach(a => a.FormId = form.formId);

               question.Answers.ForEach(a => a.SectionId = CurrentSection.SectionId);
           }

           Questions.Add(question);

           // stratup new question free text
           NewQuestionFreeText = null;

       }


       public void AddQuestionScale(tbl_IntentionalQuestion question)
       {
           var maxTag = question.Answers[0].Text;
           var minTag = question.Answers[1].Text;

           if (question.NumberOfAnswers >= 2)
           {
               question.createAnswersToQuestion(question.NumberOfAnswers.Value);
           }

           question.Answers[0].Text = minTag;
           question.Answers[question.Answers.Count-1].Text = maxTag;
        
               // we wnat to make sure that every question and answer has the form id and section id
               question.FormId = form.formId;
               question.SectionId = CurrentSection.SectionId;
               question.Answers.ForEach(a => a.FormId = form.formId);
               question.Answers.ForEach(a => a.SectionId = CurrentSection.SectionId);
               question.tbl_TypeQuestion = ScaleType;
               question.QuestionType = ScaleType.Name;
               Questions.Add(question);
               
           // stratup new question scale
               NewQuestionScale = null;

       }

       public void AddQuestionMultipleChoice(tbl_IntentionalQuestion question)
       {
           // we wnat to make sure that every question and answer has the form id and section id
           question.FormId = CurrentSection.FormId;
           question.SectionId = CurrentSection.SectionId;
           question.Answers.ForEach(a => a.FormId = CurrentSection.FormId);
           question.Answers.ForEach(a => a.SectionId = CurrentSection.SectionId);
           Questions.Add(question);

           // stratup new question Muliti choice
               NewQuestionMultipleChoice = null;
               NewQuestionMultipleChoiceList = null;

       }


       public void AddQuestionCbx(tbl_IntentionalQuestion question)
       {
           // we wnat to make sure that every question and answer has the form id and section id
           question.FormId = CurrentSection.FormId;
           question.SectionId = CurrentSection.SectionId;

           // keep only the ansewr that had been filled with text
           var cleanAnswer = question.Answers.Where(a => !string.IsNullOrEmpty(a.Text)).ToList();
           question.Answers = cleanAnswer;

           question.Answers.ForEach(a => a.FormId = CurrentSection.FormId);
           question.Answers.ForEach(a => a.SectionId = CurrentSection.SectionId);
           
           Questions.Add(question);

           // stratup new question Muliti choice
           NewQuestionCbx = null;

       }

       public void DeleteQuestion(int questionId)
       {
           var questionToDelete = Questions.Where(q => q.QuestionNumberInForm == questionId).FirstOrDefault();

           if (questionToDelete == null)
               return;

           if (questionToDelete.QuestionId != 0)
           {
               var manager = new FormManager();
               manager.deleteQustion(formId, questionToDelete.QuestionId);
           }

           Questions.RemoveAll(x => x.QuestionNumberInForm == questionId);
       }


       public void DeleteQuestions()
       {
          var deleteQuestion=Questions.Where(q => q.deleteQuestion);

          Questions.RemoveAll(x => x.deleteQuestion);
           
       }

       public void DeleteAnswer(int idAnswer)
       {
         

           for (int i = 0; i < Questions.Count; i++ )
           {
               var questionAnswers = Questions[i].Answers; ;
                var deleteAnswer = questionAnswers.Where(a => a.AnswerId == idAnswer).FirstOrDefault();
                if (deleteAnswer != null)
                Questions[i].Answers.Remove(deleteAnswer);
           }


       }


     //------------------------------------- TODO make it better after juist a try for now ----------------------------------

       public void AddQuestion(string add)
       {
           try
           {

               switch (add)
               {
                   case "addQustion": AddQuestion(NewQuestion);
                       break;

                   case "AddQustionFreeText": AddQuestion(NewQuestionFreeText);
                       break;
                   case "AddYesNoQuestion": AddQuestion(NewQuestionYesNo);
                       break;

                   case "AddScaleQuestion": AddQuestionScale(NewQuestionScale);
                       break;

                   case "AddMultipleChoiceQuestion": AddQuestionMultipleChoice(NewQuestionMultipleChoice);
                       break;

                   case "AddCbxQuestion":AddQuestionCbx(NewQuestionCbx);
                       break;

                   case "AddMultipleChoiceListQuestion": AddQuestionMultipleChoice(NewQuestionMultipleChoiceList);
                       break;

               }
           }
           catch
           {


           }

           Questions.Last().QuestionNumberInForm = Questions.Max(q => q.QuestionNumberInForm) + 1;
       }



       public void AddAnswerOptionToQuestoin(int questionNumberInForm)
       {
           var question = Questions.Where(x => x.QuestionNumberInForm == questionNumberInForm).FirstOrDefault();

           if (question == null)
               return;

           question.AddAnswerOption();
       }

    }
}
