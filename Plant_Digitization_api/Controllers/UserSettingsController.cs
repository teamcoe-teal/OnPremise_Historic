using Microsoft.Security.Application;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Text;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace Plant_Digitization_api.Controllers
{
    //[EnableCors(origins: "http://localhost:55974", headers: "*", methods: "*")]
   
    public class UserSettingsController : ApiController
    {
        //private const string Purpose = "Authentication";
        private string connectionstring = ConfigurationManager.ConnectionStrings["con"].ToString();
        private string username;
        private string password;

        public string RandomString(int size, bool lowerCase)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] uintBuffer = new byte[sizeof(uint)];

                while (size-- > 0)
                {
                    rng.GetBytes(uintBuffer);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    res.Append(valid[(int)(num % (uint)valid.Length)]);
                }
            }
            if (lowerCase)
                return res.ToString().ToLower();
            return res.ToString();
        }

    
        /// <summary>
        /// Generate Random Password
        /// </summary>
        /// <param name="size"></param>
        /// <param name="lowerCase"></param>
        /// <returns></returns>
        //public string RandomString(int size, bool lowerCase)
        //{

        //    StringBuilder builder = new StringBuilder();
        //    Random random = new Random();
        //    char ch;
        //    for (int i = 0; i < size; i++)
        //    {
        //        ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
        //        builder.Append(ch);
        //    }
        //    if (lowerCase)
        //        return builder.ToString().ToLower();
        //    return builder.ToString();
        //}

        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        public string RandomPassword(int size = 0)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(size, true));
            builder.Append(RandomNumber(10, 99));
            return builder.ToString();
        }
        /// <summary>
        /// report mail Details
        /// </summary>
        /// <param name="TO"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage Report_Mail_details(Models.ReportMail TO)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(TO.CompanyCode, TO.PlantCode, TO.linecode);
            if (con_string == "0")
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "Coudnot Connect to database", data = "" });

            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    var result = new List<Models.ReportMail>();
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SP_Report_Mail", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = TO.QueryType;
                    cmd.Parameters.Add("@Line_code", SqlDbType.NVarChar, 150).Value = TO.linecode;
                    cmd.Parameters.Add("@name", SqlDbType.NVarChar, 150).Value = "";
                    cmd.Parameters.Add("@emailid", SqlDbType.NVarChar, 150).Value = "";
                    cmd.Parameters.Add("@status", SqlDbType.NVarChar, 150).Value = "";
                    cmd.Parameters.Add("@exception", SqlDbType.NVarChar, 150).Value = "";
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = TO.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = TO.PlantCode;
                    cmd.Parameters.Add("@Unique_id", SqlDbType.Int).Value = TO.Unique_id;
                    cmd.ExecuteNonQuery();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        result.Add(item: new Models.ReportMail
                        {

                            linecode = Convert.ToString(reader["line_code"] == DBNull.Value ? "" : reader["line_code"]),
                            name = Convert.ToString(reader["Name"] == DBNull.Value ? "" : reader["Name"]),
                            emailid = Convert.ToString(reader["Email_ID"] == DBNull.Value ? "" : reader["Email_ID"]),
                            status = Convert.ToString(reader["Status"] == DBNull.Value ? "" : reader["Status"]),
                            exception = Convert.ToString(reader["exception"] == DBNull.Value ? "" : reader["exception"]),
                            Unique_id = Convert.ToInt32(reader["Unique_id"]),
                        });
                    }
                    if (result.Any())
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { status = "Success", msg = "Details Found", data = result });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "No Data Available", data = "" });
                    }
                }
            }
        }


        /// <summary>
        /// Insert & Update Department Details
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthenticationFilter]
        public IHttpActionResult edit_delete_mail_details(Models.ReportMail TO)
        {

            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(TO.CompanyCode, TO.PlantCode, TO.linecode);
            if (con_string == "0")
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Couldnot connect to database"));
            
            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    //var result = new List<Models.ReportMail>();
                    string response = string.Empty;
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SP_insert_update_Report_Mail", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = TO.QueryType;
                    cmd.Parameters.Add("@Line_code", SqlDbType.NVarChar, 150).Value = TO.linecode;
                    cmd.Parameters.Add("@name", SqlDbType.NVarChar, 150).Value = TO.name;
                    cmd.Parameters.Add("@emailid", SqlDbType.NVarChar, 150).Value = TO.emailid;
                    cmd.Parameters.Add("@status", SqlDbType.NVarChar, 150).Value = TO.status;
                    cmd.Parameters.Add("@exception", SqlDbType.NVarChar, 150).Value = TO.exception;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = TO.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = TO.PlantCode;
                    cmd.Parameters.Add("@Unique_id", SqlDbType.Int).Value = TO.Unique_id;
                    SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                    SQLReturn.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(SQLReturn);
                    cmd.ExecuteNonQuery();
                    response = SQLReturn.Value.ToString().Trim();
                    return Ok(response);
                    //cmd.ExecuteNonQuery();
                    //var reader = cmd.ExecuteReader();
                    //while (reader.Read())
                    //{
                    //    result.Add(item: new Models.ReportMail
                    //    {

                    //        linecode = Convert.ToString(reader["line_code"] == DBNull.Value ? "" : reader["line_code"]),
                    //        name = Convert.ToString(reader["Name"] == DBNull.Value ? "" : reader["Name"]),
                    //        emailid = Convert.ToString(reader["Email_ID"] == DBNull.Value ? "" : reader["Email_ID"]),
                    //        status = Convert.ToString(reader["Status"] == DBNull.Value ? "" : reader["Status"]),
                    //        Unique_id = Convert.ToInt32(reader["Unique_id"]),
                    //    });
                    //}
                    //if (result.Any())
                    //{
                    //    return Request.CreateResponse(HttpStatusCode.OK, new { status = "Success", msg = "Details Found", data = result });
                    //}
                    //else
                    //{
                    //    return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "No Data Available", data = "" });
                    //}
                }
            }
        }




        /// <summary>
        /// report mail Details
        /// </summary>
        /// <param name="TO"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage employee_line_details(Models.ReportMail TO)
        {
            
                using (SqlConnection con = new SqlConnection(connectionstring))
                {
                    var result = new List<Models.ReportMail>();
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SP_Line_add", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = TO.QueryType;
                    cmd.Parameters.Add("@Line_code", SqlDbType.NVarChar, 150).Value = TO.linecode;
                    cmd.Parameters.Add("@name", SqlDbType.NVarChar, 150).Value = "";
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = TO.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = TO.PlantCode;
                    cmd.Parameters.Add("@Unique_id", SqlDbType.Int).Value = TO.Unique_id;
                    cmd.ExecuteNonQuery();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        result.Add(item: new Models.ReportMail
                        {

                            linecode = Convert.ToString(reader["line_code"] == DBNull.Value ? "" : reader["line_code"]),
                            name = Convert.ToString(reader["UserName"] == DBNull.Value ? "" : reader["UserName"]),
                            Unique_id = Convert.ToInt32(reader["Unique_ID"]),
                        });
                    }
                    if (result.Any())
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { status = "Success", msg = "Details Found", data = result });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "No Data Available", data = "" });
                    }
                }

                
        }



        /// <summary>
        /// Insert & Update Department Details
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthenticationFilter]
        public IHttpActionResult edit_delete_linesetting_details(Models.ReportMail TO)
        {
            
                using (SqlConnection con = new SqlConnection(connectionstring))
            {
                //var result = new List<Models.ReportMail>();
                string response = string.Empty;
                con.Open();
                SqlCommand cmd = new SqlCommand("SP_insert_update_line_setting", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = TO.QueryType;
                cmd.Parameters.Add("@Line_code", SqlDbType.NVarChar, 150).Value = TO.linecode;
                cmd.Parameters.Add("@name", SqlDbType.NVarChar, 150).Value = TO.name;
                cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = TO.CompanyCode;
                cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = TO.PlantCode;
                cmd.Parameters.Add("@Unique_id", SqlDbType.Int).Value = TO.Unique_id;
                SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                SQLReturn.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(SQLReturn);
                cmd.ExecuteNonQuery();
                response = SQLReturn.Value.ToString().Trim();
                return Ok(response);
                //cmd.ExecuteNonQuery();
                //var reader = cmd.ExecuteReader();
                //while (reader.Read())
                //{
                //    result.Add(item: new Models.ReportMail
                //    {

                //        linecode = Convert.ToString(reader["line_code"] == DBNull.Value ? "" : reader["line_code"]),
                //        name = Convert.ToString(reader["Name"] == DBNull.Value ? "" : reader["Name"]),
                //        emailid = Convert.ToString(reader["Email_ID"] == DBNull.Value ? "" : reader["Email_ID"]),
                //        status = Convert.ToString(reader["Status"] == DBNull.Value ? "" : reader["Status"]),
                //        Unique_id = Convert.ToInt32(reader["Unique_id"]),
                //    });
                //}
                //if (result.Any())
                //{
                //    return Request.CreateResponse(HttpStatusCode.OK, new { status = "Success", msg = "Details Found", data = result });
                //}
                //else
                //{
                //    return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "No Data Available", data = "" });
                //}
            }
        }


        /// <summary>
        /// Variable setting Details
        /// </summary>
        /// <param name="TO"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage Variable_details(Models.VariableSetting TO)
        {
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                var result = new List<Models.VariableSetting>();
                con.Open();
                SqlCommand cmd = new SqlCommand("SP_Variable_add", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = TO.QueryType;
                cmd.Parameters.Add("@Line_code", SqlDbType.NVarChar, 150).Value = TO.LineCode;
                cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = TO.CompanyCode;
                cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = TO.PlantCode;
                cmd.Parameters.Add("@Unique_id", SqlDbType.Int).Value = TO.Unique_id;
                cmd.ExecuteNonQuery();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(item: new Models.VariableSetting
                    {
                        value = Convert.ToString(reader["VarValue"] == DBNull.Value ? "" : reader["VarValue"]),
                        varname = Convert.ToString(reader["Variable"] == DBNull.Value ? "" : reader["Variable"]),
                        propname = Convert.ToString(reader["ParameterName"] == DBNull.Value ? "" : reader["ParameterName"]),
                        groupp = Convert.ToString(reader["GroupName"] == DBNull.Value ? "" : reader["GroupName"]),
                        groupid = Convert.ToString(reader["PropertyGroup"] == DBNull.Value ? "" : reader["PropertyGroup"]),
                        Unique_id = Convert.ToInt32(reader["Unique_id"]),
                    });
                }
                if (result.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = "Success", msg = "Details Found", data = result });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "No Data Available", data = "" });
                }
            }
        }


        /// <summary>
        /// Responding to alert loading active alerts after selecting machine and alert
        /// </summary>
        /// <param name="TO"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage alert_respond_details(Models.AlertResponse param1)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(param1.CompanyCode, param1.PlantCode, param1.LineCode);
            if (con_string == "0")
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "Coudnot Connect to database", data = "" });

            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    var result = new List<Models.AlertResponse>();
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SP_list_alert_forResponse", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = param1.QueryType;
                    cmd.Parameters.Add("@Line_code", SqlDbType.NVarChar, 150).Value = param1.LineCode;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = param1.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = param1.PlantCode;
                    cmd.Parameters.Add("@machine", SqlDbType.NVarChar, 150).Value = param1.Machine;
                    cmd.Parameters.Add("@alertid", SqlDbType.NVarChar, 150).Value = param1.AlertID;
                    cmd.ExecuteNonQuery();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        result.Add(item: new Models.AlertResponse
                        {

                            AlertID = Convert.ToString(reader["AlertID"] == DBNull.Value ? "" : reader["AlertID"]),
                            Message = Convert.ToString(reader["AlertMessage"] == DBNull.Value ? "" : reader["AlertMessage"]),
                            StartTime = Convert.ToDateTime(reader["StartTime"] == DBNull.Value ? (DateTime?)null : reader["StartTime"]),
                            EndTime = Convert.ToDateTime(reader["LastTime"] == DBNull.Value ? (DateTime?)null : reader["LastTime"]),
                            Duration = Convert.ToInt32(reader["Duration"] == DBNull.Value ? 0 : reader["Duration"]),
                            Comment = Convert.ToString(reader["Comment"] == DBNull.Value ? "" : reader["Comment"]),
                            Count = Convert.ToInt32(reader["AlertCount"] == DBNull.Value ? 0 : reader["AlertCount"]),
                            Last_Respond_status = Convert.ToString(reader["LastRespondStatus"] == DBNull.Value ? "" : reader["LastRespondStatus"]),
                            ResponseSelect = Convert.ToString(reader["ResponseSelect"] == DBNull.Value ? "" : reader["ResponseSelect"]),
                            Last_Respond_Time = Convert.ToDateTime(reader["LastResponseDateTime"] == DBNull.Value ? null : reader["LastResponseDateTime"]),
                            Last_Respond_Time_string = Convert.ToString(reader["LastResponseDateTime"] == DBNull.Value ? "" : reader["LastResponseDateTime"]),
                            Last_Responded_UserName = Convert.ToString(reader["LastResponded"] == DBNull.Value ? "" : reader["LastResponded"]),
                            Machine = Convert.ToString(reader["Machine_Code"] == DBNull.Value ? "" : reader["Machine_Code"]),
                            ResponseText = Convert.ToString(reader["responsetext"] == DBNull.Value ? "" : reader["responsetext"]),
                            groupid = Convert.ToInt32(reader["GroupId"] == DBNull.Value ? 0 : reader["GroupId"])
                        });
                    }

                    if (result.Any())
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { status = "Success", msg = "Details Found", data = result });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "No Data Available", data = "" });
                    }
                }
            }
        }


        /// <summary>
        /// Update response Details in alert responding
        /// </summary>
        /// <param name="TO"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthenticationFilter]
        public IHttpActionResult edit_response_details(Models.AlertResponse TO)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(TO.CompanyCode, TO.PlantCode, TO.LineCode);
            if (con_string == "0")
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Couldnot connect to database"));

            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    //var result = new List<Models.ReportMail>();
                    string response = string.Empty;
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SP_update_responseIn_AlertSetting", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = TO.QueryType;
                    cmd.Parameters.Add("@Line_code", SqlDbType.NVarChar, 150).Value = TO.LineCode;
                    cmd.Parameters.Add("@status", SqlDbType.NVarChar, 150).Value = TO.ResponseSelect;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = TO.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = TO.PlantCode;
                    cmd.Parameters.Add("@id", SqlDbType.NVarChar, 150).Value = TO.AlertID;
                    cmd.Parameters.Add("@username", SqlDbType.NVarChar, 150).Value = TO.Last_Responded_UserName;
                    cmd.Parameters.Add("@respondtime", SqlDbType.DateTime).Value = DateTime.Now.ToLocalTime();
                    cmd.Parameters.Add("@laststatus", SqlDbType.NVarChar, 150).Value = TO.Last_Respond_status;
                    cmd.Parameters.Add("@starttime", SqlDbType.DateTime).Value = TO.StartTime;
                    SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                    SQLReturn.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(SQLReturn);
                    cmd.ExecuteNonQuery();
                    response = SQLReturn.Value.ToString().Trim();
                    return Ok(response);

                }
            }
        }

        /// <summary>
        /// Responding to alert loading comments for alerts
        /// </summary>
        /// <param name="param1"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage alert_comment_details(Models.AlertResponse param1)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(param1.CompanyCode, param1.PlantCode, param1.LineCode);
            if (con_string == "0")
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "Coudnot Connect to database", data = "" });

            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    var result = new List<Models.AlertResponse>();
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SP_list_alert_comments", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = param1.QueryType;
                    cmd.Parameters.Add("@Line_code", SqlDbType.NVarChar, 150).Value = param1.LineCode;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = param1.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = param1.PlantCode;
                    cmd.Parameters.Add("@machine", SqlDbType.NVarChar, 150).Value = param1.Machine;
                    cmd.Parameters.Add("@alertid", SqlDbType.NVarChar, 150).Value = param1.AlertID;
                    cmd.Parameters.Add("@start", SqlDbType.DateTime).Value = param1.StartTime;
                    cmd.ExecuteNonQuery();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        result.Add(item: new Models.AlertResponse
                        {
                            AlertName = Convert.ToString(reader["Alert_Name"] == DBNull.Value ? "" : reader["Alert_Name"]),
                            AlertID = Convert.ToString(reader["AlertID"] == DBNull.Value ? "" : reader["AlertID"]),
                            //Message = Convert.ToString(reader["AlertMessage"] == DBNull.Value ? "" : reader["AlertMessage"]),
                            StartTime = Convert.ToDateTime(reader["StartTime"] == DBNull.Value ? null : reader["StartTime"]),
                            EndTime = Convert.ToDateTime(reader["LastTime"] == DBNull.Value ? null : reader["LastTime"]),
                            //Duration = Convert.ToInt32(reader["Duration"] == DBNull.Value ? 0 : reader["Duration"]),
                            Comment = Convert.ToString(reader["Comment"] == DBNull.Value ? "" : reader["Comment"]),
                            //Count = Convert.ToInt32(reader["AlertCount"] == DBNull.Value ? 0 : reader["AlertCount"]),
                            //Last_Respond_status = Convert.ToString(reader["LastRespondStatus"] == DBNull.Value ? "" : reader["LastRespondStatus"]),
                            //ResponseSelect = Convert.ToString(reader["ResponseSelect"] == DBNull.Value ? "" : reader["ResponseSelect"]),
                            Last_Respond_Time = Convert.ToDateTime(reader["LastResponseDateTime"] == DBNull.Value ? null : reader["LastResponseDateTime"]),
                            Last_Responded_UserName = Convert.ToString(reader["LastResponded"] == DBNull.Value ? "" : reader["LastResponded"]),
                            //Machine = Convert.ToString(reader["Machine_Code"] == DBNull.Value ? "" : reader["Machine_Code"]),
                            //ResponseText = Convert.ToString(reader["responsetext"] == DBNull.Value ? "" : reader["responsetext"]),
                            Unique_id = Convert.ToInt32(reader["Unique_id"])
                        });
                    }
                    if (result.Any())
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { status = "Success", msg = "Details Found", data = result });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "No Data Available", data = "" });
                    }
                }
            }
        }

        /// <summary>
        /// loading group list
        /// </summary>
        /// <param name="param1"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthenticationFilter]
        public IHttpActionResult List_group_details(Models.GroupList param1)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(param1.CompanyCode, param1.PlantCode, param1.LineCode);
            if (con_string == "0")
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Couldnot connect to database"));

            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    DataTable dt = new DataTable();
                    DataTable dt1 = new DataTable();
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT distinct a.[UserID],c.GroupName " +
                        "FROM[dbo].[tbl_SMS_GroupPermission] a " +
                        "join[dbo].[tbl_SMS_Group] c on a.GroupID = c.GroupID " +
                        "where a.GroupID = @Group and a.CompanyCode = @CompanyCode " +
                        "and a.PlantCode = @PlantCode and a.Line_code = @Line_code", con);
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@Line_code", SqlDbType.NVarChar, 150).Value = param1.LineCode;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = param1.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = param1.PlantCode;
                    cmd.Parameters.Add("@group", SqlDbType.NVarChar, 150).Value = param1.Group;
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                    SqlCommand cmd1 = new SqlCommand();

                    SqlConnection conn = new SqlConnection(connectionstring);
                    conn.Open();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        cmd1 = new SqlCommand("SELECT distinct @userid as UserID,b.UserName,@groupname as GroupName from users b where b.UserID=@userid ", conn);
                        cmd1.CommandTimeout = 0;
                        cmd1.Parameters.Add("@userid", SqlDbType.NVarChar, 150).Value = dt.Rows[i][0];
                        cmd1.Parameters.Add("@groupname", SqlDbType.NVarChar, 150).Value = dt.Rows[i][1];
                        cmd1.ExecuteNonQuery();
                        SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                        da1.Fill(dt1);
                    }
                    conn.Close();
                    if (dt1.Rows.Count != 0)
                    {
                        return Ok(dt1);
                    }
                    else
                    {
                        return Ok(new string[0]);
                    }
                }
            }
        }

        /// <summary>
        /// Update response Details in alert responding
        /// </summary>
        /// <param name="param1"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthenticationFilter]
        public IHttpActionResult addComment(Models.AlertResponse param1)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(param1.CompanyCode, param1.PlantCode, param1.LineCode);
            if (con_string == "0")
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Couldnot connect to database"));

            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    //var result = new List<Models.ReportMail>();
                    string response = string.Empty;
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SP_insert_comments", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = param1.QueryType;
                    cmd.Parameters.Add("@username", SqlDbType.NVarChar, 150).Value = param1.Last_Responded_UserName;
                    cmd.Parameters.Add("@time", SqlDbType.DateTime).Value = DateTime.Now.ToString();
                    cmd.Parameters.Add("@comment", SqlDbType.NVarChar, 150).Value = param1.Comment;
                    cmd.Parameters.Add("@alertid", SqlDbType.NVarChar, 150).Value = param1.AlertID;
                    cmd.Parameters.Add("@starttime", SqlDbType.DateTime).Value = param1.StartTime;
                    cmd.Parameters.Add("@endtime", SqlDbType.DateTime).Value = param1.EndTime;
                    cmd.Parameters.Add("@uniqueid", SqlDbType.NVarChar, 150).Value = param1.Unique_id;
                    cmd.Parameters.Add("@company", SqlDbType.NVarChar, 150).Value = param1.CompanyCode;
                    cmd.Parameters.Add("@plant", SqlDbType.NVarChar, 150).Value = param1.PlantCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = param1.LineCode;
                    cmd.Parameters.Add("@machine", SqlDbType.NVarChar, 150).Value = param1.Machine;
                    cmd.Parameters.Add("@group", SqlDbType.Int).Value = param1.groupid;
                    SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                    SQLReturn.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(SQLReturn);
                    cmd.ExecuteNonQuery();
                    response = SQLReturn.Value.ToString().Trim();
                    return Ok(response);

                }
            }
        }


        /// <summary>
        /// Insert & Update Variable Details
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        /// 
        //not used
        [HttpPost]
        [CustomAuthenticationFilter]
        public IHttpActionResult edit_delete_variablesetting_details(Models.VariableSetting TO)
        {
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                //var result = new List<Models.ReportMail>();
                string response = string.Empty;
                con.Open();
                SqlCommand cmd = new SqlCommand("SP_insert_update_Variable_setting", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = TO.QueryType;
                cmd.Parameters.Add("@Line_code", SqlDbType.NVarChar, 150).Value = TO.LineCode;
                cmd.Parameters.Add("@varname", SqlDbType.NVarChar, 150).Value = TO.varname;
                cmd.Parameters.Add("@group", SqlDbType.NVarChar, 150).Value = TO.groupp;
                cmd.Parameters.Add("@propname", SqlDbType.NVarChar, 150).Value = TO.propname;
                cmd.Parameters.Add("@value", SqlDbType.NVarChar, 150).Value = TO.value;
                cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = TO.CompanyCode;
                cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = TO.PlantCode;
                cmd.Parameters.Add("@Unique_id", SqlDbType.Int).Value = TO.Unique_id;
                SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                SQLReturn.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(SQLReturn);
                cmd.ExecuteNonQuery();
                response = SQLReturn.Value.ToString().Trim();
                return Ok(response);
                //cmd.ExecuteNonQuery();
                //var reader = cmd.ExecuteReader();
                //while (reader.Read())
                //{
                //    result.Add(item: new Models.ReportMail
                //    {

                //        linecode = Convert.ToString(reader["line_code"] == DBNull.Value ? "" : reader["line_code"]),
                //        name = Convert.ToString(reader["Name"] == DBNull.Value ? "" : reader["Name"]),
                //        emailid = Convert.ToString(reader["Email_ID"] == DBNull.Value ? "" : reader["Email_ID"]),
                //        status = Convert.ToString(reader["Status"] == DBNull.Value ? "" : reader["Status"]),
                //        Unique_id = Convert.ToInt32(reader["Unique_id"]),
                //    });
                //}
                //if (result.Any())
                //{
                //    return Request.CreateResponse(HttpStatusCode.OK, new { status = "Success", msg = "Details Found", data = result });
                //}
                //else
                //{
                //    return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "No Data Available", data = "" });
                //}
            }
        }



        /// <summary>
        /// Disetlist Details
        /// </summary>
        /// <param name="TO"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage diset_details(Models.disetsetting TO)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(TO.CompanyCode, TO.PlantCode, TO.LineCode);
            if (con_string == "0")
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "Coudnot Connect to database", data = "" });

            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    var result = new List<Models.disetsetting>();
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SP_Dieset_history", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    //cmd.Parameters.Add("@Line_code", SqlDbType.NVarChar, 150).Value = "";
                    cmd.Parameters.Add("@ToolId", SqlDbType.NVarChar, 150).Value = TO.toolname;
                    //cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = TO.CompanyCode;
                    //cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = TO.PlantCode;
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        result.Add(item: new Models.disetsetting
                        {

                            toolid = Convert.ToString(reader["ToolID"] == DBNull.Value ? "" : reader["ToolID"]),
                            toolname = Convert.ToString(reader["ToolName"] == DBNull.Value ? "" : reader["ToolName"]),
                            MachineCode = Convert.ToString(reader["Machine_code"] == DBNull.Value ? "" : reader["Machine_code"]),
                            linename = Convert.ToString(reader["AssetName"] == DBNull.Value ? "" : reader["AssetName"]),
                            instance = Convert.ToInt32(reader["Instance"] == DBNull.Value ? 0 : reader["Instance"]),
                            starttime = Convert.ToDateTime(reader["Dieset_Loaded_Time"] == DBNull.Value ? null : reader["Dieset_Loaded_Time"]),
                            stoptime = Convert.ToDateTime(reader["Dieset_Unloaded_Time"] == DBNull.Value ? null : reader["Dieset_Unloaded_Time"]),
                            production = Convert.ToInt32(reader["Production_Qty"] == DBNull.Value ? 0 : reader["Production_Qty"]),
                            cummulative = Convert.ToInt32(reader["Cummulative_Qty"] == DBNull.Value ? 0 : reader["Cummulative_Qty"])

                        });
                    }
                    if (result.Any())
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { status = "Success", msg = "Details Found", data = result });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "No Data Available", data = "" });
                    }
                }
            }
        }

        /// <summary>
        /// Disetlist start stop Details
        /// </summary>
        /// <param name="TO"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage diset_startstop_details(Models.disetsetting TO)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(TO.CompanyCode, TO.PlantCode, TO.LineCode);
            if (con_string == "0")
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "Coudnot Connect to database", data = "" });

            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    var result = new List<Models.disetsetting>();
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SP_Dieset_startstop", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@toolinstance", SqlDbType.NVarChar, 150).Value = TO.LineCode;
                    cmd.Parameters.Add("@ToolId", SqlDbType.NVarChar, 150).Value = TO.toolname;
                    //cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = TO.CompanyCode;
                    //cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = TO.PlantCode;
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        result.Add(item: new Models.disetsetting
                        {

                            toolid = Convert.ToString(reader["ToolID"] == DBNull.Value ? "" : reader["ToolID"]),
                            toolname = Convert.ToString(reader["ToolName"] == DBNull.Value ? "" : reader["ToolName"]),
                            MachineCode = Convert.ToString(reader["Machine_code"] == DBNull.Value ? "" : reader["Machine_code"]),
                            linename = Convert.ToString(reader["AssetName"] == DBNull.Value ? "" : reader["AssetName"]),
                            instance = Convert.ToInt32(reader["Instance"] == DBNull.Value ? 0 : reader["Instance"]),
                            starttime = Convert.ToDateTime(reader["Start_Time"] == DBNull.Value ? null : reader["Start_Time"]),
                            stoptime = Convert.ToDateTime(reader["EndTime"] == DBNull.Value ? null : reader["EndTime"]),
                            production = Convert.ToInt32(reader["Production_Qty"] == DBNull.Value ? 0 : reader["Production_Qty"]),
                            cummulative = Convert.ToInt32(reader["Cummulative_Qty"] == DBNull.Value ? 0 : reader["Cummulative_Qty"])
                        });
                    }
                    if (result.Any())
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { status = "Success", msg = "Details Found", data = result });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "No Data Available", data = "" });
                    }
                }
            }
        }



        /// <summary>
        /// Insert and Update Customer Details
        /// </summary>
        /// <param name="C"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Customer_details(Models.Customer C)
        {
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                string response = string.Empty;
                string ran_pass = RandomPassword(6);
                if (C.QueryType == "Insert")
                {
                    string response1 = string.Empty;
                    con.Open();
                    SqlCommand cmd1 = new SqlCommand("SP_Customer_details", con);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = C.QueryType;
                    cmd1.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = C.CompanyCode;
                    cmd1.Parameters.Add("@CompanyName", SqlDbType.NVarChar, 150).Value = C.CompanyName;
                    cmd1.Parameters.Add("@DomainName", SqlDbType.NVarChar, 150).Value = C.DomainName;
                    cmd1.Parameters.Add("@ContactPerson_FirstName", SqlDbType.NVarChar, 150).Value = C.ContactPerson_FirstName;
                    cmd1.Parameters.Add("@ContactPerson_LastName", SqlDbType.NVarChar, 150).Value = C.ContactPerson_LastName;
                    cmd1.Parameters.Add("@ContactPerson_Mobile_NO", SqlDbType.BigInt).Value = C.ContactPerson_Mobile_NO;
                    cmd1.Parameters.Add("@ContactPerson_Email", SqlDbType.NVarChar, 150).Value = C.ContactPerson_Email;
                    cmd1.Parameters.Add("@LocationName", SqlDbType.NVarChar, 150).Value = C.LocationName;
                    cmd1.Parameters.Add("@Address", SqlDbType.NVarChar, 150).Value = C.Address;
                    cmd1.Parameters.Add("@City", SqlDbType.NVarChar, 150).Value = C.City;
                    cmd1.Parameters.Add("@state", SqlDbType.NVarChar, 150).Value = C.state;
                    cmd1.Parameters.Add("@Country", SqlDbType.NVarChar, 150).Value = C.Country;
                    cmd1.Parameters.Add("@PostalCode", SqlDbType.NVarChar, 150).Value = C.PostalCode;
                    cmd1.Parameters.Add("@Logo", SqlDbType.NVarChar, 150).Value = C.Logo;
                    cmd1.Parameters.Add("@Manager", SqlDbType.NVarChar, 150).Value = C.Manager;
                  
                    SqlParameter SQLReturn1 = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                    SQLReturn1.Direction = ParameterDirection.Output;
                    cmd1.Parameters.Add(SQLReturn1);
                    cmd1.ExecuteNonQuery();
                    response = SQLReturn1.Value.ToString().Trim();
                    if (response == "Inserted")
                    {
                        MailMessage mail = new MailMessage();
                        DataTable dt = new DataTable();
                        SqlCommand cmd_mail = new SqlCommand("SELECT * FROM tbl_gmail_settings", con);
                        SqlDataAdapter da = new SqlDataAdapter(cmd_mail);
                        da.Fill(dt);
                        mail.To.Add(C.ContactPerson_Email);
                        mail.From = new MailAddress(dt.Rows[0]["Smtp_user"].ToString());
                        mail.Subject = "Plant Digitization Portal Login Details";
                        mail.Body = string.Format("<b> Login Details :  </b> ");
                        mail.Body += string.Format("<br/><b>User Name :  </b> {0} ", C.ContactPerson_FirstName);
                        mail.Body += string.Format("<br/><b>Password :  </b> {0} ", ran_pass);
                        mail.IsBodyHtml = true;
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = dt.Rows[0]["Smtp_host"].ToString();
                        smtp.Port = Convert.ToInt32(dt.Rows[0]["Smtp_port"].ToString());
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = new System.Net.NetworkCredential(dt.Rows[0]["Smtp_user"].ToString(), dt.Rows[0]["Smtp_pass"].ToString());
                        smtp.EnableSsl = true;
                        smtp.Send(mail);

                        //SqlCommand cmd = new SqlCommand("SP_Customer_details", con);
                        //cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.CommandTimeout = 0;
                        //cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = C.QueryType;
                        //cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = C.CompanyCode;
                        //cmd.Parameters.Add("@CompanyName", SqlDbType.NVarChar, 150).Value = C.CompanyName;
                        //cmd.Parameters.Add("@DomainName", SqlDbType.NVarChar, 150).Value = C.DomainName;
                        //cmd.Parameters.Add("@ContactPerson_FirstName", SqlDbType.NVarChar, 150).Value = C.ContactPerson_FirstName;
                        //cmd.Parameters.Add("@ContactPerson_LastName", SqlDbType.NVarChar, 150).Value = C.ContactPerson_LastName;
                        //cmd.Parameters.Add("@ContactPerson_Mobile_NO", SqlDbType.NVarChar, 150).Value = C.ContactPerson_Mobile_NO;
                        //cmd.Parameters.Add("@ContactPerson_Email", SqlDbType.NVarChar, 150).Value = C.ContactPerson_Email;
                        //cmd.Parameters.Add("@LocationName", SqlDbType.NVarChar, 150).Value = C.LocationName;
                        //cmd.Parameters.Add("@Address", SqlDbType.NVarChar, 150).Value = C.Address;
                        //cmd.Parameters.Add("@City", SqlDbType.NVarChar, 150).Value = C.City;
                        //cmd.Parameters.Add("@state", SqlDbType.NVarChar, 150).Value = C.state;
                        //cmd.Parameters.Add("@Country", SqlDbType.NVarChar, 150).Value = C.Country;
                        //cmd.Parameters.Add("@PostalCode", SqlDbType.NVarChar, 150).Value = C.PostalCode;
                        //cmd.Parameters.Add("@Logo", SqlDbType.NVarChar, 150).Value = C.Logo;
                        //cmd.Parameters.Add("@Manager", SqlDbType.NVarChar, 150).Value = C.Manager;
                        //SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                        //SQLReturn.Direction = ParameterDirection.Output;
                        //cmd.Parameters.Add(SQLReturn);
                        //cmd.ExecuteNonQuery();
                        //response = SQLReturn.Value.ToString().Trim();
                    }
                }
                else
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SP_Customer_details", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = C.QueryType;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = C.CompanyCode;
                    cmd.Parameters.Add("@CompanyName", SqlDbType.NVarChar, 150).Value = C.CompanyName;
                    cmd.Parameters.Add("@DomainName", SqlDbType.NVarChar, 150).Value = C.DomainName;
                    cmd.Parameters.Add("@ContactPerson_FirstName", SqlDbType.NVarChar, 150).Value = C.ContactPerson_FirstName;
                    cmd.Parameters.Add("@ContactPerson_LastName", SqlDbType.NVarChar, 150).Value = C.ContactPerson_LastName;
                    cmd.Parameters.Add("@ContactPerson_Mobile_NO", SqlDbType.NVarChar, 150).Value = C.ContactPerson_Mobile_NO;
                    cmd.Parameters.Add("@ContactPerson_Email", SqlDbType.NVarChar, 150).Value = C.ContactPerson_Email;
                    cmd.Parameters.Add("@LocationName", SqlDbType.NVarChar, 150).Value = C.LocationName;
                    cmd.Parameters.Add("@Address", SqlDbType.NVarChar, 150).Value = C.Address;
                    cmd.Parameters.Add("@City", SqlDbType.NVarChar, 150).Value = C.City;
                    cmd.Parameters.Add("@state", SqlDbType.NVarChar, 150).Value = C.state;
                    cmd.Parameters.Add("@Country", SqlDbType.NVarChar, 150).Value = C.Country;
                    cmd.Parameters.Add("@PostalCode", SqlDbType.NVarChar, 150).Value = C.PostalCode;
                    cmd.Parameters.Add("@Logo", SqlDbType.NVarChar, 150).Value = C.Logo;
                    cmd.Parameters.Add("@Manager", SqlDbType.NVarChar, 150).Value = C.Manager;
                    SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                    SQLReturn.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(SQLReturn);
                    cmd.ExecuteNonQuery();
                    response = SQLReturn.Value.ToString().Trim();
                }
                return Ok(response);
            }
        }

        /// <summary>
        /// Inssert & Update Function Details
        /// </summary>
        /// <param name="S"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Function_details(Models.Function S)
        {
            string response = string.Empty;
            using (SqlConnection con = new SqlConnection(connectionstring))
            {

                con.Open();
                SqlCommand cmd = new SqlCommand("SP_Function", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = S.QueryType;
                cmd.Parameters.Add("@Unique_id", SqlDbType.NVarChar, 150).Value = S.Unique_id;
                cmd.Parameters.Add("@FunctionID", SqlDbType.NVarChar, 150).Value = S.FunctionID;
                cmd.Parameters.Add("@FunctionName", SqlDbType.NVarChar, 150).Value = S.FunctionName;
                cmd.Parameters.Add("@FunctionDescription", SqlDbType.NVarChar, 150).Value = S.FunctionDescription;
                cmd.Parameters.Add("@ParentPlant", SqlDbType.NVarChar, 150).Value = S.ParentPlant;
                cmd.Parameters.Add("@IsActive", SqlDbType.NVarChar, 150).Value = S.IsActive;
                cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = S.CompanyCode;
                cmd.Parameters.Add("@Dept_id", SqlDbType.NVarChar, 150).Value = S.Dept_id;
                SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                SQLReturn.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(SQLReturn);
                cmd.ExecuteNonQuery();
                response = SQLReturn.Value.ToString().Trim();
                //return Ok(response);
                if (response.ToString() == "Inserted" || response.ToString() == "Updated")
                {
                    database_connection d = new database_connection();
                    string con_string = d.Getconnectionstring(S.CompanyCode, S.PlantCode, S.Parameter2);
                    if (con_string == "0")
                    {
                        return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Couldnot connect to database"));

                    }
                    else
                    {
                        using (SqlConnection conn = new SqlConnection(con_string))
                        {
                            //string response1 = string.Empty;
                            conn.Open();
                            SqlCommand cmdd = new SqlCommand("SP_Function", conn);
                            cmdd.CommandType = CommandType.StoredProcedure;
                            cmdd.CommandTimeout = 0;
                            cmdd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = S.QueryType;
                            cmdd.Parameters.Add("@Unique_id", SqlDbType.NVarChar, 150).Value = S.Unique_id;
                            cmdd.Parameters.Add("@FunctionID", SqlDbType.NVarChar, 150).Value = S.FunctionID;
                            cmdd.Parameters.Add("@FunctionName", SqlDbType.NVarChar, 150).Value = S.FunctionName;
                            cmdd.Parameters.Add("@FunctionDescription", SqlDbType.NVarChar, 150).Value = S.FunctionDescription;
                            cmdd.Parameters.Add("@ParentPlant", SqlDbType.NVarChar, 150).Value = S.ParentPlant;
                            cmdd.Parameters.Add("@IsActive", SqlDbType.NVarChar, 150).Value = S.IsActive;
                            cmdd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = S.CompanyCode;
                            cmdd.Parameters.Add("@Dept_id", SqlDbType.NVarChar, 150).Value = S.Dept_id;
                            SqlParameter SQLReturn1 = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                            SQLReturn1.Direction = ParameterDirection.Output;
                            cmdd.Parameters.Add(SQLReturn1);
                            cmdd.ExecuteNonQuery();
                            response = SQLReturn1.Value.ToString().Trim();
                            return Ok(response);
                        }
                    }
                }
                else
                {
                    return Ok(response);
                }

            }

        }


        /// <summary>
        /// Insert & Update Plant Details
        /// </summary>
        /// <param name="P"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Plant_details(Models.Plant P)
        {
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                string response = string.Empty;
                con.Open();
                SqlCommand cmd = new SqlCommand("SP_Plant", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = P.QueryType;
                cmd.Parameters.Add("@Unique_id", SqlDbType.NVarChar, 150).Value = P.Unique_id;
                cmd.Parameters.Add("@PlantID", SqlDbType.NVarChar, 150).Value = P.PlantID;
                cmd.Parameters.Add("@PlantName", SqlDbType.NVarChar, 150).Value = P.PlantName;
                cmd.Parameters.Add("@PlantDescription", SqlDbType.NVarChar, 150).Value = P.PlantDescription;
                cmd.Parameters.Add("@PlantLocation", SqlDbType.NVarChar, 150).Value = P.PlantLocation;
                cmd.Parameters.Add("@TimeZone", SqlDbType.NVarChar, 150).Value = P.TimeZone;
                cmd.Parameters.Add("@ParentOrganization", SqlDbType.NVarChar, 150).Value = P.ParentOrganization;
                cmd.Parameters.Add("@IsActive", SqlDbType.NVarChar, 150).Value = P.IsActive;
                SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                SQLReturn.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(SQLReturn);
                cmd.ExecuteNonQuery();
                response = SQLReturn.Value.ToString().Trim();
                return Ok(response);
            }
        }

        /// <summary>
        /// Insert & Update Operation Details
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Operation_details(Models.Operation setting)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(setting.CompanyCode, setting.PlantCode, setting.Line_Code);
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
                    SqlCommand cmd = new SqlCommand("SP_Operation_details", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = setting.QueryType;
                    cmd.Parameters.Add("@Unique_id", SqlDbType.NVarChar, 150).Value = setting.Unique_id;
                    cmd.Parameters.Add("@OperationID", SqlDbType.NVarChar, 150).Value = setting.OperationID;
                    cmd.Parameters.Add("@OperationName", SqlDbType.NVarChar, 150).Value = setting.OperationName;
                    cmd.Parameters.Add("@OperationDescription", SqlDbType.NVarChar, 150).Value = setting.OperationDescription;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = setting.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = setting.PlantCode;
                    cmd.Parameters.Add("@LineCode", SqlDbType.NVarChar, 150).Value = setting.Line_Code;
                    SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                    SQLReturn.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(SQLReturn);
                    cmd.ExecuteNonQuery();
                    response = SQLReturn.Value.ToString().Trim();
                    return Ok(response);
                }
            }
        }

        /// <summary>
        /// Get the Settings Data
        /// </summary>
        /// <param name="U"></param>
        /// <returns></returns>
        [HttpPost]
        [Obsolete]
        [CustomAuthenticationFilter]
        public HttpResponseMessage User_settings_details(Models.Setting U)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionstring))
                {
                    string response = string.Empty;
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("SP_GetSettings_data", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = U.QueryType;
                    cmd.Parameters.Add("@Parameter", SqlDbType.NVarChar, 150).Value = U.Parameter;
                    cmd.Parameters.Add("@Parameter1", SqlDbType.NVarChar, 150).Value = U.Parameter1;
                    cmd.Parameters.Add("@Parameter2", SqlDbType.NVarChar, 150).Value = U.Parameter2;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds.Tables.Count != 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { status = "Success", msg = "Details Found", data = ds });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "No Data Available", data = "" });
                    }

                }
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "No Data Available", data = "" });

            }
        }

        public class ShiftDetails
        {
            public int ID { get; set; }
            public string ShiftName { get; set; }
            public string StartTime { get; set; }
            public string EndTime { get; set; }
            public decimal BreakTime { get; set; }
            public string CompanyCode { get; set; }
            public string PlantCode { get; set; }
            public string LineCode { get; set; }

        }


        [HttpPost]
        [Obsolete]
        [CustomAuthenticationFilter]
        public List<ShiftDetails> User_settings_details1(Models.Setting U)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(U.CompanyCode, U.PlantCode, U.LineCode);
            if (con_string == "0")
            {
                return new List<ShiftDetails>();

            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    var comp = new List<ShiftDetails>();
                    string response = string.Empty;
                    con.Open();
                    DataTable ds = new DataTable();
                    SqlCommand cmd = new SqlCommand("SP_GetSettings_data", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = U.QueryType;
                    cmd.Parameters.Add("@Parameter", SqlDbType.NVarChar, 150).Value = U.Parameter;
                    cmd.Parameters.Add("@Parameter1", SqlDbType.NVarChar, 150).Value = U.Parameter1;
                    cmd.Parameters.Add("@Parameter2", SqlDbType.NVarChar, 150).Value = U.Parameter2;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    var reader = cmd.ExecuteReader();
                    comp = (from DataRow dr in ds.Rows
                            select new ShiftDetails()
                            {
                                ID = Convert.ToInt32(reader["ID"] == DBNull.Value ? "" : reader["ID"]),
                                ShiftName = Convert.ToString(reader["ShiftName"] == DBNull.Value ? "" : reader["ShiftName"]),
                                StartTime = Convert.ToString(reader["StartTime"] == DBNull.Value ? "" : reader["StartTime"]),
                                EndTime = Convert.ToString(reader["EndTime"] == DBNull.Value ? "" : reader["EndTime"]),
                                BreakTime = Convert.ToDecimal(reader["BreakTime"] == DBNull.Value ? 0 : reader["BreakTime"]),
                                CompanyCode = Convert.ToString(reader["CompanyCode"] == DBNull.Value ? "" : reader["CompanyCode"]),
                                PlantCode = Convert.ToString(reader["PlantCode"] == DBNull.Value ? "" : reader["PlantCode"]),
                                LineCode = Convert.ToString(reader["LineCode"] == DBNull.Value ? "" : reader["LineCode"]),

                            }).ToList();
                    return comp;

                }
            }
        }

        /// <summary>
        /// loading asset list
        /// </summary>
        /// <param name="param1"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthenticationFilter]
        public IHttpActionResult List_Asset_details(Models.Setting param1)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(param1.CompanyCode, param1.PlantCode, param1.LineCode);
            if (con_string == "0")
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Couldnot connect to database"));

            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    DataTable dt = new DataTable();
                    DataTable dt1 = new DataTable();
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT distinct AssetID,AssetName,AssetDescription,Unique_id,EOL as 'sname',FunctionName " +
                        "FROM tbl_Assets WHERE CompanyCode = @Parameter1 AND PlantCode = @Parameter ", con);
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@Parameter1", SqlDbType.NVarChar, 150).Value = param1.Parameter1;
                    cmd.Parameters.Add("@Parameter", SqlDbType.NVarChar, 150).Value = param1.Parameter;
                    cmd.ExecuteNonQuery();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                    SqlCommand cmd1 = new SqlCommand();

                    SqlConnection conn = new SqlConnection(connectionstring);
                    conn.Open();
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        cmd1 = new SqlCommand("SELECT distinct @AssetID as AssetID,@AssetName as AssetName,@AssetDescription as AssetDescription,@Unique_id as Unique_id,[FunctionName] as 'funname',@EOL as 'sname' from [dbo].[tbl_Function] where [FunctionID]=@FunctionName ", conn);
                        cmd1.CommandTimeout = 0;
                        cmd1.Parameters.Add("@AssetID", SqlDbType.NVarChar, 150).Value = dt.Rows[i][0];
                        cmd1.Parameters.Add("@AssetName", SqlDbType.NVarChar, 150).Value = dt.Rows[i][1];
                        cmd1.Parameters.Add("@AssetDescription", SqlDbType.NVarChar, 150).Value = dt.Rows[i][2];
                        cmd1.Parameters.Add("@Unique_id", SqlDbType.NVarChar, 150).Value = dt.Rows[i][3];
                        cmd1.Parameters.Add("@EOL", SqlDbType.NVarChar, 150).Value = dt.Rows[i][4];
                        cmd1.Parameters.Add("@FunctionName", SqlDbType.NVarChar, 150).Value = dt.Rows[i][5];
                        cmd1.ExecuteNonQuery();
                        SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                        da1.Fill(dt1);
                    }
                    conn.Close();
                    if (dt1.Rows.Count != 0)
                    {
                        return Ok(dt1);
                    }
                    else
                    {
                        return Ok(new string[0]);
                    }
                }
            }
        }

        public IHttpActionResult settings_details(Models.Setting U)
        {
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                string response = string.Empty;
                con.Open();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_GetSettings_data", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = U.QueryType;
                cmd.Parameters.Add("@Parameter", SqlDbType.NVarChar, 150).Value = U.Parameter;
                cmd.Parameters.Add("@Parameter1", SqlDbType.NVarChar, 150).Value = U.Parameter1;
                cmd.Parameters.Add("@Parameter2", SqlDbType.NVarChar, 150).Value = U.Parameter2;
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
                //return Ok(ds.Tables[0]);
            }
        }

        public IHttpActionResult settings_details_Lines(Models.Setting U)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(U.Parameter1, U.Parameter, U.Parameter2);
            if (con_string == "0")
            {
                return Ok("No Connection");

            }
            using (SqlConnection con = new SqlConnection(con_string))
            {
                string response = string.Empty;
                con.Open();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_GetSettings_data", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = U.QueryType;
                cmd.Parameters.Add("@Parameter", SqlDbType.NVarChar, 150).Value = U.Parameter;
                cmd.Parameters.Add("@Parameter1", SqlDbType.NVarChar, 150).Value = U.Parameter1;
                cmd.Parameters.Add("@Parameter2", SqlDbType.NVarChar, 150).Value = U.Parameter2;
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
                //return Ok(ds.Tables[0]);
            }
        }

        public IHttpActionResult list_feedback_details(Models.Setting U)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(U.Parameter1, U.Parameter, U.Parameter2);
            if (con_string == "0")
            {
                return Ok(new string[0]); ;

            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    string response = string.Empty;
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("SP_GetSettings_data", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = U.QueryType;
                    cmd.Parameters.Add("@Parameter", SqlDbType.NVarChar, 150).Value = U.Parameter;
                    cmd.Parameters.Add("@Parameter1", SqlDbType.NVarChar, 150).Value = U.Parameter1;
                    cmd.Parameters.Add("@Parameter2", SqlDbType.NVarChar, 150).Value = U.Parameter2;
                    cmd.Parameters.Add("@Parameter3", SqlDbType.NVarChar, 150).Value = U.Parameter3;
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
                    //return Ok(ds.Tables[0]);
                }
            }

        }

        /// <summary>
        /// Delete Settings Data
        /// </summary>
        /// <param name="U"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthenticationFilter]
        public IHttpActionResult delete_User_settings_details(Models.Setting U)
        {
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                string response = string.Empty;
                con.Open();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_delete_usersettings", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = U.QueryType;
                cmd.Parameters.Add("@Parameter", SqlDbType.NVarChar, 150).Value = U.Parameter;
                cmd.Parameters.Add("@Parameter1", SqlDbType.NVarChar, 150).Value = U.Parameter1;
                cmd.ExecuteNonQuery();
                return Ok(response);
            }
        }


        /// <summary>
        /// Delete Settings Data
        /// </summary>
        /// <param name="U"></param>
        /// <returns></returns>
        [HttpPost]
        [CustomAuthenticationFilter]
        public IHttpActionResult new_delete_User_settings_details(Models.Setting U)
        {
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                string response = string.Empty;
                con.Open();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_new_delete_usersettings", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = U.QueryType;
                cmd.Parameters.Add("@Parameter", SqlDbType.NVarChar, 150).Value = U.Parameter;
                cmd.Parameters.Add("@Parameter1", SqlDbType.NVarChar, 150).Value = U.Parameter1;
                SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                SQLReturn.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(SQLReturn);
                cmd.ExecuteNonQuery();
                response = SQLReturn.Value.ToString().Trim();
                return Ok(response);
            }
        }

        /// <summary>
        /// Insert & Update Assets Details
        /// </summary>
        /// <param name="asts"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Assets_details(Models.Assets asts)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(asts.CompanyCode, asts.PlantCode, asts.FunctionName);
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
                    SqlCommand cmd = new SqlCommand("SP_Assets", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = asts.QueryType;
                    cmd.Parameters.Add("@Unique_id", SqlDbType.NVarChar, 150).Value = asts.Unique_id;
                    cmd.Parameters.Add("@AssetID", SqlDbType.NVarChar, 150).Value = asts.AssetID;
                    cmd.Parameters.Add("@AssetName", SqlDbType.NVarChar, 150).Value = asts.AssetName;
                    cmd.Parameters.Add("@FunctionName", SqlDbType.NVarChar, 150).Value = asts.FunctionName;
                    cmd.Parameters.Add("@AssetDescription", SqlDbType.NVarChar, 150).Value = asts.AssetDescription;
                    cmd.Parameters.Add("@ShiftID", SqlDbType.NVarChar, 150).Value = asts.ShiftID;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = asts.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = asts.PlantCode;
                    cmd.Parameters.Add("@ewonnumber", SqlDbType.NVarChar, 150).Value = asts.ewonnumber;
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
        public IHttpActionResult Production_setting(Models.production_setting setting)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(setting.companycode,setting.plantcode,setting.linecode);
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
                    SqlCommand cmd = new SqlCommand("SP_TargetProductionSetting", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = setting.querytype;
                    cmd.Parameters.Add("@Unique_id", SqlDbType.NVarChar, 150).Value = setting.id;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = setting.companycode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = setting.plantcode;
                    cmd.Parameters.Add("@Linecode", SqlDbType.NVarChar, 150).Value = setting.linecode;
                    cmd.Parameters.Add("@ProductName", SqlDbType.NVarChar, 150).Value = setting.productname;
                    cmd.Parameters.Add("@TargetProduction", SqlDbType.NVarChar, 150).Value = setting.targetproduction;
                    cmd.Parameters.Add("@ShiftID", SqlDbType.NVarChar, 150).Value = setting.shiftid;
                    cmd.Parameters.Add("@fromdate", SqlDbType.DateTime).Value = setting.fromdate;
                    cmd.Parameters.Add("@todate", SqlDbType.DateTime).Value = setting.todate;
                    SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                    SQLReturn.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(SQLReturn);
                    cmd.ExecuteNonQuery();
                    response = SQLReturn.Value.ToString().Trim();
                    return Ok(response);
                }
            }
        }
        /// <summary>
        /// Insert & Update Products Details
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Product_details(Models.Products p)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(p.CompanyCode,p.PlantCode,p.Line_Code);
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
                    SqlCommand cmd = new SqlCommand("SP_Masterproduct", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = p.QueryType;
                    cmd.Parameters.Add("@M_ID", SqlDbType.NVarChar, 150).Value = p.M_ID;
                    cmd.Parameters.Add("@Variant_Code", SqlDbType.NVarChar, 150).Value = p.Variant_Code;
                    cmd.Parameters.Add("@VariantName", SqlDbType.NVarChar, 150).Value = p.VariantName;
                    cmd.Parameters.Add("@VariantDescription", SqlDbType.NVarChar, 150).Value = p.VariantDescription;
                    cmd.Parameters.Add("@OperationName", SqlDbType.NVarChar, 150).Value = p.OperationName;
                    cmd.Parameters.Add("@Machine_Code", SqlDbType.NVarChar, 150).Value = p.Machine_Code;
                    cmd.Parameters.Add("@RecipeName", SqlDbType.NVarChar, 150).Value = p.RecipeName;
                    cmd.Parameters.Add("@CycleTime", SqlDbType.Decimal, 15).Value = p.CycleTime;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 15).Value = p.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 15).Value = p.PlantCode;
                    cmd.Parameters.Add("@LineCode", SqlDbType.NVarChar, 15).Value = p.Line_Code;
                    cmd.Parameters.Add("@Cost", SqlDbType.Decimal).Value = p.Cost;
                    cmd.Parameters.Add("@autocycletime", SqlDbType.Decimal).Value = p.Autocycletime;
                    cmd.Parameters.Add("@manualcycletime", SqlDbType.Decimal).Value = p.Manualcycletime;
                    cmd.Parameters.Add("@Ideal_cycletime", SqlDbType.Decimal).Value = p.Idlecycletime;
                    cmd.Parameters.Add("@No_of_fixtures", SqlDbType.Decimal).Value = p.Nooffixtures;
                    SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                    SQLReturn.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(SQLReturn);
                    cmd.ExecuteNonQuery();
                    response = SQLReturn.Value.ToString().Trim();
                    return Ok(response);
                }
            }
        }

        /// <summary>
        /// Insert & Update Holiday Details
        /// </summary>
        /// <param name="h"></param>
        /// <returns></returns>
        //[HttpPost]
        //public IHttpActionResult Holiday_details(Models.holiday h)
        //{
        //    using (SqlConnection con = new SqlConnection(connectionstring))
        //    {
        //        string response = string.Empty;
        //        con.Open();
        //        SqlCommand cmd = new SqlCommand("SP_holiday", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandTimeout = 0;
        //        cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = h.QueryType;
        //        cmd.Parameters.Add("@Unique_id", SqlDbType.NVarChar, 150).Value = h.Unique_id;
        //        cmd.Parameters.Add("@HolidayID", SqlDbType.NVarChar, 150).Value = h.HolidayID;
        //        cmd.Parameters.Add("@HolidayReason", SqlDbType.NVarChar, 150).Value = h.HolidayReason;
        //        cmd.Parameters.Add("@PlantID", SqlDbType.NVarChar, 150).Value = h.PlantID;
        //        cmd.Parameters.Add("@Date", SqlDbType.DateTime, 150).Value = h.Date;
        //        cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = h.CompanyCode;
        //        SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
        //        SQLReturn.Direction = ParameterDirection.Output;
        //        cmd.Parameters.Add(SQLReturn);
        //        cmd.ExecuteNonQuery();
        //        response = SQLReturn.Value.ToString().Trim();
        //        return Ok(response);
        //    }
        //}

        [HttpPost]
        public IHttpActionResult Holiday_details(Models.holiday h)
        {
            string response = string.Empty;
            using (SqlConnection con = new SqlConnection(connectionstring))
            {

                con.Open();
                SqlCommand cmd = new SqlCommand("SP_holiday", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = h.QueryType;
                // cmd.Parameters.Add("@Unique_id", SqlDbType.NVarChar, 150).Value = h.Unique_id;
                cmd.Parameters.Add("@HolidayID", SqlDbType.NVarChar, 150).Value = h.HolidayID;
                cmd.Parameters.Add("@HolidayReason", SqlDbType.NVarChar, 150).Value = h.HolidayReason;
                cmd.Parameters.Add("@PlantID", SqlDbType.NVarChar, 150).Value = h.PlantID;
                cmd.Parameters.Add("@Date", SqlDbType.DateTime, 150).Value = h.Date;
                cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = h.CompanyCode;
                SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);

                SQLReturn.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(SQLReturn);
                cmd.ExecuteNonQuery();
                response = SQLReturn.Value.ToString().Trim();
                // return Ok(response);
                if (response.ToString() == "Inserted" || response.ToString() == "Updated")
                {
                    database_connection d = new database_connection();
                    string con_string = d.Getconnectionstring(h.CompanyCode, h.PlantID, h.LineCode);
                    if (con_string == "0")
                    {
                        return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Couldnot connect to database"));

                    }
                    else
                    {
                        using (SqlConnection conn = new SqlConnection(con_string))
                        {
                            //string response1 = string.Empty;
                            conn.Open();
                            SqlCommand cmdd = new SqlCommand("SP_holiday", conn);
                            cmdd.CommandType = CommandType.StoredProcedure;
                            cmdd.CommandTimeout = 0;
                            cmdd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = h.QueryType;
                            //  cmdd.Parameters.Add("@Unique_id", SqlDbType.NVarChar, 150).Value = h.Unique_id;
                            cmdd.Parameters.Add("@HolidayID", SqlDbType.NVarChar, 150).Value = h.HolidayID;
                            cmdd.Parameters.Add("@HolidayReason", SqlDbType.NVarChar, 150).Value = h.HolidayReason;
                            cmdd.Parameters.Add("@PlantID", SqlDbType.NVarChar, 150).Value = h.PlantID;
                            cmdd.Parameters.Add("@Date", SqlDbType.DateTime, 150).Value = h.Date;
                            cmdd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = h.CompanyCode;
                            SqlParameter SQLReturnn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);

                            SQLReturnn.Direction = ParameterDirection.Output;
                            cmdd.Parameters.Add(SQLReturnn);
                            cmdd.ExecuteNonQuery();

                            return Ok(response);
                        }
                    }
                }
                else
                {
                    return Ok(response);
                }


            }
        }

        /// <summary>
        /// Insert & Update feedback Details
        /// </summary>
        /// <param name="h"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult feedback_details(Models.feedback h)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(h.CompanyCode, h.PlantCode, h.LineCode);
            if (con_string == "0")
            {
                return Ok(new string[0]); ;

            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    string response = string.Empty;
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SP_Feedback", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = h.QueryType;
                    cmd.Parameters.Add("@id", SqlDbType.NVarChar, 150).Value = h.id;
                    cmd.Parameters.Add("@username", SqlDbType.NVarChar, 150).Value = h.UserName;
                    cmd.Parameters.Add("@feedback", SqlDbType.NVarChar, 150).Value = h.Feedback;
                    cmd.Parameters.Add("@comments", SqlDbType.NVarChar, 150).Value = h.FB_Comments;
                    cmd.Parameters.Add("@document", SqlDbType.NVarChar, 150).Value = h.FB_Document;
                    cmd.Parameters.Add("@plant", SqlDbType.NVarChar, 150).Value = h.PlantCode;
                    cmd.Parameters.Add("@date", SqlDbType.DateTime, 150).Value = h.FB_Date;
                    cmd.Parameters.Add("@company", SqlDbType.NVarChar, 150).Value = h.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = h.LineCode;
                    SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                    SQLReturn.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(SQLReturn);
                    cmd.ExecuteNonQuery();
                    response = SQLReturn.Value.ToString().Trim();
                    return Ok(response);
                }
            }
            
        }

        private string EncryptPassword(string password)
        {
            string pswstr = string.Empty;
            byte[] psw_encode = new byte[password.Length];
            psw_encode = System.Text.Encoding.UTF8.GetBytes(password);
            pswstr = Convert.ToBase64String(psw_encode);
            return pswstr;
        }

        /// <summary>
        /// Insert & Update User Details
        /// </summary>
        /// <param name="u"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Users_details(Models.User u)
        {
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                var keyNew = GeneratePassword(10);
                //var password = EncodePassword(u.Password, keyNew);
                string response = string.Empty;
                con.Open();
                SqlCommand cmd = new SqlCommand("SP_Users", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = u.QueryType;
                cmd.Parameters.Add("@Unique_id", SqlDbType.NVarChar, 150).Value = u.Unique_id;
                cmd.Parameters.Add("@UserID", SqlDbType.NVarChar, 150).Value = u.UserID;
                cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 150).Value = u.FirstName;
                cmd.Parameters.Add("@LastName", SqlDbType.NVarChar, 150).Value = u.LastName;
                cmd.Parameters.Add("@PhoneNo", SqlDbType.NVarChar, 150).Value = u.PhoneNo;
                cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 150).Value = u.Email;
                cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 150).Value = EncodePassword(u.Password, keyNew);
                cmd.Parameters.Add("@Email", SqlDbType.NVarChar, 150).Value = u.Email;
                cmd.Parameters.Add("@RoleID", SqlDbType.NVarChar, 150).Value = u.RoleID;
                cmd.Parameters.Add("@IsActive", SqlDbType.NVarChar, 150).Value = u.IsActive;
                cmd.Parameters.Add("@SkillSet", SqlDbType.NVarChar, 150).Value = u.SkillSet;
                cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = u.CompanyCode;
                cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = u.PlantCode;
                cmd.Parameters.Add("@IsAdmin", SqlDbType.NVarChar, 150).Value = false;
                cmd.Parameters.Add("@VCode", SqlDbType.NVarChar, 150).Value = keyNew;
                cmd.Parameters.Add("@LineRoleID", SqlDbType.NVarChar, 150).Value = u.LineRoleID;
                SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                SQLReturn.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(SQLReturn);
                cmd.ExecuteNonQuery();
                response = SQLReturn.Value.ToString().Trim();
                if (response == "Inserted")
                {
                    MailMessage mail = new MailMessage();
                    mail.To.Add(u.Email);
                    DataTable dt = new DataTable();
                    SqlCommand cmd_mail = new SqlCommand("SELECT * FROM tbl_gmail_settings", con);
                    SqlDataAdapter da = new SqlDataAdapter(cmd_mail);
                    da.Fill(dt);
                    mail.From = new MailAddress(dt.Rows[0]["Smtp_user"].ToString());
                    mail.Subject = "Plant Digitalization Portal Login credentials";
                    mail.Body = string.Format("<b> Login Details  </b> ");
                    mail.Body += string.Format("<br/><b>User Name :  </b> {0} ", u.Email);
                    mail.Body += string.Format("<br/><b>Password :  </b> {0} ", u.Password);
                    DataTable dt1 = new DataTable();
                    SqlCommand cmd_mail1 = new SqlCommand("SELECT distinct [URL] FROM [dbo].[Portal_URL] where [CompanyCode]=@CompanyCode and [PlantCode]=@PlantCode", con);
                    cmd_mail1.Parameters.AddWithValue("@CompanyCode", u.CompanyCode);
                    cmd_mail1.Parameters.AddWithValue("@PlantCode", u.PlantCode);
                    SqlDataAdapter da1 = new SqlDataAdapter(cmd_mail1);
                    da1.Fill(dt1);
                    var s = dt1.Rows[0]["URL"].ToString();
                    mail.Body += string.Format("<br/><br/><b>To Login in to portal " + "<a href='" + s + "'> click here</a><b><br/>");
                    mail.Body += string.Format("<br/> * **Mail generated from TEAL IIOT Portal Email App Service * **");
                    mail.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = dt.Rows[0]["Smtp_host"].ToString();
                    smtp.Port = Convert.ToInt32(dt.Rows[0]["Smtp_port"].ToString());
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = new System.Net.NetworkCredential(dt.Rows[0]["Smtp_user"].ToString(), dt.Rows[0]["Smtp_pass"].ToString());
                    smtp.EnableSsl = true;
                    smtp.Send(mail);
                }
                return Ok(response);
            }
        }

     
        //[HttpPost]
        //public IHttpActionResult Roles_details(Models.Roles R)
        //{
        //    //database_connection d = new database_connection();
        //    //string con_string = d.Getconnectionstring(R.CompanyCode,R.PlantCode,R.Line_Code);
        //    //if (con_string == "0")
        //    //{
        //    //    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Couldnot connect to database"));

        //    //}
        //    //else
        //    //{
        //    using (SqlConnection con = new SqlConnection(connectionstring))
        //    {
        //        string response = string.Empty;
        //        con.Open();
        //        SqlCommand cmd = new SqlCommand("SP_Roles", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandTimeout = 0;
        //        cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = R.QueryType;
        //        cmd.Parameters.Add("@Unique_id", SqlDbType.NVarChar, 150).Value = R.Unique_id;
        //        cmd.Parameters.Add("@RoleID", SqlDbType.NVarChar, 150).Value = R.RoleID;
        //        cmd.Parameters.Add("@RoleName", SqlDbType.NVarChar, 150).Value = R.RoleName;
        //        cmd.Parameters.Add("@RoleDescription", SqlDbType.NVarChar, 150).Value = R.RoleDescription;
        //        cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = R.CompanyCode;
        //        cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = R.PlantCode;
        //        cmd.Parameters.Add("@LineCode", SqlDbType.NVarChar, 150).Value = R.Line_Code;
        //        SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
        //        SQLReturn.Direction = ParameterDirection.Output;
        //        cmd.Parameters.Add(SQLReturn);
        //        cmd.ExecuteNonQuery();
        //        response = SQLReturn.Value.ToString().Trim();
        //        return Ok(response);
        //    }
        //    // }

        //}

        /// <summary>
        /// Insert & Update Department Details
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Dept_details(Models.Dept de)
        {
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                string response = string.Empty;
                con.Open();
                SqlCommand cmd = new SqlCommand("SP_Department_details", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = de.QueryType;
                cmd.Parameters.Add("@Unique_id", SqlDbType.NVarChar, 150).Value = de.Unique_id;
                cmd.Parameters.Add("@Dept_id", SqlDbType.NVarChar, 150).Value = de.Dept_id;
                cmd.Parameters.Add("@Dept_name", SqlDbType.NVarChar, 150).Value = de.Dept_name;
                cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = de.CompanyCode;
                cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = de.PlantCode;
                cmd.Parameters.Add("@AreaCode", SqlDbType.NVarChar, 150).Value = de.Area_id;
                SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                SQLReturn.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(SQLReturn);
                cmd.ExecuteNonQuery();
                response = SQLReturn.Value.ToString().Trim();
                return Ok(response);
            }
        }

        /// <summary>
        /// Insert & Update URL Details
        /// </summary>
        /// <param name="de"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult URL_details(Models.URL_table de)
        {

            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                string response = string.Empty;
                con.Open();
                SqlCommand cmd = new SqlCommand("SP_URL_details", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = de.QueryType;
                cmd.Parameters.Add("@Unique_id", SqlDbType.NVarChar, 150).Value = de.Unique_id;
                cmd.Parameters.Add("@url", SqlDbType.NVarChar, 150).Value = de.URL;
                cmd.Parameters.Add("@LineCode", SqlDbType.NVarChar, 150).Value = de.LineCode;
                cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = de.CompanyCode;
                cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = de.PlantCode;
                SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                SQLReturn.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(SQLReturn);
                cmd.ExecuteNonQuery();
                response = SQLReturn.Value.ToString().Trim();
                return Ok(response);
            }

        }

        /// <summary>
        /// Insert & Update Role Permission Details
        /// </summary>
        /// <param name="P"></param>
        /// <returns></returns>
        //[HttpPost]
        //public IHttpActionResult Permission_details(Models.Permission P)
        //{
        //    using (SqlConnection con = new SqlConnection(connectionstring))
        //    {
        //        con.Open();
        //        if (P.QueryType == "Update")
        //        {
        //            DataSet ds = new DataSet();
        //            SqlCommand cmd_qtype = new SqlCommand("SELECT count(*) FROM tbl_Permission" +
        //                  " WHERE Module_name=@Module_name AND Roleid=@Roleid", con);
        //            cmd_qtype.CommandTimeout = 0;
        //            cmd_qtype.Parameters.AddWithValue("Module_name", P.Module_name);
        //            cmd_qtype.Parameters.AddWithValue("Roleid", P.RoleID);
        //            SqlDataAdapter da = new SqlDataAdapter(cmd_qtype);

        //            int count = (int)cmd_qtype.ExecuteScalar();
        //            if (count == 0)
        //            {
        //                P.QueryType = "Insert";
        //            }
        //        }



        //        string response = string.Empty;

        //        SqlCommand cmd = new SqlCommand("SP_Role_Permission", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandTimeout = 0;
        //        cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = P.QueryType;
        //        cmd.Parameters.Add("@Unique_id", SqlDbType.NVarChar, 150).Value = P.Unique_id;
        //        cmd.Parameters.Add("@Permission_id", SqlDbType.NVarChar, 150).Value = P.Permission_id;
        //        cmd.Parameters.Add("@RoleID", SqlDbType.NVarChar, 150).Value = P.RoleID;
        //        cmd.Parameters.Add("@Module_name", SqlDbType.NVarChar, 150).Value = P.Module_name;
        //        cmd.Parameters.Add("@Edit_form", SqlDbType.NVarChar, 150).Value = P.Edit_form;
        //        cmd.Parameters.Add("@View_form", SqlDbType.NVarChar, 150).Value = P.View_form;
        //        cmd.Parameters.Add("@Delete_form", SqlDbType.NVarChar, 150).Value = P.Delete_form;
        //        cmd.Parameters.Add("@Add_form", SqlDbType.NVarChar, 150).Value = P.Add_form;
        //        cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = P.CompanyCode;
        //        SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
        //        SQLReturn.Direction = ParameterDirection.Output;
        //        cmd.Parameters.Add(SQLReturn);
        //        cmd.ExecuteNonQuery();
        //        response = SQLReturn.Value.ToString().Trim();
        //        return Ok(response);
        //    }
        //}

        /// <summary>
        /// Insert & Update Skills Details
        /// </summary>
        /// <param name="SK"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Skills_details(Models.Skills SK)
        {
            string response1 = string.Empty;
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                string response = string.Empty;
                con.Open();
                SqlCommand cmd = new SqlCommand("SP_Skills", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = SK.QueryType;
                cmd.Parameters.Add("@Unique_id", SqlDbType.NVarChar, 150).Value = SK.Unique_id;
                cmd.Parameters.Add("@Skill_ID", SqlDbType.NVarChar, 150).Value = SK.Skill_ID;
                cmd.Parameters.Add("@SkillName", SqlDbType.NVarChar, 150).Value = SK.SkillName;
                cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = SK.CompanyCode;
                cmd.Parameters.Add("@LineCode", SqlDbType.NVarChar, 150).Value = SK.Line_Code;
                cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = SK.Plant_Code;
                SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                SQLReturn.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(SQLReturn);
                cmd.ExecuteNonQuery();
                response = SQLReturn.Value.ToString().Trim();

                if (response == "Inserted" || response == "Updated")
                {
                    database_connection d = new database_connection();
                    string con_string = d.Getconnectionstring(SK.CompanyCode, SK.Plant_Code, SK.Line_Code);
                    if (con_string == "0")
                    {
                        return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Couldnot connect to database"));

                    }
                    else
                    {
                        using (SqlConnection con1 = new SqlConnection(con_string))
                        {
                            con1.Open();
                            SqlCommand cmd1 = new SqlCommand("SP_Skills", con1);
                            cmd1.CommandType = CommandType.StoredProcedure;
                            cmd1.CommandTimeout = 0;
                            cmd1.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = SK.QueryType;
                            cmd1.Parameters.Add("@Unique_id", SqlDbType.NVarChar, 150).Value = SK.Unique_id;
                            cmd1.Parameters.Add("@Skill_ID", SqlDbType.NVarChar, 150).Value = SK.Skill_ID;
                            cmd1.Parameters.Add("@SkillName", SqlDbType.NVarChar, 150).Value = SK.SkillName;
                            cmd1.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = SK.CompanyCode;
                            cmd1.Parameters.Add("@LineCode", SqlDbType.NVarChar, 150).Value = SK.Line_Code;
                            cmd1.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = SK.Plant_Code;
                            SqlParameter SQLReturn1 = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                            SQLReturn1.Direction = ParameterDirection.Output;
                            cmd1.Parameters.Add(SQLReturn1);
                            cmd1.ExecuteNonQuery();
                            response1 = SQLReturn1.Value.ToString().Trim();

                        }
                    }
                    return Ok(response1);
                }
                else
                {
                    return Ok(response);
                }
            }
        }

        /// <summary>
        /// Insert & Update Operator Skill Details
        /// </summary>
        /// <param name="OP"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult OperatorSkill_details(Models.Operator_skill OP)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(OP.CompanyCode,OP.PlantCode,OP.Line_Code);
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
                    SqlCommand cmd = new SqlCommand("SP_OperatorSkill", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = OP.QueryType;
                    cmd.Parameters.Add("@O_ID", SqlDbType.NVarChar, 150).Value = OP.O_ID;
                    cmd.Parameters.Add("@OperatorID", SqlDbType.NVarChar, 150).Value = OP.OperatorID;
                    cmd.Parameters.Add("@SkillName", SqlDbType.NVarChar, 150).Value = OP.SkillName;
                    cmd.Parameters.Add("@ScoreValue", SqlDbType.NVarChar, 150).Value = OP.ScoreValue;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = OP.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = OP.PlantCode;
                    cmd.Parameters.Add("@LineCode", SqlDbType.NVarChar, 150).Value = OP.Line_Code;
                    SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                    SQLReturn.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(SQLReturn);
                    cmd.ExecuteNonQuery();
                    response = SQLReturn.Value.ToString().Trim();
                    return Ok(response);
                }
            }
        }

        /// <summary>
        /// Insert & Update Alarmtable Settings Details
        /// </summary>
        /// <param name="AS"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AlarmSettins_details(Models.AlarmSettings AS)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(AS.CompanyCode,AS.PlantCode,AS.Line_Code);
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
                    SqlCommand cmd = new SqlCommand("SP_AlarmSettings", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = AS.QueryType;
                    cmd.Parameters.Add("@ID", SqlDbType.NVarChar, 150).Value = AS.A_ID;
                    cmd.Parameters.Add("@Line_Code", SqlDbType.NVarChar, 150).Value = AS.Line_Code;
                    cmd.Parameters.Add("@Machine_Code", SqlDbType.NVarChar, 150).Value = AS.Machine_Code;
                    cmd.Parameters.Add("@Alarm_ID", SqlDbType.NVarChar, 150).Value = AS.Alarm_ID;
                    cmd.Parameters.Add("@Alarm_Description", SqlDbType.NVarChar, 150).Value = AS.Alarm_Description;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = AS.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = AS.PlantCode;
                    cmd.Parameters.Add("@Help", SqlDbType.NVarChar, 150).Value = AS.Help;
                    SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                    SQLReturn.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(SQLReturn);
                    cmd.ExecuteNonQuery();
                    response = SQLReturn.Value.ToString().Trim();
                    return Ok(response);
                }
            }
        }

        /// <summary>
        /// Insert & Delete Rejection Details
        /// </summary>
        /// <param name="RJ"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Rejection_details(Models.Rejections RJ)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(RJ.CompanyCode,RJ.PlantCode,RJ.Line_Code);
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
                    SqlCommand cmd = new SqlCommand("SP_Rejection", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = RJ.QueryType;
                    cmd.Parameters.Add("@R_ID", SqlDbType.NVarChar, 150).Value = RJ.R_ID;
                    cmd.Parameters.Add("@RejectionCode", SqlDbType.NVarChar, 150).Value = RJ.RejectionCode;
                    cmd.Parameters.Add("@RejectionName", SqlDbType.NVarChar, 150).Value = RJ.RejectionName;
                    cmd.Parameters.Add("@RejectionDescription", SqlDbType.NVarChar, 150).Value = RJ.RejectionDescription;
                    cmd.Parameters.Add("@ProductName", SqlDbType.NVarChar, 150).Value = RJ.ProductName;
                    cmd.Parameters.Add("@OperationName", SqlDbType.NVarChar, 150).Value = RJ.OperationName;
                    cmd.Parameters.Add("@AssetName", SqlDbType.NVarChar, 150).Value = RJ.AssetName;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = RJ.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = RJ.PlantCode;
                    cmd.Parameters.Add("@LineCode", SqlDbType.NVarChar, 150).Value = RJ.Line_Code;
                    SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                    SQLReturn.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(SQLReturn);
                    cmd.ExecuteNonQuery();
                    response = SQLReturn.Value.ToString().Trim();
                    return Ok(response);
                }
            }
        }

        /// <summary>
        /// Insert & Update Losses Settings Details
        /// </summary>
        /// <param name="LO"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult LossesSettings_details(Models.LossesSetting LO)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(LO.CompanyCode,LO.PlantCode,LO.Line_Code);
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
                    SqlCommand cmd = new SqlCommand("SP_LossestblSettings", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = LO.QueryType;
                    cmd.Parameters.Add("@ID", SqlDbType.NVarChar, 150).Value = LO.ID;
                    cmd.Parameters.Add("@Line_Code", SqlDbType.NVarChar, 150).Value = LO.Line_Code;
                    cmd.Parameters.Add("@Machine_Code", SqlDbType.NVarChar, 150).Value = LO.Machine_Code;
                    cmd.Parameters.Add("@Loss_ID", SqlDbType.NVarChar, 150).Value = LO.Loss_ID;
                    cmd.Parameters.Add("@Loss_Description", SqlDbType.NVarChar, 150).Value = LO.Loss_Description;
                    cmd.Parameters.Add("@Loss_Category", SqlDbType.NVarChar, 150).Value = LO.Loss_Category;
                    cmd.Parameters.Add("@Loss_Type", SqlDbType.NVarChar, 150).Value = LO.Loss_Type;
                    cmd.Parameters.Add("@Subassambly", SqlDbType.NVarChar, 150).Value = LO.Subassembly;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = LO.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = LO.PlantCode;
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
        public IHttpActionResult LossCategorySettings_details(Models.LossCategory LO)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(LO.CompanyCode,LO.PlantCode,LO.Line_Code);
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
                    SqlCommand cmd = new SqlCommand("SP_Losses_category_Settings", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = LO.QueryType;
                    cmd.Parameters.Add("@ID", SqlDbType.NVarChar, 150).Value = LO.ID;
                    cmd.Parameters.Add("@Line_Code", SqlDbType.NVarChar, 150).Value = LO.Line_Code;
                    cmd.Parameters.Add("@Loss_Category", SqlDbType.NVarChar, 150).Value = LO.Loss_Category;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = LO.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = LO.PlantCode;
                    SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                    SQLReturn.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(SQLReturn);
                    cmd.ExecuteNonQuery();
                    response = SQLReturn.Value.ToString().Trim();
                    return Ok(response);
                }
            }
        }

        /// <summary>
        /// Insert & Update Loss Type
        /// </summary>
        /// <param name="LO"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult LossTypeSettings_details(Models.LossType LO)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(LO.CompanyCode,LO.PlantCode,LO.Line_Code);
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
                    SqlCommand cmd = new SqlCommand("SP_Losses_type_Settings", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = LO.QueryType;
                    cmd.Parameters.Add("@ID", SqlDbType.NVarChar, 150).Value = LO.ID;
                    cmd.Parameters.Add("@Line_Code", SqlDbType.NVarChar, 150).Value = LO.Line_Code;
                    cmd.Parameters.Add("@Loss_Type", SqlDbType.NVarChar, 150).Value = LO.Loss_Type;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = LO.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = LO.PlantCode;
                    SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                    SQLReturn.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(SQLReturn);
                    cmd.ExecuteNonQuery();
                    response = SQLReturn.Value.ToString().Trim();
                    return Ok(response);
                }
            }
        }
        /// <summary>
        /// Insert & Update Toollist Details
        /// </summary>
        /// <param name="TO"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Tooli_list_details(Models.Toollist TO)
        {
            try
            {
                database_connection d = new database_connection();
                string con_string = d.Getconnectionstring(TO.CompanyCode, TO.PlantCode, TO.Line_Code);
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
                        SqlCommand cmd = new SqlCommand("SP_toollist", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = TO.QueryType;
                        cmd.Parameters.Add("@ID", SqlDbType.NVarChar, 150).Value = TO.ID;
                        cmd.Parameters.Add("@Line_Code", SqlDbType.NVarChar, 150).Value = TO.Line_Code;
                        cmd.Parameters.Add("@Machine_Code", SqlDbType.NVarChar, 150).Value = TO.Machine_Code;
                        cmd.Parameters.Add("@ToolName", SqlDbType.NVarChar, 150).Value = TO.ToolName;
                        cmd.Parameters.Add("@ToolID", SqlDbType.NVarChar, 150).Value = TO.ToolID;
                        cmd.Parameters.Add("@Make", SqlDbType.NVarChar, 150).Value = TO.Make;
                        cmd.Parameters.Add("@UOM", SqlDbType.NVarChar, 150).Value = TO.UOM;
                        cmd.Parameters.Add("@Part_number", SqlDbType.NVarChar, 150).Value = TO.Part_number;
                        cmd.Parameters.Add("@Classfication", SqlDbType.NVarChar, 150).Value = TO.Classification;
                        cmd.Parameters.Add("@RatedLifeCycle", SqlDbType.NVarChar, 150).Value = TO.RatedLifeCycle;
                        cmd.Parameters.Add("@Conversion_parameter", SqlDbType.NVarChar, 150).Value = TO.Conversion_parameter;
                        cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = TO.CompanyCode;
                        cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = TO.PlantCode;
                        cmd.Parameters.Add("@SerialNo", SqlDbType.NVarChar, 150).Value = TO.SerialNo;
                        cmd.Parameters.Add("@SubAssembly", SqlDbType.NVarChar, 150).Value = TO.Subassembly;
                        cmd.Parameters.Add("@RecommendationText", SqlDbType.NVarChar, 150).Value = TO.Rectext;
                        
                        SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                        SQLReturn.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(SQLReturn);
                        cmd.ExecuteNonQuery();
                        response = SQLReturn.Value.ToString().Trim();
                        return Ok(response);
                    }
                }
            }
            catch(Exception ex)
            {
                return Ok("1");
            }
        }


        [HttpPost]
        public IHttpActionResult Insert_LifeCycle(Models.Toollist TO)
        {
            try
            {
                database_connection d = new database_connection();
                string con_string = d.Getconnectionstring(TO.CompanyCode, TO.PlantCode, TO.Line_Code);
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
                        SqlCommand cmd = new SqlCommand("SP_toollist_child", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;


                        cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = TO.QueryType;
                        cmd.Parameters.Add("@ID", SqlDbType.NVarChar, 150).Value = TO.ID;
                        cmd.Parameters.Add("@Line_Code", SqlDbType.NVarChar, 150).Value = TO.Line_Code;
                        cmd.Parameters.Add("@Machine_Code", SqlDbType.NVarChar, 150).Value = TO.Machine_Code;
                        cmd.Parameters.Add("@ToolName", SqlDbType.NVarChar, 150).Value = TO.ToolName;
                        cmd.Parameters.Add("@ToolID", SqlDbType.NVarChar, 150).Value = TO.ToolID;
                        //cmd.Parameters.Add("@Make", SqlDbType.NVarChar, 150).Value = TO.Make;
                        //cmd.Parameters.Add("@SerialNo", SqlDbType.NVarChar, 150).Value = TO.SerialNo;
                        cmd.Parameters.Add("@SubAssembly", SqlDbType.NVarChar, 150).Value = TO.Subassembly;
                        //cmd.Parameters.Add("@UOM", SqlDbType.NVarChar, 150).Value = TO.UOM;
                        cmd.Parameters.Add("@Part_number", SqlDbType.NVarChar, 150).Value = TO.Part_number;
                        //cmd.Parameters.Add("@Classfication", SqlDbType.NVarChar, 150).Value = TO.Classification;
                        cmd.Parameters.Add("@RatedLifeCycle", SqlDbType.NVarChar, 150).Value = TO.RatedLifeCycle;
                        //cmd.Parameters.Add("@Conversion_parameter", SqlDbType.NVarChar, 150).Value = TO.Conversion_parameter;
                        cmd.Parameters.Add("@RecommendationText", SqlDbType.NVarChar, 150).Value = TO.Rectext;

                        cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = TO.CompanyCode;
                        cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = TO.PlantCode;
                        SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                        SQLReturn.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(SQLReturn);
                        cmd.ExecuteNonQuery();
                        response = SQLReturn.Value.ToString().Trim();
                        return Ok(response);
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok("1");
            }

        }
        /// <summary>
        /// Insert & Update Operators Details
        /// </summary>
        /// <param name="OP"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Operators_details(Models.Operators OP)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(OP.CompanyCode,OP.PlantCode,OP.Line_Code);
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
                    SqlCommand cmd = new SqlCommand("SP_Operator", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = OP.QueryType;
                    cmd.Parameters.Add("@OP_ID", SqlDbType.NVarChar, 150).Value = OP.OP_ID;
                    cmd.Parameters.Add("@OPeratorName", SqlDbType.NVarChar, 150).Value = OP.OperatorName;
                    cmd.Parameters.Add("@OperatorID", SqlDbType.NVarChar, 150).Value = OP.OperatorID;
                    cmd.Parameters.Add("@AssetName", SqlDbType.NVarChar, 150).Value = OP.AssetName;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = OP.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = OP.PlantCode;
                    cmd.Parameters.Add("@LineCode", SqlDbType.NVarChar, 150).Value = OP.Line_Code;
                    SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                    SQLReturn.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(SQLReturn);
                    cmd.ExecuteNonQuery();
                    response = SQLReturn.Value.ToString().Trim();
                    return Ok(response);
                }
            }
        }

        /// <summary>
        /// Insert & Update Shift Settings Details 
        /// </summary>
        /// <param name="SS"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Shiftsettings_details(Models.ShiftSettings SS)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(SS.CompanyCode,SS.PlantCode,SS.Line_Code);
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
                    SqlCommand cmd = new SqlCommand("SP_ShiftSettings", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = SS.QueryType;
                    cmd.Parameters.Add("@ID", SqlDbType.NVarChar, 150).Value = SS.ID;
                    cmd.Parameters.Add("@ShiftName", SqlDbType.NVarChar, 150).Value = SS.ShiftName;
                    cmd.Parameters.Add("@StartTime", SqlDbType.Time, 150).Value = SS.StartTime;
                    cmd.Parameters.Add("@EndTime", SqlDbType.Time, 150).Value = SS.EndTime;
                    cmd.Parameters.Add("@BreakTime", SqlDbType.NVarChar, 150).Value = SS.BreakTime;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = SS.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = SS.PlantCode;
                    cmd.Parameters.Add("@LineCode", SqlDbType.NVarChar, 150).Value = SS.Line_Code;
                    SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                    SQLReturn.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(SQLReturn);
                    cmd.ExecuteNonQuery();
                    response = SQLReturn.Value.ToString().Trim();
                    return Ok(response);
                }
            }
        }

        /// <summary>
        /// Check User Login Details
        /// </summary>
        /// <param name="lo"></param>
        /// <returns></returns>
        //[HttpPost]
        //public IHttpActionResult Check_login(Models.Login lo)
        //{
        //    using (SqlConnection con = new SqlConnection(connectionstring))
        //    {
        //        var result = new List<Models.User>();
        //        SqlCommand cmd = new SqlCommand(@"SELECT VCode from tbl_Users where UserName=@UserName ", con);
        //        cmd.Parameters.AddWithValue("@UserName", lo.UserName);
        //        con.Open();
        //        using (SqlDataReader reader = cmd.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                result.Add(item: new Models.User
        //                {

        //                    VCode = (string)reader["VCode"],
        //                });
        //            if (result.VCode != null)
        //            {
        //                string response = string.Empty;
        //                Models.Loginres loginres = new Models.Loginres();
        //                con.Open();
        //                SqlCommand cmd1 = new SqlCommand("SP_Checklogin", con);
        //                cmd1.CommandType = CommandType.StoredProcedure;
        //                cmd1.Parameters.Add("@UserName", SqlDbType.NVarChar, 150).Value = lo.UserName;
        //                cmd1.Parameters.Add("@Password", SqlDbType.NVarChar, 150).Value = EncryptPassword(lo.Password);
        //                cmd1.Parameters.Add("@Lastlogin", SqlDbType.NVarChar, 150).Value = lo.lastlogin;
        //                SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
        //                SqlParameter lastlogindate = new SqlParameter("@lastlogindate", SqlDbType.DateTime);
        //                SQLReturn.Direction = ParameterDirection.Output;
        //                lastlogindate.Direction = ParameterDirection.Output;
        //                cmd1.Parameters.Add(SQLReturn);
        //                cmd1.Parameters.Add(lastlogindate);
        //                cmd1.ExecuteNonQuery();
        //                //response = SQLReturn.Value.ToString().Trim();
        //                loginres.loginstatus = SQLReturn.Value.ToString().Trim();
        //                loginres.lastlogindate = lastlogindate.Value.ToString();
        //                //HttpResponseMessage res = Request.CreateResponse(HttpStatusCode.OK, response);
        //                return Ok(loginres);
        //            }

        //        }

        //        }
        //        return Ok();
        //    }
        //}
        [HttpPost]
        
        public  HttpResponseMessage ValidLogin(Models.Login lo)
        {
            
            if(username == lo.UserName && password == lo.Password)
            {
                return Request.CreateResponse(HttpStatusCode.OK, TokenManager.GenerateToken(lo.UserName));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadGateway, "username and password invalid");
            }
        }
        
        [HttpGet]
        [CustomAuthenticationFilter]
        
        public HttpResponseMessage GetEmployee()
        {
            return Request.CreateResponse(HttpStatusCode.OK, value: "Successfully valid");
        }


        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage GetEmployee1()
        {
            return Request.CreateResponse(HttpStatusCode.OK, value: "Successfully valid");
        } 


        /// <summary>
        /// Check User Login Details
        /// </summary>
        /// <param name="lo"></param>
        /// <returns></returns>
        [HttpPost]
        public Models.Loginres Check_login(Models.Login lo)
        {

            //var hashCode = lo.VCode;
            //Password Hasing Process Call Helper Class Method    
            //var encodingPasswordString = EncodePassword(lo.Password, hashCode);


            //var keyNew = GeneratePassword(10);
            //var password = EncodePassword(lo.Password, keyNew);
            try
            {
                using (SqlConnection con = new SqlConnection(connectionstring))
                {
                    //con.Open();
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd1 = new SqlCommand("SELECT VCode FROM Users WHERE UserName=@UserName", con);
                    cmd1.Parameters.AddWithValue("UserName", lo.UserName);

                    SqlDataAdapter da = new SqlDataAdapter(cmd1);
                    da.Fill(ds);
                    object r1 = cmd1.ExecuteScalar();

                    if (r1 != null)
                    {
                        string hc = ds.Tables[0].Rows[0][0].ToString();
                        string response = string.Empty;
                        Models.Loginres loginres = new Models.Loginres();
                        //con.Open();
                        SqlCommand cmd = new SqlCommand("SP_Checklogin", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 150).Value = lo.UserName;
                        cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 150).Value = EncodePassword(lo.Password, hc);
                        //cmd.Parameters.Add("@Password", SqlDbType.NVarChar, 150).Value = EncryptPassword(lo.Password);
                        cmd.Parameters.Add("@Lastlogin", SqlDbType.NVarChar, 150).Value = lo.lastlogin;
                        SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                        SqlParameter lastlogindate = new SqlParameter("@lastlogindate", SqlDbType.DateTime);
                        SQLReturn.Direction = ParameterDirection.Output;
                        lastlogindate.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(SQLReturn);
                        cmd.Parameters.Add(lastlogindate);
                        cmd.ExecuteNonQuery();
                        //response = SQLReturn.Value.ToString().Trim();
                        loginres.loginstatus = SQLReturn.Value.ToString().Trim();
                        loginres.lastlogindate = lastlogindate.Value.ToString();
                        //HttpResponseMessage res = Request.CreateResponse(HttpStatusCode.OK, response);

                        if (loginres.loginstatus == "Login Successfull...!")
                        {
                            string uname = "";
                            using (SqlConnection con1 = new SqlConnection(connectionstring))
                            {
                                string QueryType = "Get_User";
                                //con.Open();
                                con1.Open();
                                DataSet ds1 = new DataSet();
                                SqlCommand cmd2 = new SqlCommand("SP_GetSettings_data", con1);
                                cmd2.CommandType = CommandType.StoredProcedure;
                                //SqlCommand cmd2 = new SqlCommand("SELECT UserName FROM Users WHERE UserName=UserName", con1);
                                cmd2.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = QueryType;
                                cmd2.Parameters.Add("@Parameter", SqlDbType.NVarChar, 150).Value = lo.UserName;

                                SqlDataAdapter da1 = new SqlDataAdapter(cmd2);
                                da1.Fill(ds1);
                                var uid = ds1.Tables[0].Rows[0]["Username"].ToString();

                                if (uid != lo.UserName)
                                {
                                    loginres.loginstatus = "Username is incorrect....!";
                                }
                                else
                                {

                                    loginres.loginstatus = "Login Successfull...!";

                                }
                            }
                            string token1 = TokenManager.GenerateToken(lo.UserName);
                            loginres.token = token1;
                            return loginres;
                        }
                        else
                        {
                            return loginres;
                        }

                    }

                    else
                    {
                        Models.Loginres loginres = new Models.Loginres();
                        loginres.loginstatus = ("Username is incorrect");
                        return loginres;
                    }
                }
            }
            catch(Exception ex)
            {
                Models.Loginres loginres1 = new Models.Loginres();
                var exp = ex;
                return loginres1;
            }
        }



        [HttpPost]
        public Models.Loginres InsertSessionId(Models.Login lo)
        {

            //var hashCode = lo.VCode;
            //Password Hasing Process Call Helper Class Method    
            //var encodingPasswordString = EncodePassword(lo.Password, hashCode);


            //var keyNew = GeneratePassword(10);
            //var password = EncodePassword(lo.Password, keyNew);

            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                con.Open();

                string response = string.Empty;
                Models.Loginres loginres = new Models.Loginres();
                //con.Open();
                SqlCommand cmd = new SqlCommand("SP_InsertSession", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 150).Value = lo.UserName;
                cmd.Parameters.Add("@SessionId", SqlDbType.NVarChar, 150).Value = lo.UserSessionId;


                SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                SQLReturn.Direction = ParameterDirection.Output;
               
                cmd.Parameters.Add(SQLReturn);
               
                cmd.ExecuteNonQuery();
                //response = SQLReturn.Value.ToString().Trim();
                loginres.loginstatus = SQLReturn.Value.ToString().Trim();

                //HttpResponseMessage res = Request.CreateResponse(HttpStatusCode.OK, response);

                return loginres;


            }

        
            
        }

        [HttpPost]
        public Models.Loginres CheckSessionId(Models.Login lo)
        {

            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                con.Open();
                DataSet ds1 = new DataSet();
                SqlCommand cmd1 = new SqlCommand("SELECT SessionId FROM Users WHERE UserName=@UserName", con);
                cmd1.Parameters.AddWithValue("@UserName", lo.UserName);
                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                da1.Fill(ds1);
                object r1 = cmd1.ExecuteScalar();
                string r11 = r1.ToString();
                string r2 = lo.UserSessionId;
                Models.Loginres loginres = new Models.Loginres();
                if (r11 == r2)
                {
                   
                    loginres.loginstatus = "Not Applicable";
                    return loginres;
                }
                else
                {
                    loginres.loginstatus = "Applicable";
                    return loginres;
                   
                }
            }
          

        }

        //public HttpResponseMessage Validate(string token, string username)
        //{
        //    int UserId = new UserRepository().GetUser(username);
        //    if (UserId == 0) return new ResponseVM
        //    {
        //        Status = "Invalid",
        //        Message = "Invalid User."
        //    };
        //    string tokenUsername = TokenManager.ValidateToken(token);
        //    if (username.Equals(tokenUsername))
        //    {
        //        return new ResponseVM
        //        {
        //            Status = "Success",
        //            Message = "OK",
        //        };
        //    }
        //    return new ResponseVM
        //    {
        //        Status = "Invalid",
        //        Message = "Invalid Token."
        //    };
        //}
        public static string GeneratePassword(int length) //length of salt    
        {
            const string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789";
            var randNum = new Random();
            var chars = new char[length];
            var allowedCharCount = allowedChars.Length;
            for (var i = 0; i < length ; i++)
            {
                chars[i] = allowedChars[Convert.ToInt32((allowedCharCount) * randNum.NextDouble())];
            }
            return new string(chars);
        }

        public static string EncodePassword(string pass, string salt) //encrypt password    
        {
            byte[] bytes = Encoding.Unicode.GetBytes(pass);
            byte[] src = Encoding.Unicode.GetBytes(salt);
            byte[] dst = new byte[src.Length + bytes.Length];
            System.Buffer.BlockCopy(src, 0, dst, 0, src.Length);
            System.Buffer.BlockCopy(bytes, 0, dst, src.Length, bytes.Length);
            HashAlgorithm algorithm = HashAlgorithm.Create("SHA1");
            byte[] inArray = algorithm.ComputeHash(dst);
            return Convert.ToBase64String(inArray);    
            //return EncodePasswordMd5(Convert.ToBase64String(inArray));
        }
        

        public static string EncodePasswordMd5(string pass) //Encrypt using MD5    
        {
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;
            //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)    
            md5 = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(pass);
            encodedBytes = md5.ComputeHash(originalBytes);
            //Convert encoded bytes back to a 'readable' string    
            return BitConverter.ToString(encodedBytes);
        }
        public static string base64Encode(string sData) // Encode    
        {
            try
            {
                byte[] encData_byte = new byte[sData.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(sData);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }
        public static string base64Decode(string sData) //Decode    
        {
            try
            {
                var encoder = new System.Text.UTF8Encoding();
                System.Text.Decoder utf8Decode = encoder.GetDecoder();
                byte[] todecodeByte = Convert.FromBase64String(sData);
                int charCount = utf8Decode.GetCharCount(todecodeByte, 0, todecodeByte.Length);
                char[] decodedChar = new char[charCount];
                utf8Decode.GetChars(todecodeByte, 0, todecodeByte.Length, decodedChar, 0);
                string result = new String(decodedChar);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Decode" + ex.Message);
            }
        }

        /// <summary>
        /// Get User Login Response
        /// </summary>
        /// <param name="lo"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Get_Login_details(Models.Login lo)
        {
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                DataSet ds1 = new DataSet();
                SqlCommand cmd1 = new SqlCommand("SELECT VCode FROM Users WHERE UserName=@UserName", con);
                cmd1.Parameters.AddWithValue("@UserName", lo.UserName);
                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                da1.Fill(ds1);

                string hc = ds1.Tables[0].Rows[0][0].ToString();
                string pass = EncodePassword(lo.Password, hc);
                string response = string.Empty;
                con.Open();
                DataSet ds = new DataSet();
                //SqlCommand cmd = new SqlCommand("SELECT A.UserID,A.UserName,A.Email,A.IsSuperAdmin,A.IsAdmin,A.CompanyCode,B.RoleName,A.PlantCode,A.LastLoginDate,Y.Logo FROM Users AS A LEFT JOIN tbl_Role AS B ON A.RoleID=B.RoleID  LEFT JOIN tbl_Customer AS Y ON A.CompanyCode=Y.CompanyCode" +
                 //   " WHERE A.UserName='" + lo.UserName + "' AND A.Password='" + EncryptPassword(lo.Password) + "'", con);
                SqlCommand cmd = new SqlCommand("SELECT A.UserID,A.UserName,A.Email,A.IsSuperAdmin,A.IsAdmin,A.CompanyCode,B.RoleName,A.PlantCode,A.LastLoginDate,Y.Logo,A.CurrentLoginDate " +
                    "FROM Users AS A LEFT JOIN tbl_Role AS B ON A.RoleID = B.RoleID " +
                    "LEFT JOIN tbl_Customer AS Y ON A.CompanyCode = Y.CompanyCode " +
                    "WHERE A.UserName = @UserName AND A.Password = @Password", con);
                cmd.Parameters.AddWithValue("UserName", lo.UserName);
                cmd.Parameters.AddWithValue("Password", pass);
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
            }
        }

        [HttpPost]
        public IHttpActionResult Get_Customer_details(Models.Customer Cus)
        {
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                string response = string.Empty;
                con.Open();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("SELECT * FROM tbl_Customer WHERE CompanyCode=@CompanyCode", con);
                cmd.Parameters.AddWithValue("@CompanyCode", Cus.CompanyCode);
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
            }
        }


        [HttpPost]
        public IHttpActionResult Get_Plant_details(Models.Plant P)
        {
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                string response = string.Empty;
                con.Open();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("SELECT * FROM tbl_Plant WHERE PlantID=@PlantID ", con);
                cmd.Parameters.AddWithValue("@PlantID", P.PlantID);
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
            }
        }

        [HttpPost]
        public IHttpActionResult Get_Line_details(Models.Function F)
        {
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                string response = string.Empty;
                con.Open();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("SELECT * FROM tbl_Function WHERE FunctionID=@FunctionID", con);
                cmd.Parameters.AddWithValue("@FunctionID", F.FunctionID);
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
            }
        }


        /// <summary>
        /// Forgot Password
        /// </summary>
        /// <param name="fo"></param>
        /// <returns></returns>

        [HttpPost]
        public string Forgot_Password(Models.Forgot fo)
        {
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                con.Open();
                DataSet ds1 = new DataSet();
                SqlCommand cmd1 = new SqlCommand("SELECT VCode FROM Users WHERE Email=@Email", con);
                cmd1.Parameters.AddWithValue("@Email", fo.Input1);
                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                da1.Fill(ds1);
                object r1 = cmd1.ExecuteScalar();
                if (r1 != null)
                {
                    string hc = ds1.Tables[0].Rows[0][0].ToString();

                    string response = string.Empty;
                    string ran_pass = RandomPassword(6);
                    //con.Open();
                    SqlCommand cmd = new SqlCommand("SP_Forgotpassword", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@Input1", SqlDbType.NVarChar, 150).Value = fo.Input1;
                    cmd.Parameters.Add("@Input2", SqlDbType.NVarChar, 150).Value = EncodePassword(ran_pass, hc);
                    SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                    SQLReturn.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(SQLReturn);
                    cmd.ExecuteNonQuery();
                    response = SQLReturn.Value.ToString().Trim();
                    if (response == "OK")
                    {
                        using (SqlConnection con_mail = new SqlConnection(connectionstring))
                        {
                            con_mail.Open();
                            DataTable dt = new DataTable();
                            SqlCommand cmd_mail = new SqlCommand("SELECT * FROM tbl_gmail_settings", con_mail);
                            SqlDataAdapter da = new SqlDataAdapter(cmd_mail);
                            da.Fill(dt);
                            string sTime = DateTime.Now.AddMinutes(5).ToString("dd MMM yyyy") + " " + DateTime.Now.ToShortTimeString();
                            MailMessage mail = new MailMessage();
                            mail.To.Add(fo.Input1);
                            mail.From = new MailAddress(dt.Rows[0]["Smtp_user"].ToString());
                            var dateee = DateTime.Now.ToString();
                            DataTable dt1 = new DataTable();
                            SqlCommand cmd_mail1 = new SqlCommand("SELECT distinct a.[URL] FROM [dbo].[Portal_URL] a join users b on a.CompanyCode=b.[CompanyCode] and a.PlantCode=b.[PlantCode] where Email=@Email", con);
                            cmd_mail1.Parameters.AddWithValue("@Email", fo.Input1);
                            SqlDataAdapter da11 = new SqlDataAdapter(cmd_mail1);
                            da11.Fill(dt1);
                            var s = dt1.Rows[0]["URL"].ToString();
                            mail.Body += string.Format("<br/><br/><b>To Login in to portal " + "<a href='" + s + "'> click here</a><b><br/>");
                            mail.Body += string.Format("<br/> * **Mail generated from TEAL IIOT Portal Email App Service * **");
                            mail.Subject = "Forgot Password-New Password for TEAL IIOT Portal "+dateee+"; Do Not Reply";
                            mail.Body = string.Format("Dear user,<br/><br/>Please use the following password to login the TEAL IIOT Portal.<br/><b>New Password: </b> {0}<br/><b>Portal Login: <a href='"+s+ "'>click here</a> </b> <br/><br/>Got feedback? Please reach out to us: prakesh@titan.co.in ", ran_pass);
                            mail.Headers.Add("expiry-date", sTime);
                            mail.IsBodyHtml = true;
                            SmtpClient smtp = new SmtpClient();
                            smtp.Host = dt.Rows[0]["Smtp_host"].ToString();
                            smtp.Port = Convert.ToInt32(dt.Rows[0]["Smtp_port"].ToString());
                            smtp.UseDefaultCredentials = true;
                            smtp.Credentials = new System.Net.NetworkCredential(dt.Rows[0]["Smtp_user"].ToString(), dt.Rows[0]["Smtp_pass"].ToString());
                            smtp.EnableSsl = true;
                            smtp.Send(mail);
                            return response;
                        }
                    }
                    else
                    {
                        return response;
                    }
                }
                else
                {
                    Models.Forgot loginres = new Models.Forgot();
                    string response = "notok";
                    return response;
                }
            }
        }
        public static string GetTimestamp(DateTime value)
        {
            return value.ToString("dd/MM/yyyy hh:mm:ss");
        }
        [HttpPost]
        public string CheckPassword(Models.Forgot fo)
        {
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                con.Open();
                DataSet ds1 = new DataSet();
                SqlCommand cmd1 = new SqlCommand("SELECT  one_time FROM Users WHERE Email=@Email", con);
                cmd1.Parameters.AddWithValue("@Email", fo.Input1);

                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                da1.Fill(ds1);
                object r1 = cmd1.ExecuteScalar();
                if (r1 != null)
                {
                    DateTime t1 = (DateTime)r1;
                    DateTime updated = t1.Add(new TimeSpan(5, 30, 0));

                    DateTime t2 = DateTime.Now;
                    if (t2.TimeOfDay <= updated.TimeOfDay)
                    {


                        string response = "OK";

                        return response;

                    }
                    else
                    {
                        
                        SqlCommand cmd = new SqlCommand("update Users set Password=''  WHERE Email=@Email", con);
                        cmd.Parameters.AddWithValue("@Email", fo.Input1);
                        cmd.ExecuteNonQuery();
                        con.Close();
                        string response = "ONOK";
                        return response;
                    }
                }
                else
                {
                    string response = "NOK";
                    return response;
                }


            }
        }

        /// <summary>
        /// Change Password 
        /// </summary>
        /// <param name="Cha"></param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Changepassword(Models.Change_password Cha)
        {
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                con.Open();
                DataSet ds = new DataSet();
                SqlCommand cmd1 = new SqlCommand("SELECT VCode,Password FROM Users WHERE Email=@Email", con);
                cmd1.Parameters.AddWithValue("@Email", Cha.Input1);
                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                da.Fill(ds);
                object r1 = cmd1.ExecuteScalar();
               
                if (r1 != null)
                {
                    string response = string.Empty;
                    string hc = ds.Tables[0].Rows[0][0].ToString();
                    string pass = ds.Tables[0].Rows[0][1].ToString();
                    string encpass = EncodePassword(Cha.Input3, hc);
                    if (encpass != pass)
                    {

                        SqlCommand cmd = new SqlCommand("SP_ChangePassword", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add("@Input1", SqlDbType.NVarChar, 150).Value = Cha.Input1;
                        cmd.Parameters.Add("@Input2", SqlDbType.NVarChar, 150).Value = EncodePassword(Cha.Input2, hc);
                        cmd.Parameters.Add("@Input3", SqlDbType.NVarChar, 150).Value = EncodePassword(Cha.Input3, hc);
                        SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                        SQLReturn.Direction = ParameterDirection.Output;
                        cmd.Parameters.Add(SQLReturn);
                        cmd.ExecuteNonQuery();
                        response = SQLReturn.Value.ToString().Trim();
                        con.Close();
                    }
                    else
                    {
                         response = "Old and New Passwords are same";
                    }
                    if (response == "Changed")
                    {
                        return Ok("OK");
                    }
                    else if(response == "Old and New Passwords are same")
                    {
                        return Ok("Old and New Passwords are same");
                    }
                    else 
                    {
                        return Ok("NOTOK");
                    }
                }
                else
                {
                    return Ok("ENOK");
                }
            }
        }


        [HttpPost]
        public IHttpActionResult production_plan(Models.Setting U)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(U.Parameter1, U.Parameter, U.Parameter2);
            if (con_string == "0")
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Couldnot connect to database"));

            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    string date = DateTime.Now.ToString("yyyy-MM-dd");

                    con.Open();

                    DataSet ds = new DataSet();
                    //SqlCommand cmd = new SqlCommand(@"SELECT ID as id,t.Shift_Id as Shift_ID,f.functionname as Line_Code,mp.variantname as  Product_Name,t.TargetProduction as target_production,Fromdate as fromdate,Todate as todate FROM dbo.tbl_Production_setting t inner join tbl_MasterProduct mp on  mp.line_code=t.Line_Code and mp.CompanyCode=t.CompanyCode and mp.PlantCode=t.PlantCode and t.productname=mp.Variant_code inner join tbl_Function f on f.companycode=t.CompanyCode and f.parentplant=t.PlantCode WHERE t.CompanyCode='" + U.Parameter1 + "' AND t.PlantCode='" + U.Parameter + "'  ", con);
                    SqlCommand cmd = new SqlCommand(@"SELECT ID as id,t.Shift_Id as Shift_ID,f.functionname as Line_Code,mp.variantname as  Product_Name,
                    t.TargetProduction as target_production,Fromdate as fromdate,Todate as todate 
                    FROM dbo.tbl_Production_setting t 
                    inner join tbl_MasterProduct mp on  mp.line_code=t.Line_Code and mp.CompanyCode=t.CompanyCode and mp.Line_Code=t.Line_code 
                    and mp.PlantCode=t.PlantCode and t.productname=mp.Variant_code  
                    inner join tbl_Function f on f.companycode=t.CompanyCode and f.parentplant=t.PlantCode and f.FunctionID=t.Line_code 
                    WHERE t.CompanyCode=@company AND t.PlantCode=@plant and t.Line_code=@line", con);
                    cmd.Parameters.AddWithValue("@company", U.Parameter1);
                    cmd.Parameters.AddWithValue("@plant", U.Parameter);
                    cmd.Parameters.AddWithValue("@line", U.Parameter2);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(ds);
                    if (ds.Tables.Count != 0)
                    {
                        return Ok(ds.Tables[0]);
                        //return Request.CreateResponse(HttpStatusCode.OK, new { status = "Success", msg = "Details Found", data = ds });
                    }

                    return Ok(new String[0]);
                    //return Request.CreateResponse(HttpStatusCode.OK, new { status = "Success", msg = "Details Found", data = ds });


                }
            }
        }
        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage production_plan_edit(Models.Setting U)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(U.Parameter1, U.Parameter, U.Parameter2);
            if (con_string == "0")
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "Coudnot Connect to database", data = "" });

            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    string date = DateTime.Now.ToString("yyyy-MM-dd");

                    con.Open();

                    DataSet ds = new DataSet();
                    //SqlCommand cmd = new SqlCommand(@"SELECT ID as id,Shift_Id as Shift_ID,Line_COde as Line_Code,Productname as  Product_Name,TargetProduction as target_production,Fromdate as fromdate,Todate as todate FROM dbo.tbl_Production_setting WHERE CompanyCode='" + U.Parameter1 + "' AND PlantCode='" + U.Parameter + "'And id='"+U.QueryType+"'  ", con);

                    SqlCommand cmd = new SqlCommand(@"SELECT ID as id,Shift_Id as Shift_ID,Line_COde as Line_Code,Productname as  Product_Name,
                        TargetProduction as target_production,Fromdate as fromdate,Todate as todate 
                        FROM dbo.tbl_Production_setting 
                        WHERE CompanyCode=@company AND PlantCode=@plant And id=@id and Line_Code=@line  ", con);

                    cmd.Parameters.AddWithValue("@company", U.Parameter1);
                    cmd.Parameters.AddWithValue("@plant", U.Parameter);
                    cmd.Parameters.AddWithValue("@line", U.Parameter2);
                    cmd.Parameters.AddWithValue("@id", U.QueryType);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    da.Fill(ds);
                    if (ds.Tables.Count != 0)
                    {
                        // return Ok(ds.Tables[0]);
                        return Request.CreateResponse(HttpStatusCode.OK, new { status = "Success", msg = "Details Found", data = ds });
                    }

                    //return Ok(new String[0]);
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = "Success", msg = "Details Found", data = ds });


                }
            }
        }

        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage Ewon_details(Models.getewondetails details)
        {
            //database_connection d = new database_connection();
            //string con_string = d.Getconnectionstring(details.CompanyCode,details.PlantCode,details.linecode);
            //if (con_string == "0")
            //{
            //    return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "Coudnot Connect to database", data = "" });

            //}
            //else
            //{
           
                using (SqlConnection con = new SqlConnection(connectionstring))
                {

                    //cmd1.Parameters.AddWithValue("UserName", lo.UserName);

                    //SqlDataAdapter da = new SqlDataAdapter(cmd1);
                    //da.Fill(ds);
                    //object r1 = cmd1.ExecuteScalar();

                    //if (r1 != null)
                    //{ }
                    var result = new List<Models.EwonDetails>();
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SP_Ewon_Details", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = details.QueryType;
                    cmd.Parameters.Add("@Line_code", SqlDbType.NVarChar, 150).Value = details.linecode;

                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = details.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = details.PlantCode;
                    cmd.Parameters.Add("@Unique_id", SqlDbType.Int).Value = details.Unique_id;
                    cmd.ExecuteNonQuery();
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        result.Add(item: new Models.EwonDetails
                        {

                            linecode = Convert.ToString(reader["linecode"] == DBNull.Value ? "" : reader["linecode"]),
                            device_id = Convert.ToString(reader["device_id"] == DBNull.Value ? "" : reader["device_id"]),
                            deviceip = Convert.ToString(reader["deviceip"] == DBNull.Value ? "" : reader["deviceip"]),
                            devicename = Convert.ToString(reader["devicename"] == DBNull.Value ? "" : reader["devicename"]),
                            t2maccount = Convert.ToString(reader["t2maccount"] == DBNull.Value ? "" : reader["t2maccount"]),
                            t2musername = Convert.ToString(reader["t2musername"] == DBNull.Value ? "" : reader["t2musername"]),
                            t2mpassword = base64Decode(base64Decode(Convert.ToString(reader["t2mpassword"] == DBNull.Value ? "" : reader["t2mpassword"]))),
                            t2mdeveloperid = Convert.ToString(reader["t2mdeveloperid"] == DBNull.Value ? "" : reader["t2mdeveloperid"]),
                            t2mdeviceusername = Convert.ToString(reader["t2mdeviceusername"] == DBNull.Value ? "" : reader["t2mdeviceusername"]),
                            t2mdevicepassword = base64Decode(base64Decode(Convert.ToString(reader["t2mdevicepassword"] == DBNull.Value ? "" : reader["t2mdevicepassword"]))),
                            Unique_id = Convert.ToInt32(reader["id"]),
                            status = Convert.ToString(reader["status"] == DBNull.Value ? "" : reader["status"]),
                            ewonurl = Convert.ToString(reader["ewonurl"] == DBNull.Value ? "" : reader["ewonurl"]),
                        });
                    }
                    if (result.Any())
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { status = "Success", msg = "Details Found", data = result });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "No Data Available", data = "" });
                    }
                    //}
                
            }

            

        }
        [HttpPost]
        //[CustomAuthenticationFilter]
        public IHttpActionResult edit_update_Ewondetails(Models.EwonDetails details)
        {
            //database_connection d = new database_connection();
            //string con_string = d.Getconnectionstring(details.CompanyCode,details.PlantCode,details.linecode);
            //if (con_string == "0")
            //{
            //    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Couldnot connect to database"));

            //}
            //else
            //
            try
            {
                string response1 = string.Empty;
                var keynew = GeneratePassword(10);
                using (SqlConnection con = new SqlConnection(connectionstring))
                {
                    //var result = new List<Models.ReportMail>();
                    string response = string.Empty;
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SP_insert_update_EwonDetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = details.QueryType;
                    cmd.Parameters.Add("@Line_code", SqlDbType.NVarChar, 150).Value = details.linecode;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = details.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = details.PlantCode;
                    cmd.Parameters.Add("@Unique_id", SqlDbType.Int).Value = details.Unique_id;
                    cmd.Parameters.Add("@deviceid", SqlDbType.NVarChar, 150).Value = details.device_id;
                    cmd.Parameters.Add("@deviceip", SqlDbType.NVarChar, 150).Value = details.deviceip;
                    cmd.Parameters.Add("@devicename", SqlDbType.NVarChar, 150).Value = details.devicename;
                    cmd.Parameters.Add("@t2maccount", SqlDbType.NVarChar, 150).Value = details.t2maccount;
                    cmd.Parameters.Add("@t2musername", SqlDbType.NVarChar, 150).Value = details.t2musername;
                    cmd.Parameters.Add("@t2mpassword", SqlDbType.NVarChar, 150).Value = base64Encode(base64Encode(details.t2mpassword));
                    cmd.Parameters.Add("@t2mdeveloperid", SqlDbType.NVarChar, 150).Value = details.t2mdeveloperid;
                    cmd.Parameters.Add("@t2mdeviceusername", SqlDbType.NVarChar, 150).Value = details.t2mdeviceusername;
                    cmd.Parameters.Add("@t2mdevicepassword", SqlDbType.NVarChar, 150).Value = base64Encode(base64Encode(details.t2mdevicepassword));


                    cmd.Parameters.Add("@status", SqlDbType.NVarChar, 150).Value = details.status;
                    cmd.Parameters.Add("@ewonurl", SqlDbType.NVarChar, 150).Value = details.ewonurl;
                    SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                    SQLReturn.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(SQLReturn);
                    cmd.ExecuteNonQuery();
                    response = SQLReturn.Value.ToString().Trim();
                    if (response == "Inserted" || response == "Updated")
                    {
                        database_connection d = new database_connection();
                        string con_string = d.Getconnectionstring(details.CompanyCode, details.PlantCode, details.linecode);
                        if (con_string == "0")
                        {
                            return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Couldnot connect to database"));

                        }
                        else
                        {
                            using (SqlConnection con1 = new SqlConnection(con_string))
                            {

                                con1.Open();
                                SqlCommand cmd1 = new SqlCommand("SP_insert_update_EwonDetails", con1);
                                cmd1.CommandType = CommandType.StoredProcedure;
                                cmd1.CommandTimeout = 0;
                                cmd1.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = details.QueryType;
                                cmd1.Parameters.Add("@Line_code", SqlDbType.NVarChar, 150).Value = details.linecode;
                                cmd1.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = details.CompanyCode;
                                cmd1.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = details.PlantCode;
                                cmd1.Parameters.Add("@Unique_id", SqlDbType.Int).Value = details.Unique_id;
                                cmd1.Parameters.Add("@deviceid", SqlDbType.NVarChar, 150).Value = details.device_id;
                                cmd1.Parameters.Add("@deviceip", SqlDbType.NVarChar, 150).Value = details.deviceip;
                                cmd1.Parameters.Add("@devicename", SqlDbType.NVarChar, 150).Value = details.devicename;
                                cmd1.Parameters.Add("@t2maccount", SqlDbType.NVarChar, 150).Value = details.t2maccount;
                                cmd1.Parameters.Add("@t2musername", SqlDbType.NVarChar, 150).Value = details.t2musername;
                                cmd1.Parameters.Add("@t2mpassword", SqlDbType.NVarChar, 150).Value = base64Encode(base64Encode(details.t2mpassword));
                                cmd1.Parameters.Add("@t2mdeveloperid", SqlDbType.NVarChar, 150).Value = details.t2mdeveloperid;
                                cmd1.Parameters.Add("@t2mdeviceusername", SqlDbType.NVarChar, 150).Value = details.t2mdeviceusername;
                                cmd1.Parameters.Add("@t2mdevicepassword", SqlDbType.NVarChar, 150).Value = base64Encode(base64Encode(details.t2mdevicepassword));


                                cmd1.Parameters.Add("@status", SqlDbType.NVarChar, 150).Value = details.status;
                                cmd1.Parameters.Add("@ewonurl", SqlDbType.NVarChar, 150).Value = details.ewonurl;
                                SqlParameter SQLReturn1 = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                                SQLReturn1.Direction = ParameterDirection.Output;
                                cmd1.Parameters.Add(SQLReturn1);
                                cmd1.ExecuteNonQuery(); response1 = SQLReturn1.Value.ToString().Trim();
                            }
                        }

                        return Ok(response1);
                    }
                    else
                    {
                        return Ok(response);
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok("1");
            }
        }
        [HttpPost]
        //[CustomAuthenticationFilter]
        public IHttpActionResult delete_Ewondetails(Models.EwonDetails details)
        {
            //database_connection d = new database_connection();
            //string con_string = d.Getconnectionstring(details.CompanyCode,details.PlantCode,details.linecode);
            //if (con_string == "0")
            //{
            //    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Couldnot connect to database"));

            //}
            //else
            //
            try
            {
                string response1 = string.Empty;
                //var keynew = GeneratePassword(10);
                using (SqlConnection con = new SqlConnection(connectionstring))
                {
                    //var result = new List<Models.ReportMail>();
                    string response = string.Empty;
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SP_insert_update_EwonDetails", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = details.QueryType;
                    cmd.Parameters.Add("@Line_code", SqlDbType.NVarChar, 150).Value = details.linecode;
                    cmd.Parameters.Add("@deviceid", SqlDbType.NVarChar, 150).Value = details.device_id;
                    cmd.Parameters.Add("@deviceip", SqlDbType.NVarChar, 150).Value = "";
                    cmd.Parameters.Add("@devicename", SqlDbType.NVarChar, 150).Value = "";
                    cmd.Parameters.Add("@t2maccount", SqlDbType.NVarChar, 150).Value = "";
                    cmd.Parameters.Add("@t2musername", SqlDbType.NVarChar, 150).Value = "";
                    cmd.Parameters.Add("@t2mpassword", SqlDbType.NVarChar, 150).Value = "";
                    cmd.Parameters.Add("@t2mdeveloperid", SqlDbType.NVarChar, 150).Value = "";
                    cmd.Parameters.Add("@t2mdeviceusername", SqlDbType.NVarChar, 150).Value = "";
                    cmd.Parameters.Add("@t2mdevicepassword", SqlDbType.NVarChar, 150).Value = "";
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = details.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = details.PlantCode;
                    cmd.Parameters.Add("@Unique_id", SqlDbType.Int).Value = details.Unique_id;
                    cmd.Parameters.Add("@status", SqlDbType.NVarChar, 150).Value = "";
                    SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                    SQLReturn.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(SQLReturn);
                    cmd.ExecuteNonQuery();
                    response = SQLReturn.Value.ToString().Trim();
                    con.Close();
                    if (response == "Deleted Successfully")
                    {
                        database_connection d = new database_connection();
                        string con_string = d.Getconnectionstring(details.CompanyCode, details.PlantCode, details.linecode);
                        if (con_string == "0")
                        {
                            return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Couldnot connect to database"));

                        }
                        else
                        {
                            using (SqlConnection con1 = new SqlConnection(con_string))
                            {

                                con1.Open();
                                SqlCommand cmd1 = new SqlCommand("SP_insert_update_EwonDetails", con1);
                                cmd1.CommandType = CommandType.StoredProcedure;
                                cmd1.CommandTimeout = 0;
                                cmd1.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = details.QueryType;
                                cmd1.Parameters.Add("@Line_code", SqlDbType.NVarChar, 150).Value = details.linecode;
                                cmd1.Parameters.Add("@deviceid", SqlDbType.NVarChar, 150).Value = details.device_id;
                                cmd1.Parameters.Add("@deviceip", SqlDbType.NVarChar, 150).Value = "";
                                cmd1.Parameters.Add("@devicename", SqlDbType.NVarChar, 150).Value = "";
                                cmd1.Parameters.Add("@t2maccount", SqlDbType.NVarChar, 150).Value = "";
                                cmd1.Parameters.Add("@t2musername", SqlDbType.NVarChar, 150).Value = "";
                                cmd1.Parameters.Add("@t2mpassword", SqlDbType.NVarChar, 150).Value = "";
                                cmd1.Parameters.Add("@t2mdeveloperid", SqlDbType.NVarChar, 150).Value = "";
                                cmd1.Parameters.Add("@t2mdeviceusername", SqlDbType.NVarChar, 150).Value = "";
                                cmd1.Parameters.Add("@t2mdevicepassword", SqlDbType.NVarChar, 150).Value = "";
                                cmd1.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = details.CompanyCode;
                                cmd1.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = details.PlantCode;
                                cmd1.Parameters.Add("@Unique_id", SqlDbType.Int).Value = details.Unique_id;
                                cmd1.Parameters.Add("@status", SqlDbType.NVarChar, 150).Value = "";
                                SqlParameter SQLReturn1 = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                                SQLReturn1.Direction = ParameterDirection.Output;
                                cmd1.Parameters.Add(SQLReturn1);
                                cmd1.ExecuteNonQuery();
                                response1 = SQLReturn1.Value.ToString().Trim();
                            }
                        }

                        return Ok(response1);
                    }
                    else
                    {
                        return Ok(response);
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok("1");
            }
        }
        [HttpPost]
        //[CustomAuthenticationFilter]
        public IHttpActionResult delete_Function_settings_details(Models.Function U)
        {


            string response1 = string.Empty;
            string response = string.Empty;
            using (SqlConnection con = new SqlConnection(connectionstring))
            {

                con.Open();
                SqlCommand cmd = new SqlCommand("SP_Function", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = U.QueryType;
                cmd.Parameters.Add("@Unique_id", SqlDbType.NVarChar, 150).Value = "";
                cmd.Parameters.Add("@FunctionID", SqlDbType.NVarChar, 150).Value = U.FunctionID;
                cmd.Parameters.Add("@FunctionName", SqlDbType.NVarChar, 150).Value = "";
                cmd.Parameters.Add("@FunctionDescription", SqlDbType.NVarChar, 150).Value = "";
                cmd.Parameters.Add("@ParentPlant", SqlDbType.NVarChar, 150).Value = "";
                cmd.Parameters.Add("@IsActive", SqlDbType.NVarChar, 150).Value = "";
                cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = "";
                cmd.Parameters.Add("@Dept_id", SqlDbType.NVarChar, 150).Value = "";
                SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                SQLReturn.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(SQLReturn);
                cmd.ExecuteNonQuery();
                response = SQLReturn.Value.ToString().Trim();
                //return Ok(response);
                if (response.ToString() == "Deleted")
                {
                    database_connection d = new database_connection();
                    string con_string = d.Getconnectionstring(U.CompanyCode, U.PlantCode, U.Parameter2);
                    if (con_string == "0")
                    {
                        return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Couldnot connect to database"));

                    }
                    else
                    {
                        using (SqlConnection conn = new SqlConnection(con_string))
                        {
                            //string response1 = string.Empty;
                            conn.Open();
                            SqlCommand cmdd = new SqlCommand("SP_Function", conn);
                            cmdd.CommandType = CommandType.StoredProcedure;
                            cmdd.CommandTimeout = 0;
                            cmdd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = U.QueryType;
                            cmdd.Parameters.Add("@Unique_id", SqlDbType.NVarChar, 150).Value = "";
                            cmdd.Parameters.Add("@FunctionID", SqlDbType.NVarChar, 150).Value = U.FunctionID;
                            cmdd.Parameters.Add("@FunctionName", SqlDbType.NVarChar, 150).Value = "";
                            cmdd.Parameters.Add("@FunctionDescription", SqlDbType.NVarChar, 150).Value = "";
                            cmdd.Parameters.Add("@ParentPlant", SqlDbType.NVarChar, 150).Value = "";
                            cmdd.Parameters.Add("@IsActive", SqlDbType.NVarChar, 150).Value = "";
                            cmdd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = "";
                            cmdd.Parameters.Add("@Dept_id", SqlDbType.NVarChar, 150).Value = "";
                            SqlParameter SQLReturn1 = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                            SQLReturn1.Direction = ParameterDirection.Output;
                            cmdd.Parameters.Add(SQLReturn1);
                            cmdd.ExecuteNonQuery();
                            response = SQLReturn1.Value.ToString().Trim();
                            return Ok(response);
                        }
                    }
                }
                else
                {
                    return Ok(response);
                }

            }
        }




        //encrytion and decryption only for ewon details
        //public static string Protect(string unprotectedText)
        //{
        //var unprotectedBytes = Encoding.UTF8.GetBytes(unprotectedText);
        //    var protectedBytes = MachineKey.Protect(unprotectedBytes, Purpose);
        //    var protectedText = Convert.ToBase64String(protectedBytes);
        //    return protectedText;
        //}

        //public static string Unprotect(string protectedText)
        //{
        //    var protectedBytes = Convert.FromBase64String(protectedText);
        //    var unprotectedBytes = MachineKey.Unprotect(protectedBytes, Purpose);
        //    var unprotectedText = Encoding.UTF8.GetString(unprotectedBytes);
        //    return unprotectedText;
        //}
        [HttpPost]
        public IHttpActionResult Groupsusers_details(Models.UserGroups R)
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
                    SqlCommand cmd = new SqlCommand("SP_Groups_Users", con);
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
                SqlCommand cmd = new SqlCommand("SELECT userid,username,firstname,lastname from users where plantcode=@plantcode and companycode=@companycode ",con);
                cmd.Parameters.AddWithValue("@plantcode",U.Parameter);
                cmd.Parameters.AddWithValue("@companycode", U.Parameter1);
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
        [CustomAuthenticationFilter]
        public IHttpActionResult UserGroup_Allocation(Models.UserGroup_Allocation P)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(P.CompanyCode,P.PlantCode,P.LineCode);
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
                    SqlCommand cmd = new SqlCommand("SP_UserGroup_Permission", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = P.QueryType;
                    cmd.Parameters.Add("@Userid", SqlDbType.NVarChar, 150).Value = P.UserID;
                    cmd.Parameters.Add("@Groupid", SqlDbType.NVarChar, 150).Value = P.GroupID;
                    cmd.Parameters.Add("@Companycode", SqlDbType.NVarChar, 150).Value = P.CompanyCode;
                    cmd.Parameters.Add("@Plantcode", SqlDbType.NVarChar, 150).Value = P.PlantCode;
                    cmd.Parameters.Add("@Linecode", SqlDbType.NVarChar, 150).Value = P.LineCode;
                    SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                    SQLReturn.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(SQLReturn);
                    cmd.ExecuteNonQuery();
                    response = SQLReturn.Value.ToString().Trim();
                    return Ok(response);
                }
            }
        }
        [CustomAuthenticationFilter]

        public IHttpActionResult UserGroup_Deletion(Models.UserGroups P)
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

                    SqlCommand cmd = new SqlCommand("SP_UserGroup_Permission", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = "Delete";
                    cmd.Parameters.Add("@Groupid", SqlDbType.NVarChar, 150).Value = P.GroupID;
                    cmd.Parameters.Add("@Companycode", SqlDbType.NVarChar, 150).Value = P.CompanyCode;
                    cmd.Parameters.Add("@Plantcode", SqlDbType.NVarChar, 150).Value = P.PlantCode;
                    SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                    SQLReturn.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(SQLReturn);
                    cmd.ExecuteNonQuery();
                    response = SQLReturn.Value.ToString().Trim();
                    return Ok(response);
                }
            }
        }


        //area settings
        [HttpPost]
        public IHttpActionResult Area_details(Models.Area de)
        {

            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                string response = string.Empty;
                con.Open();
                SqlCommand cmd = new SqlCommand("SP_Area_details", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = de.QueryType;
                cmd.Parameters.Add("@Unique_id", SqlDbType.NVarChar, 150).Value = de.Unique_id;
                cmd.Parameters.Add("@Area_id", SqlDbType.NVarChar, 150).Value = de.Area_id;
                cmd.Parameters.Add("@Area_name", SqlDbType.NVarChar, 150).Value = de.Area_name;
                cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = de.CompanyCode;
                cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = de.PlantCode;
                SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                SQLReturn.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(SQLReturn);
                cmd.ExecuteNonQuery();
                response = SQLReturn.Value.ToString().Trim();
                return Ok(response);
            }
        }
        [HttpPost]
        public IHttpActionResult Subassembly_details(Models.Subassembly de)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(de.CompanyCode, de.PlantCode, de.LineCode);
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
                    SqlCommand cmd = new SqlCommand("SP_Subassembly_details", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = de.QueryType;
                    cmd.Parameters.Add("@Unique_id", SqlDbType.NVarChar, 150).Value = de.Unique_id;
                    cmd.Parameters.Add("@Subassembly_id", SqlDbType.NVarChar, 150).Value = de.Subassembly_id;
                    cmd.Parameters.Add("@Subassembly_name", SqlDbType.NVarChar, 150).Value = de.Subassembly_name;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = de.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = de.PlantCode;
                    cmd.Parameters.Add("@LineCode", SqlDbType.NVarChar, 150).Value = de.LineCode;
                    cmd.Parameters.Add("@MachineCode", SqlDbType.NVarChar, 150).Value = de.MachineCode;
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
        public IHttpActionResult Plantstructure(Models.GroupList param1)
        {
           
                using (SqlConnection con = new SqlConnection(connectionstring))
                {
                    DataSet ds = new DataSet();
                    con.Open();
                    SqlCommand cmd = new SqlCommand("select (Select distinct c.companycode,c.companyname,plantid,plantname," +
                        "area_id,area_name,dept.dept_id,dept_name," +
                        "Functionid,Functionname from tbl_customer c " +
                        "inner join tbl_plant b on c.companycode=b.ParentOrganization  " +
                        "inner join tbl_area nodes   on nodes.plantcode=b.plantid and nodes.companycode=b.ParentOrganization inner join " +
                        "tbl_department dept on nodes.area_id=dept.areacode inner join tbl_function" +
                        " f on f.dept_id=dept.dept_id and nodes.companycode=b.ParentOrganization where c.companycode=@CompanyCode " +
                        "and f.companycode=@CompanyCode for JSON AUTO ) as companystructure", con);
                    cmd.CommandTimeout = 0;
                    
                 cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = param1.CompanyCode;
                   // cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = "TEAL";
                cmd.ExecuteNonQuery();
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
                }
            
        }

        [HttpPost]
        [CustomAuthenticationFilter]
        public IHttpActionResult Line_Roles_details(Models.Roles R)
        {
           
                using (SqlConnection con = new SqlConnection(connectionstring))
                {
                    string response = string.Empty;
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SP_Line_Roles", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = R.QueryType;
                    cmd.Parameters.Add("@Unique_id", SqlDbType.NVarChar, 150).Value = R.Unique_id;
                    cmd.Parameters.Add("@RoleID", SqlDbType.NVarChar, 150).Value = R.RoleID;
                    cmd.Parameters.Add("@RoleName", SqlDbType.NVarChar, 150).Value = R.RoleName;
                    cmd.Parameters.Add("@RoleDescription", SqlDbType.NVarChar, 150).Value = R.RoleDescription;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = R.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = R.PlantCode;
                    //cmd.Parameters.Add("@LineCode", SqlDbType.NVarChar, 150).Value = R.Line_Code;
                    SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                    SQLReturn.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(SQLReturn);
                    cmd.ExecuteNonQuery();
                    response = SQLReturn.Value.ToString().Trim();
                    return Ok(response);
                }
            

        }


        [HttpPost]
        [CustomAuthenticationFilter]
        public IHttpActionResult Line_Permission_details(Models.Line_Permission P)
        {
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                con.Open();
                //if (P.QueryType == "Update")
                //{
                //    DataSet ds = new DataSet();
                //    SqlCommand cmd_qtype = new SqlCommand("Delete * FROM tbl_Line_Permission" +
                //          " WHERE RoleID=@RoleID", con);
                //    cmd_qtype.CommandTimeout = 0;
                //    cmd_qtype.Parameters.AddWithValue("RoleID", P.RoleID);
                  


                //    SqlDataAdapter da = new SqlDataAdapter(cmd_qtype);

                //    int count = (int)cmd_qtype.ExecuteScalar();
                //    if (count == 0)
                //    {
                //        P.QueryType = "Insert";
                //    }
                //}



                string response = string.Empty;

                SqlCommand cmd = new SqlCommand("SP_Line_Role_Permission", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = P.QueryType;
                //cmd.Parameters.Add("@Unique_id", SqlDbType.NVarChar, 150).Value = P.Unique_id;
               
                cmd.Parameters.Add("@Line_Code", SqlDbType.NVarChar, 150).Value = P.Line_Code;
                cmd.Parameters.Add("@Plant_Code", SqlDbType.NVarChar, 150).Value = P.Plant_Code;
                cmd.Parameters.Add("@RoleID", SqlDbType.NVarChar, 150).Value = P.RoleID;
                cmd.Parameters.Add("@Area_Code", SqlDbType.NVarChar, 150).Value = P.Area_Code;
                cmd.Parameters.Add("@Dept_Code", SqlDbType.NVarChar, 150).Value = P.Dept_Code;
              
                cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = P.CompanyCode;
                SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                SQLReturn.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(SQLReturn);
                cmd.ExecuteNonQuery();
                response = SQLReturn.Value.ToString().Trim();
                return Ok(response);
            }
        }
        [HttpPost]
        [CustomAuthenticationFilter]
        public IHttpActionResult manualupload(Models.manual h)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(h.CompanyCode, h.PlantCode, h.linecode);
            if (con_string == "0")
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Couldnot connect to database"));

            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    DataSet ds = new DataSet();
                    string response = string.Empty;
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SP_Manualupload", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = h.QueryType;
                    cmd.Parameters.Add("@id", SqlDbType.NVarChar, 150).Value = h.id;
                    cmd.Parameters.Add("@username", SqlDbType.NVarChar, 150).Value = h.UserName;
                    cmd.Parameters.Add("@filename", SqlDbType.NVarChar, 150).Value = h.filename;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = h.PlantCode;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = h.CompanyCode;
                    cmd.Parameters.Add("@Line_code", SqlDbType.NVarChar, 150).Value = h.linecode;
                    cmd.Parameters.Add("@remarks", SqlDbType.NVarChar, 150).Value = h.remarks;
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
        public IHttpActionResult uploaddetails(Models.manual h)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(h.CompanyCode, h.PlantCode, h.linecode);
            if (con_string == "0")
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Couldnot connect to database"));

            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    DataSet ds = new DataSet();
                    string response = string.Empty;
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SP_Manualupload", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = h.QueryType;
                    cmd.Parameters.Add("@id", SqlDbType.NVarChar, 150).Value = h.id;
                    cmd.Parameters.Add("@username", SqlDbType.NVarChar, 150).Value = h.UserName;
                    cmd.Parameters.Add("@filename", SqlDbType.NVarChar, 150).Value = h.filename;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = h.PlantCode;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = h.CompanyCode;
                    cmd.Parameters.Add("@Line_code", SqlDbType.NVarChar, 150).Value = h.linecode;
                    cmd.Parameters.Add("@remarks", SqlDbType.NVarChar, 150).Value = h.remarks;
                    SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                    SQLReturn.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(SQLReturn);
                    cmd.ExecuteNonQuery();
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
                }
            }
        }


        [HttpPost]
        [CustomAuthenticationFilter]
        public IHttpActionResult Cycle_time_details(Models.Cycletime_setting Cy)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(Cy.CompanyCode, Cy.PlantCode, Cy.Line_Code);
            if ((Cy.QueryType == "Insert" || Cy.QueryType == "Insert_others"))
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    string response = string.Empty;
                    string response2 = string.Empty;
                    string response1 = string.Empty;
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("SP_GetSettings_data", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = "Get_Variants";
                    cmd.Parameters.Add("@Parameter", SqlDbType.NVarChar, 150).Value = Cy.Machine;
                    cmd.Parameters.Add("@Parameter1", SqlDbType.NVarChar, 150).Value = Cy.CompanyCode;
                    cmd.Parameters.Add("@Parameter2", SqlDbType.NVarChar, 150).Value = Cy.PlantCode;
                    cmd.Parameters.Add("@Parameter4", SqlDbType.NVarChar, 150).Value = Cy.Line_Code;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (Cy.if_applicable == null)
                    {
                        SqlCommand cmd2 = new SqlCommand("SP_cycle_time", con);
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.CommandTimeout = 0;
                        cmd2.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = Cy.QueryType;
                        cmd2.Parameters.Add("@ID", SqlDbType.NVarChar, 150).Value = Cy.ID;
                        cmd2.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = Cy.CompanyCode;
                        cmd2.Parameters.Add("@Line_Code", SqlDbType.NVarChar, 150).Value = Cy.Line_Code;
                        cmd2.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = Cy.PlantCode;
                        cmd2.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = Cy.Machine;
                        cmd2.Parameters.Add("@Variant", SqlDbType.NVarChar, 150).Value = Cy.Variant;
                        cmd2.Parameters.Add("@Type", SqlDbType.NVarChar, 150).Value = Cy.Type;
                        cmd2.Parameters.Add("@Movement", SqlDbType.NVarChar, 150).Value = Cy.Movement;
                        cmd2.Parameters.Add("@Cycle_time", SqlDbType.NVarChar, 150).Value = Cy.Cycle_time;
                        cmd2.Parameters.Add("@Status", SqlDbType.NVarChar, 150).Value = Cy.Status;
                        cmd2.Parameters.Add("@if_applicable", SqlDbType.NVarChar, 150).Value = Cy.if_applicable;
                        SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                        SQLReturn.Direction = ParameterDirection.Output;
                        cmd2.Parameters.Add(SQLReturn);
                        cmd2.ExecuteNonQuery();
                        response1 = SQLReturn.Value.ToString().Trim();

                    }


                    else if (Cy.if_applicable == "Y")
                    {
                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            using (SqlConnection con1 = new SqlConnection(con_string))
                            {

                                con1.Open();
                                SqlCommand cmd1 = new SqlCommand("SP_cycle_time", con1);
                                cmd1.CommandType = CommandType.StoredProcedure;
                                cmd1.CommandTimeout = 0;
                                cmd1.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = Cy.QueryType;
                                cmd1.Parameters.Add("@ID", SqlDbType.NVarChar, 150).Value = Cy.ID;
                                cmd1.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = Cy.CompanyCode;
                                cmd1.Parameters.Add("@Line_Code", SqlDbType.NVarChar, 150).Value = Cy.Line_Code;
                                cmd1.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = Cy.PlantCode;
                                cmd1.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = Cy.Machine;
                                cmd1.Parameters.Add("@Variant", SqlDbType.NVarChar, 150).Value = ds.Tables[0].Rows[i]["Variant_Code"].ToString();
                                cmd1.Parameters.Add("@Type", SqlDbType.NVarChar, 150).Value = Cy.Type;
                                cmd1.Parameters.Add("@Movement", SqlDbType.NVarChar, 150).Value = Cy.Movement;
                                cmd1.Parameters.Add("@Cycle_time", SqlDbType.NVarChar, 150).Value = Cy.Cycle_time;
                                cmd1.Parameters.Add("@Status", SqlDbType.NVarChar, 150).Value = Cy.Status;
                                cmd1.Parameters.Add("@if_applicable", SqlDbType.NVarChar, 150).Value = Cy.if_applicable;
                                SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                                SQLReturn.Direction = ParameterDirection.Output;
                                cmd1.Parameters.Add(SQLReturn);
                                cmd1.ExecuteNonQuery();
                                response1 = SQLReturn.Value.ToString().Trim();
                            }
                        }
                    }
                    else
                    {
                        response1 = "No Variant";
                    }
                    return Ok(response1);
                }
            }
            else if (Cy.Variant == "All" && Cy.QueryType == "Update")
            {
                using (SqlConnection con1 = new SqlConnection(con_string))
                {

                    string response = string.Empty;
                    con1.Open();
                    SqlCommand cmd1 = new SqlCommand("SP_cycle_time", con1);
                    cmd1.CommandType = CommandType.StoredProcedure;
                    cmd1.CommandTimeout = 0;
                    cmd1.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = Cy.QueryType;
                    cmd1.Parameters.Add("@ID", SqlDbType.NVarChar, 150).Value = Cy.ID;
                    cmd1.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = Cy.CompanyCode;
                    cmd1.Parameters.Add("@Line_Code", SqlDbType.NVarChar, 150).Value = Cy.Line_Code;
                    cmd1.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = Cy.PlantCode;
                    cmd1.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = Cy.Machine;
                    cmd1.Parameters.Add("@Type", SqlDbType.NVarChar, 150).Value = Cy.Type;
                    cmd1.Parameters.Add("@Movement", SqlDbType.NVarChar, 150).Value = Cy.Movement;
                    cmd1.Parameters.Add("@Cycle_time", SqlDbType.NVarChar, 150).Value = Cy.Cycle_time;
                    cmd1.Parameters.Add("@Status", SqlDbType.NVarChar, 150).Value = Cy.Status;
                    cmd1.Parameters.Add("@if_applicable", SqlDbType.NVarChar, 150).Value = Cy.if_applicable;
                    SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                    SQLReturn.Direction = ParameterDirection.Output;
                    cmd1.Parameters.Add(SQLReturn);
                    cmd1.ExecuteNonQuery();
                    response = SQLReturn.Value.ToString().Trim();
                    return Ok(response);
                }
            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    string response = string.Empty;
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SP_cycle_time", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = Cy.QueryType;
                    cmd.Parameters.Add("@ID", SqlDbType.NVarChar, 150).Value = Cy.ID;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = Cy.CompanyCode;
                    cmd.Parameters.Add("@Line_Code", SqlDbType.NVarChar, 150).Value = Cy.Line_Code;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = Cy.PlantCode;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = Cy.Machine;
                    cmd.Parameters.Add("@Movement", SqlDbType.NVarChar, 150).Value = Cy.Movement;
                    cmd.Parameters.Add("@Type", SqlDbType.NVarChar, 150).Value = Cy.Type;
                    cmd.Parameters.Add("@Cycle_time", SqlDbType.NVarChar, 150).Value = Cy.Cycle_time;
                    cmd.Parameters.Add("@Status", SqlDbType.NVarChar, 150).Value = Cy.Status;
                    cmd.Parameters.Add("@if_applicable", SqlDbType.NVarChar, 150).Value = Cy.if_applicable;
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
        public IHttpActionResult Breaksettings_details(Models.BreakSettings SS)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(SS.CompanyCode, SS.PlantCode, SS.Line_Code);
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
                    SqlCommand cmd = new SqlCommand("SP_BreakSettings", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = SS.QueryType;
                    cmd.Parameters.Add("@ID", SqlDbType.NVarChar, 150).Value = SS.ID;
                    cmd.Parameters.Add("@ShiftID", SqlDbType.NVarChar, 150).Value = SS.ShiftID;
                    cmd.Parameters.Add("@StartTime", SqlDbType.Time, 150).Value = SS.StartTime;
                    cmd.Parameters.Add("@EndTime", SqlDbType.Time, 150).Value = SS.EndTime;
                    cmd.Parameters.Add("@BreakReason", SqlDbType.NVarChar, 150).Value = SS.BreakReason;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = SS.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = SS.PlantCode;
                    cmd.Parameters.Add("@LineCode", SqlDbType.NVarChar, 150).Value = SS.Line_Code;
                    SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                    SQLReturn.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(SQLReturn);
                    cmd.ExecuteNonQuery();
                    response = SQLReturn.Value.ToString().Trim();
                    return Ok(response);
                }
            }
        }



        //[HttpPost]
        //public IHttpActionResult Roles_details1(Models.Roles R)
        //{
        //    using (SqlConnection con = new SqlConnection(connectionstring))
        //    {
        //        string response = string.Empty;
        //        con.Open();
        //        SqlCommand cmd = new SqlCommand("SP_Roles", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandTimeout = 0;
        //        cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = R.QueryType;
        //        cmd.Parameters.Add("@Unique_id", SqlDbType.NVarChar, 150).Value = R.Unique_id;
        //        cmd.Parameters.Add("@RoleID", SqlDbType.NVarChar, 150).Value = R.RoleID;
        //        cmd.Parameters.Add("@RoleName", SqlDbType.NVarChar, 150).Value = R.RoleName;
        //        cmd.Parameters.Add("@RoleDescription", SqlDbType.NVarChar, 150).Value = R.RoleDescription;
        //        cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = R.CompanyCode;
        //        cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = R.PlantCode;
        //        cmd.Parameters.Add("@LineCode", SqlDbType.NVarChar, 150).Value = R.Line_Code;
        //        SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
        //        SQLReturn.Direction = ParameterDirection.Output;
        //        cmd.Parameters.Add(SQLReturn);
        //        cmd.ExecuteNonQuery();
        //        response = SQLReturn.Value.ToString().Trim();
        //        return Ok(response);
        //    }
        //}


        //[HttpPost]
        //public IHttpActionResult Permission_details1(Models.Permission P)
        //{
        //    //foreach (Models.Permission permissiondetails in groupData.Permission_data)
        //    //{

        //    //}

        //    using (SqlConnection con = new SqlConnection(connectionstring))
        //    {
        //        con.Open();
        //        string response = string.Empty;
        //        SqlCommand cmd = new SqlCommand("SP_Role_Permission", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.CommandTimeout = 0;
        //        cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = P.QueryType;
        //        cmd.Parameters.Add("@Unique_id", SqlDbType.NVarChar, 150).Value = P.Unique_id;
        //        cmd.Parameters.Add("@Permission_id", SqlDbType.NVarChar, 150).Value = P.Permission_id;
        //        cmd.Parameters.Add("@RoleID", SqlDbType.NVarChar, 150).Value = P.RoleID;
        //        cmd.Parameters.Add("@Module_name", SqlDbType.NVarChar, 150).Value = P.Module_name;
        //        cmd.Parameters.Add("@Edit_form", SqlDbType.NVarChar, 150).Value = P.Edit_form;
        //        cmd.Parameters.Add("@View_form", SqlDbType.NVarChar, 150).Value = P.View_form;
        //        cmd.Parameters.Add("@Delete_form", SqlDbType.NVarChar, 150).Value = P.Delete_form;
        //        cmd.Parameters.Add("@Add_form", SqlDbType.NVarChar, 150).Value = P.Add_form;
        //        cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = P.CompanyCode;
        //        SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
        //        SQLReturn.Direction = ParameterDirection.Output;
        //        cmd.Parameters.Add(SQLReturn);
        //        cmd.ExecuteNonQuery();
        //        response = SQLReturn.Value.ToString().Trim();
        //        return Ok(response);
        //    }
        //}



        [HttpPost]
        public IHttpActionResult Roles_details(Models.Roles R)
        {
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                string response = string.Empty;
                con.Open();
                SqlCommand cmd = new SqlCommand("SP_Roles", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = R.QueryType;
                cmd.Parameters.Add("@Unique_id", SqlDbType.NVarChar, 150).Value = R.Unique_id;
                cmd.Parameters.Add("@RoleID", SqlDbType.NVarChar, 150).Value = R.RoleID;
                cmd.Parameters.Add("@RoleName", SqlDbType.NVarChar, 150).Value = R.RoleName;
                cmd.Parameters.Add("@RoleDescription", SqlDbType.NVarChar, 150).Value = R.RoleDescription;
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

        [HttpPost]
        public IHttpActionResult Permission_details(Models.Permission P)
        {
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                con.Open();

                if (P.QueryType == "Update")
                {
                    DataSet ds = new DataSet();
                    SqlCommand cmd_qtype = new SqlCommand("SELECT count(*) FROM tbl_Permission" +
                          " WHERE Module_name=@Module_name AND Roleid=@Roleid", con);
                    cmd_qtype.CommandTimeout = 0;
                    cmd_qtype.Parameters.AddWithValue("Module_name", P.Module_name);
                    cmd_qtype.Parameters.AddWithValue("Roleid", P.RoleID);
                    SqlDataAdapter da = new SqlDataAdapter(cmd_qtype);

                    int count = (int)cmd_qtype.ExecuteScalar();
                    if (count == 0)
                    {
                        P.QueryType = "Insert";
                    }
                }


                string response = string.Empty;
                SqlCommand cmd = new SqlCommand("SP_Role_Permission", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = P.QueryType;
                cmd.Parameters.Add("@Unique_id", SqlDbType.NVarChar, 150).Value = P.Unique_id;
                cmd.Parameters.Add("@Permission_id", SqlDbType.NVarChar, 150).Value = P.Permission_id;
                cmd.Parameters.Add("@RoleID", SqlDbType.NVarChar, 150).Value = P.RoleID;
                cmd.Parameters.Add("@Module_name", SqlDbType.NVarChar, 150).Value = P.Module_name;
                cmd.Parameters.Add("@Edit_form", SqlDbType.NVarChar, 150).Value = P.Edit_form;
                cmd.Parameters.Add("@View_form", SqlDbType.NVarChar, 150).Value = P.View_form;
                cmd.Parameters.Add("@Delete_form", SqlDbType.NVarChar, 150).Value = P.Delete_form;
                cmd.Parameters.Add("@Add_form", SqlDbType.NVarChar, 150).Value = P.Add_form;
                cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = P.CompanyCode;
                SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                SQLReturn.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(SQLReturn);
                cmd.ExecuteNonQuery();
                response = SQLReturn.Value.ToString().Trim();
                return Ok(response);
            }
        }


        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage Get_hourly_tracker_data(Models.hourly_tra L)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(L.CompanyCode, L.PlantCode, L.Line_Code);
            if (con_string == "0")
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "Coudnot Connect to database", data = "" });

            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("SP_New_HourlyTrackerNew_Details", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Date", SqlDbType.NVarChar, 150).Value = L.Date;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = L.CompanyCode;
                    cmd.Parameters.Add("@Plantcode", SqlDbType.NVarChar, 150).Value = L.PlantCode;
                    cmd.Parameters.Add("@LineCode", SqlDbType.NVarChar, 150).Value = L.LineCode;
                    cmd.Parameters.Add("@MachineCode", SqlDbType.NVarChar, 150).Value = L.Machine;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { data = ds });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "No Data Available", data = "" });
                    }
                }
            }

        }


        [HttpPost]
        public IHttpActionResult URL_details1(Models.URL_table de)
        {

            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                string response = string.Empty;
                con.Open();
                SqlCommand cmd = new SqlCommand("SP_URL_details", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = de.QueryType;
                cmd.Parameters.Add("@Unique_id", SqlDbType.NVarChar, 150).Value = de.Unique_id;
                cmd.Parameters.Add("@url", SqlDbType.NVarChar, 150).Value = de.URL;
                cmd.Parameters.Add("@LineCode", SqlDbType.NVarChar, 150).Value = de.LineCode;
                cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = de.CompanyCode;
                cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = de.PlantCode;
                SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                SQLReturn.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(SQLReturn);
                cmd.ExecuteNonQuery();
                response = SQLReturn.Value.ToString().Trim();
                return Ok(response);
            }

        }



        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage Get_Historic_Rejection_Reason(Models.Rejections AS)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(AS.CompanyCode, AS.PlantCode, AS.Line_Code);
            if (con_string == "0")
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "Coudnot Connect to database", data = "" });

            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("SP_Show_Rejection", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = AS.CompanyCode;
                    cmd.Parameters.Add("@PlantCode ", SqlDbType.NVarChar, 150).Value = AS.PlantCode;
                    cmd.Parameters.Add("@line ", SqlDbType.NVarChar, 150).Value = AS.Line_Code;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = AS.Machine;
                    cmd.Parameters.Add("@RejectionCode", SqlDbType.NVarChar, 150).Value = AS.RejectionCode;
                    //cmd.Parameters.Add("@Alarm_Description", SqlDbType.NVarChar, 150).Value = AS.Alarm_Description;
                    cmd.Parameters.Add("@FromDate", SqlDbType.NVarChar, 150).Value = AS.FromDate;
                    cmd.Parameters.Add("@ToDate", SqlDbType.NVarChar, 150).Value = AS.ToDate;
                    cmd.Parameters.Add("@variant", SqlDbType.NVarChar, 150).Value = AS.variant;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { data = ds });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "No Data Available", data = "" });
                    }
                }
            }
        }

        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage Get_Historic_Rejection_Reason_yearwise(Models.Rejections AS)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(AS.CompanyCode, AS.PlantCode, AS.Line_Code);
            if (con_string == "0")
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "Coudnot Connect to database", data = "" });

            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("SP_Show_Rejection_yearwise", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = AS.CompanyCode;
                    cmd.Parameters.Add("@PlantCode ", SqlDbType.NVarChar, 150).Value = AS.PlantCode;
                    cmd.Parameters.Add("@line ", SqlDbType.NVarChar, 150).Value = AS.Line_Code;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = AS.Machine;
                    cmd.Parameters.Add("@RejectionCode", SqlDbType.NVarChar, 150).Value = AS.RejectionCode;
                    //cmd.Parameters.Add("@Alarm_Description", SqlDbType.NVarChar, 150).Value = AS.Alarm_Description;
                    cmd.Parameters.Add("@year", SqlDbType.NVarChar, 150).Value = AS.year;
                    cmd.Parameters.Add("@variant", SqlDbType.NVarChar, 150).Value = AS.variant;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { data = ds });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "No Data Available", data = "" });
                    }
                }
            }
        }
        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage Get_Historic_Rejection_Reason_monthwise(Models.Rejections AS)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(AS.CompanyCode, AS.PlantCode, AS.Line_Code);
            if (con_string == "0")
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "Coudnot Connect to database", data = "" });

            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("SP_Show_Rejection_monthwise", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = AS.CompanyCode;
                    cmd.Parameters.Add("@PlantCode ", SqlDbType.NVarChar, 150).Value = AS.PlantCode;
                    cmd.Parameters.Add("@line ", SqlDbType.NVarChar, 150).Value = AS.Line_Code;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = AS.Machine;
                    cmd.Parameters.Add("@RejectionCode", SqlDbType.NVarChar, 150).Value = AS.RejectionCode;
                    //cmd.Parameters.Add("@Alarm_Description", SqlDbType.NVarChar, 150).Value = AS.Alarm_Description;
                    cmd.Parameters.Add("@month", SqlDbType.NVarChar, 150).Value = AS.month;
                    cmd.Parameters.Add("@variant", SqlDbType.NVarChar, 150).Value = AS.variant;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { data = ds });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "No Data Available", data = "" });
                    }
                }
            }
        }




        //[HttpPost]
        //[CustomAuthenticationFilter]
        //public HttpResponseMessage Get_Historic_Rejection_Reason_yearwise(Models.Rejections AS)
        //{
        //    database_connection d = new database_connection();
        //    string con_string = d.Getconnectionstring(AS.CompanyCode, AS.PlantCode, AS.Line_Code);
        //    if (con_string == "0")
        //    {
        //        return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "Coudnot Connect to database", data = "" });

        //    }
        //    else
        //    {
        //        using (SqlConnection con = new SqlConnection(con_string))
        //        {
        //            con.Open();
        //            DataSet ds = new DataSet();
        //            SqlCommand cmd = new SqlCommand("SP_Show_Rejection_yearwise", con);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = AS.CompanyCode;
        //            cmd.Parameters.Add("@PlantCode ", SqlDbType.NVarChar, 150).Value = AS.PlantCode;
        //            cmd.Parameters.Add("@line ", SqlDbType.NVarChar, 150).Value = AS.Line_Code;
        //            cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = AS.Machine;
        //            cmd.Parameters.Add("@RejectionDesc", SqlDbType.NVarChar, 150).Value = AS.RejectionDescription;
        //            //cmd.Parameters.Add("@Alarm_Description", SqlDbType.NVarChar, 150).Value = AS.Alarm_Description;
        //            cmd.Parameters.Add("@year", SqlDbType.NVarChar, 150).Value = AS.year;
        //            cmd.Parameters.Add("@variant", SqlDbType.NVarChar, 150).Value = AS.variant;
        //            SqlDataAdapter da = new SqlDataAdapter(cmd);
        //            da.Fill(ds);
        //            if (ds.Tables[0].Rows.Count != 0)
        //            {
        //                return Request.CreateResponse(HttpStatusCode.OK, new { data = ds });
        //            }
        //            else
        //            {
        //                return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "No Data Available", data = "" });
        //            }
        //        }
        //    }
        //}



        //[HttpPost]
        //[CustomAuthenticationFilter]
        //public HttpResponseMessage Get_Historic_Rejection_Reason_monthwise(Models.Rejections AS)
        //{
        //    database_connection d = new database_connection();
        //    string con_string = d.Getconnectionstring(AS.CompanyCode, AS.PlantCode, AS.Line_Code);
        //    if (con_string == "0")
        //    {
        //        return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "Coudnot Connect to database", data = "" });

        //    }
        //    else
        //    {
        //        using (SqlConnection con = new SqlConnection(con_string))
        //        {
        //            con.Open();
        //            DataSet ds = new DataSet();
        //            SqlCommand cmd = new SqlCommand("SP_Show_Rejection_monthwise", con);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = AS.CompanyCode;
        //            cmd.Parameters.Add("@PlantCode ", SqlDbType.NVarChar, 150).Value = AS.PlantCode;
        //            cmd.Parameters.Add("@line ", SqlDbType.NVarChar, 150).Value = AS.Line_Code;
        //            cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = AS.Machine;
        //            cmd.Parameters.Add("@RejectionDesc", SqlDbType.NVarChar, 150).Value = AS.RejectionDescription;
        //            //cmd.Parameters.Add("@Alarm_Description", SqlDbType.NVarChar, 150).Value = AS.Alarm_Description;
        //            cmd.Parameters.Add("@month", SqlDbType.NVarChar, 150).Value = AS.month;
        //            cmd.Parameters.Add("@variant", SqlDbType.NVarChar, 150).Value = AS.variant;
        //            SqlDataAdapter da = new SqlDataAdapter(cmd);
        //            da.Fill(ds);
        //            if (ds.Tables[0].Rows.Count != 0)
        //            {
        //                return Request.CreateResponse(HttpStatusCode.OK, new { data = ds });
        //            }
        //            else
        //            {
        //                return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "No Data Available", data = "" });
        //            }
        //        }
        //    }
        //}





        [HttpPost]
        public IHttpActionResult Show_Holiday_details(Models.holiday h)
        {
            try
            {

                database_connection d = new database_connection();
                string con_string = d.Getconnectionstring(h.CompanyCode, h.PlantID, h.LineCode);

                using (SqlConnection con = new SqlConnection(con_string))
                {
                    string response = string.Empty;
                    string res = string.Empty;

                    con.Open();
                    SqlCommand cmd = new SqlCommand("SP_Show_holiday", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@PlantID", SqlDbType.NVarChar, 150).Value = h.PlantID;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = h.CompanyCode;
                    SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);

                    SQLReturn.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(SQLReturn);
                    cmd.ExecuteNonQuery();
                    response = SQLReturn.Value.ToString().Trim();


                    if (response == "0")
                    {
                        res = "";
                    }
                    else if (response == "1")
                    {
                        res = "1";
                    }
                    else if (response == "2")
                    {
                        res = "Sunday";
                    }
                    else
                    {
                        res = response;
                    }

                    return Ok(res);
                }
            }
            catch(Exception e)
            {
                return Ok();
            }
        }





        [HttpPost]
        [CustomAuthenticationFilter]
        public IHttpActionResult new_delete_Holiday_settings_details(Models.Setting U)
        {

            string response = string.Empty;
            using (SqlConnection con = new SqlConnection(connectionstring))
            {

                con.Open();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("SP_delete_usersettings", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = U.QueryType;
                cmd.Parameters.Add("@Parameter", SqlDbType.NVarChar, 150).Value = U.Parameter;
                cmd.Parameters.Add("@Parameter1", SqlDbType.NVarChar, 150).Value = U.Parameter1;
                cmd.ExecuteNonQuery();

            }
            database_connection db = new database_connection();

            string con_string = db.Getconnectionstring(U.CompanyCode, U.PlantCode, U.LineCode);

            if (con_string == "0")
            {
                return Ok("No Connection");

            }
            else
            {
                using (SqlConnection conn = new SqlConnection(con_string))
                {

                    conn.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmdd = new SqlCommand("SP_delete_usersettings", conn);
                    cmdd.CommandType = CommandType.StoredProcedure;
                    cmdd.CommandTimeout = 0;
                    cmdd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = U.QueryType;
                    cmdd.Parameters.Add("@Parameter", SqlDbType.NVarChar, 150).Value = U.Parameter;
                    cmdd.Parameters.Add("@Parameter1", SqlDbType.NVarChar, 150).Value = U.Parameter1;

                    cmdd.ExecuteNonQuery();
                    //  response = "Selected record is deleted";

                }
            }

            return Ok(response);


        }


        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage Get_DataPostingStatus(Models.dataPostingStatus AS)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(AS.CompanyCode, AS.PlantCode, AS.LineCode);
            if (con_string == "0")
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "Coudnot Connect to database", data = "" });

            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("SP_DataPostingStatus", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@duration", SqlDbType.NVarChar, 150).Value = AS.Duration;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { data = ds });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "No Data Available", data = "" });
                    }
                }
            }
        }


        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage Popup_Machinewise_Live_Hourly(Models.popupHourly L)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(L.CompanyCode, L.PlantCode, L.Line_Code);
            if (con_string == "0")
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "Coudnot Connect to database", data = "" });

            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("SP_Popup_Machinewise_Live_Hourly", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Date", SqlDbType.NVarChar, 150).Value = L.Date;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = L.CompanyCode;
                    cmd.Parameters.Add("@Plantcode", SqlDbType.NVarChar, 150).Value = L.PlantCode;
                    cmd.Parameters.Add("@LineCode", SqlDbType.NVarChar, 150).Value = L.Line_Code;
                    cmd.Parameters.Add("@MachineCode", SqlDbType.NVarChar, 150).Value = L.Machine;
                    cmd.Parameters.Add("@Shift", SqlDbType.NVarChar, 150).Value = L.Shift;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { data = ds });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "No Data Available", data = "" });
                    }
                }
            }

        }



    }
}
