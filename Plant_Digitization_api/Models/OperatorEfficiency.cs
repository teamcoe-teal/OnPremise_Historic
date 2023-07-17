using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Plant_Digitization_api.Models
{
    public class OPEDaywisereport
    {
        public string linecode { get; set; }
        public string machinecode { get; set; }
        public string variantcode { get; set; }
        public string operatorid { get; set; }
        public string month { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
    }

    public class OPEweekwisereport
    {
        public string linecode { get; set; }
        public string machinecode { get; set; }
        public string variantcode { get; set; }
        public string operatorid { get; set; }
        public string month { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
    }

    public class OPEshiftwisereport
    {
        public string linecode { get; set; }
        public string machinecode { get; set; }
        public string variantcode { get; set; }
        public string operatorid { get; set; }
        public string month { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
    }

    public class OPEyearwisereport
    {
        public string linecode { get; set; }
        public string machinecode { get; set; }
        public string variantcode { get; set; }
        public string operatorid { get; set; }
        public string year { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
    }

    public class OPEcustomwisereport
    {
        public string linecode { get; set; }
        public string machinecode { get; set; }
        public string variantcode { get; set; }
        public string operatorid { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
    }
}