using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Plant_Digitization_api.Models
{
    public class Alert
    {
        public string Machine_Code { get; set; }
        public string alertID { get; set; }
        public string Alert_Name { get; set; }
        public string P1_Variable { get; set; }
        public string P1_PG { get; set; }
        public string P1_Param { get; set; }
        public string P2_Variable { get; set; }
        public string P2_PG { get; set; }
        public string P2_Param { get; set; }
        public string Expression { get; set; }
        public Int32 Constant { get; set; }
        public float DurationForAlert { get; set; }
        public int Group_id3 { get; set; }
        public string MessageText { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string Line_Code { get; set; }
        public string QueryType { get; set; }
        public string Remarks { get; set; }
        public string Machine_Code1 { get; set; }
        public string Machine_Code2 { get; set; }
        public float DurationForRepetetion { get; set; }
        public int Group_id1 { get; set; }
        public int Group_id2 { get; set; }
        public int unique_id { get; set; }
    }
    public class Alert_History
    {
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string LineCode { get; set; }
        public string MachineCode { get; set; }
        public DateTime fromdate { get; set; }
        public DateTime todate { get; set; }
        public string AlertID { get; set; }
        public string Alerttext { get; set; }
        public DateTime AlertDateAndTime { get; set; }
        public string SentGroup { get; set; }
        public int InstanceCount { get; set; }
        public string CommentBy { get; set; }
        public int Counts { get; set; }

    }
    public class Alert_Comments
    {
        public string QueryType { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string LineCode { get; set; }
        public string MachineCode { get; set; }
        public DateTime fromdate { get; set; }
        public DateTime todate { get; set; }
        public string AlertID { get; set; }
        public string Comment { get; set; }
        public DateTime CommentDateAndTime { get; set; }
        public string UserName { get; set; }
        public string SentGroup { get; set; }
        public string InstanceStartTime { get; set; }
        public DateTime InstanceEndTime { get; set; }
       
        

    }
    public class Getalertid
    {
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string LineCode { get; set; }
        public string MachineCode { get; set; }
        public DateTime fromdate { get; set; }
        public DateTime todate { get; set; }
        public string AlertID { get; set; }
        



    }

}