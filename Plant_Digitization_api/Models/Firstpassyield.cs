using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Plant_Digitization_api.Models
{
    public class Daywisereport
    {
        public string linecode { get; set; }
        public string Variantcode { get; set; }
        public string date { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
    }

    public class weekwisereport
    {
        public string line { get; set; }
        public string Variantcode { get; set; }
        public string date { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
    }

    public class shiftwisereport
    {
        public string line { get; set; }
        public string Variantcode { get; set; }
        public string date { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
    }

    public class yearwisereport
    {
        public string linecode { get; set; }
        public string Variantcode { get; set; }
        public string date { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
    }

    public class customwisereport
    {
        public string linecode { get; set; }
        public string Variantcode { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
    }
    public class hourwisereport
    {
        public string linecode { get; set; }
        public string Machinecode { get; set; }
        public string Fromtime { get; set; }
        public string Totime { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string Shift { get; set; }
    }

    public class FPY_Breakdown
    {
        public string line { get; set; }
        public string variant { get; set; }
        public string Fromdate { get; set; }
        public string Todate { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
    }

    public class FPY_weekBreakdown
    {
        public string line { get; set; }
        public string variant { get; set; }
        public DateTime Date { get; set; }
        public Int32 WeekNo { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
    }

    public class FPY_shiftBreakdown
    {
        public string line { get; set; }
        public string variant { get; set; }
        public string shift { get; set; }
        public DateTime Fromdate { get; set; }
        public DateTime Todate { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
    }
}