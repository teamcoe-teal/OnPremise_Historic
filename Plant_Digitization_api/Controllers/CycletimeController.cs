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
    public class CycleTimeController : ApiController
    {
        private string connectionstring = ConfigurationManager.ConnectionStrings["con"].ToString();

        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage Get_Cycletime_Live(Models.CycleTime_Live CY)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(CY.CompanyCode, CY.PlantCode, CY.Line);
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
                    DataTable dt_Sorted = new DataTable();
                    SqlCommand cmd = new SqlCommand("SP_Live_Cycletime", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = CY.QueryType;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = CY.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = CY.Line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = CY.Machine;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = CY.PlantCode;
                    cmd.Parameters.Add("@records", SqlDbType.NVarChar, 150).Value = CY.records;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                    //con.Close();
                    foreach (DataTable table in ds.Tables)
                    {
                        for (int j = 0; j < table.Rows.Count; j++)
                        {
                            decimal sum = 0;
                            for (int i = 0; i < table.Columns.Count; i++)
                            {
                                dynamic temp = table.Columns[i].ToString();
                                dynamic temp1 = table.Columns[i].ToString();
                                // sum = sum + Convert.ToDecimal(table.Columns[i].ToString());

                            }

                        }

                    }
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
        public HttpResponseMessage Get_Cycletime_Histogram(Models.CycleTime_Histogram CY)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(CY.CompanyCode, CY.PlantCode, CY.Line);
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
                    SqlCommand cmd = new SqlCommand("SP_Historic_Histogram", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = CY.QueryType;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = CY.CompanyCode;
                    cmd.Parameters.Add("@Line", SqlDbType.NVarChar, 150).Value = CY.Line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = CY.Machine;
                    cmd.Parameters.Add("@Date", SqlDbType.NVarChar, 150).Value = CY.Date;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = CY.PlantCode;
                    cmd.Parameters.Add("@Variant", SqlDbType.NVarChar, 150).Value = CY.Variant;
                    cmd.Parameters.Add("@Shift", SqlDbType.NVarChar, 150).Value = CY.Shift;
                    cmd.Parameters.Add("@Month", SqlDbType.NVarChar, 150).Value = CY.Month;
                    cmd.Parameters.Add("@Year", SqlDbType.NVarChar, 150).Value = CY.Year;
                    cmd.Parameters.Add("@FDate", SqlDbType.NVarChar, 150).Value = CY.FDate;
                    cmd.Parameters.Add("@TDate", SqlDbType.NVarChar, 150).Value = CY.TDate;
                    cmd.Parameters.Add("@Operation", SqlDbType.NVarChar, 150).Value = CY.Operation;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds.Tables[0].Rows.Count != 0)
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
        public HttpResponseMessage Get_Cycletime_Average(Models.CycleTime_Average CY)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(CY.CompanyCode, CY.PlantCode, CY.Line);
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
                    SqlCommand cmd = new SqlCommand("SP_Historic_Average", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = CY.QueryType;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = CY.CompanyCode;
                    cmd.Parameters.Add("@Line", SqlDbType.NVarChar, 150).Value = CY.Line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = CY.Machine;
                    cmd.Parameters.Add("@Date", SqlDbType.NVarChar, 150).Value = CY.Date;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = CY.PlantCode;
                    cmd.Parameters.Add("@Shift", SqlDbType.NVarChar, 150).Value = CY.Shift;
                    cmd.Parameters.Add("@Month", SqlDbType.NVarChar, 150).Value = CY.Month;
                    cmd.Parameters.Add("@Year", SqlDbType.NVarChar, 150).Value = CY.Year;
                    cmd.Parameters.Add("@FDate", SqlDbType.NVarChar, 150).Value = CY.FDate;
                    cmd.Parameters.Add("@TDate", SqlDbType.NVarChar, 150).Value = CY.TDate;

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
        public HttpResponseMessage Get_Cycletime_Partwise(Models.CycleTime_Partwise CY)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(CY.CompanyCode, CY.PlantCode, CY.Line);
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
                    SqlCommand cmd = new SqlCommand("SP_Partwise_Cycletime", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = CY.QueryType;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = CY.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = CY.Line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = CY.Machine;
                    cmd.Parameters.Add("@Variant", SqlDbType.NVarChar, 150).Value = CY.Variant;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = CY.PlantCode;
                    cmd.Parameters.Add("@Date", SqlDbType.NVarChar, 150).Value = CY.Date;
                    cmd.Parameters.Add("@FTime", SqlDbType.NVarChar, 150).Value = CY.FTime;
                    cmd.Parameters.Add("@TTime", SqlDbType.NVarChar, 150).Value = CY.TTime;
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
        public HttpResponseMessage GetDataGroup(Models.CycleTime_Histogram c)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(c.CompanyCode, c.PlantCode, c.Line);
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
                    SqlCommand cmd = new SqlCommand("SP_LineAvgCycleTime", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@fromdate", SqlDbType.NVarChar, 150).Value = c.FDate;
                    cmd.Parameters.Add("@todate", SqlDbType.NVarChar, 150).Value = c.TDate;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { status = "Success", msg = "Details Found", data = ds.Tables[0] });
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
