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
    public class AlertController : ApiController
    {
        //retrieving connection string from web config
        private string connectionstring = ConfigurationManager.ConnectionStrings["con"].ToString();

        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage Getalerthistorical(Models.Alert_History alert)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(alert.CompanyCode, alert.PlantCode, alert.LineCode);
            if (con_string == "0")
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "Coudnot Connect to database", data = "" });
                
            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    //To Get the response from database and display 
                    var result = new List<Models.Alert_History>();
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SPAlertSummary", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add("@LineCode", SqlDbType.NVarChar, 150).Value = alert.LineCode;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = alert.MachineCode;
                    cmd.Parameters.Add("@Fromdate", SqlDbType.Date, 150).Value = alert.fromdate;
                    cmd.Parameters.Add("@Todate", SqlDbType.Date, 150).Value = alert.todate;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = alert.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = alert.PlantCode;
                    var reader = cmd.ExecuteReader();
                    //bind values returned from sp to the model 
                    while (reader.Read())
                    {
                        result.Add(item: new Models.Alert_History
                        {

                            AlertID = Convert.ToString(reader["AlertID"] == DBNull.Value ? "" : reader["AlertID"]),

                            //Alerttext= Convert.ToString(reader["AlertText"] == DBNull.Value ? "" : reader["AlertText"]),
                            //AlertDateAndTime = Convert.ToDateTime(reader["AlertDateAndTime"] == DBNull.Value ? null : reader["AlertDateAndTime"]),

                            Alerttext = Convert.ToString(reader["MessageText"] == DBNull.Value ? "" : reader["MessageText"]),
                            AlertDateAndTime = Convert.ToDateTime(reader["AlertDateAndTime"] == DBNull.Value ? "" : reader["AlertDateAndTime"]),

                            SentGroup = Convert.ToString(reader["SentGroup"] == DBNull.Value ? 0 : reader["SentGroup"]),
                            InstanceCount = Convert.ToInt32(reader["InstanceCount"] == DBNull.Value ? 0 : reader["InstanceCount"]),



                            Counts = Convert.ToInt32(reader["counts"] == DBNull.Value ? 0 : reader["counts"]),
                        }); ;
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


        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage Defaultalerthistorical(Models.Alert_History alert)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(alert.CompanyCode, alert.PlantCode, alert.LineCode);
            if (con_string == "0")
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "Coudnot Connect to database", data = "" });

            }
            else
            {

                using (SqlConnection con = new SqlConnection(con_string))
                {
                    //To Get the response from database and display 
                    var result = new List<Models.Alert_History>();
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SPAlertSummary_Default", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add("@LineCode", SqlDbType.NVarChar, 150).Value = alert.LineCode;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = alert.MachineCode;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = alert.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = alert.PlantCode;
                    var reader = cmd.ExecuteReader();
                    //bind values returned from sp to the model 
                    while (reader.Read())
                    {
                        result.Add(item: new Models.Alert_History
                        {

                            AlertID = Convert.ToString(reader["AlertID"] == DBNull.Value ? "" : reader["AlertID"]),

                            //Alerttext= Convert.ToString(reader["AlertText"] == DBNull.Value ? "" : reader["AlertText"]),
                            //AlertDateAndTime = Convert.ToDateTime(reader["AlertDateAndTime"] == DBNull.Value ? null : reader["AlertDateAndTime"]),

                            Alerttext = Convert.ToString(reader["MessageText"] == DBNull.Value ? "" : reader["MessageText"]),
                            AlertDateAndTime = Convert.ToDateTime(reader["AlertDateAndTime"] == DBNull.Value ? "" : reader["AlertDateAndTime"]),

                            SentGroup = Convert.ToString(reader["SentGroup"] == DBNull.Value ? 0 : reader["SentGroup"]),
                            InstanceCount = Convert.ToInt32(reader["InstanceCount"] == DBNull.Value ? 0 : reader["InstanceCount"]),



                            Counts = Convert.ToInt32(reader["counts"] == DBNull.Value ? 0 : reader["counts"]),
                        }); ;
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


        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage settings_details(Models.Setting U)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(U.CompanyCode,U.PlantCode,U.LineCode);
            if (con_string == "0")
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "Coudnot Connect to database", data = "" });

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
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = U.QueryType;
                    cmd.Parameters.Add("@Parameter", SqlDbType.NVarChar, 150).Value = U.Parameter;
                    cmd.Parameters.Add("@Parameter1", SqlDbType.NVarChar, 150).Value = U.Parameter1;
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
        }

        [HttpPost]
        [CustomAuthenticationFilter]
        public IHttpActionResult Alert_details(Models.Setting U)
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
                    string response = string.Empty;
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("SP_GetSettings_data", con);
                    cmd.CommandType = CommandType.StoredProcedure;
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

                    //  return Ok(ds.Tables[0]);
                }
            }
        }



        [HttpPost]
        [CustomAuthenticationFilter]
        public IHttpActionResult Add_Alert(Models.Alert A)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(A.CompanyCode, A.PlantCode, A.Line_Code);
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
                    SqlCommand cmd = new SqlCommand("sp_alerts", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = A.QueryType;
                    cmd.Parameters.Add("@Machine_Code", SqlDbType.NVarChar, 150).Value = A.Machine_Code;
                    cmd.Parameters.Add("@AlertID", SqlDbType.NVarChar, 150).Value = A.alertID;
                    cmd.Parameters.Add("@unique_id", SqlDbType.NVarChar, 150).Value = A.unique_id;
                    cmd.Parameters.Add("@Alert_Name", SqlDbType.NVarChar, 150).Value = A.Alert_Name;

                    cmd.Parameters.Add("@P1_Variable", SqlDbType.NVarChar, 150).Value = A.P1_Variable;
                    cmd.Parameters.Add("@P1_PG", SqlDbType.NVarChar, 150).Value = A.P1_PG;
                    cmd.Parameters.Add("@P1_Param", SqlDbType.NVarChar, 150).Value = A.P1_Param;
                    cmd.Parameters.Add("@P2_Variable", SqlDbType.NVarChar, 150).Value = A.P2_Variable;
                    cmd.Parameters.Add("@P2_PG", SqlDbType.NVarChar, 150).Value = A.P2_PG;
                    cmd.Parameters.Add("@P2_Param", SqlDbType.NVarChar, 150).Value = A.P2_Param;
                    cmd.Parameters.Add("@Expression", SqlDbType.NVarChar, 150).Value = A.Expression;
                    cmd.Parameters.Add("@Constant", SqlDbType.NVarChar, 150).Value = A.Constant;
                    cmd.Parameters.Add("@DurationForAlert", SqlDbType.NVarChar, 150).Value = A.DurationForAlert;
                    cmd.Parameters.Add("@Group_id1", SqlDbType.Int).Value = A.Group_id1;
                    cmd.Parameters.Add("@Group_id2", SqlDbType.Int).Value = A.Group_id2;
                    cmd.Parameters.Add("@Group_id3", SqlDbType.Int).Value = A.Group_id3;
                    cmd.Parameters.Add("@MessageText", SqlDbType.NVarChar, 150).Value = A.MessageText;
                    cmd.Parameters.Add("@DurationForRepetetion", SqlDbType.NVarChar, 150).Value = A.DurationForRepetetion;

                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = A.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = A.PlantCode;
                    cmd.Parameters.Add("@Line_Code", SqlDbType.NVarChar, 150).Value = A.Line_Code;
                    cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar, 150).Value = A.Remarks;

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
        //[CustomAuthenticationFilter]
        //public IHttpActionResult delete_details(Models.Setting U)
        //{
        //    using (SqlConnection con = new SqlConnection(connectionstring))
        //    {
        //        string response = string.Empty;
        //        con.Open();
        //        DataSet ds = new DataSet();
        //        SqlCommand cmd = new SqlCommand("SP_delete_usersettings", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = U.QueryType;
        //        cmd.Parameters.Add("@Parameter", SqlDbType.NVarChar, 150).Value = U.Parameter;
        //        cmd.Parameters.Add("@Parameter1", SqlDbType.NVarChar, 150).Value = U.Parameter1;
        //        cmd.ExecuteNonQuery();
        //        return Ok(response);
        //    }
        //}



        [HttpPost]
        [CustomAuthenticationFilter]
        public IHttpActionResult Bulk_copy_alert(Models.Alert A)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(A.CompanyCode, A.PlantCode, A.Line_Code);
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
                    SqlCommand cmd = new SqlCommand("SP_bulkcopy_Machine", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Machine_Code1", SqlDbType.NVarChar, 150).Value = A.Machine_Code1;
                    cmd.Parameters.Add("@Machine_Code2", SqlDbType.NVarChar, 150).Value = A.Machine_Code2;
                    cmd.Parameters.Add("@Line_Code1", SqlDbType.NVarChar, 150).Value = A.Line_Code;
                    cmd.Parameters.Add("@CompanyCode1", SqlDbType.NVarChar, 150).Value = A.CompanyCode;
                    cmd.Parameters.Add("@PlantCode1", SqlDbType.NVarChar, 150).Value = A.PlantCode;
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
        //public IHttpActionResult Bulk_copy_group(Models.Alert A)
        //{
        //    using (SqlConnection con = new SqlConnection(connectionstring))
        //    {
        //        string response = string.Empty;
        //        con.Open();
        //        SqlCommand cmd = new SqlCommand("SP_bulkcopy_Group", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("@Group_id1", SqlDbType.NVarChar, 150).Value = A.Group_id1;
        //        cmd.Parameters.Add("@Group_id2", SqlDbType.NVarChar, 150).Value = A.Group_id2;

        //        SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
        //        SQLReturn.Direction = ParameterDirection.Output;
        //        cmd.Parameters.Add(SQLReturn);
        //        cmd.ExecuteNonQuery();
        //        response = SQLReturn.Value.ToString().Trim();
        //        return Ok(response);

        //    }
        //}


        [HttpPost]
        [CustomAuthenticationFilter]
        public IHttpActionResult Defaultalertcomments(Models.Alert_Comments alert)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(alert.CompanyCode,alert.PlantCode,alert.LineCode);
            if (con_string == "0")
            {

                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Couldnot connect to database"));
            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    DataSet ds = new DataSet();
                    //To Get the response from database and display 
                    var result = new List<Models.Alert_Comments>();
                    con.Open();

                    var type = "";

                    SqlCommand cmd = new SqlCommand("SP_list_alert_comments_Default", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    if (alert.InstanceStartTime == "AllInstance")
                    {
                        type = "CommentDetails_All";
                    }
                    else
                    {
                        type = "CommentDetails_history";
                        cmd.Parameters.Add("@start", SqlDbType.DateTime).Value = Convert.ToDateTime(alert.InstanceStartTime);
                    }

                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = type;
                    cmd.Parameters.Add("@Line_code", SqlDbType.NVarChar, 150).Value = alert.LineCode;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = alert.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = alert.PlantCode;
                    cmd.Parameters.Add("@machine", SqlDbType.NVarChar, 150).Value = alert.MachineCode;
                    cmd.Parameters.Add("@alertid", SqlDbType.NVarChar, 150).Value = alert.AlertID;
                    cmd.ExecuteNonQuery();


                    

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    con.Close();
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

        [System.Web.Http.HttpPost]
        [CustomAuthenticationFilter]
        public IHttpActionResult Defaultalertid(Models.Getalertid alert)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(alert.CompanyCode, alert.PlantCode, alert.LineCode);
            if (con_string == "0")
            {

                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Couldnot connect to database"));
            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    //To Get the response from database and display 
                    var result = new List<Models.Alert_Comments>();
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT distinct top 5 Alertid FROM tbl_Comments  " +
                        "WHERE CompanyCode=@CompanyCode and Plantcode=@PlantCode and Machine_Code=@Machine and line_code=@LineCode", con);
                    DataSet ds = new DataSet();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add("@LineCode", SqlDbType.NVarChar, 150).Value = alert.LineCode;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = alert.MachineCode;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = alert.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = alert.PlantCode;
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
        public IHttpActionResult Defaultinstance(Models.Getalertid alert)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(alert.CompanyCode, alert.PlantCode,alert.LineCode);
            if (con_string == "0")
            {

                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Couldnot connect to database"));
            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    //To Get the response from database and display 
                    var result = new List<Models.Alert_Comments>();
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT distinct top 5 InstanceStartTime FROM tbl_Comments  " +
                        "WHERE CompanyCode=@CompanyCode and Plantcode=@PlantCode and Machine_Code=@Machine and line_code=@LineCode " +
                        "and alertid=@alertid", con);
                    DataSet ds = new DataSet();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add("@LineCode", SqlDbType.NVarChar, 150).Value = alert.LineCode;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = alert.MachineCode;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = alert.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = alert.PlantCode;
                    cmd.Parameters.Add("@alertid", SqlDbType.NVarChar, 150).Value = alert.AlertID;
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
        public IHttpActionResult Getalertcomments(Models.Alert_Comments alert)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(alert.CompanyCode, alert.PlantCode,alert.LineCode);
            if (con_string == "0")
            {

                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Couldnot connect to database"));
            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    DataSet ds = new DataSet();
                    //To Get the response from database and display 
                    var result = new List<Models.Alert_Comments>();
                    con.Open();

                    var type = "";

                    SqlCommand cmd = new SqlCommand("SP_list_alert_comments", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    if (alert.InstanceStartTime == "AllInstance")
                    {
                        type = "CommentDetails_All";
                    }
                    else
                    {
                        type = "CommentDetails_history";
                        cmd.Parameters.Add("@start", SqlDbType.DateTime).Value = Convert.ToDateTime(alert.InstanceStartTime);
                    }

                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = type;
                    cmd.Parameters.Add("@Line_code", SqlDbType.NVarChar, 150).Value = alert.LineCode;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = alert.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = alert.PlantCode;
                    cmd.Parameters.Add("@machine", SqlDbType.NVarChar, 150).Value = alert.MachineCode;
                    cmd.Parameters.Add("@alertid", SqlDbType.NVarChar, 150).Value = alert.AlertID;
                    cmd.Parameters.Add("@Fromdate", SqlDbType.DateTime).Value = alert.fromdate;
                    cmd.Parameters.Add("@Todate", SqlDbType.DateTime).Value = alert.todate;
                    cmd.ExecuteNonQuery();


                  


                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    con.Close();
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
        
        [System.Web.Http.HttpPost]
        [CustomAuthenticationFilter]
        public IHttpActionResult Getalertid(Models.Getalertid alert)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(alert.CompanyCode, alert.PlantCode, alert.LineCode);
            if (con_string == "0")
            {

                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Couldnot connect to database"));
            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    //To Get the response from database and display 
                    var result = new List<Models.Alert_Comments>();
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT Distinct Alertid FROM tbl_Comments WHERE CompanyCode=@CompanyCode and Plantcode=@PlantCode and Machine_Code=@Machine and line_code=@LineCode and CommentDateTime " +
                        "BETWEEN Convert(datetime,@Fromdate) AND (Convert(datetime,@Todate)+1)", con);
                    DataSet ds = new DataSet();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add("@LineCode", SqlDbType.NVarChar, 150).Value = alert.LineCode;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = alert.MachineCode;
                    cmd.Parameters.Add("@Fromdate", SqlDbType.DateTime).Value = alert.fromdate;
                    cmd.Parameters.Add("@Todate", SqlDbType.DateTime).Value = alert.todate;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = alert.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = alert.PlantCode;
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
        public IHttpActionResult Getinstance(Models.Getalertid alert)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(alert.CompanyCode, alert.PlantCode, alert.LineCode);
            if (con_string == "0")
            {

                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Couldnot connect to database"));
            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    //To Get the response from database and display 
                    var result = new List<Models.Alert_Comments>();
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SELECT distinct InstanceStartTime FROM tbl_Comments " +
                        "WHERE CompanyCode=@CompanyCode and Plantcode=@PlantCode and Machine_Code=@Machine and line_code=@LineCode " +
                        "and CommentDatetIme BETWEEN Convert(datetime,@Fromdate) AND (Convert(datetime,@Todate)+1) " +
                        "and alertid=@alertid", con);
                    DataSet ds = new DataSet();
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add("@LineCode", SqlDbType.NVarChar, 150).Value = alert.LineCode;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = alert.MachineCode;
                    cmd.Parameters.Add("@Fromdate", SqlDbType.DateTime).Value = alert.fromdate;
                    cmd.Parameters.Add("@Todate", SqlDbType.DateTime).Value = alert.todate;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = alert.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = alert.PlantCode;
                    cmd.Parameters.Add("@alertid", SqlDbType.NVarChar, 150).Value = alert.AlertID;
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
    }
}