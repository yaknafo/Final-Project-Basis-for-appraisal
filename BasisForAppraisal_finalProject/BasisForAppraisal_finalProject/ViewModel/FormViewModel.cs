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

       // type of new Question
        private tbl_TypeQuestion intentionalType;
        private tbl_TypeQuestion freeTextType;
        private tbl_TypeQuestion scaleType;

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

        //-------------End The Type Question Area 

       //------------- The new Question Area 
        public tbl_IntentionalQuestion NewQuestion
        {
            get
            {

                return newQuestion ?? new tbl_IntentionalQuestion(3, formId, CurrentSection.SectionId, IntentionalType);

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
        //-------------End The new Question Area 

       //--------------- Init New question
        private tbl_IntentionalQuestion InitNewQuestionScale()
        {
            NewQuestionScale = new tbl_IntentionalQuestion(2, formId, CurrentSection.SectionId, ScaleType);
            return NewQuestionScale;
        }
       
       private tbl_IntentionalQuestion InitNewQuestionFreeText()
        {
            NewQuestionFreeText = new tbl_IntentionalQuestion(1, formId, CurrentSection.SectionId, FreeTextType);
            return NewQuestionFreeText;
        }

       //--------------- End  Init New question

       public void AddQuestion(tbl_IntentionalQuestion question)
        {
            if (question.FormId == 0 || question.SectionId == 0)
            {
                question.FormId = form.formId;
                //question.SectionId = form.Sections.First().SectionId;
                question.SectionId = CurrentSection.SectionId;
                question.Answers.ForEach(a => a.FormId = form.formId);
                //question.Answers.ForEach(a => a.SectionId = form.Sections.First().SectionId);

                question.Answers.ForEach(a => a.SectionId = CurrentSection.SectionId);
            }

            

            Questions.Add(question);
        }


       public void AddQuestionFreeText(tbl_IntentionalQuestion question)
       {
           if (question.FormId == 0 || question.SectionId == 0)
           {
               question.FormId = form.formId;
               //question.SectionId = form.Sections.First().SectionId;
               question.SectionId = CurrentSection.SectionId;
               question.Answers.ForEach(a => a.FormId = form.formId);
               //question.Answers.ForEach(a => a.SectionId = form.Sections.First().SectionId);

               question.Answers.ForEach(a => a.SectionId = CurrentSection.SectionId);
           }


           Questions.Add(question);
       }


       public void AddQuestionScale(tbl_IntentionalQuestion question)
       {
           var maxTag = question.Answers[0].Text;
           var minTag = question.Answers[1].Text;

           if (question.NumberOfAnswers >= 2)
           question.createAnswersToQuestion(question.NumberOfAnswers);

           question.Answers[0].Text = minTag;
           question.Answers[question.Answers.Count-1].Text = maxTag;



           if (question.FormId == 0 || question.SectionId == 0)
           {
               // we wnat to make sure that every question and answer has the form id and section id
               question.FormId = form.formId;
               question.SectionId = CurrentSection.SectionId;
               question.Answers.ForEach(a => a.FormId = form.formId);
               question.Answers.ForEach(a => a.SectionId = CurrentSection.SectionId);
           }


           Questions.Add(question);
       }

       public void DeleteQuestions()
       {
          var deleteQuestion=Questions.Where(q => q.deleteQuestion);

          Questions.RemoveAll(x => x.deleteQuestion);
           
       }


     

    }
}
