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
using BasisForAppraisal_finalProject.BL;

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
            var listOfConnecor = dmo.ConnectorFormFill.ToList();

            //------------------------- here is the part for know which Manager fill what column 
            var resForYair = new List<FormFillByManager>();

            Dictionary<tblForm, List<string>> managerNameForColumn = new Dictionary<tblForm, List<string>>();

            //---- check if frim need to be print more than 1

            var numberOfFormToPrint = NumerOfFromColumnToPrint(EmployeeOfTheUnit, listOfConnecor, resForYair, managerNameForColumn);
     
            List<int> avrageQuestionsId = new List<int>();

            getQuestionIDForAvrageMangers(numberOfFormToPrint, avrageQuestionsId);
            
            //-------------------------------End of the part ---------------------------------------------------------

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
                    var toPrintMore = true;
                    while (toPrintMore)
                    {
                        foreach (tbl_IntentionalQuestion question in sec.Questions)
                        {
                            if (!numberOfFormToPrint.ContainsKey(sec.tblForm))
                                products.Columns.Add(string.Format(",\"{0}\"", form.FormName + ":" + question.Title), typeof(string));
                            else
                            {
                                // if we here we know that question is by manager 
                                var managerName = managerNameForColumn[form].ToArray()[numberOfFormToPrint[sec.tblForm] - 1];
                                products.Columns.Add(string.Format(",\"{0}\"", form.FormName + "\n [ " + managerName + " ] :  " + question.Title), typeof(string));
                            }
                            questionsId.Add(question.QuestionId);
                            counter++;
                        }
                        if (numberOfFormToPrint.ContainsKey(sec.tblForm))
                        {
                            // make sure we will not go Out of bounes 
                        if (numberOfFormToPrint[sec.tblForm] > 0)
                        numberOfFormToPrint[sec.tblForm]--;

                        if (numberOfFormToPrint[sec.tblForm] == 0)
                            toPrintMore = false;
                        }
                        else
                            toPrintMore = false;
                    }
                }
            }
            // if need to calculcation avrage for Question only for manager!!
            var toDoCalculateAvrageForMangers = false;

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
                       var currentQuestion = questionsId.ToArray()[i - 7];
                       if (avrageQuestionsId.Contains(currentQuestion))
                       {
                           // if that is true that mean we got the first time we print question for manager --> we need to print avrage
                           // might be problem is the question id in order is not first or last --> will need refactor after a while :-(
                           avrageQuestionsId.Remove(currentQuestion);
                           toDoCalculateAvrageForMangers = true;
                       }
                       row[i] = getScoreForQuestion(currentQuestion, emp.employeeId, listOfConnecorAnswers, listOfQuestions, toDoCalculateAvrageForMangers).ToString();

                       toDoCalculateAvrageForMangers= false;
                   }
                       products.Rows.Add(row);
               }
           


            var grid = new GridView();
            grid.DataSource = products;
            grid.DataBind();
            return grid;
        }

        private void getQuestionIDForAvrageMangers(Dictionary<tblForm, int> numberOfFormToPrint, List<int> avrageQuestionsId)
        {
            foreach(tblForm form in numberOfFormToPrint.Keys)
            {
                avrageQuestionsId.Add(form.Sections.First().Questions.Last().QuestionId);
            }
        }

        private Dictionary<tblForm, int> NumerOfFromColumnToPrint(List<DBML.tbl_Employee> EmployeeOfTheUnit, List<tbl_ConnectorFormFill> listOfConnecor, List<FormFillByManager> resForYair, Dictionary<tblForm, List<string>> managerNameForColumn)
        {
            var className = EmployeeOfTheUnit.First().tbl_Class;


            foreach (tbl_ConnectorFormFill c in listOfConnecor)
            {
                var temp = new FormFillByManager { Employee = c.tbl_Employee, Form= c.tblForm };
                if (c.tbl_Employee1.tbl_Class.Equals(className) && c.tbl_Employee.IsManagerWrapper && !resForYair.Contains(temp))
                {
                    resForYair.Add(temp);
                    if (!managerNameForColumn.ContainsKey(temp.Form))
                    managerNameForColumn.Add(temp.Form, new List<string>());
                }
            }

            Dictionary<tblForm, int> numberOfFormToPrint = new Dictionary<tblForm, int>();

            foreach (FormFillByManager c in resForYair)
            {
                if (numberOfFormToPrint.ContainsKey(c.Form))
                {
                    numberOfFormToPrint[c.Form]++;
                    managerNameForColumn[c.Form].Add(c.Employee.FullName);
                }
                else
                {
                    numberOfFormToPrint.Add(c.Form, 2);
                    managerNameForColumn[c.Form].Add(c.Employee.FullName);
                }
            }

            // for the name in Avrage coulmn for Managers
            foreach(List<string> s in managerNameForColumn.Values)
            {
                s.Add("Avrage");
            }

         
            return numberOfFormToPrint;
        }


        private string getScoreForQuestion(int questionId, string employeeId, List<tbl_ConnectorAnswer> ConnectorAnswers, List<tbl_IntentionalQuestion> allQuestions, bool toDoCalculateAvrageForMangers)
        {
            var quetionResult = new QuestionReport();
            if (toDoCalculateAvrageForMangers)
            {
                quetionResult.CalculationQuestion(employeeId, questionId, ConnectorAnswers, allQuestions);
                if (quetionResult.directorCounter > 0)
                    return quetionResult.directorAverage.ToString();
                if (quetionResult.colleagueCounter > 0)
                    return quetionResult.colleagueAverage.ToString();
                if(quetionResult.selfCounter>0)
                return quetionResult.selfAverage.ToString();
                return "ריק";
            }
      
              var res  = quetionResult.CalculationQuestionFoeType(employeeId, questionId, ConnectorAnswers, allQuestions);
              if (res == -1)
                  return "ריק";

                    return res.ToString();
        }
    }
}