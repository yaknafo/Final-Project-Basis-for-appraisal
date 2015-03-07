using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasisForAppraisal_finalProject.Models
{
    public class FormManager
    {
        public void deleteQustion(int formID, int quesNumber)
        {
            DataManager db =  new DataManager();
            db.deleteQustion(formID, quesNumber);
        }

    }
}