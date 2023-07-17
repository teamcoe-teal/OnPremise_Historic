using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Plant_Digitization_api.Models
{
    public class Plannedcycletime
    {
        public string QueryType { get; set; }
        public string LineCode { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string Machine { get; set; }
        public string Variant { get; set; }
        public string Date { get; set; }
        public string Shift { get; set; }

    }
    public class Diagnostics
    {
        public string QueryType { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string LineCode { get; set; }
        public string DeviceID { get; set; }
        public string Devicename { get; set; }
        public string DeviceRef { get; set; }
        public string Event { get; set; }
        public int Unique_ID { get; set; }
        public DateTime LogTime { get; set; }
        public DateTime fromdate { get; set; }
        public DateTime todate { get; set; }
    }

    public class Diagnostics_settings
    {
        public string QueryType { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string LineCode { get; set; }
        public string DeviceID { get; set; }
        public string make { get; set; }
        public string gateway { get; set; }
        public string DeviceRef { get; set; }
        public string ioserver { get; set; }
        public int Unique_ID { get; set; }
        public string partnumber { get; set; }
        public string installed { get; set; }
        public string connectedto { get; set; }
        public string macaddress { get; set; }
        public string remarks { get; set; }
    }


    public class Customer
    {
        public string QueryType { get; set; }
        public string Unique_id { get; set; }
        public string CompanyCode { get; set; }
        public string CompanyName { get; set; }
        public string DomainName { get; set; }
        public string ContactPerson_FirstName { get; set; }
        public string ContactPerson_LastName { get; set; }
        public string ContactPerson_Mobile_NO { get; set; }
        public string ContactPerson_Email { get; set; }
        public string LocationName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string state { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string Logo { get; set; }
        public string Manager { get; set; }
        public bool IsActive { get; set; }
    }

    public class VariableSetting
    {
        public string QueryType { get; set; }
        public string varname { get; set; }
        public string groupp { get; set; }
        public string propname { get; set; }
        public string value { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string LineCode { get; set; }
        public int Unique_id { get; set; }
        public string groupid { get; set; }
    }

    public class AlertResponse
    {

        public string QueryType { get; set; }
        public string AlertID { get; set; }
        public string AlertName { get; set; }
        public string Machine { get; set; }
        public string Message { get; set; }
        public string Comment { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string LineCode { get; set; }
        public int Unique_id { get; set; }
        public int Duration { get; set; }
        public int Count { get; set; }
        public Nullable<DateTime> StartTime { get; set; }
        public Nullable<DateTime> EndTime { get; set; }
        public string Last_Respond_status { get; set; }
        public Nullable<DateTime> Last_Respond_Time { get; set; }
        public string Last_Respond_Time_string { get; set; }
        public string ResponseSelect { get; set; }
        public string Status { get; set; }
        public string ResponseText { get; set; }
        public string Last_Responded_UserName { get; set; }
        public int groupid { get; set; }
    }

    public class GroupList
    {

        public string QueryType { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string LineCode { get; set; }
        public int Group { get; set; }
    }

    public class Function
    {
        public string QueryType { get; set; }
        public string Unique_id { get; set; }
        public string FunctionID { get; set; }
        public string FunctionName { get; set; }
        public string FunctionDescription { get; set; }
        public string ParentPlant { get; set; }
        public string IsActive { get; set; }
        public string Dept_id { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string Parameter2 { get; set; }


    }

    public class Plant
    {
        public string QueryType { get; set; }
        public string Unique_id { get; set; }
        public string PlantID { get; set; }
        public string PlantName { get; set; }
        public string PlantDescription { get; set; }
        public string PlantLocation { get; set; }
        public string TimeZone { get; set; }
        public string ParentOrganization { get; set; }
        public string IsActive { get; set; }
    }

    public class Operation
    {
        public string QueryType { get; set; }
        public string Unique_id { get; set; }
        public string OperationID { get; set; }
        public string OperationName { get; set; }
        public string OperationDescription { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string Line_Code { get; set; }
    }

    public class Setting
    {
        public string QueryType { get; set; }
        public string Parameter { get; set; }
        public string Parameter1 { get; set; }
        public string Parameter2 { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string LineCode { get; set; }
        public string Unique_id { get; set; }
        public string Parameter4 { get; set; }
        public string Parameter3 { get; set; }
        public string Parameter5 { get; set; }
        public string Parameter6 { get; set; }

    }

    public class Assets
    {
        public string QueryType { get; set; }
        public string Unique_id { get; set; }
        public string AssetID { get; set; }
        public string AssetName { get; set; }
        public string FunctionName { get; set; }
        public string AssetDescription { get; set; }
        public string ShiftID { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string ewonnumber { get; set; }
    }

    public class Products
    {
        public string QueryType { get; set; }
        public string M_ID { get; set; }
        public string Variant_Code { get; set; }
        public string VariantName { get; set; }
        public string VariantDescription { get; set; }
        public string OperationName { get; set; }
        public string Machine_Code { get; set; }
        public string RecipeName { get; set; }
        public string CycleTime { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string Line_Code { get; set; }
        public decimal Cost { get; set; }
        public decimal? Autocycletime { get; set; }
        public decimal? Manualcycletime { get; set; }
        public decimal? Idlecycletime { get; set; }
        public decimal? Nooffixtures { get; set; }

    }

    public class feedback
    {
        public string QueryType { get; set; }
        public string id { get; set; }
        public string UserName { get; set; }
        public string Feedback { get; set; }
        public string FB_Comments { get; set; }
        public DateTime FB_Date { get; set; }
        public string FB_Document { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string LineCode { get; set; }
    }

    public class holiday
    {
        public string QueryType { get; set; }
        public string Unique_id { get; set; }
        public string HolidayID { get; set; }
        public string HolidayReason { get; set; }
        public string PlantID { get; set; }
        public DateTime Date { get; set; }
        public string CompanyCode { get; set; }
        public string LineCode { get; set; }
    }

    public class User
    {
        public string VCode { get; set; }
        public string QueryType { get; set; }
        public string Unique_id { get; set; }
        public string UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string User_Function { get; set; }
        public string RoleID { get; set; }
        public string IsActive { get; set; }
        public string SkillSet { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string LineRoleID { get; set; }
    }

    public class Roles
    {
        public string QueryType { get; set; }
        public string Unique_id { get; set; }
        public string RoleID { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string Line_Code { get; set; }
    }
    public class UserGroups
    {
        public string QueryType { get; set; }
        public string Unique_id { get; set; }
        public string GroupID { get; set; }
        public string GroupName { get; set; }
        public string GroupDescription { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string Line_Code { get; set; }
    }
    public class Skills
    {
        public string QueryType { get; set; }
        public string Unique_id { get; set; }
        public string Skill_ID { get; set; }
        public string SkillName { get; set; }
        public string CompanyCode { get; set; }
        public string Line_Code { get; set; }
        public string Plant_Code { get; set; }
    }

    public class Operator_skill
    {
        public string QueryType { get; set; }
        public string O_ID { get; set; }
        public string OperatorID { get; set; }
        public string SkillName { get; set; }
        public string ScoreValue { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string Line_Code { get; set; }
    }

    public class AlarmSettings
    {
        public string QueryType { get; set; }
        public string ID { get; set; }
        public string A_ID { get; set; }
        public string Line_Code { get; set; }
        public string Machine_Code { get; set; }
        public string Alarm_ID { get; set; }
        public string Alarm_Description { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }

        public string Parameter4 { get; set; }
        public string Parameter5 { get; set; }
        public string Help { get; set; }
    }

    public class Rejections
    {
        public string QueryType { get; set; }
        public string R_ID { get; set; }
        public string RejectionCode { get; set; }
        public string RejectionName { get; set; }
        public string RejectionDescription { get; set; }
        public string ProductName { get; set; }
        public string OperationName { get; set; }
        public string AssetName { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string Line_Code { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string Machine { get; set; }
        public string variant { get; set; }
        public string year { get; set; }
        public string month { get; set; }

    }

    public class LossesSetting
    {
        public string QueryType { get; set; }
        public string ID { get; set; }
        public string Line_Code { get; set; }
        public string Machine_Code { get; set; }
        public string Loss_ID { get; set; }
        public string Loss_Description { get; set; }
        public string Loss_Category { get; set; }
        public string Loss_Type { get; set; }
        public string Subassembly { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
    }

    public class LossCategory
    {
        public string QueryType { get; set; }
        public string ID { get; set; }
        public string Line_Code { get; set; }
        public string Loss_Category { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
    }

    public class LossType
    {
        public string QueryType { get; set; }
        public string ID { get; set; }
        public string Line_Code { get; set; }
        public string Loss_Type { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
    }

    public class disetsetting
    {

        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string Flag { get; set; }
        public string QueryType { get; set; }
        public string LineCode { get; set; }
        public string MachineCode { get; set; }
        public int instance { get; set; }
        public DateTime date { get; set; }
        public string id { get; set; }
        public DateTime loaded { get; set; }
        public DateTime unloaded { get; set; }
        public int production { get; set; }
        public int cummulative { get; set; }
        public DateTime starttime { get; set; }
        public DateTime stoptime { get; set; }
        public string reason { get; set; }
        public string toolname { get; set; }
        public string toolid { get; set; }
        public string linename { get; set; }
    }

    public class Toollist
    {
        public string QueryType { get; set; }
        public string ID { get; set; }
        public string Line_Code { get; set; }
        public string ToolName { get; set; }
        public string ToolID { get; set; }
        public string UOM { get; set; }
        public string Make { get; set; }
        public string Classification { get; set; }
        public string Part_number { get; set; }
        public string RatedLifeCycle { get; set; }
        public string Machine_Code { get; set; }
        public string Conversion_parameter { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string Rectext { get; set; }
        public string SerialNo { get; set; }
        public string Subassembly { get; set; }
    }

    public class Operators
    {
        public string QueryType { get; set; }
        public string OP_ID { get; set; }
        public string OperatorName { get; set; }
        public string OperatorID { get; set; }
        public string AssetName { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string Line_Code { get; set; }
    }

    public class ShiftSettings
    {
        public string QueryType { get; set; }
        public string ID { get; set; }
        public string ShiftName { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public decimal BreakTime { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string Line_Code { get; set; }
    }
    public class BreakSettings
    {
        public string QueryType { get; set; }
        public string ID { get; set; }
        public string ShiftID { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string BreakReason { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string Line_Code { get; set; }
    }

    public class Dept
    {
        public string QueryType { get; set; }
        public string Unique_id { get; set; }
        public string Dept_id { get; set; }
        public string Dept_name { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string Area_id { get; set; }
    }

    public class URL_table
    {
        public string QueryType { get; set; }
        public string Unique_id { get; set; }
        public string URL { get; set; }
        public string LineCode { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
    }

    public class Area
    {
        public string QueryType { get; set; }
        public string Unique_id { get; set; }
        public string Area_id { get; set; }
        public string Area_name { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
    }
    public class Subassembly
    {
        public string QueryType { get; set; }
        public string Subassembly_id { get; set; }
        public string Subassembly_name { get; set; }
        public string LineCode { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string Unique_id { get; set; }
        public string MachineCode { get; set; }
    }
    public class Permission
    {
        public string QueryType { get; set; }
        public string Unique_id { get; set; }
        public string RoleID { get; set; }
        public string Module_name { get; set; }
        public string Edit_form { get; set; }
        public string View_form { get; set; }
        public string Add_form { get; set; }
        public string Delete_form { get; set; }
        public string Permission_id { get; set; }
        public string CompanyCode { get; set; }
    }
    public class Line_Permission
    {
        public string QueryType { get; set; }
        public string Line_Code { get; set; }
        public string Plant_Code { get; set; }
        public string RoleID { get; set; }
        public string Area_Code { get; set; }
        public string Dept_Code { get; set; }
        public string CompanyCode { get; set; }

    }
    public class Login
    {
        public string UserSessionId { get; set; }
        public string VCode { get; set; }
        public string UserName { get; set; }
        public string lastlogin { get; set; }
        public string Password { get; set; }
    }
    public class Loginres
    {
        public string token { get; set; }
        public string loginstatus { get; set; }
        public string lastlogindate { get; set; }
    }
    public class Forgot
    {
       public string Otp { get; set; }
        public string Input1 { get; set; }
        public string Input2 { get; set; }
    }

    public class Change_password
    {
        public string Input1 { get; set; }
        public string Input2 { get; set; }
        public string Input3 { get; set; }
    }
    public class production_setting
    {
        public string shiftid { get; set; }
        public string linecode { get; set; }
        public string productname { get; set; }
        public string targetproduction { get; set; }
        public DateTime fromdate { get; set; }
        public DateTime todate { get; set; }
        public string companycode { get; set; }
        public string plantcode { get; set; }
        public int id { get; set; }
        public string querytype { get; set; }
    }
    public class UserGroup_Allocation
    {
        public string QueryType { get; set; }
        public string CompanyCode { get; set; }
        public string Unique_id { get; set; }
        public string GroupID { get; set; }
        public string UserID { get; set; }
        public string PlantCode { get; set; }
        public string LineCode { get; set; }
        // public string Module_name { get; set; }

    }

    public class RawTables
    {
        public string QueryType { get; set; }
        public string Parameter { get; set; }
        public string Parameter1 { get; set; }
        public string Parameter2 { get; set; }
        public string Parameter3 { get; set; }
        public string Parameter4 { get; set; }
        public string Parameter5 { get; set; }
        public string Parameter6 { get; set; }
        public string Parameter7 { get; set; }
        public string Parameter8 { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
        public string duration { get; set; }

    }
    public class manual
    {
        public string QueryType { get; set; }
        public string id { get; set; }
        public string UserName { get; set; }
        public string filename { get; set; }
        public string remarks { get; set; }
        
        public string linecode { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
    }

    public class hourly_tra
    {
        public string Date { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string LineCode { get; set; }
        public string Machine { get; set; }
        public string Line_Code { get; set; }
    }

    public class cumulative_data
    {
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string LineCode { get; set; }
        public string Machine { get; set; }
        public string Line_Code { get; set; }

        public string Month { get; set; }
        public string QueryType { get; set; }
    }

    public class dataPostingStatus
    {
        public string Duration { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string LineCode { get; set; }
        public string MachineCode { get; set; }

    }

    public class popupHourly
    {
        public string Date { get; set; }
        public string CompanyCode { get; set; }
        public string PlantCode { get; set; }
        public string Machine { get; set; }
        public string Line_Code { get; set; }
        public string Shift { get; set; }
    }


}