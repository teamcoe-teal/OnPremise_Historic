using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Plant_Digitization_api.Models
{
    public class Cycletime
    {
    }
    public class CycleTime_Live
    {
        public string QueryType { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string Line { get; set; }
        public string Machine { get; set; }
        public string records { get; set; }
    }

    public class CycleTime_Histogram
    {
        public string QueryType { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string Line { get; set; }
        public string Machine { get; set; }
        public string Date { get; set; }
        public string FDate { get; set; }
        public string TDate { get; set; }
        public string Variant { get; set; }
        public string Shift { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string Operation { get; set; }
    }

    public class CycleTime_Average
    {
        public string QueryType { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string Line { get; set; }
        public string Machine { get; set; }
        public string Date { get; set; }
        public string FDate { get; set; }
        public string TDate { get; set; }
        public string Variant { get; set; }
        public string Shift { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
    }


    public class CycleTime_Partwise
    {
        public string QueryType { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string Line { get; set; }
        public string Machine { get; set; }
        public string Date { get; set; }
        public string FTime { get; set; }
        public string TTime { get; set; }
        public string Variant { get; set; }
    }

    public class Cycletime_setting
    {
        public string QueryType { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string Line_Code { get; set; }
        public string Machine { get; set; }
        public string Variant { get; set; }
        public string Movement { get; set; }
        public string Type { get; set; }
        public string Cycle_time { get; set; }
        public string ID { get; set; }
        public string Status { get; set; }

        public string if_applicable { get; set; }
    }


}