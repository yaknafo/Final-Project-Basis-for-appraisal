using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using BasisForAppraisal_finalProject.DBML;
namespace BasisForAppraisal_finalProject.Models
{
    public class FromFillerManager
    {
        DataMangerCompany DMC = new DataMangerCompany();

        public async void SaveConnectorAnswers(List<tbl_ConnectorAnswer> ca)
        {
            var tasks = ca.Select(async item =>
            {
                DMC.AddConnectorAnswers(item);
            });
            await Task.WhenAll(tasks);
            
        }


    }
}