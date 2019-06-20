using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ARAVINDMSOLUTION.Utilities{
   public static class Util
    {
        public static string ParseYearandMonth(string strYearMonth)
        {  
            if (strYearMonth.Length == 8)
            {
                string strMonth = string.Empty;
                int intYear = default(int);
                string strfinalYearMonth = string.Empty;
                strMonth = strYearMonth.Substring(0, 3).ToLower();
                intYear = Convert.ToInt32(strYearMonth.Substring(4, 4));
                strfinalYearMonth = intYear  + ParseMonth(strMonth);
                if (strfinalYearMonth.Length==7)
                {
                    return strfinalYearMonth;
                }
                return string.Empty;
            }
            return string.Empty;  
        }

        private static string ParseMonth(string strMonth)
        {
            switch (strMonth)
            {
                case "jan":
                    return "-01";
                case "feb":
                    return "-02";
                case "mar":
                    return "-03";
                case "apr":
                    return "-04";
                case "may":
                    return "-05";
                case "jun":
                    return "-06";
                case "jul":
                    return "-07";
                case "aug":
                    return "-08";
                case "sep":
                    return "-09";
                case "oct":
                    return "-10";
                case "nov":
                    return "-11";
                case "dec":
                    return "-12";          
                default:
                    return string.Empty;                   
            }

        }
    }
}
