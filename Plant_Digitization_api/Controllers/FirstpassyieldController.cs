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

    public class FirstpassyieldController : ApiController
    {
        private string connectionstring = ConfigurationManager.ConnectionStrings["con"].ToString();
        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage Gethourwisereport(Models.Daywisereport pass)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(pass.CompanyCode, pass.PlantCode, pass.linecode);
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
                    SqlCommand cmd = new SqlCommand("SP_Hourly_Daywise_Report", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = pass.CompanyCode;
                    cmd.Parameters.Add("@linecode", SqlDbType.NVarChar, 150).Value = pass.linecode;
                    cmd.Parameters.Add("@Variantcode", SqlDbType.NVarChar, 150).Value = pass.Variantcode;
                    cmd.Parameters.Add("@date", SqlDbType.NVarChar, 150).Value = pass.date;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = pass.PlantCode;
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
        public HttpResponseMessage GetHourlysummarydata(Models.hourwisereport first)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(first.CompanyCode, first.PlantCode, first.linecode);
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
                    SqlCommand cmd = new SqlCommand("SP_Hourly_live_Summary", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = first.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = first.linecode;
                    cmd.Parameters.Add("@MachineCode", SqlDbType.NVarChar, 150).Value = first.Machinecode;
                    cmd.Parameters.Add("@Fromdate", SqlDbType.NVarChar, 150).Value = first.Fromtime;
                    cmd.Parameters.Add("@Todate", SqlDbType.NVarChar, 150).Value = first.Totime;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = first.PlantCode;
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
        public HttpResponseMessage GetPrevHourlysummarydata(Models.hourwisereport first)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(first.CompanyCode, first.PlantCode, first.linecode);
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
                    SqlCommand cmd = new SqlCommand("SP_Prev_Hourly_Live", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = first.CompanyCode;
                    cmd.Parameters.Add("@linecode", SqlDbType.NVarChar, 150).Value = first.linecode;
                    cmd.Parameters.Add("@MachineCode", SqlDbType.NVarChar, 150).Value = first.Machinecode;
                    //cmd.Parameters.Add("@Fromdate", SqlDbType.NVarChar, 150).Value = first.Fromtime;
                    cmd.Parameters.Add("@Todate", SqlDbType.NVarChar, 150).Value = first.Totime;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = first.PlantCode;
                    cmd.Parameters.Add("@Shift", SqlDbType.NVarChar, 150).Value = first.Shift;
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
        public HttpResponseMessage GetTimeVariantdata(Models.hourwisereport first)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(first.CompanyCode, first.PlantCode, first.linecode);
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
                    SqlCommand cmd = new SqlCommand("SP_TimeVariant_Hourly_Live", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = first.CompanyCode;
                    cmd.Parameters.Add("@linecode", SqlDbType.NVarChar, 150).Value = first.linecode;
                    cmd.Parameters.Add("@MachineCode", SqlDbType.NVarChar, 150).Value = first.Machinecode;
                    //cmd.Parameters.Add("@Fromdate", SqlDbType.NVarChar, 150).Value = first.Fromtime;
                    cmd.Parameters.Add("@Todate", SqlDbType.NVarChar, 150).Value = first.Totime;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = first.PlantCode;
                    cmd.Parameters.Add("@Shift", SqlDbType.NVarChar, 150).Value = first.Shift;
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
        public HttpResponseMessage GetDaywisereport(Models.Daywisereport pass)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(pass.CompanyCode, pass.PlantCode, pass.linecode);
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
                    SqlCommand cmd = new SqlCommand("SP_DaywiseReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = pass.CompanyCode;
                    cmd.Parameters.Add("@linecode", SqlDbType.NVarChar, 150).Value = pass.linecode;
                    cmd.Parameters.Add("@Variantcode", SqlDbType.NVarChar, 150).Value = pass.Variantcode;
                    cmd.Parameters.Add("@date", SqlDbType.NVarChar, 150).Value = pass.date;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = pass.PlantCode;
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
        public HttpResponseMessage GetWeekwisereport(Models.weekwisereport pass)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(pass.CompanyCode, pass.PlantCode, pass.line);
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
                    SqlCommand cmd = new SqlCommand("SP_WeekwiseReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = pass.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = pass.line;
                    cmd.Parameters.Add("@Variantcode", SqlDbType.NVarChar, 150).Value = pass.Variantcode;
                    cmd.Parameters.Add("@date", SqlDbType.VarChar, 150).Value = pass.date;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = pass.PlantCode;
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
        public HttpResponseMessage Getshiftwisereport(Models.shiftwisereport pass)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(pass.CompanyCode, pass.PlantCode, pass.line);
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
                    SqlCommand cmd = new SqlCommand("SP_ShiftwiseReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = pass.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = pass.line;
                    cmd.Parameters.Add("@Variantcode", SqlDbType.NVarChar, 150).Value = pass.Variantcode;
                    cmd.Parameters.Add("@date", SqlDbType.NVarChar, 150).Value = pass.date;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = pass.PlantCode;
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
        public HttpResponseMessage Getyearwisereport(Models.yearwisereport pass)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(pass.CompanyCode, pass.PlantCode, pass.linecode);
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
                    SqlCommand cmd = new SqlCommand("SP_YearwiseReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = pass.CompanyCode;
                    cmd.Parameters.Add("@linecode", SqlDbType.NVarChar, 150).Value = pass.linecode;
                    cmd.Parameters.Add("@Variantcode", SqlDbType.NVarChar, 150).Value = pass.Variantcode;
                    cmd.Parameters.Add("@date", SqlDbType.NVarChar, 150).Value = pass.date;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = pass.PlantCode;
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
        public HttpResponseMessage GetFPYBreakdown(Models.FPY_Breakdown pass)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(pass.CompanyCode, pass.PlantCode, pass.line);
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
                    SqlCommand cmd = new SqlCommand("SP_FPY_Breakdown", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = pass.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = pass.line;
                    cmd.Parameters.Add("@variant", SqlDbType.NVarChar, 150).Value = pass.variant;
                    cmd.Parameters.Add("@Fromdate", SqlDbType.Date, 150).Value = pass.Fromdate;
                    cmd.Parameters.Add("@Todate", SqlDbType.Date, 150).Value = pass.Todate;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = pass.PlantCode;
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
        public HttpResponseMessage GetFPYBreakdownPareto(Models.FPY_Breakdown pass)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(pass.CompanyCode, pass.PlantCode, pass.line);
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
                    SqlCommand cmd = new SqlCommand("SP_FPY_Breakdown_pareto", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = pass.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = pass.line;
                    cmd.Parameters.Add("@variant", SqlDbType.NVarChar, 150).Value = pass.variant;
                    cmd.Parameters.Add("@Fromdate", SqlDbType.Date, 150).Value = pass.Fromdate;
                    cmd.Parameters.Add("@Todate", SqlDbType.Date, 150).Value = pass.Todate;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = pass.PlantCode;
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
        public HttpResponseMessage GetFPYweekBreakdown(Models.FPY_weekBreakdown pass)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(pass.CompanyCode, pass.PlantCode, pass.line);
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
                    SqlCommand cmd = new SqlCommand("SP_FPY_Week_Breakdown", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = pass.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = pass.line;
                    cmd.Parameters.Add("@variant", SqlDbType.NVarChar, 150).Value = pass.variant;
                    cmd.Parameters.Add("@Date", SqlDbType.Date, 150).Value = pass.Date;
                    cmd.Parameters.Add("@WeekNo", SqlDbType.NVarChar, 150).Value = pass.WeekNo;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = pass.PlantCode;
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
        public HttpResponseMessage GetFPYweekBreakdownPareto(Models.FPY_weekBreakdown pass)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(pass.CompanyCode, pass.PlantCode, pass.line);
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
                    SqlCommand cmd = new SqlCommand("SP_FPY_Week_Breakdown_pareto", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = pass.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = pass.line;
                    cmd.Parameters.Add("@variant", SqlDbType.NVarChar, 150).Value = pass.variant;
                    cmd.Parameters.Add("@Date", SqlDbType.Date, 150).Value = pass.Date;
                    cmd.Parameters.Add("@WeekNo", SqlDbType.NVarChar, 150).Value = pass.WeekNo;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = pass.PlantCode;
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
        public HttpResponseMessage GetFPYshiftkBreakdown(Models.FPY_shiftBreakdown pass)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(pass.CompanyCode, pass.PlantCode, pass.line);
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
                    SqlCommand cmd = new SqlCommand("SP_FPY_Shift_Breakdown", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = pass.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = pass.line;
                    cmd.Parameters.Add("@variant", SqlDbType.NVarChar, 150).Value = pass.variant;
                    cmd.Parameters.Add("@Fromdate", SqlDbType.Date, 150).Value = pass.Fromdate;
                    cmd.Parameters.Add("@shift", SqlDbType.NVarChar, 150).Value = pass.shift;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = pass.PlantCode;
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
        public HttpResponseMessage GetFPYshiftkBreakdownPareto(Models.FPY_shiftBreakdown pass)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(pass.CompanyCode, pass.PlantCode, pass.line);
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
                    SqlCommand cmd = new SqlCommand("SP_FPY_Shift_Breakdown_pareto", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = pass.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = pass.line;
                    cmd.Parameters.Add("@variant", SqlDbType.NVarChar, 150).Value = pass.variant;
                    cmd.Parameters.Add("@Fromdate", SqlDbType.Date, 150).Value = pass.Fromdate;
                    cmd.Parameters.Add("@shift", SqlDbType.NVarChar, 150).Value = pass.shift;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = pass.PlantCode;
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
        public HttpResponseMessage Getcustomwisereport(Models.customwisereport pass)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(pass.CompanyCode, pass.PlantCode, pass.linecode);
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
                    SqlCommand cmd = new SqlCommand("SP_CustomReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = pass.CompanyCode;
                    cmd.Parameters.Add("@linecode", SqlDbType.NVarChar, 150).Value = pass.linecode;
                    cmd.Parameters.Add("@Variantcode", SqlDbType.NVarChar, 150).Value = pass.Variantcode;
                    cmd.Parameters.Add("@FromDate", SqlDbType.Date, 150).Value = pass.FromDate;
                    cmd.Parameters.Add("@ToDate", SqlDbType.Date, 150).Value = pass.ToDate;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = pass.PlantCode;
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
        public HttpResponseMessage GetAvlBreakdown(Models.OEE_Breakdown oee)
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

    }
}
