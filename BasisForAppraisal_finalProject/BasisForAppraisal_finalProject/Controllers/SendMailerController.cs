using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using BasisForAppraisal_finalProject.DBML;
using BasisForAppraisal_finalProject.Models;
using BasisForAppraisal_finalProject.Controllers;
using BasisForAppraisal_finalProject.Common.Constans;
using System.Threading.Tasks;

namespace SendMail.Controllers
{
    public class SendMailerController : Controller
    {
        //
        // GET: /SendMailer/  
     
        [HttpPost]
        public ActionResult Index(int id,string unit,string cl)
        {
           var res =  Task.Factory.StartNew(() =>SendMails(id, unit, cl));
            return RedirectToAction("ManageCompany", "MainCompanies", new { id = id, unit = unit, cl = cl });
        }
 
        private void             SendMails(int id, string unit, string cl)
        {
            DataMangerCompany dm = new DataMangerCompany();
            List<tbl_Employee> NotSendMail = new List<tbl_Employee>();
            string body = "שלום רב" +
                          "<br />מייל זה נשלח לך על ידי בסיס להערכה" +
                          " <br /> למייל זה מצורף לינק  לאתר החברה" +
                          "<br /בכניסה לאתר תצטרך להכניס שם משתמש וסיסמא אשר מצורפים למייל זה <br />" +
                          "<br />לאחר כניסה לאתר תוכל לצפות בכל הטפסים אשר עלייך למלא<br />" +
                          "<br />:קישור לאתר<br />" +
                          "<br />http://localhost:21981/Account/Login<br />";
            String name = "<br /> שם משתמש:";
            String password = "<br /> סיסמא:";
           
            string endbody = "<br /><br /><br />תודה צוות בסיס להערכה" + "</div>";

            foreach (var emp in dm.getEmpForEmail(id, unit, cl))
            {
                BasisForAppraisal_finalProject.Models.MailModel _objModelMail = new BasisForAppraisal_finalProject.Models.MailModel();
                if (ModelState.IsValid)
                {
                    try
                    {
                        _objModelMail.From = "dontreplaybasisforappraisal@gmail.com";
                        _objModelMail.Body = "<div dir='rtl'>" + emp.firstName + " " + emp.lastName + " " + body + name + " " + emp.employeeId + password + " " + emp.employeeId + endbody + "</div>";

                        _objModelMail.Subject = "בסיס להערכה- שאלון למילוי";
                        _objModelMail.To = emp.Email;
                        MailMessage mail = new MailMessage();
                        mail.To.Add(_objModelMail.To);
                        mail.From = new MailAddress(_objModelMail.From);
                        mail.Subject = _objModelMail.Subject;
                        string Body = _objModelMail.Body;
                        mail.Body = Body;
                        mail.IsBodyHtml = true;
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = "smtp.gmail.com";
                        smtp.Port = 587;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new System.Net.NetworkCredential("dontreplaybasisforappraisal@gmail.com", "basis123456");// Enter seders User name and password
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                    catch (Exception ex)
                    {
                        NotSendMail.Add(emp);
                    }
                }
            }
            var Persons = "</br > :פרט לאנשים הבאים</br>";
            foreach (var v in NotSendMail)
                Persons += v.firstName + " " + v.lastName + " </br>";
            if (NotSendMail.Count < 1)
                Persons = "";
            TempData[ResultOperationConstans.Success] = "שלח מייל בהצלחה" + Persons;
        }

        private object DataMangerCompany()
        {
            throw new NotImplementedException();
        }

        private object DataManger()
        {
            throw new NotImplementedException();
        }
    }
}
