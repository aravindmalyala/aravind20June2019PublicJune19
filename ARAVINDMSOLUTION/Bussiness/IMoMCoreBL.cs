using System.Collections.Generic;
using AravindSolution.Model;

namespace ARAVINDMSOLUTION.Bussiness
{
    public interface IMoMCoreBL
    {
        /// <summary>
        ///   //this is for  I want to be able to specify dates in format(mmm-yyyy e.g.Jan-2017) to get the data for    that period.
        /// </summary>
        List<Data> GeDataByPeriod(string endOfMonth);
        /// <summary>
        /// I want to be able to compare the overall average of financial companies rates against    bank rates.
        /// </summary>
        /// <param name="fromMonth"></param>
        /// <param name="toMonth"></param>
        /// <returns></returns>
        List<Data> GeDataByPeriodForAverageComparison(string fromMonth, string toMonth);
        /// <summary>
        /// I want to be able to compare the financial companies rates against bank rates by    months, and see on which months financial companies have a higher rate.
        /// </summary>
        /// <param name="fromMonth"></param>
        /// <param name="toMonth"></param>
        /// <returns></returns>
        List<Data> GeDataByPeriodForComparison(string fromMonth, string toMonth);
        /// <summary>
        /// I want to know if interest rates slope are on an upward or downward trend during this period.
        /// </summary>
        /// <param name="fromMonth"></param>
        /// <param name="toMonth"></param>
        /// <returns></returns>
        List<Data> GeDataByPeriodForInterestRatesSlopeComparison(string fromMonth, string toMonth);
        /// <summary>
        ///  async method to call the data this is for  I want to be able to specify dates in format(mmm-yyyy e.g.Jan-2017) to get the data for    that period.
        /// </summary>
        /// <returns></returns>
        List<Data> GetInitialDataBuildByMonth();
    }
}