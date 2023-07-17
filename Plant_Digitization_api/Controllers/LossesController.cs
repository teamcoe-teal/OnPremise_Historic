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
    public class LossesController : ApiController
    {
        private string connectionstring = ConfigurationManager.ConnectionStrings["con"].ToString();

        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage Get_Loss_live_details(Models.Losses L)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(L.CompanyCode, L.PlantCode, L.Line);
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
                    SqlCommand cmd = new SqlCommand("SP_16_Big_loss_live", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = L.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = L.PlantCode;
                    cmd.Parameters.Add("@Line", SqlDbType.NVarChar, 150).Value = L.Line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = L.Machine;
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
        public HttpResponseMessage Get_Loss_Customwise(Models.Loss_custom L)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(L.CompanyCode, L.PlantCode, L.line);
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
                    SqlCommand cmd = new SqlCommand("SP_16Loss_CustomReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = L.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = L.PlantCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = L.line;
                    cmd.Parameters.Add("@FDate", SqlDbType.NVarChar, 150).Value = L.FDate;
                    cmd.Parameters.Add("@TDate", SqlDbType.NVarChar, 150).Value = L.TDate;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = L.Machine;
                    cmd.Parameters.Add("@Loss", SqlDbType.NVarChar, 150).Value = L.Loss;
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
        public HttpResponseMessage Get_Loss_Daywise(Models.Loss_days L)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(L.CompanyCode, L.PlantCode, L.line);
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
                    SqlCommand cmd = new SqlCommand("SP_16Loss_DaywiseReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = L.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = L.PlantCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = L.line;
                    cmd.Parameters.Add("@Date", SqlDbType.NVarChar, 150).Value = L.Date;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = L.Machine;
                    cmd.Parameters.Add("@Loss", SqlDbType.NVarChar, 150).Value = L.Loss;
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
        public HttpResponseMessage Get_Loss_Monthwise(Models.Loss_month L)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(L.CompanyCode, L.PlantCode, L.line);
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
                    SqlCommand cmd = new SqlCommand("SP_16Loss_MonthwiseReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = L.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = L.PlantCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = L.line;
                    cmd.Parameters.Add("@Date", SqlDbType.NVarChar, 150).Value = L.Date;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = L.Machine;
                    cmd.Parameters.Add("@Loss", SqlDbType.NVarChar, 150).Value = L.Loss;
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
        public HttpResponseMessage Get_Loss_Shiftwise(Models.Loss_shift L)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(L.CompanyCode, L.PlantCode, L.line);
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
                    SqlCommand cmd = new SqlCommand("SP_16Loss_ShiftwiseReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = L.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = L.PlantCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = L.line;
                    cmd.Parameters.Add("@shift", SqlDbType.NVarChar, 150).Value = L.shift;
                    cmd.Parameters.Add("@Date", SqlDbType.NVarChar, 150).Value = L.Date;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = L.Machine;
                    cmd.Parameters.Add("@Loss", SqlDbType.NVarChar, 150).Value = L.Loss;
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
        public HttpResponseMessage Get_Loss_Yearwise(Models.Loss_year L)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(L.CompanyCode, L.PlantCode, L.line);
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
                    SqlCommand cmd = new SqlCommand("SP_16Loss_YearwiseReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = L.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = L.PlantCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = L.line;
                    cmd.Parameters.Add("@Date", SqlDbType.NVarChar, 150).Value = L.Date;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = L.Machine;
                    cmd.Parameters.Add("@Loss", SqlDbType.NVarChar, 150).Value = L.Loss;
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
        public HttpResponseMessage Get_Loss_Occurence_chart(Models.Occurence_chart L)
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
                    SqlCommand cmd = new SqlCommand("SP_Loss_Occurence_Chart", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = L.QueryType;
                    cmd.Parameters.Add("@Line_Code", SqlDbType.NVarChar, 150).Value = L.Line_Code;
                    cmd.Parameters.Add("@Machine_Code", SqlDbType.NVarChar, 150).Value = L.Machine_Code;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = L.CompanyCode;
                    cmd.Parameters.Add("@Plantcode", SqlDbType.NVarChar, 150).Value = L.PlantCode;
                    cmd.Parameters.Add("@date", SqlDbType.NVarChar, 150).Value = L.date;
                    cmd.Parameters.Add("@month", SqlDbType.NVarChar, 150).Value = L.month;
                    cmd.Parameters.Add("@year", SqlDbType.NVarChar, 150).Value = L.year;
                    cmd.Parameters.Add("@shiftid", SqlDbType.NVarChar, 150).Value = L.shiftid;
                    cmd.Parameters.Add("@fromdate", SqlDbType.NVarChar, 150).Value = L.fromdate;
                    cmd.Parameters.Add("@todate", SqlDbType.NVarChar, 150).Value = L.todate;
                    cmd.Parameters.Add("@report_type", SqlDbType.NVarChar, 150).Value = L.report_type;
                    cmd.Parameters.Add("@loss_category", SqlDbType.NVarChar, 150).Value = L.loss_category;


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
        public IHttpActionResult Insert_loss_details(Models.Loss_de de)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(de.CompanyCode, de.PlantCode, de.Line_Code);
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
                    SqlCommand cmd = new SqlCommand("SP_Insert_Loss_details", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@Querytype", SqlDbType.NVarChar, 150).Value = de.querytype;
                    cmd.Parameters.Add("@ID", SqlDbType.NVarChar, 150).Value = de.ID;
                    cmd.Parameters.Add("@Reason", SqlDbType.NVarChar, 150).Value = de.Reason;
                    cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar, 150).Value = de.Remarks;
                    cmd.Parameters.Add("@ETime", SqlDbType.NVarChar, 150).Value = de.ETime;
                    cmd.Parameters.Add("@STime", SqlDbType.NVarChar, 150).Value = de.STime;
                    cmd.Parameters.Add("@newstart", SqlDbType.NVarChar, 150).Value = de.newstart;
                    cmd.Parameters.Add("@newend", SqlDbType.NVarChar, 150).Value = de.newend;
                    // cmd.Parameters.Add("@lossid", SqlDbType.NVarChar, 150).Value = de.lossid;
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
