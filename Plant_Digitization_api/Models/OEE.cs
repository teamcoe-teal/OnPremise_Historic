using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Plant_Digitization_api.Models
{
    public class OEE_Daywisereport
    {
        public string line { get; set; }
        public string Machine { get; set; }
        public string Year { get; set; }
        public string  CompanyCode { get; set; }
        public string PlantCode { get; set; }
    }

    public class OEE_weekwisereport
    {
        public string line { get; set; }
        public string Machine { get; set; }
        public string Year { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
    }

    public class OEE_shiftwisereport
    {
        public string line { get; set; }
        public string Machine { get; set; }
        public string Year { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
    }

    public class OEE_yearwisereport
    {
        public string line { get; set; }
        public string Machine { get; set; }
        public string Year { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
    }

    public class OEE_Breakdown
    {
        public string line { get; set; }
        public string Machine { get; set; }
        public string Fromdate { get; set; }
        public string Todate { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
    }

    public class OEE_weekBreakdown
    {
        public string line { get; set; }
        public string Machine { get; set; }
        public DateTime Date { get; set; }
        public Int32 WeekNo { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
    }

    public class OEE_shiftBreakdown
    {
        public string line { get; set; }
        public string Machine { get; set; }
        public string shift { get; set; }
        public DateTime Fromdate { get; set; }
        public DateTime Todate { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
    }

    public class OEE_custom
    {
        public string line { get; set; }
        public string Machine { get; set; }
        public DateTime Fromdate { get; set; }
        public DateTime Todate { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string Year { get; set; }
    }
}