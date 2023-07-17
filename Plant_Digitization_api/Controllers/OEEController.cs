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

namespace Plant_Digitization_api.Controllers
{
    //[EnableCors(origins: "http://localhost:55974", headers: "*", methods: "*")]

    public class OEEController : ApiController
    {
        private string connectionstring = ConfigurationManager.ConnectionStrings["con"].ToString();
        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage GetOEEDaywise(Models.OEE_Daywisereport oee)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(oee.CompanyCode, oee.PlantCode, oee.line);
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
                    SqlCommand cmd = new SqlCommand("SP_OEE_DaywiseReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = oee.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = oee.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = oee.Machine;
                    cmd.Parameters.Add("@Year", SqlDbType.NVarChar, 150).Value = oee.Year;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = oee.PlantCode;
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
        public HttpResponseMessage GetOEEweekwise(Models.OEE_weekwisereport oee)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(oee.CompanyCode, oee.PlantCode, oee.line);
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
                    SqlCommand cmd = new SqlCommand("SP_OEE_WeekwiseReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = oee.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = oee.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = oee.Machine;
                    cmd.Parameters.Add("@Year", SqlDbType.NVarChar, 150).Value = oee.Year;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = oee.PlantCode;
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
        public HttpResponseMessage GetOEEshiftwise(Models.OEE_shiftwisereport oee)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(oee.CompanyCode, oee.PlantCode, oee.line);
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
                    SqlCommand cmd = new SqlCommand("SP_OEE_ShiftwiseReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = oee.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = oee.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = oee.Machine;
                    cmd.Parameters.Add("@Year", SqlDbType.NVarChar, 150).Value = oee.Year;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = oee.PlantCode;
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
        public HttpResponseMessage GetOEEyearwise(Models.OEE_yearwisereport oee)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(oee.CompanyCode, oee.PlantCode, oee.line);
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
                    SqlCommand cmd = new SqlCommand("SP_OEE_YearwiseReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = oee.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = oee.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = oee.Machine;
                    cmd.Parameters.Add("@Year", SqlDbType.NVarChar, 150).Value = oee.Year;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = oee.PlantCode;
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
        public HttpResponseMessage GetOEEBreakdown(Models.OEE_Breakdown oee)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(oee.CompanyCode, oee.PlantCode, oee.line);
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
                    SqlCommand cmd = new SqlCommand("SP_OEE_Breakdown", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = oee.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = oee.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = oee.Machine;
                    cmd.Parameters.Add("@Fromdate", SqlDbType.Date, 150).Value = oee.Fromdate;
                    cmd.Parameters.Add("@Todate", SqlDbType.Date, 150).Value = oee.Todate;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = oee.PlantCode;
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
        public HttpResponseMessage GetOEEweekBreakdown(Models.OEE_weekBreakdown oee)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(oee.CompanyCode, oee.PlantCode, oee.line);
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
                    SqlCommand cmd = new SqlCommand("SP_OEE_Week_Breakdown", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = oee.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = oee.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = oee.Machine;
                    cmd.Parameters.Add("@Date", SqlDbType.Date, 150).Value = oee.Date;
                    cmd.Parameters.Add("@WeekNo", SqlDbType.NVarChar, 150).Value = oee.WeekNo;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = oee.PlantCode;
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
        public HttpResponseMessage GetOEEshiftkBreakdown(Models.OEE_shiftBreakdown oee)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(oee.CompanyCode, oee.PlantCode, oee.line);
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
                    SqlCommand cmd = new SqlCommand("SP_OEE_Shift_Breakdown", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = oee.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = oee.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = oee.Machine;
                    cmd.Parameters.Add("@Fromdate", SqlDbType.Date, 150).Value = oee.Fromdate;
                    cmd.Parameters.Add("@shift", SqlDbType.NVarChar, 150).Value = oee.shift;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = oee.PlantCode;
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
        public HttpResponseMessage GetOEEcustomreport(Models.OEE_custom oee)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(oee.CompanyCode, oee.PlantCode, oee.line);
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
                    SqlCommand cmd = new SqlCommand("SP_OEE_CustomReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = oee.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = oee.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = oee.Machine;
                    cmd.Parameters.Add("@Fromdate", SqlDbType.Date, 150).Value = oee.Fromdate;
                    cmd.Parameters.Add("@Todate", SqlDbType.Date, 150).Value = oee.Todate;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = oee.PlantCode;
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
        public HttpResponseMessage ShiftwiseOEE(Models.OEE_custom oee)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(oee.CompanyCode, oee.PlantCode, oee.line);
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
                    SqlCommand cmd = new SqlCommand("SP_Shiftwise_Batch_Cumulation_OEE", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@machinecode", SqlDbType.NVarChar, 150).Value = oee.Machine;
                    cmd.Parameters.Add("@fromdate", SqlDbType.Date, 150).Value = oee.Fromdate;
                    cmd.Parameters.Add("@todate", SqlDbType.Date, 150).Value = oee.Todate;
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
        public HttpResponseMessage MonthlyShiftwiseOEE(Models.OEE_custom oee)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(oee.CompanyCode, oee.PlantCode, oee.line);
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
                    SqlCommand cmd = new SqlCommand("SP_MonthlyShiftwise_Batch_Cumulation_OEE", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@machinecode", SqlDbType.NVarChar, 150).Value = oee.Machine;
                    cmd.Parameters.Add("@Year", SqlDbType.NVarChar, 150).Value = oee.Year;
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
        public HttpResponseMessage YearlyShiftwiseOEE(Models.OEE_custom oee)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(oee.CompanyCode, oee.PlantCode, oee.line);
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
                    SqlCommand cmd = new SqlCommand("SP_YearlyShiftwise_Batch_Cumulation_OEE", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@machinecode", SqlDbType.NVarChar, 150).Value = oee.Machine;
                    cmd.Parameters.Add("@Year", SqlDbType.NVarChar, 150).Value = oee.Year;
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
