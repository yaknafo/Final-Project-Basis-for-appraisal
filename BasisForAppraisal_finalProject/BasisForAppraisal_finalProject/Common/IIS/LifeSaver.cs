using Microsoft.Web.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasisForAppraisal_finalProject.Common.IIS
{
    public static class LifeSaver
    {
        public static void RecycleAppPools()
        {
            ServerManager serverManager = new ServerManager();
            ApplicationPoolCollection appPools = serverManager.ApplicationPools;
            foreach (ApplicationPool ap in appPools)
            {
                if (ap.Name == "BFE")
                {
                    ap.Recycle();
                }
            }
        }
    }
}