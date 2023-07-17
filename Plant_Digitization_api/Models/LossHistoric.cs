using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Plant_Digitization_api.Models
{
    public class LossHistoric
    {
    }

    public class Losses
    {
        public string Line { get; set; }
        public string Machine { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
    }

    public class Loss_custom
    {
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string line { get; set; }
        public string FDate { get; set; }
        public string TDate { get; set; }
        public string Machine { get; set; }
        public string Loss { get; set; }
    }

    public class Loss_days
    {
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string line { get; set; }
        public string Date { get; set; }
        public string Machine { get; set; }
        public string Loss { get; set; }

    }

    public class Loss_month
    {
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string line { get; set; }
        public string Date { get; set; }
        public string Machine { get; set; }
        public string Loss { get; set; }

    }

    public class Loss_shift
    {
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string line { get; set; }
        public string shift { get; set; }
        public string Date { get; set; }
        public string Machine { get; set; }
        public string Loss { get; set; }

    }

    public class Loss_year
    {
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string line { get; set; }
        public string Date { get; set; }
        public string Machine { get; set; }
        public string Loss { get; set; }

    }
    public class Occurence_chart
    {
        public string QueryType { get; set; }
        public string Line_Code { get; set; }
        public string Machine_Code { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string date { get; set; }
        public string month { get; set; }
        public string year { get; set; }
        public string shiftid { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
        public string report_type { get; set; }
        public string loss_category { get; set; }
    }


    public class Loss_de
    {
        public string ID { get; set; }
        public string Reason { get; set; }
        public string Remarks { get; set; }
        public string ETime { get; set; }
        public string lossid { get; set; }
        public string Line_Code { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string STime { get; set; }
        public string newstart { get; set; }
        public string newend { get; set; }
        public string querytype { get; set; }
    }

}