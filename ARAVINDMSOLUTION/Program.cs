using ARAVINDMSOLUTION.Bussiness;
using AravindSolution.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace ARAVINDMSOLUTION
{
    class Program
    {
        /// <summary>
        /// main entry point of the application 
        /// Facade Design Pattern.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Console.WriteLine("There are 4 Functionalities available for this application as Extraction Type 1,2,3,4 as in below ");
            Console.WriteLine("1.specify dates in format (mmm-yyyy e.g. Jan-2017) to get the data for that period parameters required (1,FromPeriod) eg from command prompt call the ARAVINDMSOLUTION.exe 1 JAN-2017 and click enter button");
            Console.WriteLine("2.compare the financial companies rates against bank rates by months, and see on which months financial companies have a higher rate. parameters required (2, Pass from-period, to-period) " +
            " eg From command prompt call the ARAVINDMSOLUTION.exe 2 JAN-2017 DEC-2017 and click enter button");
            Console.WriteLine("3.compare overall average of financial companies rates against bank rates. parameters required ( 3, from-period, to-period) eg: " +
            "3 JAN-2017 DEC-2017, From command prompt call the ARAVINDMSOLUTION.exe 3 JAN-2017 DEC-2017 and click enter button");
            Console.WriteLine("4.interest rates slope are on an upward or downward trend during this period. parameters required (4, from-period, to-period) eg: " +
            " From command prompt call the ARAVINDMSOLUTION.exe 4 JAN-2017 DEC-2017 as input and click enter button");
            Console.WriteLine();
            try
            {
                //validations
                if (args.Length > 0)
                {

                    string selectResult = args[0];
                    string selectedPeriod = string.Empty;
                    string selecteDateFrom = string.Empty;
                    string selecteDateTo = string.Empty;
                    //validations
                    if (selectResult == "1" || selectResult == "2" || selectResult == "3" || selectResult == "4")
                    {
                        Console.WriteLine("Arguments Passed to the Application : Extraction Type : " + selectResult);
                    }
                    else
                    {
                        Console.WriteLine("Arguments Passed to the Application : Extraction Type : " + selectResult);
                        Console.WriteLine("Which is not a valid one Please input with the information provided as above");
                        Console.Read();
                    }

                    List<Data> lsData = new List<Data>();
                    MoMCoreBL momCoreBL = new MoMCoreBL(); //sub system one

                    if (args[0] == "1")//First Requirement 
                    {
                        GetDataByPeriodDisplay(args, momCoreBL);
                    }
                    //validations
                    if (args[0] == "2" || args[0] == "3" || args[0] == "4")
                    {
                        Console.WriteLine("Arguments Passed to the Application : FromPeriod : " + args[1]);
                        Console.WriteLine("Arguments Passed to the Application : TOPeriod : " + args[2]);
                        selecteDateFrom = Utilities.Util.ParseYearandMonth(args[1]);  //another sub system 
                        selecteDateTo = Utilities.Util.ParseYearandMonth(args[2]);  //another sub system 
                        if (selecteDateFrom.Length != 7)
                        {
                            Console.WriteLine("Arguments : FromPeriod : " + args[1] + " is not right Please enter in format of MMM-YYYY and try again.");
                            Console.Read();
                        }
                        if (selecteDateTo.Length != 7)
                        {
                            Console.WriteLine("Arguments : ToPeriod : " + args[2] + " is not right Please enter in format of MMM-YYYY and try again.");
                            Console.Read();
                        }
                    }

                    if (args[0] == "2") //Second Requirement 
                    {
                        GeDataByPeriodForComparisonDisplay(selecteDateFrom, selecteDateTo, momCoreBL);
                    }


                    if (args[0] == "3")//Third Requirement 
                    {
                        List<Data> objDataCompareAverageMonths = new List<Data>();
                        objDataCompareAverageMonths = GeDataByPeriodForAverageComparisonDisplay(selecteDateFrom, selecteDateTo, momCoreBL);
                    }


                    if (args[0] == "4")//Fourth Requirement 
                    {
                        GeDataByPeriodForInterestRatesSlopeComparisonDisplay(selecteDateFrom, selecteDateTo, momCoreBL);
                    }
                }
                
            }
            catch (Exception ex)
            {

                Console.WriteLine("Some thing went wrong,Please check with your administrator");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                Console.Read();
            }

        }

        private static void GeDataByPeriodForInterestRatesSlopeComparisonDisplay(string selecteDateFrom, string selecteDateTo, MoMCoreBL cb)
        {
            List<Data> objDataInterestRatesSlopeCompareAverageMonths = new List<Data>();
            string status = string.Empty; ;
            string banks_fixed_deposits_3m_prevalue = "1210.021212";//some dummy initial value
            objDataInterestRatesSlopeCompareAverageMonths = cb.GeDataByPeriodForInterestRatesSlopeComparison(selecteDateFrom, selecteDateTo);
            if (objDataInterestRatesSlopeCompareAverageMonths.Count >= 1)
            {
                Console.WriteLine("Service-GeDataByPeriodForInterestRatesSlopeComparison Found the data for Sepecific Criteria From Months - " + selecteDateFrom + " To Months : " + selecteDateTo);
                Console.WriteLine("Total Number of Months betwen From Months - " + selecteDateFrom + " To Months : " + selecteDateTo + " is " + objDataInterestRatesSlopeCompareAverageMonths.Count);
                Console.WriteLine("Compare ALL Months beween Banks and Financial Companies - " + selecteDateFrom + " To Months : " + selecteDateTo + " is " + objDataInterestRatesSlopeCompareAverageMonths.Count);
                Console.WriteLine("Bussiness Method  Extraction is Started Date and Time -: " + System.DateTime.Now);
                foreach (Data data in objDataInterestRatesSlopeCompareAverageMonths)
                {
                    Console.WriteLine("end_of_month-" + data.end_of_month);
                    Console.WriteLine("banks_fixed_deposits_3m -" + data.banks_fixed_deposits_3m + " | " + "fc_fixed_deposits_3m -" + data.fc_fixed_deposits_3m);
                    Console.WriteLine("banks_fixed_deposits_6m -" + data.banks_fixed_deposits_6m + " | " + "fc_fixed_deposits_6m -" + data.fc_fixed_deposits_6m);
                    Console.WriteLine("banks_fixed_deposits_12m -" + data.banks_fixed_deposits_12m + " | " + "fc_fixed_deposits_12m -" + data.fc_fixed_deposits_12m);
                    Console.WriteLine("banks_savings_deposits -" + data.banks_savings_deposits + " | " + "fc_savings_deposits -" + data.fc_savings_deposits);

                    if (Convert.ToDecimal(data.banks_fixed_deposits_3m) == Convert.ToDecimal(banks_fixed_deposits_3m_prevalue))
                    {
                        status = "no change trend";
                    }

                    if (Convert.ToDecimal(data.banks_fixed_deposits_3m) < Convert.ToDecimal(banks_fixed_deposits_3m_prevalue))
                    {
                        status = "downward trend";
                    }
                    if (Convert.ToDecimal(data.banks_fixed_deposits_3m) > Convert.ToDecimal(banks_fixed_deposits_3m_prevalue))
                    {
                        status = "upward trend";
                    }

                    if (banks_fixed_deposits_3m_prevalue != "1210.021212")
                    {
                        Console.WriteLine("interest rates slope -: " + status);
                    }
                    Console.WriteLine();

                    if (banks_fixed_deposits_3m_prevalue != data.banks_fixed_deposits_3m)
                    {
                        banks_fixed_deposits_3m_prevalue = data.banks_fixed_deposits_3m;
                    }
                }
                Console.WriteLine("Bussiness Method  Extraction is Completed Date and Time -: " + System.DateTime.Now);
                Console.Read();
            }
            else
            {
                Console.WriteLine("Service-GeDataByPeriod Did not able to find the data for Sepecific Criteria - " + "GeDataByPeriod");
                Console.WriteLine("Please try again changing the parameter " + " Date and Time : " + System.DateTime.Now);
                Console.Read();
            }
        }

        private static List<Data> GeDataByPeriodForAverageComparisonDisplay(string selecteDateFrom, string selecteDateTo, MoMCoreBL cb)
        {
            List<Data> objDataCompareAverageMonths = cb.GeDataByPeriodForAverageComparison(selecteDateFrom, selecteDateTo);
            if (objDataCompareAverageMonths.Count >= 1)
            {
                Console.WriteLine("Service-GeDataByPeriodForAverageComparison Found the data for Sepecific Criteria From Months - " + selecteDateFrom + " To Months : " + selecteDateTo);
                Console.WriteLine("Compare ALL Months beween Banks and Financial Companies - " + selecteDateFrom + " To Months : " + selecteDateTo + " is ");
                Console.WriteLine("Bussiness Method  Extraction is Started Date and Time -: " + System.DateTime.Now);
                foreach (Data data in objDataCompareAverageMonths)
                {
                    Console.WriteLine("Avg banks_fixed_deposits_3m - " + Convert.ToDecimal(data.banks_fixed_deposits_3m).ToString("F2") + " | " + "Avg fc_fixed_deposits_3m - " + Convert.ToDecimal(data.fc_fixed_deposits_3m).ToString("F2"));
                    Console.WriteLine("Avg banks_fixed_deposits_6m - " + Convert.ToDecimal(data.banks_fixed_deposits_6m).ToString("F2") + " | " + "Avg fc_fixed_deposits_6m - " + Convert.ToDecimal(data.fc_fixed_deposits_6m).ToString("F2"));
                    Console.WriteLine("Avg banks_fixed_deposits_12m - " + Convert.ToDecimal(data.banks_fixed_deposits_12m).ToString("F2") + " | " + "Avg fc_fixed_deposits_12m - " + Convert.ToDecimal(data.fc_fixed_deposits_12m).ToString("F2"));
                    Console.WriteLine();
                }
                Console.WriteLine("Bussiness Method  Extraction is Completed Date and Time -: " + System.DateTime.Now);
                Console.Read();
            }
            else
            {
                Console.WriteLine("Service-GeDataByPeriodForAverageComparison Did not able to find the data for Sepecific Criteria - From Months - " + selecteDateFrom + " To Months: " + selecteDateTo);
                Console.WriteLine("Please try again changing the parameter " + " Date and Time : " + System.DateTime.Now);
                Console.Read();
            }

            return objDataCompareAverageMonths;
        }

        private static void GeDataByPeriodForComparisonDisplay(string selecteDateFrom, string selecteDateTo, MoMCoreBL cb)
        {
            List<Data> objDataCompareMonths = new List<Data>();
            objDataCompareMonths = cb.GeDataByPeriodForComparison(selecteDateFrom, selecteDateTo);
            if (objDataCompareMonths.Count >= 1)
            {
                Console.WriteLine("Service-GeDataByPeriodForComparison Found the data for Sepecific Criteria From Months - " + selecteDateFrom + " To Months : " + selecteDateTo);
                Console.WriteLine("Total Number of Months betwen From Months - " + selecteDateFrom + " To Months : " + selecteDateTo + " is " + objDataCompareMonths.Count);
                Console.WriteLine("Compare ALL Months beween Banks and Financial Companies - " + selecteDateFrom + " To Months : " + selecteDateTo + " is " + objDataCompareMonths.Count);
                Console.WriteLine("Bussiness Method  Extraction is Started Date and Time -: " + System.DateTime.Now);
                foreach (Data data in objDataCompareMonths)
                {
                    Console.WriteLine("end_of_month-" + data.end_of_month);
                    Console.WriteLine("banks_fixed_deposits_3m -" + data.banks_fixed_deposits_3m + " | " + "fc_fixed_deposits_3m -" + data.fc_fixed_deposits_3m);
                    Console.WriteLine("banks_fixed_deposits_6m -" + data.banks_fixed_deposits_6m + " | " + "fc_fixed_deposits_6m -" + data.fc_fixed_deposits_6m);
                    Console.WriteLine("banks_fixed_deposits_12m -" + data.banks_fixed_deposits_12m + " | " + "fc_fixed_deposits_12m -" + data.fc_fixed_deposits_12m);
                    Console.WriteLine("banks_savings_deposits -" + data.banks_savings_deposits + " | " + "fc_savings_deposits -" + data.fc_savings_deposits);
                    Console.WriteLine();
                }
                Console.WriteLine("Bussiness Method  Extraction is Completed Date and Time -: " + System.DateTime.Now);
                Console.Read();
            }
            else
            {
                Console.WriteLine("Service-GeDataByPeriodForComparison Did not able to find the data for Sepecific Criteria From Months - " + selecteDateFrom + " To Months : " + selecteDateTo);
                Console.WriteLine("Please try again changing the parameter " + " Date and Time : " + System.DateTime.Now);
                Console.Read();
            }
        }

        private static string GetDataByPeriodDisplay(string[] args, MoMCoreBL cb)
        {
            string selectedPeriod;
            Console.WriteLine("Arguments Passed to the Application : FromPeriod : " + args[1]);
            selectedPeriod = Utilities.Util.ParseYearandMonth(args[1]); //another sub system 
            if (selectedPeriod.Length != 7)
            {
                Console.WriteLine("Arguments : FromPeriod : " + args[1] + " is not right Please enter in format of MMM-YYYY and try again");
            }
            else
            {
                List<Data> objDatafrSelectedMonth = new List<Data>();
                objDatafrSelectedMonth = cb.GeDataByPeriod(selectedPeriod);
                if (objDatafrSelectedMonth.Count == 1)
                {
                    foreach (Data data in objDatafrSelectedMonth)
                    {
                        Console.WriteLine("Bussiness Method  Extraction is Started Date and Time -: " + System.DateTime.Now);
                        Console.WriteLine("Service-GeDataByPeriod Found the data for Sepecific Criteria - " + "( " + args[1] + " )" + selectedPeriod);
                        Console.WriteLine("end_of_month-" + data.end_of_month);
                        Console.WriteLine("prime_lending_rate- " + data.prime_lending_rate);
                        Console.WriteLine("banks_fixed_deposits_3m -" + data.banks_fixed_deposits_3m);
                        Console.WriteLine("banks_fixed_deposits_6m -" + data.banks_fixed_deposits_6m);
                        Console.WriteLine("banks_fixed_deposits_12m -" + data.banks_fixed_deposits_12m);
                        Console.WriteLine("banks_savings_deposits -" + data.banks_savings_deposits);
                        Console.WriteLine("fc_hire_purchase_motor_3y -" + data.fc_hire_purchase_motor_3y);
                        Console.WriteLine("fc_housing_loans_15y -" + data.fc_housing_loans_15y);
                        Console.WriteLine("fc_fixed_deposits_3m -" + data.fc_fixed_deposits_3m);
                        Console.WriteLine("fc_fixed_deposits_6m -" + data.fc_fixed_deposits_6m);
                        Console.WriteLine("fc_fixed_deposits_12m -" + data.fc_fixed_deposits_12m);
                        Console.WriteLine("fc_savings_deposits -" + data.fc_savings_deposits);
                        Console.WriteLine("Bussiness Method  Extraction is completed Date and Time -: " + System.DateTime.Now);
                        Console.Read();
                    }
                }
                else
                {
                    Console.WriteLine("Service-GeDataByPeriod Did not able to find the data for Sepecific Criteria - " + "( " + args[1] + " )" + selectedPeriod);
                    Console.WriteLine("Please try again changing the parameter " + " Date and Time : " + System.DateTime.Now);
                    Console.Read();
                }

            }

            return selectedPeriod;
        }
    }
}
