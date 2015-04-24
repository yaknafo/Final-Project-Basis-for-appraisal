using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BasisForAppraisal_finalProject.DBML;

namespace BasisForAppraisal_finalProject.Models
{
    public class CompanyManger
    {

        public void addCompany(tbl_Company cmp)
        {
            var manger =new DataMangerCompany();
            manger.addCompany(cmp);
        }
        // method add data from excel file to sql server db- workers to company
        public void upload_excelfile(string path, int idCompany)
        {
            var db = new DataMangerCompany();
            db.upload_excelfile(path,idCompany);

        }

         public void  deleteWorker(String workerid,int companyNumber)
        {
            var db = new DataMangerCompany();
            db.deleteWorker(workerid, companyNumber);

        }
        
       
    }
}