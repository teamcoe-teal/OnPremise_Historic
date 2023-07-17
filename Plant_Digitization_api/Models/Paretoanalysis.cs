using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Plant_Digitization_api.Models
{
    public class MTBF_CustomReport
    {
        public string line { get; set; }
        public string Machine { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string CompanyCode { get; set; }
        public string records { get; set; }
        public string PlantCode { get; set; }
        public string QueryType { get; set; }
    }

    public class MTBF_MonthwiseReport
    {
        public string line { get; set; }
        public string Machine { get; set; }
        public string Year { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string records { get; set; }
        public string QueryType { get; set; }
        public string Variant { get; set; }
    }

    public class MTBF_YearwiseReport
    {
        public string line { get; set; }
        public string Machine { get; set; }
        public string Year { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string records { get; set; }
        public string QueryType { get; set; }
    }

    public class MTTR_CustomReport
    {
        public string line { get; set; }
        public string Machine { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string records { get; set; }
    }

    public class MTTR_MonthwiseReport
    {
        public string line { get; set; }
        public string Machine { get; set; }
        public string Year { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string records { get; set; }
    }

    public class MTTR_YearwiseReport
    {
        public string line { get; set; }
        public string Machine { get; set; }
        public string Year { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string records { get; set; }
    }

    public class MTTR_TodayReport
    {
        public string line { get; set; }
        public string Machine { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string records { get; set; }
    }

    public class MTTR_YesterdayReport
    {
        public string line { get; set; }
        public string Machine { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string records { get; set; }
    }

    public class Pareto_CustomReport
    {
        public string line { get; set; }
        public string Machine { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string records { get; set; }
    }

    public class Pareto_MonthwiseReport
    {
        public string line { get; set; }
        public string Machine { get; set; }
        public string Year { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string records { get; set; }
    }

    public class Pareto_YearwiseReport
    {
        public string line { get; set; }
        public string Machine { get; set; }
        public string Year { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string records { get; set; }
    }


    public class MOA_CustomReport
    {
        public string line { get; set; }
        public string Machine { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
    }

    public class MOA_MonthwiseReport
    {
        public string line { get; set; }
        public string Machine { get; set; }
        public string Year { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
    }

    public class MOA_YearwiseReport
    {
        public string line { get; set; }
        public string Machine { get; set; }
        public string Year { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
    }

    public class Four_LineReport
    {
        public string line { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string QueryType { get; set; }
    }

    public class Popup_Machinewise_Historic
    {
        public string line { get; set; }
        public string Machine { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string records { get; set; }
        public string QueryType { get; set; }
        public string Variant { get; set; }
        public string loss_category { get; set; }
    }

    public class Popup_Machinewise_Historic_1
    {
        public string line { get; set; }
        public string Machine { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string records { get; set; }
        public string QueryType { get; set; }
        public string Variant { get; set; }
        public string loss_category { get; set; }
    }

    public class Check_NoDataAVailable
    {
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string line { get; set; }
        public string Machine { get; set; }
    }

    public class shiftwisedata
    {

        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string line { get; set; }
        public string Machine { get; set; }
        public string Year { get; set; }
        public string variant { get; set; }
    }



}