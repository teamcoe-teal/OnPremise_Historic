using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using System.Web.Http.Cors;
using System.Security.Cryptography;
using System.Text;

namespace Plant_Digitization_api.Controllers
{
    public class MISReportController : ApiController
    {
        // GET: MISReport

        private string connectionstring = ConfigurationManager.ConnectionStrings["con"].ToString();

        ///MIS Group & User Allowcation
        [HttpPost]
        public IHttpActionResult Groupsusers_details(Models.mis_group R)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(R.CompanyCode,R.PlantCode,R.Line_Code);
            if (con_string == "0")
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Couldnot connect to database"));
            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    string response = string.Empty;
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SP_MIS_Groups_Users", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = R.QueryType;
                    cmd.Parameters.Add("@Unique_id", SqlDbType.NVarChar, 150).Value = R.Unique_id;
                    cmd.Parameters.Add("@GroupID", SqlDbType.NVarChar, 150).Value = R.GroupID;
                    cmd.Parameters.Add("@GroupName", SqlDbType.NVarChar, 150).Value = R.GroupName;
                    cmd.Parameters.Add("@GroupDescription", SqlDbType.NVarChar, 150).Value = R.GroupDescription;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = R.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = R.PlantCode;
                    cmd.Parameters.Add("@LineCode", SqlDbType.NVarChar, 150).Value = R.Line_Code;
                    SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                    SQLReturn.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(SQLReturn);
                    cmd.ExecuteNonQuery();
                    response = SQLReturn.Value.ToString().Trim();
                    return Ok(response);
                }
            }
        }

        [HttpPost]
        [CustomAuthenticationFilter]
        public IHttpActionResult Get_Users(Models.Setting U)
        {
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                con.Open();

                DataSet ds = new DataSet();
                //SqlCommand cmd = new SqlCommand("SELECT A.UserID,A.UserName,A.Email,A.IsSuperAdmin,A.IsAdmin,A.CompanyCode,B.RoleName,A.PlantCode,A.LastLoginDate,Y.Logo FROM Users AS A LEFT JOIN tbl_Role AS B ON A.RoleID=B.RoleID  LEFT JOIN tbl_Customer AS Y ON A.CompanyCode=Y.CompanyCode" +
                //   " WHERE A.UserName='" + lo.UserName + "' AND A.Password='" + EncryptPassword(lo.Password) + "'", con);
                SqlCommand cmd = new SqlCommand("SELECT userid,username,firstname,lastname from users where plantcode=@plantcode and companycode=@companycode", con);
                cmd.Parameters.AddWithValue("@plantcode", U.Parameter);
                cmd.Parameters.AddWithValue("@companycode", U.Parameter1);
                cmd.Parameters.AddWithValue("@linecode", U.Parameter2);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                if (ds.Tables.Count != 0)
                {
                    return Ok(ds.Tables[0]);
                }
                else
                {
                    return Ok(new string[0]);
                }

                con.Close();
            }
        }

        [HttpPost]
        public IHttpActionResult UserGroup_Allocation(Models.mis_group_allowcation P)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(P.CompanyCode,P.PlantCode,P.Line_Code);
            if (con_string == "0")
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Couldnot connect to database"));
            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    con.Open();
                    string response = string.Empty;
                    SqlCommand cmd = new SqlCommand("SP_MIS_UserGroup_Permission", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = P.QueryType;
                    cmd.Parameters.Add("@Userid", SqlDbType.NVarChar, 150).Value = P.UserID;
                    cmd.Parameters.Add("@Groupid", SqlDbType.NVarChar, 150).Value = P.GroupID;
                    cmd.Parameters.Add("@Companycode", SqlDbType.NVarChar, 150).Value = P.CompanyCode;
                    cmd.Parameters.Add("@Plantcode", SqlDbType.NVarChar, 150).Value = P.PlantCode;
                    cmd.Parameters.Add("@Linecode", SqlDbType.NVarChar, 150).Value = P.Line_Code;
                    SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                    SQLReturn.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(SQLReturn);
                    cmd.ExecuteNonQuery();
                    response = SQLReturn.Value.ToString().Trim();
                    return Ok(response);
                }
            }
        }

        public IHttpActionResult UserGroup_Deletion(Models.mis_group P)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(P.CompanyCode, P.PlantCode, P.Line_Code);
            if (con_string == "0")
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Couldnot connect to database"));
            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    con.Open();

                    string response = string.Empty;

                    SqlCommand cmd = new SqlCommand("SP_MIS_UserGroup_Permission", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = "Delete";
                    cmd.Parameters.Add("@Groupid", SqlDbType.NVarChar, 150).Value = P.GroupID;
                    cmd.Parameters.Add("@Companycode", SqlDbType.NVarChar, 150).Value = P.CompanyCode;
                    cmd.Parameters.Add("@Plantcode", SqlDbType.NVarChar, 150).Value = P.PlantCode;
                    cmd.Parameters.Add("@Linecode", SqlDbType.NVarChar, 150).Value = P.Line_Code;
                    SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                    SQLReturn.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(SQLReturn);
                    cmd.ExecuteNonQuery();
                    response = SQLReturn.Value.ToString().Trim();
                    return Ok(response);
                }
            }
        }


        ///Report Set & Group Allowcation
        [HttpPost]
        public IHttpActionResult Add_Reportset_details(Models.mis_report R)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(R.CompanyCode,R.PlantCode,R.Line_Code);
            if (con_string == "0")
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Couldnot connect to database"));
            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    string response = string.Empty;
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SP_MIS_Report", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = R.QueryType;
                    cmd.Parameters.Add("@ReportID", SqlDbType.NVarChar, 150).Value = R.ReportID;
                    cmd.Parameters.Add("@GroupID", SqlDbType.NVarChar, 150).Value = R.GroupID;
                    cmd.Parameters.Add("@Shift1", SqlDbType.NVarChar, 150).Value = R.Shift1;
                    cmd.Parameters.Add("@Shift2", SqlDbType.NVarChar, 150).Value = R.Shift2;
                    cmd.Parameters.Add("@Shift3", SqlDbType.NVarChar, 150).Value = R.Shift3;
                    cmd.Parameters.Add("@Day", SqlDbType.NVarChar, 150).Value = R.Day;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = R.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = R.PlantCode;
                    cmd.Parameters.Add("@LineCode", SqlDbType.NVarChar, 150).Value = R.Line_Code;
                    SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                    SQLReturn.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(SQLReturn);
                    cmd.ExecuteNonQuery();
                    response = SQLReturn.Value.ToString().Trim();
                    return Ok(response);
                }
            }
        }

        [HttpPost]
        public IHttpActionResult Report_Allocation(Models.mis_report_allowcation P)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(P.CompanyCode, P.PlantCode, P.Line_Code);
            if (con_string == "0")
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Couldnot connect to database"));
            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    con.Open();
                    string response = string.Empty;
                    SqlCommand cmd = new SqlCommand("SP_MIS_Report_Permission", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = P.QueryType;
                    cmd.Parameters.Add("@ReportID", SqlDbType.NVarChar, 150).Value = P.ReportID;
                    cmd.Parameters.Add("@GroupId", SqlDbType.NVarChar, 150).Value = P.GroupID;
                    cmd.Parameters.Add("@ReportName", SqlDbType.NVarChar, 150).Value = P.ReportName;
                    cmd.Parameters.Add("@Companycode", SqlDbType.NVarChar, 150).Value = P.CompanyCode;
                    cmd.Parameters.Add("@Plantcode", SqlDbType.NVarChar, 150).Value = P.PlantCode;
                    cmd.Parameters.Add("@LineCode", SqlDbType.NVarChar, 150).Value = P.Line_Code;
                    SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                    SQLReturn.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(SQLReturn);
                    cmd.ExecuteNonQuery();
                    response = SQLReturn.Value.ToString().Trim();
                    return Ok(response);
                }
            }
        }

        public IHttpActionResult ReportSet_Deletion(Models.mis_report P)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(P.CompanyCode, P.PlantCode, P.Line_Code);
            if (con_string == "0")
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Couldnot connect to database"));
           
            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    con.Open();

                    string response = string.Empty;

                    SqlCommand cmd = new SqlCommand("SP_MIS_Report_Permission", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = "Delete";
                    cmd.Parameters.Add("@ReportID", SqlDbType.NVarChar, 150).Value = P.ReportID;
                    cmd.Parameters.Add("@Companycode", SqlDbType.NVarChar, 150).Value = P.CompanyCode;
                    cmd.Parameters.Add("@Plantcode", SqlDbType.NVarChar, 150).Value = P.PlantCode;
                    cmd.Parameters.Add("@LineCode", SqlDbType.NVarChar, 150).Value = P.Line_Code;
                    SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                    SQLReturn.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(SQLReturn);
                    cmd.ExecuteNonQuery();
                    response = SQLReturn.Value.ToString().Trim();
                    return Ok(response);
                }
            }
        }

        


    }
}