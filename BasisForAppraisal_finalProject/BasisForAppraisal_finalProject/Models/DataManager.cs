using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BasisForAppraisal_finalProject.DBML;
using System.Data.Linq;

namespace BasisForAppraisal_finalProject.Models
{
    public class DataManager
    {

      BFADataBasedbmlDataContext manager = new BFADataBasedbmlDataContext();

         public DataManager()
        {
            this.manager = DbmlBFADataContext.GetDataContextInstance();
            var cultureinfo = new System.Globalization.CultureInfo("en-US");
            System.Threading.Thread.CurrentThread.CurrentCulture = cultureinfo;
            System.Threading.Thread.CurrentThread.CurrentUICulture = cultureinfo;
        }

         /// <summary>
         /// get method for all tables
         /// </summary>
         public Table<tbl_IntentionalAnswer> IntentionalAnswer
         {
             get { return manager.tbl_IntentionalAnswers; }
         }

    }
}