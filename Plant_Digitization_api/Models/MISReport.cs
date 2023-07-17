using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Plant_Digitization_api.Models
{
    public class MISReport
    {
    }

    public class mis_group
    {
        public string GroupID { get; set; }
        public string GroupName { get; set; }
        public string GroupDescription { get; set; }
        public string QueryType { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string Line_Code { get; set; }
        public string Unique_id { get; set; }
    }

    public class mis_group_allowcation
    {
        public string GroupID { get; set; }
        public string UserID { get; set; }
        public string QueryType { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string Line_Code { get; set; }
    }

    public class mis_report
    {
        public string QueryType { get; set; }
        public string Unique_id { get; set; }
        public string GroupID { get; set; }
        public string ReportID { get; set; }
        public string Shift1 { get; set; }
        public string Shift2 { get; set; }
        public string Shift3 { get; set; }
        public string Day { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string Line_Code { get; set; }
    }

    public class mis_report_allowcation
    {
        public string ReportName { get; set; }
        public string GroupID { get; set; }
        public string ReportID { get; set; }
        public string QueryType { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string Line_Code { get; set; }
    }


}