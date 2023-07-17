using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Plant_Digitization_api.Models
{
    public class ReportMail
    {
        public string name { get; set; }
        public string emailid { get; set; }
        public string status { get; set; }
        public string exception { get; set; }
        public string linecode { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public int Unique_id { get; set; }
        public string QueryType { get; set; }
    }
    
}