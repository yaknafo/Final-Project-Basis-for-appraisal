using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BasisForAppraisal_finalProject.DBML;
using BasisForAppraisal_finalProject.Models;
using System.IO;
using System.Web.UI.WebControls;
using System.Data;
using System.Globalization;

namespace BasisForAppraisal_finalProject.Models
{
    public class ExcelExportDataService
    {

        public GridView ExportToCSV(List<DBML.tblForm> res, List<DBML.tbl_Employee> EmployeeOfTheUnit)
        {
            var dm = new DataManager();
             var dmo = new DataMangerCompany();
            var listOfQuestions = dm.Questions.ToList();
            var listOfConnecorAnswers =dmo.ConnectorAnswer.ToList();
            //he-IL
            int counter = 0;
            List<int> questionsId = new List<int>();
            var products = new System.Data.DataTable("teste");
            //CultureInfo myCultureInfo = new CultureInfo("he-IL");
            //products.Locale = myCultureInfo;
            products.Columns.Add("ID", typeof(string));
            products.Columns.Add("First Name", typeof(string));
            products.Columns.Add("Last Name", typeof(string));
            products.Columns.Add("חברה", typeof(string));
            products.Columns.Add("יחידה", typeof(string));
            products.Columns.Add("מחזור", typeof(string));
            products.Columns.Add("מייל", typeof(string));
            foreach (tblForm form in res)
            {
                foreach (tbl_Section sec in form.Sections)
                {
                    foreach (tbl_IntentionalQuestion question in sec.Questions)
                    {
                        products.Columns.Add(string.Format(",\"{0}\"", form.FormName + ":" + question.Title), typeof(string));
                        questionsId.Add(question.QuestionId);
                        counter++;
                    }
                }
            }

               foreach (tbl_Employee emp in EmployeeOfTheUnit)
               {
                   var row = products.NewRow();
                   row[0] = emp.employeeId;
                   row[1] = emp.firstName;
                   row[2] = emp.lastName;
                   row[3] = emp.companyId;
                   row[4] = emp.unitName;
                   row[5] = emp.className;
                   row[6] = emp.Email;
                   for (int i = 7; i < counter + 7; i++)
                   {
                       row[i] = getScoreForQuestion(questionsId.ToArray()[i - 7], emp.employeeId, listOfConnecorAnswers,listOfQuestions).ToString();
                   }
                       products.Rows.Add(row);
               }
           


            var grid = new GridView();
            grid.DataSource = products;
            grid.DataBind();
            return grid;
        }


        private int getScoreForQuestion(int questionId, string employeeId, List<tbl_ConnectorAnswer> ConnectorAnswers, List<tbl_IntentionalQuestion> allQuestions)
        {
            var quetionResult = new QuestionReport();
            quetionResult.CalculationQuestion(employeeId, questionId, ConnectorAnswers, allQuestions);
            if (quetionResult.directorCounter > 0)
                return quetionResult.directorAverage;
            if (quetionResult.colleagueCounter > 0)
                return quetionResult.colleagueAverage;

            return quetionResult.selfAverage;
        }
    }
}