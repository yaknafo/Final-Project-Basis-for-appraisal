using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasisForAppraisal_finalProject.ViewModel.ExportData
{
    public class ReportForOrganiztionLinesViewModel
    {
        public int QuestionId { set; get; }

        public int HighScore {set; get;}

        public int MidScore { set; get; }

        public int LowScore { set; get; }

        public string TypeQuestion { set; get; }

        public string TitleQuestion { set; get; }
    }
}
