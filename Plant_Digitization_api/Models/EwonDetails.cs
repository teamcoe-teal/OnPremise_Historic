using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Plant_Digitization_api.Models
{
    public class EwonDetails
    {
        public string linecode { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public int Unique_id { get; set; }
        public string device_id { get; set; }
        public string deviceip { get; set; }
        public string devicename { get; set; }
        public string t2maccount { get; set; }
        public string t2musername { get; set; }
        public string t2mpassword { get; set; }
        public string t2mdeveloperid { get; set; }
        public string t2mdeviceusername { get; set; }
        public string t2mdevicepassword { get; set; }
        public string QueryType { get; set; }

        public string status { get; set; }
        public string ewonurl { get; set; }
    }
    public class getewondetails{
        public string linecode { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public int Unique_id { get; set; }
        public string QueryType { get; set; }
    }
    
}