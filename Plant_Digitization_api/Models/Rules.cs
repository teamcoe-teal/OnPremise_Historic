using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Plant_Digitization_api.Models
{
    public class Rules
    {
        public string QueryType { get; set; }
        public string id { get; set; }
        public string value1 { get; set; }
        public string value2 { get; set; }
        public string condition { get; set; }
        public string status { get; set; }
    }

}