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
    public class DiagnosticsController : ApiController
    {
        //retrieving connection string from web config
        private string connectionstring = ConfigurationManager.ConnectionStrings["con"].ToString();
        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage GetDiagnostichistorical(Models.Diagnostics alert)
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
                    var result = new List<Models.Diagnostics>();
                    con.Open();
                    SqlCommand cmd = new SqlCommand("sp_Diagnostic_history", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add("@LineCode", SqlDbType.NVarChar, 150).Value = alert.LineCode;
                    cmd.Parameters.Add("@Fromdate", SqlDbType.Date, 150).Value = alert.fromdate;
                    cmd.Parameters.Add("@Todate", SqlDbType.Date, 150).Value = alert.todate;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = alert.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = alert.PlantCode;
                    var reader = cmd.ExecuteReader();
                    //bind values returned from sp to the model 
                    while (reader.Read())
                    {
                        result.Add(item: new Models.Diagnostics
                        {

                            DeviceID = Convert.ToString(reader["Device_ID"] == DBNull.Value ? "" : reader["Device_ID"]),
                            DeviceRef = Convert.ToString(reader["Device_ref"] == DBNull.Value ? "" : reader["Device_ref"]),
                            Devicename = Convert.ToString(reader["DeviceName"] == DBNull.Value ? "" : reader["DeviceName"]),
                            Event = Convert.ToString(reader["EventName"] == DBNull.Value ? 0 : reader["EventName"]),
                            LogTime = Convert.ToDateTime(reader["LogTime"] == DBNull.Value ? 0 : reader["LogTime"]),

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
        public HttpResponseMessage DefaultDiagnostichistorical(Models.Diagnostics alert)
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
                    var result = new List<Models.Diagnostics>();
                    con.Open();
                    SqlCommand cmd = new SqlCommand("sp_Diagnostic_history_Default", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add("@LineCode", SqlDbType.NVarChar, 150).Value = alert.LineCode;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = alert.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = alert.PlantCode;
                    var reader = cmd.ExecuteReader();
                    //bind values returned from sp to the model 
                    while (reader.Read())
                    {
                        result.Add(item: new Models.Diagnostics
                        {

                            DeviceID = Convert.ToString(reader["Device_ID"] == DBNull.Value ? "" : reader["Device_ID"]),
                            DeviceRef = Convert.ToString(reader["Device_ref"] == DBNull.Value ? "" : reader["Device_ref"]),
                            Devicename = Convert.ToString(reader["devicename"] == DBNull.Value ? "" : reader["devicename"]),
                            Event = Convert.ToString(reader["EventName"] == DBNull.Value ? 0 : reader["EventName"]),
                            LogTime = Convert.ToDateTime(reader["Time_stamp"] == DBNull.Value ? 0 : reader["Time_stamp"]),

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
            string con_string = d.Getconnectionstring(U.CompanyCode, U.PlantCode, U.LineCode);
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
        public IHttpActionResult Diagnostics_details(Models.Setting U)
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
        public IHttpActionResult Add_Update_delete_Diagnostic(Models.Diagnostics_settings A)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(A.CompanyCode,A.PlantCode,A.LineCode);
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
                    SqlCommand cmd = new SqlCommand("sp_Diagnostics", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = A.QueryType;
                    cmd.Parameters.Add("@deviceid", SqlDbType.NVarChar, 150).Value = A.DeviceID;
                    cmd.Parameters.Add("@deviceref", SqlDbType.NVarChar, 150).Value = A.DeviceRef;
                    cmd.Parameters.Add("@make", SqlDbType.NVarChar, 150).Value = A.make;
                    cmd.Parameters.Add("@gateway", SqlDbType.NVarChar, 150).Value = A.gateway;
                    cmd.Parameters.Add("@ioserver", SqlDbType.NVarChar, 150).Value = A.ioserver;
                    cmd.Parameters.Add("@macaddress", SqlDbType.NVarChar, 150).Value = A.macaddress;
                    cmd.Parameters.Add("@connectedTo", SqlDbType.NVarChar, 150).Value = A.connectedto;
                    cmd.Parameters.Add("@installed", SqlDbType.NVarChar, 150).Value = A.installed;
                    cmd.Parameters.Add("@remarks", SqlDbType.NVarChar, 500).Value = A.remarks;
                    cmd.Parameters.Add("@partnumber", SqlDbType.NVarChar, 150).Value = A.partnumber;
                    cmd.Parameters.Add("@unique_id", SqlDbType.NVarChar, 150).Value = A.Unique_ID;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = A.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = A.PlantCode;
                    cmd.Parameters.Add("@LineCode", SqlDbType.NVarChar, 150).Value = A.LineCode;

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