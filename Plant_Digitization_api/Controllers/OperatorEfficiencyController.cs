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

    public class OperatorEfficiencyController : ApiController
    {
        private string connectionstring = ConfigurationManager.ConnectionStrings["con"].ToString();
        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage GetOPEDaywise(Models.OPEDaywisereport ope)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(ope.CompanyCode, ope.PlantCode, ope.linecode);
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
                    SqlCommand cmd = new SqlCommand("SP_OPE_DaywiseReport ", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = ope.linecode;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = ope.machinecode;
                    cmd.Parameters.Add("@Variant", SqlDbType.NVarChar, 150).Value = ope.variantcode;
                    cmd.Parameters.Add("@operatorID", SqlDbType.NVarChar, 150).Value = ope.operatorid;
                    cmd.Parameters.Add("@Year", SqlDbType.NVarChar, 150).Value = ope.month;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = ope.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = ope.PlantCode;
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
        public HttpResponseMessage GetOPEweekwise(Models.OPEweekwisereport ope)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(ope.CompanyCode, ope.PlantCode, ope.linecode);
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
                    SqlCommand cmd = new SqlCommand("SP_OPE_WeekwiseReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = ope.linecode;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = ope.machinecode;
                    cmd.Parameters.Add("@Variant", SqlDbType.NVarChar, 150).Value = ope.variantcode;
                    cmd.Parameters.Add("@operatorID", SqlDbType.NVarChar, 150).Value = ope.operatorid;
                    cmd.Parameters.Add("@Year", SqlDbType.NVarChar, 150).Value = ope.month;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = ope.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = ope.PlantCode;
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
        public HttpResponseMessage GetOPEshiftwise(Models.OPEshiftwisereport ope)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(ope.CompanyCode, ope.PlantCode, ope.linecode);
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
                    SqlCommand cmd = new SqlCommand("SP_OPE_ShiftwiseReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = ope.linecode;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = ope.machinecode;
                    cmd.Parameters.Add("@Variant", SqlDbType.NVarChar, 150).Value = ope.variantcode;
                    cmd.Parameters.Add("@operatorID", SqlDbType.NVarChar, 150).Value = ope.operatorid;
                    cmd.Parameters.Add("@Year", SqlDbType.NVarChar, 150).Value = ope.month;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = ope.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = ope.PlantCode;
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
        public HttpResponseMessage GetOPEyearwise(Models.OPEyearwisereport ope)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(ope.CompanyCode, ope.PlantCode, ope.linecode);
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
                    SqlCommand cmd = new SqlCommand("SP_OPE_YearwiseReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = ope.linecode;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = ope.machinecode;
                    cmd.Parameters.Add("@Variant", SqlDbType.NVarChar, 150).Value = ope.variantcode;
                    cmd.Parameters.Add("@operatorID", SqlDbType.NVarChar, 150).Value = ope.operatorid;
                    cmd.Parameters.Add("@Year", SqlDbType.NVarChar, 150).Value = ope.year;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = ope.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = ope.PlantCode;
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
        public HttpResponseMessage Getcustomwisereport(Models.OPEcustomwisereport ope)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(ope.CompanyCode, ope.PlantCode, ope.linecode);
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
                    SqlCommand cmd = new SqlCommand("SP_OPE_CustomReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = ope.linecode;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = ope.machinecode;
                    cmd.Parameters.Add("@Variant", SqlDbType.NVarChar, 150).Value = ope.variantcode;
                    cmd.Parameters.Add("@operatorID", SqlDbType.NVarChar, 150).Value = ope.operatorid;
                    cmd.Parameters.Add("@FromDate", SqlDbType.NVarChar, 150).Value = ope.FromDate;
                    cmd.Parameters.Add("@ToDate", SqlDbType.NVarChar, 150).Value = ope.ToDate;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = ope.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = ope.PlantCode;
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
