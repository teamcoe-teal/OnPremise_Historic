using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Plant_Digitization_api.Controllers
{
    public class Live_AlarmController : ApiController
    {
        private string connectionstring = ConfigurationManager.ConnectionStrings["con"].ToString();

        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage Get_live_Alarm_data(Models.AlarmSettings AS)
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
                    SqlCommand cmd = new SqlCommand("SP_Alarm_Live_historic", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = AS.QueryType;
                    //cmd.Parameters.Add("@ID", SqlDbType.NVarChar, 150).Value = AS.A_ID;
                    cmd.Parameters.Add("@line ", SqlDbType.NVarChar, 150).Value = AS.Line_Code;
                    //cmd.Parameters.Add("@Machine_Code", SqlDbType.NVarChar, 150).Value = AS.Machine_Code;
                    //cmd.Parameters.Add("@Alarm_ID", SqlDbType.NVarChar, 150).Value = AS.Alarm_ID;
                    //cmd.Parameters.Add("@Alarm_Description", SqlDbType.NVarChar, 150).Value = AS.Alarm_Description;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = AS.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = AS.PlantCode;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { data = ds });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "No Live Alarm", data = "" });
                    }
                }
            }
        }

        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage Get_Historic_Alarm_data(Models.AlarmSettings AS)
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
                    SqlCommand cmd = new SqlCommand("SP_Alarm_Live_historic", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = AS.QueryType;
                    cmd.Parameters.Add("@Fdate ", SqlDbType.NVarChar, 150).Value = AS.Parameter4;
                    cmd.Parameters.Add("@line ", SqlDbType.NVarChar, 150).Value = AS.Line_Code;
                    cmd.Parameters.Add("@Tdate", SqlDbType.NVarChar, 150).Value = AS.Parameter5;
                    cmd.Parameters.Add("@Alarm_ID", SqlDbType.NVarChar, 150).Value = AS.Alarm_ID;
                    //cmd.Parameters.Add("@Alarm_Description", SqlDbType.NVarChar, 150).Value = AS.Alarm_Description;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = AS.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = AS.PlantCode;
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
        public HttpResponseMessage Popup_Machinewise_Live_AlarmLoss(Models.AlarmSettings AS)
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
                    SqlCommand cmd = new SqlCommand("SP_Popup_Machinewise_Live_AlarmLoss", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = AS.QueryType;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = AS.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = AS.PlantCode;
                    cmd.Parameters.Add("@line ", SqlDbType.NVarChar, 150).Value = AS.Line_Code;
                    cmd.Parameters.Add("@machine ", SqlDbType.NVarChar, 150).Value = AS.Machine_Code;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { data = ds });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "No Live Alarm", data = "" });
                    }
                }
            }
        }

    }
}
