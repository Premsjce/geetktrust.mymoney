using Geektrust.MyMoney.App.Constants;
using Geektrust.MyMoney.App.CustomExceptions;

namespace Geektrust.MyMoney.App.Helpers
{
    internal class MonthHelper
    {
        public static string GetRebalancebleMonth(string month)
        {
            switch (month)
            {
                case Months.JANUARY:
                case Months.FEBRUARY:
                case Months.MARCH:
                case Months.APRIL:
                case Months.MAY:
                    return null;

                case Months.JUNE:
                case Months.JULY:
                case Months.AUGUST:
                case Months.SEPTEMBER:
                case Months.OCTOBER:
                case Months.NOVEMBER:
                    return Months.JUNE;

                case Months.DECEMBER:
                    return Months.DECEMBER;
            }

            return null;
        }

        public static string GetMonthNameFromNumber(int month)
        {
            switch (month)
            {
                case 1:
                    return Months.JANUARY;
                case 2:
                    return Months.FEBRUARY;
                case 3:
                    return Months.MARCH;
                case 4:
                    return Months.APRIL;
                case 5:
                    return Months.MAY;
                case 6:
                    return Months.JUNE;
                case 7:
                    return Months.JULY;
                case 8:
                    return Months.AUGUST;
                case 9:
                    return Months.SEPTEMBER;
                case 10:
                    return Months.OCTOBER;
                case 11:
                    return Months.NOVEMBER;
                case 12:
                    return Months.DECEMBER;

            }
            throw new InvalidMonthlyNumberException("Month number sent is not within range of 1-12");
        }

        public static string GetPreviousMonth(string month)
        {
            switch (month)
            {
                case Months.JANUARY:
                    return Months.DECEMBER;
                case Months.FEBRUARY:
                    return Months.JANUARY;
                case Months.MARCH:
                    return Months.FEBRUARY;
                case Months.APRIL:
                    return Months.MARCH;
                case Months.MAY:
                    return Months.APRIL;
                case Months.JUNE:
                    return Months.MAY;
                case Months.JULY:
                    return Months.JUNE;
                case Months.AUGUST:
                    return Months.JULY; ;
                case Months.SEPTEMBER:
                    return Months.AUGUST;
                case Months.OCTOBER:
                    return Months.SEPTEMBER;
                case Months.NOVEMBER:
                    return Months.OCTOBER;
                case Months.DECEMBER:
                    return Months.NOVEMBER;
            }

            return null;
        }
    }
}
