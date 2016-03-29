using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BasisForAppraisal_finalProject.Helpr
{
    public class ConvertAnswerHelperScore
    {
        public int ConvertAnswerHelperScoreForReport(int score)
        {
            if (4 <= score && score <= 5)
                return 3;
            if (score == 3)
                return 2;
            return 1;
        }

        public string AverageToColor(double avrage)
        {
            if (avrage >= 2.5)
                return "green";
            if (avrage >= 1.5)
                return "Gray";
            return "Red";
        }

        public string ScoreTitle(double avrage)
        {
            if (avrage <= 1)
                return "נמוך";
            if (avrage <= 2)
                return "בינוני";
            return "גבוה";
        }

        public int OverUnder(double self, double otherAvrage)
        {

            if (self == 1)
            {
                if (otherAvrage >= 1.5)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }

            else if (self == 2)
            {
                if (otherAvrage >= 2.5)
                {
                    return -1;
                }
                else if (otherAvrage <= 1.5)
                {
                    return 1;
                }

                return 0;
            }

            else
            {
                if (otherAvrage < 2.5)
                    return 1;

                return 0;
            }
        }
    }
}