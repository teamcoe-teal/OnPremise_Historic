using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Data.SqlClient;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Web.Http.Cors;

namespace Plant_Digitization_api.Controllers
{
    //[EnableCors(origins: "http://localhost:55974", headers: "*", methods: "*")]
    
    public class AvailabilityController : ApiController
    {
        //Models.PlanDigitizationEntities db = new Models.PlanDigitizationEntities();
        private string connectionstring = ConfigurationManager.ConnectionStrings["con"].ToString();

        [Route("api/Availability/GetAvlDaywise")]
        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage GetAvlDaywise(Models.Avl_Daywisereport avl)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(avl.CompanyCode, avl.PlantCode, avl.line);
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
                    SqlCommand cmd = new SqlCommand("SP_Avl_DaywiseReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = avl.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = avl.Machine;
                    cmd.Parameters.Add("@date", SqlDbType.NVarChar, 150).Value = avl.Month;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = avl.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = avl.PlantCode;
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
        public HttpResponseMessage GetAvlweekwise(Models.Avl_weekwisereport avl)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(avl.CompanyCode, avl.PlantCode, avl.line);
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
                    SqlCommand cmd = new SqlCommand("SP_Avl_WeekwiseReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = avl.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = avl.Machine;
                    cmd.Parameters.Add("@Date", SqlDbType.NVarChar, 150).Value = avl.Month;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = avl.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = avl.PlantCode;
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
        public HttpResponseMessage GetAvlshiftwise(Models.Avl_shiftwisereport avl)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(avl.CompanyCode, avl.PlantCode, avl.line);
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
                    SqlCommand cmd = new SqlCommand("SP_Avl_ShiftwiseReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = avl.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = avl.Machine;
                    cmd.Parameters.Add("@date", SqlDbType.NVarChar, 150).Value = avl.Month;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = avl.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = avl.PlantCode;
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
        [Route("api/Availability/GetAvlyearwise")]
        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage GetAvlyearwise(Models.Avl_yearwisereport avl)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(avl.CompanyCode, avl.PlantCode, avl.line);
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
                    SqlCommand cmd = new SqlCommand("SP_Avl_YearwiseReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = avl.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = avl.Machine;
                    cmd.Parameters.Add("@Year", SqlDbType.NVarChar, 150).Value = avl.Year;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = avl.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = avl.PlantCode;
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
        public HttpResponseMessage GetAvlReasons(Models.Avl_Breakdown avl)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(avl.CompanyCode, avl.PlantCode, avl.line);
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
                    SqlCommand cmd = new SqlCommand("SP_Avl_Reasons ", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = avl.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = avl.Machine;
                    cmd.Parameters.Add("@Fromdate", SqlDbType.DateTime, 150).Value = avl.Fromdate;
                    cmd.Parameters.Add("@Todate", SqlDbType.DateTime, 150).Value = avl.Todate;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = avl.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = avl.PlantCode;
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

        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage GetAvlWeekReasons(Models.Avl_weekBreakdown avl)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(avl.CompanyCode, avl.PlantCode, avl.line);
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
                    SqlCommand cmd = new SqlCommand("SP_Avl_Week_Reasons", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = avl.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = avl.Machine;
                    cmd.Parameters.Add("@Date", SqlDbType.DateTime, 150).Value = avl.Date;
                    cmd.Parameters.Add("@WeekNo", SqlDbType.NVarChar, 150).Value = avl.WeekNo;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = avl.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = avl.PlantCode;
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

        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage GetAvlShiftReasons(Models.Avl_shiftBreakdown avl)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(avl.CompanyCode, avl.PlantCode, avl.line);
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
                    SqlCommand cmd = new SqlCommand("SP_Avl_Shift_Reasons", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = avl.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = avl.Machine;
                    cmd.Parameters.Add("@Fromdate", SqlDbType.DateTime, 150).Value = avl.Fromdate;
                    //cmd.Parameters.Add("@Todate", SqlDbType.DateTime, 150).Value = avl.Todate;
                    cmd.Parameters.Add("@shift", SqlDbType.NVarChar, 150).Value = avl.shift;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = avl.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = avl.PlantCode;
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

        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage GetAvlcustomreport(Models.Avl_custom avl)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(avl.CompanyCode, avl.PlantCode, avl.line);
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
                    SqlCommand cmd = new SqlCommand("SP_Avl_CustomReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = avl.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = avl.Machine;
                    cmd.Parameters.Add("@Fromdate", SqlDbType.DateTime, 150).Value = avl.Fromdate;
                    cmd.Parameters.Add("@Todate", SqlDbType.DateTime, 150).Value = avl.Todate;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = avl.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = avl.PlantCode;
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
    }
}
