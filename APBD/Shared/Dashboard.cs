using System;
using System.Collections.Generic;

namespace APBD.Shared
{

    public class DashboardSetchList
    {
        public List<DashboardSearch> results { get; set; }
    }

    public class DashboardSearch
    {
        public string ticker { get; set; } = "";
        public string name { get; set; } = "";
    }

    public class DashboardTickerResult
    {
        public DashboardTicker results { get; set; }
    }


    public class DashboardTicker
    {
        public string ticker { get; set; } = "";
        public string name { get; set; } = "";
        public string sic_description { get; set; } = "";
        public int total_employees { get; set; } = 0;
        public string locale { get; set; } = "";
        public string phone_number { get; set; } = "";
        public Icon branding { get; set; }
    }

    public class DashboardTickerList
    {
        public List<DashboardTicker> dashboardTickers { get; set; }
    }

    public class Icon
    {
        public string icon_url { get; set; } = "";
    }
    public class StockDateInformationDTO
    {
        public string ticker { get; set; } = "";
        public List<StockDateInformation> results { get; set; }
    }

    public class StockDateInformationForDashboardDTO
    {
        public string ticker { get; set; } = "";
        public List<StockDateInformation> day { get; set; }
        public List<StockDateInformation> threeMonths { get; set; }
        public List<StockDateInformation> sevenDaysStock { get; set; }

    }

    public class DashboardTrickerDTO
    {
        public DashboardTickerResult CompanyDetailsInformation { get; set; }
        public StockDateInformationForDashboardDTO CompanyStockInformaion { get; set; }
    }

    public class StockDateInformation
    {
        public long t { get; set; } = 0l;
        public double l { get; set; } = 0.0;
        public double h { get; set; } = 0.0;
        public double o { get; set; } = 0.0;
        public double c { get; set; } = 0.0;
    }

    public class SaveStockInformation
    {
        public string LastDay { get; set; } = "";
        public string LastSevenDay { get; set; } = "";
        public string LastThreeMonthsDay { get; set; } = "";
    }


    public class StockDataToShowInChart : StockDateInformation
    {
        public DateTime date { get; set; }
    }
}

