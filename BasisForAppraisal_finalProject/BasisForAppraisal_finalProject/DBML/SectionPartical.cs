using BasisForAppraisal_finalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BasisForAppraisal_finalProject.DBML;

namespace BasisForAppraisal_finalProject.DBML
{
    public partial class tbl_Section
    {

        private List<tbl_IntentionalQuestion> questions = new List<tbl_IntentionalQuestion>();


        public List<tbl_IntentionalQuestion> Questions
        {
            get
            {
                if(questions == null || questions.Count ==0)
                    questions = GetAllQuestions();
                return questions;
            }
            set
            {
                questions = value;
            }

        }




        public List<tbl_IntentionalQuestion> GetAllQuestions()
        {
            var manager = new DataManager();
            return manager.IntentionalQuestion.Where(x => x.SectionId == this.SectionId).ToList();
        }

    }
}