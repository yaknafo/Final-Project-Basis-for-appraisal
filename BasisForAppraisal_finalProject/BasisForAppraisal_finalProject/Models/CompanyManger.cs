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
    }
}