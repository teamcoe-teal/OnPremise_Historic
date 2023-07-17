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
    public class RawTableController : ApiController
    {
        private string connectionstring = ConfigurationManager.ConnectionStrings["con"].ToString();

        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage Get_RawTable_datas(Models.RawTables R)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(R.Parameter, R.Parameter1, R.Parameter2);
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
                    SqlCommand cmd = new SqlCommand("SP_Getting_Raw_Datas", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = R.QueryType;
                    cmd.Parameters.Add("@Parameter", SqlDbType.NVarChar, 150).Value = R.Parameter;
                    cmd.Parameters.Add("@Parameter1", SqlDbType.NVarChar, 150).Value = R.Parameter1;
                    cmd.Parameters.Add("@Parameter2", SqlDbType.NVarChar, 150).Value = R.Parameter2;
                    cmd.Parameters.Add("@Parameter3", SqlDbType.Int).Value = R.Parameter3;
                    cmd.Parameters.Add("@Parameter4", SqlDbType.NVarChar, 150).Value = R.Parameter4;
                    cmd.Parameters.Add("@Parameter5", SqlDbType.NVarChar, 150).Value = R.Parameter5;
                    cmd.Parameters.Add("@Parameter6", SqlDbType.NVarChar, 150).Value = R.Parameter6;
                    cmd.Parameters.Add("@Parameter7", SqlDbType.NVarChar, 150).Value = R.Parameter7;
                    cmd.Parameters.Add("@Parameter8", SqlDbType.NVarChar, 150).Value = R.Parameter8;
                    cmd.Parameters.Add("@fromdate", SqlDbType.NVarChar, 150).Value = R.fromdate;
                    cmd.Parameters.Add("@todate", SqlDbType.NVarChar, 150).Value = R.todate;
                    cmd.Parameters.Add("@duration", SqlDbType.NVarChar, 150).Value = R.duration;
                    
                    cmd.CommandTimeout = 0;
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
        public HttpResponseMessage GetMachine_statuswisedata(Models.quality_day_wise Q)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(Q.CompanyCode, Q.PlantCode, Q.line);
            if (con_string == "0")
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "Coudnot Connect to database", data = "" });

            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    var result = new List<Models.quality_day_wise>();
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("Get_oknok_basedon_Machinestatus", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    //cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = Q.QueryType;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = Q.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = Q.PlantCode;
                    cmd.Parameters.Add("@lineCode", SqlDbType.NVarChar, 150).Value = Q.line;
                    cmd.Parameters.Add("@MachineCode", SqlDbType.NVarChar, 150).Value = Q.Machine;
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
        public HttpResponseMessage Get_Cumulative_data(Models.cumulative_data C)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(C.CompanyCode, C.PlantCode, C.Line_Code);
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
                    SqlCommand cmd = new SqlCommand("SP_cumulative_data", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = C.QueryType;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = C.CompanyCode;
                    cmd.Parameters.Add("@Plantcode", SqlDbType.NVarChar, 150).Value = C.PlantCode;
                    cmd.Parameters.Add("@Line", SqlDbType.NVarChar, 150).Value = C.LineCode;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = C.Machine;
                    cmd.Parameters.Add("@Month", SqlDbType.NVarChar, 150).Value = C.Month;

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
        public HttpResponseMessage Get_Planned_Cycletime(Models.Plannedcycletime CY)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(CY.CompanyCode, CY.PlantCode, CY.LineCode);
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
                    SqlCommand cmd = new SqlCommand("SP_plannedcycletime", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = CY.QueryType;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = CY.CompanyCode;
                    cmd.Parameters.Add("@Line", SqlDbType.NVarChar, 150).Value = CY.LineCode;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = CY.Machine;
                    cmd.Parameters.Add("@Date", SqlDbType.NVarChar, 150).Value = CY.Date;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = CY.PlantCode;
                    cmd.Parameters.Add("@Variant", SqlDbType.NVarChar, 150).Value = CY.Variant;
                    cmd.Parameters.Add("@Shift", SqlDbType.NVarChar, 150).Value = CY.Shift;

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
