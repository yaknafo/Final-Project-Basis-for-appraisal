using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasisForAppraisal_finalProject.DBML
{
    public class DbmlBFADataContext
    {
        // public static string connectionStringYair= ConfigurationSettings.AppSettings["ConnectionStringYair"];
        public static string connectionStringYair = "Data Source=LENOVO-PC-YAIR;Initial Catalog=BasisForAppraisalDB;Integrated Security=True;" + "MultipleActiveResultSets=true";
        public static string connectionStringfrenkel = "Data Source=ASUS\\FINALPROJECT;Initial Catalog=BasisForAppraisalDB;Integrated Security=True";
        public static string connectionStringServer = "Data Source=132.75.252.109;Initial Catalog=oy2015;Persist Security Info=True;User ID=oy2015;Password=ed56t19;MultipleActiveResultSets=true";



        public static BFADataBasedbmlDataContext dbDataContext = new BFADataBasedbmlDataContext(connectionStringYair);

        public static BFADataBasedbmlDataContext GetDataContextInstance()
        {
            return dbDataContext;
        }
    }
}