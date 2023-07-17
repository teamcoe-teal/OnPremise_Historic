using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Plant_Digitization_api.Controllers
{
    //[EnableCors(origins: "http://localhost:55974", headers: "*", methods: "*")]

    public class ParetoanalysisController : ApiController
    {
        private string connectionstring = ConfigurationManager.ConnectionStrings["con"].ToString();

        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage GetMTBF_CustomReport(Models.MTBF_CustomReport mtbf)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(mtbf.CompanyCode, mtbf.PlantCode, mtbf.line);
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
                    SqlCommand cmd = new SqlCommand("SP_MTBF_CustomReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = mtbf.QueryType;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = mtbf.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = mtbf.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = mtbf.Machine;
                    cmd.Parameters.Add("@FromDate", SqlDbType.DateTime, 150).Value = mtbf.FromDate;
                    cmd.Parameters.Add("@ToDate", SqlDbType.DateTime, 150).Value = mtbf.ToDate;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = mtbf.PlantCode;
                    cmd.Parameters.Add("@records", SqlDbType.NVarChar, 150).Value = mtbf.records;
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
        public HttpResponseMessage GetMTBF_MonthwiseReport(Models.MTBF_MonthwiseReport mtbf)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(mtbf.CompanyCode, mtbf.PlantCode, mtbf.line);
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
                    SqlCommand cmd = new SqlCommand("SP_MTBF_MonthwiseReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = mtbf.QueryType;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = mtbf.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = mtbf.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = mtbf.Machine;
                    cmd.Parameters.Add("@Year", SqlDbType.NVarChar, 150).Value = mtbf.Year;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = mtbf.PlantCode;
                    cmd.Parameters.Add("@records", SqlDbType.NVarChar, 150).Value = mtbf.records;
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
        public HttpResponseMessage GetMTBF_YearwiseReport(Models.MTBF_YearwiseReport mtbf)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(mtbf.CompanyCode, mtbf.PlantCode, mtbf.line);
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
                    SqlCommand cmd = new SqlCommand("SP_MTBF_YearwiseReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = mtbf.QueryType;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = mtbf.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = mtbf.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = mtbf.Machine;
                    cmd.Parameters.Add("@Year", SqlDbType.NVarChar, 150).Value = mtbf.Year;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = mtbf.PlantCode;
                    cmd.Parameters.Add("@records", SqlDbType.NVarChar, 150).Value = mtbf.records;
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
        public HttpResponseMessage GetMTTR_CustomReport(Models.MTTR_CustomReport mttr)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(mttr.CompanyCode, mttr.PlantCode, mttr.line);
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
                    SqlCommand cmd = new SqlCommand("SP_MTTR_CustomReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = mttr.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = mttr.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = mttr.Machine;
                    cmd.Parameters.Add("@FromDate", SqlDbType.DateTime, 150).Value = mttr.FromDate;
                    cmd.Parameters.Add("@ToDate", SqlDbType.DateTime, 150).Value = mttr.ToDate;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = mttr.PlantCode;
                    cmd.Parameters.Add("@records", SqlDbType.NVarChar, 150).Value = mttr.records;
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
        public HttpResponseMessage GetMTTR_MonthwiseReport(Models.MTTR_MonthwiseReport mttr)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(mttr.CompanyCode, mttr.PlantCode, mttr.line);
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
                    SqlCommand cmd = new SqlCommand("SP_MTTR_MonthwiseReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = mttr.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = mttr.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = mttr.Machine;
                    cmd.Parameters.Add("@Year", SqlDbType.NVarChar, 150).Value = mttr.Year;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = mttr.PlantCode;
                    cmd.Parameters.Add("@records", SqlDbType.NVarChar, 150).Value = mttr.records;
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
        public HttpResponseMessage GetMTTR_YearwiseReport(Models.MTTR_YearwiseReport mttr)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(mttr.CompanyCode, mttr.PlantCode, mttr.line);
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
                    SqlCommand cmd = new SqlCommand("SP_MTTR_YearwiseReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = mttr.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = mttr.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = mttr.Machine;
                    cmd.Parameters.Add("@Year", SqlDbType.NVarChar, 150).Value = mttr.Year;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = mttr.PlantCode;
                    cmd.Parameters.Add("@records", SqlDbType.NVarChar, 150).Value = mttr.records;
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
        public HttpResponseMessage GetMTTR_TodayReport(Models.MTTR_TodayReport mttr)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(mttr.CompanyCode, mttr.PlantCode, mttr.line);
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
                    SqlCommand cmd = new SqlCommand("SP_MTTR_TodayReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = mttr.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = mttr.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = mttr.Machine;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = mttr.PlantCode;
                    cmd.Parameters.Add("@records", SqlDbType.NVarChar, 150).Value = mttr.records;
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
        public HttpResponseMessage GetMTTR_YesterdayReport(Models.MTTR_YesterdayReport mttr)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(mttr.CompanyCode, mttr.PlantCode, mttr.line);
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
                    SqlCommand cmd = new SqlCommand("SP_MTTR_YesterdayReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = mttr.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = mttr.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = mttr.Machine;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = mttr.PlantCode;
                    cmd.Parameters.Add("@records", SqlDbType.NVarChar, 150).Value = mttr.records;
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
        public HttpResponseMessage GetPareto_CustomReport(Models.Pareto_CustomReport pareto)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(pareto.CompanyCode, pareto.PlantCode, pareto.line);
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
                    SqlCommand cmd = new SqlCommand("SP_Pareto_CustomReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = pareto.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = pareto.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = pareto.Machine;
                    cmd.Parameters.Add("@FromDate", SqlDbType.DateTime, 150).Value = pareto.FromDate;
                    cmd.Parameters.Add("@ToDate", SqlDbType.DateTime, 150).Value = pareto.ToDate;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = pareto.PlantCode;
                    cmd.Parameters.Add("@records", SqlDbType.NVarChar, 150).Value = pareto.records;
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
        public HttpResponseMessage GetPareto_CustomDuration(Models.Pareto_CustomReport pareto)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(pareto.CompanyCode, pareto.PlantCode, pareto.line);
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
                    SqlCommand cmd = new SqlCommand("SP_Pareto_CustomDuration", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = pareto.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = pareto.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = pareto.Machine;
                    cmd.Parameters.Add("@FromDate", SqlDbType.DateTime, 150).Value = pareto.FromDate;
                    cmd.Parameters.Add("@ToDate", SqlDbType.DateTime, 150).Value = pareto.ToDate;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = pareto.PlantCode;
                    cmd.Parameters.Add("@records", SqlDbType.NVarChar, 150).Value = pareto.records;
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
        public HttpResponseMessage GetPareto_MonthwiseReport(Models.Pareto_MonthwiseReport pareto)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(pareto.CompanyCode, pareto.PlantCode, pareto.line);
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
                    SqlCommand cmd = new SqlCommand("SP_Pareto_MonthwiseReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = pareto.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = pareto.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = pareto.Machine;
                    cmd.Parameters.Add("@Year", SqlDbType.NVarChar, 150).Value = pareto.Year;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = pareto.PlantCode;
                    cmd.Parameters.Add("@records", SqlDbType.NVarChar, 150).Value = pareto.records;
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
        public HttpResponseMessage GetPareto_MonthwiseDuration(Models.Pareto_MonthwiseReport pareto)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(pareto.CompanyCode, pareto.PlantCode, pareto.line);
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
                    SqlCommand cmd = new SqlCommand("SP_Pareto_MonthwiseDuration", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = pareto.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = pareto.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = pareto.Machine;
                    cmd.Parameters.Add("@Year", SqlDbType.NVarChar, 150).Value = pareto.Year;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = pareto.PlantCode;
                    cmd.Parameters.Add("@records", SqlDbType.NVarChar, 150).Value = pareto.records;
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
        public HttpResponseMessage GetPareto_YearwiseReport(Models.Pareto_YearwiseReport pareto)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(pareto.CompanyCode, pareto.PlantCode, pareto.line);
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
                    SqlCommand cmd = new SqlCommand("SP_Pareto_YearwiseReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = pareto.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = pareto.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = pareto.Machine;
                    cmd.Parameters.Add("@Year", SqlDbType.NVarChar, 150).Value = pareto.Year;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = pareto.PlantCode;
                    cmd.Parameters.Add("@records", SqlDbType.NVarChar, 150).Value = pareto.records;
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
        public HttpResponseMessage GetPareto_YearwiseDuration(Models.Pareto_YearwiseReport pareto)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(pareto.CompanyCode, pareto.PlantCode, pareto.line);
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
                    SqlCommand cmd = new SqlCommand("SP_Pareto_YearwiseDuration", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = pareto.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = pareto.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = pareto.Machine;
                    cmd.Parameters.Add("@Year", SqlDbType.NVarChar, 150).Value = pareto.Year;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = pareto.PlantCode;
                    cmd.Parameters.Add("@records", SqlDbType.NVarChar, 150).Value = pareto.records;
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
        public HttpResponseMessage GetMOA_CustomReport(Models.MOA_CustomReport moa)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(moa.CompanyCode, moa.PlantCode, moa.line);
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
                    SqlCommand cmd = new SqlCommand("SP_MOA_CustomReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = moa.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = moa.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = moa.Machine;
                    cmd.Parameters.Add("@FromDate", SqlDbType.DateTime, 150).Value = moa.FromDate;
                    cmd.Parameters.Add("@ToDate", SqlDbType.DateTime, 150).Value = moa.ToDate;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = moa.PlantCode;
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
        public HttpResponseMessage GetMOA_MonthwiseReport(Models.MOA_MonthwiseReport moa)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(moa.CompanyCode, moa.PlantCode, moa.line);
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
                    SqlCommand cmd = new SqlCommand("SP_MOA_MonthwiseReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = moa.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = moa.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = moa.Machine;
                    cmd.Parameters.Add("@Year", SqlDbType.NVarChar, 150).Value = moa.Year;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = moa.PlantCode;
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
        public HttpResponseMessage GetMOA_YearwiseReport(Models.MOA_YearwiseReport moa)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(moa.CompanyCode, moa.PlantCode, moa.line);
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
                    SqlCommand cmd = new SqlCommand("SP_MOA_YearwiseReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = moa.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = moa.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = moa.Machine;
                    cmd.Parameters.Add("@Year", SqlDbType.NVarChar, 150).Value = moa.Year;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = moa.PlantCode;
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
        public HttpResponseMessage Get_LineReport(Models.Four_LineReport fourLine)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(fourLine.CompanyCode, fourLine.PlantCode, fourLine.line);
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
                    SqlCommand cmd = new SqlCommand("SP_Alarm_Line_POC", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = fourLine.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = fourLine.line;
                    cmd.Parameters.Add("@Fdate", SqlDbType.DateTime, 150).Value = fourLine.FromDate;
                    cmd.Parameters.Add("@Tdate", SqlDbType.DateTime, 150).Value = fourLine.ToDate;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = fourLine.PlantCode;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = fourLine.QueryType;
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
        public HttpResponseMessage Popup_Machinewise_Historic(Models.Popup_Machinewise_Historic popup)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(popup.CompanyCode, popup.PlantCode, popup.line);
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
                    SqlCommand cmd = new SqlCommand("SP_Popup_Machinewise_Historic", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = popup.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = popup.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = popup.Machine;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = popup.PlantCode;
                    cmd.Parameters.Add("@records", SqlDbType.NVarChar, 150).Value = popup.records;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = popup.QueryType;
                    cmd.Parameters.Add("@Variant", SqlDbType.NVarChar, 150).Value = popup.Variant;
                    cmd.Parameters.Add("@loss_category", SqlDbType.NVarChar, 150).Value = popup.loss_category;
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
        public HttpResponseMessage Popup_Machinewise_Historic_Alarm(Models.Popup_Machinewise_Historic popup)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(popup.CompanyCode, popup.PlantCode, popup.line);
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
                    SqlCommand cmd = new SqlCommand("SP_Popup_Machinewise_Historic_Alarm", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = popup.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = popup.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = popup.Machine;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = popup.PlantCode;
                    cmd.Parameters.Add("@records", SqlDbType.NVarChar, 150).Value = popup.records;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = popup.QueryType;
                    cmd.Parameters.Add("@Variant", SqlDbType.NVarChar, 150).Value = popup.Variant;
                    cmd.Parameters.Add("@loss_category", SqlDbType.NVarChar, 150).Value = popup.loss_category;
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
        public HttpResponseMessage Popup_Machinewise_Historic_Rej(Models.Popup_Machinewise_Historic popup)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(popup.CompanyCode, popup.PlantCode, popup.line);
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
                    SqlCommand cmd = new SqlCommand("SP_Popup_Machinewise_Historic_Rej", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = popup.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = popup.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = popup.Machine;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = popup.PlantCode;
                    cmd.Parameters.Add("@records", SqlDbType.NVarChar, 150).Value = popup.records;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = popup.QueryType;
                    cmd.Parameters.Add("@Variant", SqlDbType.NVarChar, 150).Value = popup.Variant;
                    cmd.Parameters.Add("@loss_category", SqlDbType.NVarChar, 150).Value = popup.loss_category;
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
        public HttpResponseMessage Popup_Machinewise_Historic_CT(Models.Popup_Machinewise_Historic popup)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(popup.CompanyCode, popup.PlantCode, popup.line);
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
                    SqlCommand cmd = new SqlCommand("SP_Popup_Machinewise_Historic_CT", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = popup.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = popup.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = popup.Machine;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = popup.PlantCode;
                    cmd.Parameters.Add("@records", SqlDbType.NVarChar, 150).Value = popup.records;
                    //cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = popup.QueryType;
                    cmd.Parameters.Add("@Variant", SqlDbType.NVarChar, 150).Value = popup.Variant;
                    cmd.Parameters.Add("@loss_category", SqlDbType.NVarChar, 150).Value = popup.loss_category;
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
        public HttpResponseMessage Popup_Machinewise_Historic_Loss(Models.Popup_Machinewise_Historic_1 popup)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(popup.CompanyCode, popup.PlantCode, popup.line);
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
                    SqlCommand cmd = new SqlCommand("SP_Popup_Machinewise_Historic_Loss", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = popup.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = popup.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = popup.Machine;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = popup.PlantCode;
                    cmd.Parameters.Add("@records", SqlDbType.NVarChar, 150).Value = popup.records;
                    //cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = popup.QueryType;
                    cmd.Parameters.Add("@Variant", SqlDbType.NVarChar, 150).Value = popup.Variant;
                    cmd.Parameters.Add("@loss_category", SqlDbType.NVarChar, 150).Value = popup.loss_category;
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
        public HttpResponseMessage Check_NoDataAVailable(Models.Check_NoDataAVailable popup)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(popup.CompanyCode, popup.PlantCode, popup.line);
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
                    SqlCommand cmd = new SqlCommand("SP_Check_NoDataAvailable", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = popup.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = popup.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = popup.Machine;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = popup.PlantCode;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { status = "Success", msg = "Details Found", data = ds });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "No Information", data = "" });
                    }
                }
            }
        }


        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage ShiwiseAggreagte(Models.shiftwisedata u)
        {


            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(u.CompanyCode, u.PlantCode, u.line);
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
                    SqlCommand cmd = new SqlCommand("SP_Shiftwise_Variant_Cumulative_Data", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add("@fromdate", SqlDbType.Date, 150).Value = u.FromDate;
                    cmd.Parameters.Add("@todate", SqlDbType.Date, 150).Value = u.ToDate;
                    cmd.Parameters.Add("@machinecode", SqlDbType.NVarChar, 150).Value = u.Machine;
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
        public HttpResponseMessage yearlyShiwiseAggreagte(Models.shiftwisedata u)
        {


            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(u.CompanyCode, u.PlantCode, u.line);
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
                    SqlCommand cmd = new SqlCommand("SP_YearlyShiftwise_Variant_Cumulative_Data", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add("@Year", SqlDbType.NVarChar, 150).Value = u.Year;
                    cmd.Parameters.Add("@machinecode", SqlDbType.NVarChar, 150).Value = u.Machine;
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
        public HttpResponseMessage monthlyShiwiseAggreagte(Models.shiftwisedata u)
        {


            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(u.CompanyCode, u.PlantCode, u.line);
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
                    SqlCommand cmd = new SqlCommand("SP_MonthlyShiftwise_Variant_Cumulative_Data", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add("@Year", SqlDbType.NVarChar, 150).Value = u.Year;
                    cmd.Parameters.Add("@machinecode", SqlDbType.NVarChar, 150).Value = u.Machine;
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
        public HttpResponseMessage DaywiseShiftProduction(Models.shiftwisedata u)
        {


            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(u.CompanyCode, u.PlantCode, u.line);
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
                    SqlCommand cmd = new SqlCommand("SP_Shiftwise_Day_Cumulative_Data", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add("@fromdate", SqlDbType.Date, 150).Value = u.FromDate;
                    cmd.Parameters.Add("@todate", SqlDbType.Date, 150).Value = u.ToDate;
                    cmd.Parameters.Add("@machinecode", SqlDbType.NVarChar, 150).Value = u.Machine;
                    cmd.Parameters.Add("@variant", SqlDbType.NVarChar, 150).Value = u.variant;
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
        public HttpResponseMessage yearlyDaywiseShiftProduction(Models.shiftwisedata u)
        {


            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(u.CompanyCode, u.PlantCode, u.line);
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
                    SqlCommand cmd = new SqlCommand("SP_YearlyShiftwise_Day_Cumulative_Data", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add("@year", SqlDbType.NVarChar, 150).Value = u.Year;
                    cmd.Parameters.Add("@machinecode", SqlDbType.NVarChar, 150).Value = u.Machine;
                    cmd.Parameters.Add("@variant", SqlDbType.NVarChar, 150).Value = u.variant;
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
        public HttpResponseMessage monthlyDaywiseShiftProduction(Models.shiftwisedata u)
        {


            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(u.CompanyCode, u.PlantCode, u.line);
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
                    SqlCommand cmd = new SqlCommand("SP_MonthlyShiftwise_Day_Cumulative_Data", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add("@Year", SqlDbType.NVarChar, 150).Value = u.Year;
                    cmd.Parameters.Add("@machinecode", SqlDbType.NVarChar, 150).Value = u.Machine;
                    cmd.Parameters.Add("@variant", SqlDbType.NVarChar, 150).Value = u.variant;
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
        public HttpResponseMessage MTTRandMTBF(Models.shiftwisedata u)
        {



            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(u.CompanyCode, u.PlantCode, u.line);
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
                    SqlCommand cmd = new SqlCommand("SP_Daywise_Stoppage_Analysis", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add("@FROMDATE", SqlDbType.Date, 150).Value = u.FromDate;
                    cmd.Parameters.Add("@TODATE", SqlDbType.Date, 150).Value = u.ToDate;
                    cmd.Parameters.Add("@MACHINECODE", SqlDbType.NVarChar, 150).Value = u.Machine;

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
        public HttpResponseMessage MonthlyMTTRandMTBF(Models.shiftwisedata u)
        {



            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(u.CompanyCode, u.PlantCode, u.line);
            if (con_string == "0")
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "Coudnot Connect to database", data = "" });

            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    
                    //hii test
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("SP_MonthlyDaywise_Stoppage_Analysis", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;

                    cmd.Parameters.Add("@year", SqlDbType.NVarChar, 150).Value = u.Year;
                    cmd.Parameters.Add("@MACHINECODE", SqlDbType.NVarChar, 150).Value = u.Machine;

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
