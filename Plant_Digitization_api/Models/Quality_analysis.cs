using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Plant_Digitization_api.Models
{
    public class Rejection_CustomReport
    {
        public string line { get; set; }
        public string Machine { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string Variant { get; set; }
    }
    public class Rejection_MonthReport
    {
        public string line { get; set; }
        public string Machine { get; set; }
        public string Year { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
    }
    public class live_report
    {
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string linecode { get; set; }
        public string Machinecode { get; set; }
        public string Shift { get; set; }


        public string ok_parts { get; set; }
        public string not_parts { get; set; }
        public string rework_parts { get; set; }
        public string operator_name { get; set; }
        public string stime { get; set; }
        public string ctime { get; set; }
        public string last_rejection { get; set; }
        public string mins_ago { get; set; }
        public string components { get; set; }
        public string no_of_times { get; set; }
        public string target_qty { get; set; }
        public string continuous_rejection { get; set; }
        public string shift_id { get; set; }
        public string batch { get; set; }
        public string reason { get; set; }
        public DateTime curtime { get; set; }
        public string no_of_stoppage { get; set; }
    }
    public class quality_shift_wise
    {
        public string QueryType { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string line { get; set; }
        public string shift { get; set; }
        public string Date { get; set; }
        public string Machine { get; set; }

        public string TotalOkParts { get; set; }
        public string TotalNokParts { get; set; }
        public string TotalReworkParts { get; set; }
        public string Firstpassyeild { get; set; }
        public string Rework_Percentage { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public string target_qty { get; set; }
        public string Hour { get; set; }

        public string Report_type { get; set; }
    }

    public class specific_reason_hourly
    {
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string linecode { get; set; }
        public string Machinecode { get; set; }
        public string shift { get; set; }
        public string RejectReason { get; set; }
        public string Date { get; set; }
        public string Report_type { get; set; }
        public string subassembly { get; set; }
    }

    public class quality_day_wise
    {
        public string QueryType { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string line { get; set; }
        public string Date { get; set; }
        public string Machine { get; set; }

        public string TotalOkParts { get; set; }
        public string TotalNokParts { get; set; }
        public string TotalReworkParts { get; set; }
        public string Firstpassyeild { get; set; }
        public string Rework_Percentage { get; set; }
        public string target_qty { get; set; }
        public string ShiftID { get; set; }
        public string Starttime { get; set; }
        public string Endtime { get; set; }

    }
    public class specific_reason_daywise
    {
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string line { get; set; }
        public string Machine { get; set; }
        public string Reason { get; set; }
        public string Date { get; set; }
        public string subassembly { get; set; }
    }

    public class quality_month_wise
    {
        public string QueryType { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string line { get; set; }
        public string Date { get; set; }
        public string Machine { get; set; }
    }

    public class quality_year_wise
    {
        public string QueryType { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string line { get; set; }
        public string Date { get; set; }
        public string Machine { get; set; }
    }

    public class quality_custom_wise
    {
        public string QueryType { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string line { get; set; }
        public string FDate { get; set; }
        public string TDate { get; set; }
    }

    public class specific_reason_custom
    {
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string line { get; set; }
        public string Machine { get; set; }
        public string Reason { get; set; }
        public string FDate { get; set; }
        public string TDate { get; set; }
        public string subassembly { get; set; }
    }

    public class quality_week_wise
    {
        public string QueryType { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string line { get; set; }
        public string Date { get; set; }
        public string Machine { get; set; }
    }


    public class specific_timestamp
    {
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string line { get; set; }
        public string Machine { get; set; }
        public string Reason { get; set; }
        public string Date { get; set; }
        public string Shift_id { get; set; }
        public string Report_type { get; set; }
    }
}