using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using AravindSolution.Model;
using System.Linq;
using System.Configuration;
using System.Net;

namespace ARAVINDMSOLUTION.Bussiness
{
    public class MoMCoreBL : IMoMCoreBL
    {
        private List<Data> lsData = new List<Data>();
        public List<Data> GeDataByPeriodForInterestRatesSlopeComparison(string fromMonth, string toMonth)
        {
            IEnumerable<Data> objendOfMonth;
            try
            {
                GetInitialDatafrRestClientByMonth().GetAwaiter().GetResult();
                objendOfMonth = lsData.Where((Data c) => c.end_of_month.CompareTo(fromMonth) >= 0 && c.end_of_month.CompareTo(toMonth) <= 0);

            }
            catch (System.Exception ex)
            {
                throw;
            }
            return objendOfMonth.ToList();
        }


        public List<Data> GeDataByPeriodForAverageComparison(string fromMonth, string toMonth)
        {
            List<Data> lsDataTemp = new List<Data>();
            decimal avgbanks_fixed_deposits_3m;
            decimal avgbanks_fixed_deposits_6m;
            decimal avgbanks_fixed_deposits_12m;
            decimal avgfc_fixed_deposits_3m;
            decimal avgfc_fixed_deposits_6m;
            decimal avgfc_fixed_deposits_12m;
            try
            {

                GetInitialDatafrRestClientByMonth().GetAwaiter().GetResult();
                avgbanks_fixed_deposits_3m = lsData.Where((Data c) => c.end_of_month.CompareTo(fromMonth) >= 0 && c.end_of_month.CompareTo(toMonth) <= 0).Average((Data x) => Convert.ToDecimal(x.banks_fixed_deposits_3m));
                avgbanks_fixed_deposits_6m = lsData.Where((Data c) => c.end_of_month.CompareTo(fromMonth) >= 0 && c.end_of_month.CompareTo(toMonth) <= 0).Average((Data x) => Convert.ToDecimal(x.banks_fixed_deposits_6m));
                avgbanks_fixed_deposits_12m = lsData.Where((Data c) => c.end_of_month.CompareTo(fromMonth) >= 0 && c.end_of_month.CompareTo(toMonth) <= 0).Average((Data x) => Convert.ToDecimal(x.banks_fixed_deposits_12m));

                avgfc_fixed_deposits_3m = lsData.Where((Data c) => c.end_of_month.CompareTo(fromMonth) >= 0 && c.end_of_month.CompareTo(toMonth) <= 0).Average((Data x) => Convert.ToDecimal(x.fc_fixed_deposits_3m));
                avgfc_fixed_deposits_6m = lsData.Where((Data c) => c.end_of_month.CompareTo(fromMonth) >= 0 && c.end_of_month.CompareTo(toMonth) <= 0).Average((Data x) => Convert.ToDecimal(x.fc_fixed_deposits_6m));
                avgfc_fixed_deposits_12m = lsData.Where((Data c) => c.end_of_month.CompareTo(fromMonth) >= 0 && c.end_of_month.CompareTo(toMonth) <= 0).Average((Data x) => Convert.ToDecimal(x.fc_fixed_deposits_12m));



                lsDataTemp.Add(new Data()
                {
                    banks_fixed_deposits_12m = avgbanks_fixed_deposits_12m.ToString(),
                    banks_fixed_deposits_3m = avgbanks_fixed_deposits_3m.ToString(),
                    banks_fixed_deposits_6m = avgbanks_fixed_deposits_6m.ToString(),
                    fc_fixed_deposits_3m = avgfc_fixed_deposits_3m.ToString(),
                    fc_fixed_deposits_6m = avgfc_fixed_deposits_6m.ToString(),
                    fc_fixed_deposits_12m = avgfc_fixed_deposits_12m.ToString()

                }
                    );
                ;

            }
            catch (System.Exception ex)
            {
                throw;
            }
            return lsDataTemp;
        }

        public List<Data> GeDataByPeriodForComparison(string fromMonth, string toMonth)
        {
            IEnumerable<Data> objendOfMonth;
            try
            {
                GetInitialDatafrRestClientByMonth().GetAwaiter().GetResult();
                objendOfMonth = lsData.Where((Data c) => c.end_of_month.CompareTo(fromMonth) >= 0 && c.end_of_month.CompareTo(toMonth) <= 0).OrderByDescending((Data x) => x.fc_fixed_deposits_3m).ThenByDescending((Data x) => x.fc_fixed_deposits_6m).ThenByDescending((Data x) => x.fc_fixed_deposits_12m);
            }
            catch (System.Exception ex)
            {
                throw;
            }
            return objendOfMonth.ToList();
        }

        public List<Data> GeDataByPeriod(string endOfMonth)
        {
            IEnumerable<Data> objendOfMonth;
            try
            {
                GetInitialDatafrRestClientByMonth().GetAwaiter().GetResult();
                objendOfMonth = lsData.Where((Data c) => c.end_of_month == endOfMonth);

            }
            catch (System.Exception ex)
            {
                throw;
            }
            return objendOfMonth.ToList();
        }

        public List<Data> GetInitialDataBuildByMonth()
        {
            GetInitialDatafrRestClientByMonth().GetAwaiter().GetResult();
            return lsData;
        }



        private async Task<List<Data>> GetInitialDatafrRestClientByMonth()
        {
            try
            {
                HttpClient client = new HttpClient();
                string path = "https://eservices.mas.gov.sg/api/action/datastore/search.json?resource_id=5f2b18a8-0883-4769-a635-879c63d3caac&limit=1000";
                lsData.Clear();
                string jsonResponse = "";
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
                System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                HttpResponseMessage response = await client.GetAsync(ConfigurationManager.AppSettings["MomEndByMonths"]);
                if (response.IsSuccessStatusCode)
                {
                    jsonResponse = await response.Content.ReadAsStringAsync();
                    var jsonObj = JObject.Parse(jsonResponse);
                    dynamic DataList = JsonConvert.DeserializeObject(jsonResponse);
                    var data = DataList.result.records;
                    foreach (var detail in data)
                    {
                        lsData.Add(new Data()
                        {
                            end_of_month = detail["end_of_month"],
                            prime_lending_rate = detail["prime_lending_rate"],
                            banks_fixed_deposits_3m = detail["banks_fixed_deposits_3m"],
                            banks_fixed_deposits_6m = detail["banks_fixed_deposits_6m"],
                            banks_fixed_deposits_12m = detail["banks_fixed_deposits_12m"],
                            banks_savings_deposits = detail["banks_savings_deposits"],
                            fc_hire_purchase_motor_3y = detail["fc_hire_purchase_motor_3y"],
                            fc_housing_loans_15y = detail["fc_housing_loans_15y"],
                            fc_fixed_deposits_3m = detail["fc_fixed_deposits_3m"],
                            fc_fixed_deposits_6m = detail["fc_fixed_deposits_6m"],
                            fc_fixed_deposits_12m = detail["fc_fixed_deposits_12m"],
                            fc_savings_deposits = detail["fc_savings_deposits"]

                        });
                    }
                }

            }
            catch (System.Exception ex)
            {
                throw;
            }
            return lsData;
        }


    }
}
