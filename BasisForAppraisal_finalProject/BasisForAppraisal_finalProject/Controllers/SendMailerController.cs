using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
using BasisForAppraisal_finalProject.DBML;
using BasisForAppraisal_finalProject.Models;
namespace SendMail.Controllers
{
    public class SendMailerController : Controller
    {
        //
        // GET: /SendMailer/  
     
        [HttpPost]
        public ActionResult Index(int id,string unit,string cl)
        {
            DataMangerCompany dm = new DataMangerCompany();

            foreach (var emp in dm.getEmpForEmail(id,unit,cl))
            {
                BasisForAppraisal_finalProject.Models.MailModel _objModelMail = new BasisForAppraisal_finalProject.Models.MailModel();
                if (ModelState.IsValid)
                {
                    _objModelMail.From = "dontreplaybasisforappraisal@gmail.com";
                    _objModelMail.Body = "בדיקת מייל";
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
                    smtp.Credentials = new System.Net.NetworkCredential
                    ("dontreplaybasisforappraisal@gmail.com", "basis123456");// Enter seders User name and password
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
            }

            return RedirectToAction("ManageCompany", "MainCompanies", new { id = id, unit = unit, cl = cl });
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
