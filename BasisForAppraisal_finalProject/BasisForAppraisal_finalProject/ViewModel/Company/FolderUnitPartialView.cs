using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BasisForAppraisal_finalProject.DBML;
using BasisForAppraisal_finalProject.Models;

namespace BasisForAppraisal_finalProject.ViewModel.Company
{
    public class FolderUnitPartialView
    {
        public IEnumerable<BasisForAppraisal_finalProject.DBML.tbl_Class> myClass;
        public int count;

        public FolderUnitPartialView(IEnumerable<BasisForAppraisal_finalProject.DBML.tbl_Class> list, int number)
        {
            myClass = list;
            count = number;
        }
    }
}