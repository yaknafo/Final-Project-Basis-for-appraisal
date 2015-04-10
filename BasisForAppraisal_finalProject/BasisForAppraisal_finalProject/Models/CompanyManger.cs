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
            var manger = new DataManager();
            manger.addCompany(cmp);
        }
        // method add data from excel file to sql server db- workers to company
        public void upload_excelfile(string path, int idCompany)
        {
            DataManager db = new DataManager();
            db.upload_excelfile(path,idCompany);

        }
    }
}