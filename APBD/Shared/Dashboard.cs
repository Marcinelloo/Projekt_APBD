using System;
using System.Collections.Generic;

namespace APBD.Shared
{
    public class Dashboard
    {
        public WatchItem WatchItem { get; set; }
        public ChartData CurrentDay { get; set; }
        public ChartData OneWeekAgo { get; set; }
        public ChartData OneMonthAgo { get; set; }
        public ChartData ThreeMonthAgo { get; set; }
    }

    public class ChartData
    {
        public string[] XValue { get; set; }
        public string[] YValue { get; set; }
    }

    public class DashboardSetchList
    {
        public List<DashboardSearch> results { get; set; }
    }

    public class DashboardSearch
    {
        public string ticker { get; set; }
        public string name { get; set; }
    }

    public class DashboardTickerResult
    {
        public DashboardTicker results { get; set; }
    }


    public class DashboardTicker
    {
        public string ticker { get; set; }
        public string name { get; set; }
        public string sic_description { get; set; }
        public int total_employees { get; set; }
        public string locale { get; set; }
        public string phone_number { get; set; }
        public Icon branding { get; set; }
    }

    public class DashboardTickerList
    {
        public List<DashboardTicker> dashboardTickers { get; set; }
    }

    public class Icon
    {
        public string icon_url { get; set; }
    }
    public class StockDateInformationDTO
    {
        public string ticker { get; set; }
        public List<StockDateInformation> results { get; set; }
    }

    public class StockDateInformationForDashboardDTO
    {
        public string ticker { get; set; }
        public List<StockDateInformation> day { get; set; }
        public List<StockDateInformation> threeMonths { get; set; }
        public List<StockDateInformation> sevenDaysStock { get; set; }

    }

    public class StockDateInformation
    {
        public long t { get; set; }
        public double l { get; set; }
        public double h { get; set; }
    }

}

