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
        public static string connectionString;



        public static BFADataBasedbmlDataContext dbDataContext = new BFADataBasedbmlDataContext(connectionStringYair);

        public static BFADataBasedbmlDataContext GetDataContextInstance()
        {
            return dbDataContext;
        }
    }
}