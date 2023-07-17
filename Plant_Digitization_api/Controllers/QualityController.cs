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

    public class QualityController : ApiController
    {
        private string connectionstring = ConfigurationManager.ConnectionStrings["con"].ToString();
        //to get rejection pareto data
        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage GetRejection_Pareto_CustomReport(Models.Rejection_CustomReport rej)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(rej.CompanyCode, rej.PlantCode, rej.line);
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
                    SqlCommand cmd = new SqlCommand("SP_Rejection_Pareto_CustomReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = rej.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = rej.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = rej.Machine;
                    cmd.Parameters.Add("@FromDate", SqlDbType.DateTime, 150).Value = rej.FromDate;
                    cmd.Parameters.Add("@ToDate", SqlDbType.DateTime, 150).Value = rej.ToDate;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = rej.PlantCode;
                    cmd.Parameters.Add("@Variant", SqlDbType.NVarChar, 150).Value = rej.Variant;
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

        //Weekwise quality trends
        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage GetRejection_Pareto_WeekwiseReport(Models.quality_week_wise Q)
        {
            try
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
                        con.Open();
                        DataSet ds = new DataSet();
                        SqlCommand cmd = new SqlCommand("SP_Rejection_Pareto_WeekReport", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = Q.CompanyCode;
                        cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = Q.line;
                        cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = Q.Machine;
                        cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = Q.PlantCode;


                        cmd.Parameters.Add("@Date", SqlDbType.NVarChar, 150).Value = Q.Date;
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
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "No Data Available", data = "" });

            }
        }


        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage GetRejection_Pareto_MonthwiseReport(Models.MTBF_MonthwiseReport mtbf)
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
                    SqlCommand cmd = new SqlCommand("SP_Rejection_Pareto_MonthwiseReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = mtbf.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = mtbf.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = mtbf.Machine;
                    cmd.Parameters.Add("@Year", SqlDbType.NVarChar, 150).Value = mtbf.Year;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = mtbf.PlantCode;
                    cmd.Parameters.Add("@Variant", SqlDbType.NVarChar, 150).Value = mtbf.Variant;
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
        public HttpResponseMessage GetRejection_Pareto_YearwiseReport(Models.MTBF_MonthwiseReport mtbf)
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
                    SqlCommand cmd = new SqlCommand("SP_Rejection_Pareto_YearwiseReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = mtbf.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = mtbf.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = mtbf.Machine;
                    cmd.Parameters.Add("@Year", SqlDbType.NVarChar, 150).Value = mtbf.Year;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = mtbf.PlantCode;
                    cmd.Parameters.Add("@Variant", SqlDbType.NVarChar, 150).Value = mtbf.Variant;
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
        public HttpResponseMessage GetRejection_Heatmap_CustomReport(Models.Rejection_CustomReport rej)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(rej.CompanyCode, rej.PlantCode, rej.line);
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
                    SqlCommand cmd = new SqlCommand("SP_Rejection_HeatMap_CustomReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = rej.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = rej.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = rej.Machine;
                    cmd.Parameters.Add("@FromDate", SqlDbType.DateTime, 150).Value = rej.FromDate;
                    cmd.Parameters.Add("@ToDate", SqlDbType.DateTime, 150).Value = rej.ToDate;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = rej.PlantCode;
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
        public HttpResponseMessage GetRejection_Heatmap_MonthwiseReport(Models.MTBF_MonthwiseReport mtbf)
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
                    SqlCommand cmd = new SqlCommand("SP_Rejection_HeatMap_MonthwiseReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = mtbf.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = mtbf.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = mtbf.Machine;
                    cmd.Parameters.Add("@Year", SqlDbType.NVarChar, 150).Value = mtbf.Year;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = mtbf.PlantCode;
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
        public HttpResponseMessage GetRejection_Heatmap_YearwiseReport(Models.MTBF_MonthwiseReport mtbf)
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
                    SqlCommand cmd = new SqlCommand("SP_Rejection_HeatMap_YearwiseReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = mtbf.CompanyCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = mtbf.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = mtbf.Machine;
                    cmd.Parameters.Add("@Year", SqlDbType.NVarChar, 150).Value = mtbf.Year;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = mtbf.PlantCode;
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
        public HttpResponseMessage GetQuality_details(Models.live_report Q)
        {

            try {
                database_connection d = new database_connection();
                string con_string = d.Getconnectionstring(Q.CompanyCode, Q.PlantCode, Q.linecode);
                if (con_string == "0")
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "Coudnot Connect to database", data = "" });

                }
                else
                {
                    using (SqlConnection con = new SqlConnection(con_string))
                    {

                        var result = new List<Models.live_report>();
                        var result1 = new List<Models.live_report>();
                        var endresult = new List<Models.live_report>();
                        con.Open();
                        DataSet ds = new DataSet();
                        SqlCommand cmd = new SqlCommand("SP_Live_QA_Rejection", con);
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = Q.CompanyCode;
                        cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = Q.PlantCode;
                        cmd.Parameters.Add("@linecode", SqlDbType.NVarChar, 150).Value = Q.linecode;
                        cmd.Parameters.Add("@Machinecode", SqlDbType.NVarChar, 150).Value = Q.Machinecode;
                        cmd.Parameters.Add("@Shift", SqlDbType.NVarChar, 150).Value = Q.Shift;
                        //var reader = cmd.ExecuteReader();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(ds);
                        if (ds.Tables.Count != 0)
                        {
                            //foreach (DataTable data in ds.Tables)
                            //{
                            foreach (DataRow dr in ds.Tables[0].Rows)
                            {
                                result.Add(item: new Models.live_report
                                {
                                    //  last_rejection = Convert.ToString(reader["lastRejection"] == DBNull.Value ? "" : reader["lastRejection"]),
                                    mins_ago = Convert.ToString(dr["Mins_Ago"] == DBNull.Value ? "" : dr["Mins_Ago"]),
                                    components = Convert.ToString(dr["components"] == DBNull.Value ? "" : dr["components"]),
                                    no_of_times = Convert.ToString(dr["no_oftimes"] == DBNull.Value ? "" : dr["no_oftimes"]),
                                    continuous_rejection = Convert.ToString(dr["continuous_rejection"] == DBNull.Value ? "" : dr["continuous_rejection"]),
                                    batch = Convert.ToString(dr["batch"] == DBNull.Value ? "" : dr["batch"]),
                                    reason = Convert.ToString(dr["reason"] == DBNull.Value ? "" : dr["reason"]),

                                });
                            }

                            //}
                            foreach (DataRow dr in ds.Tables[1].Rows)
                            {
                                result1.Add(item: new Models.live_report
                                {
                                    operator_name = Convert.ToString(dr["OperatorName"] == DBNull.Value ? "" : dr["OperatorName"]),
                                    ok_parts = Convert.ToString(dr["TotalOkParts"] == DBNull.Value ? "" : dr["TotalOkParts"]),
                                    not_parts = Convert.ToString(dr["TotalNOkParts"] == DBNull.Value ? "" : dr["TotalNOkParts"]),
                                    rework_parts = Convert.ToString(dr["TotalReworkParts"] == DBNull.Value ? "" : dr["TotalReworkParts"]),
                                    stime = Convert.ToString(dr["sTime"] == DBNull.Value ? "" : dr["sTime"]),
                                    ctime = Convert.ToString(dr["cTime"] == DBNull.Value ? "" : dr["cTime"]),
                                    target_qty = Convert.ToString(dr["target_qty"] == DBNull.Value ? "" : dr["target_qty"]),
                                    shift_id = Convert.ToString(dr["shift_id"] == DBNull.Value ? "" : dr["shift_id"]),
                                    no_of_stoppage = Convert.ToString(dr["no_of_stoppage"] == DBNull.Value ? "" : dr["no_of_stoppage"]),

                                });
                            }
                        }
                        endresult = result.Concat(result1).ToList();


                        if (endresult.Any())
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new { status = "Success", msg = "Details Found", data = endresult.ToArray() });
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "No Data Available", data = "" });
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "No Data Available", data = "" }); 
            }
            
        }

        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage GetShiftId(Models.live_report Q)
        {
            try
            {
                database_connection d = new database_connection();
                string con_string = d.Getconnectionstring(Q.CompanyCode, Q.PlantCode, Q.linecode);
                if (con_string == "0")
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "Coudnot Connect to database", data = "" });

                }
                else
                {
                    using (SqlConnection con = new SqlConnection(con_string))
                    {
                        string shiftname = "s2";
                        con.Open();
                        DataSet ds = new DataSet();
                        SqlCommand cmd = new SqlCommand("select ShiftName from ShiftSetting where [CompanyCode] = @CompanyCode and[PlantCode] = @PlantCode and[LineCode] = @linecode and StartTime < Convert(Time, dateadd(mi, 330, getdate())) and EndTime > Convert(Time, dateadd(mi, 330, getdate()))", con);

                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = Q.CompanyCode;
                        cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = Q.PlantCode;
                        cmd.Parameters.Add("@linecode", SqlDbType.NVarChar, 150).Value = Q.linecode;



                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(ds);
                        object r1 = cmd.ExecuteScalar();

                        if (r1 != null)
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new { status = "Success", msg = "Details Found", data = r1.ToString() });
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "No Data Available", data = "" });
                        }

                    }
                }
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "No Data Available", data = "" }); ;
            }
        }


        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage GetShifts(Models.live_report Q)
        {

            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(Q.CompanyCode, Q.PlantCode, Q.linecode);
            if (con_string == "0")
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "Coudnot Connect to database", data = "" });

            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    var result = new List<Models.quality_shift_wise>();
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("select ShiftName from ShiftSetting where CompanyCode = @CompanyCode and PlantCode = @PlantCode and LineCode = @linecode )", con);

                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = Q.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = Q.PlantCode;
                    cmd.Parameters.Add("@linecode", SqlDbType.NVarChar, 150).Value = Q.linecode;

                    cmd.ExecuteNonQuery();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);


                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { status = "Success", data = ds });
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
        public HttpResponseMessage GetShiftwise_Quality(Models.quality_shift_wise Q)
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
                    var result = new List<Models.quality_shift_wise>();
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("SP_Quality_ShiftwiseReportnew", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = Q.QueryType;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = Q.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = Q.PlantCode;
                    cmd.Parameters.Add("@LineCode", SqlDbType.NVarChar, 150).Value = Q.line;
                    //cmd.Parameters.Add("@shift", SqlDbType.NVarChar, 150).Value = Q.shift;
                    //cmd.Parameters.Add("@Date", SqlDbType.NVarChar, 150).Value = Q.Date;
                    cmd.Parameters.Add("@MachineCode", SqlDbType.NVarChar, 150).Value = Q.Machine;
                    //cmd.Parameters.Add("@Report_type", SqlDbType.NVarChar, 150).Value = Q.Report_type;
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
        public HttpResponseMessage GetSpecific_Reason_hourly(Models.specific_reason_hourly S)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(S.CompanyCode, S.PlantCode, S.linecode);
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
                    SqlCommand cmd = new SqlCommand("SP_SpecificReasonWise_Hourly", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = S.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = S.PlantCode;
                    cmd.Parameters.Add("@linecode", SqlDbType.NVarChar, 150).Value = S.linecode;
                    cmd.Parameters.Add("@Machinecode", SqlDbType.NVarChar, 150).Value = S.Machinecode;
                    cmd.Parameters.Add("@shift", SqlDbType.NVarChar, 150).Value = S.shift;
                    cmd.Parameters.Add("@RejectReason", SqlDbType.NVarChar, 150).Value = S.RejectReason;
                    cmd.Parameters.Add("@Report_type", SqlDbType.NVarChar, 150).Value = S.Report_type;
                    cmd.Parameters.Add("@subassembly", SqlDbType.NVarChar, 150).Value = S.subassembly;

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

        //Daywise Quality trends
        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage GetDaywise_Quality(Models.quality_day_wise Q)
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
                    SqlCommand cmd = new SqlCommand("SP_Quality_DaywiseReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = Q.QueryType;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = Q.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = Q.PlantCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = Q.line;
                    cmd.Parameters.Add("@Date", SqlDbType.NVarChar, 150).Value = Q.Date;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = Q.Machine;
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
        public HttpResponseMessage GetSpecific_Reason_daywise(Models.specific_reason_daywise S)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(S.CompanyCode, S.PlantCode, S.line);
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
                    SqlCommand cmd = new SqlCommand("SP_Specific_reason_DaywiseReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = S.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = S.PlantCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = S.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = S.Machine;
                    cmd.Parameters.Add("@Reason", SqlDbType.NVarChar, 150).Value = S.Reason;
                    cmd.Parameters.Add("@Date", SqlDbType.NVarChar, 150).Value = S.Date;
                    cmd.Parameters.Add("@subassembly", SqlDbType.NVarChar, 150).Value = S.subassembly;

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


        //Monthwise Quality trends
        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage GetMonthwise_Quality(Models.quality_month_wise Q)
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
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("SP_Quality_MonthwiseReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = Q.QueryType;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = Q.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = Q.PlantCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = Q.line;
                    cmd.Parameters.Add("@Year", SqlDbType.NVarChar, 150).Value = Q.Date;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = Q.Machine;
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
        public HttpResponseMessage GetSpecific_Reason_monthwise(Models.specific_reason_daywise S)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(S.CompanyCode, S.PlantCode, S.line);
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
                    SqlCommand cmd = new SqlCommand("SP_Specific_reason_MonthwiseReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = S.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = S.PlantCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = S.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = S.Machine;
                    cmd.Parameters.Add("@Reason", SqlDbType.NVarChar, 150).Value = S.Reason;
                    cmd.Parameters.Add("@Date", SqlDbType.NVarChar, 150).Value = S.Date;
                    cmd.Parameters.Add("@subassembly", SqlDbType.NVarChar, 150).Value = S.subassembly;

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


        //Yearwise Quality trends
        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage GetYearwise_Quality(Models.quality_year_wise Q)
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
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("SP_Quality_YearwiseReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = Q.QueryType;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = Q.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = Q.PlantCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = Q.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = Q.Machine;
                    cmd.Parameters.Add("@Year", SqlDbType.NVarChar, 150).Value = Q.Date;
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
        public HttpResponseMessage GetSpecific_Reason_yearwise(Models.specific_reason_daywise S)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(S.CompanyCode, S.PlantCode, S.line);
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
                    SqlCommand cmd = new SqlCommand("SP_Specific_reason_YearwiseReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = S.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = S.PlantCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = S.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = S.Machine;
                    cmd.Parameters.Add("@Reason", SqlDbType.NVarChar, 150).Value = S.Reason;
                    cmd.Parameters.Add("@Date", SqlDbType.NVarChar, 150).Value = S.Date;
                    cmd.Parameters.Add("@subassembly", SqlDbType.NVarChar, 150).Value = S.subassembly;

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

        //Custom quality trends
        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage GetCustom_Quality(Models.quality_custom_wise Q)
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
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("SP_Quality_CustomReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = Q.QueryType;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = Q.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = Q.PlantCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = Q.line;
                    cmd.Parameters.Add("@FDate", SqlDbType.NVarChar, 150).Value = Q.FDate;
                    cmd.Parameters.Add("@TDate", SqlDbType.NVarChar, 150).Value = Q.TDate;
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
        public HttpResponseMessage GetSpecific_Reason_customwise(Models.specific_reason_custom S)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(S.CompanyCode, S.PlantCode, S.line);
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
                    SqlCommand cmd = new SqlCommand("SP_Specific_reason_CustomReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = S.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = S.PlantCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = S.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = S.Machine;
                    cmd.Parameters.Add("@Reason", SqlDbType.NVarChar, 150).Value = S.Reason;
                    cmd.Parameters.Add("@FDate", SqlDbType.NVarChar, 150).Value = S.FDate;
                    cmd.Parameters.Add("@TDate", SqlDbType.NVarChar, 150).Value = S.TDate;
                    cmd.Parameters.Add("@subassembly", SqlDbType.NVarChar, 150).Value = S.subassembly;

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


        //Weekwise quality trends
        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage GetWeekwise_Quality(Models.quality_week_wise Q)
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
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("SP_Quality_WeekwiseReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = Q.QueryType;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = Q.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = Q.PlantCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = Q.line;
                    cmd.Parameters.Add("@Date", SqlDbType.NVarChar, 150).Value = Q.Date;
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
        public HttpResponseMessage GetSpecific_Reason_weekwise(Models.specific_reason_daywise S)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(S.CompanyCode, S.PlantCode, S.line);
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
                    SqlCommand cmd = new SqlCommand("SP_Specific_reason_WeekwiseReport", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = S.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = S.PlantCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = S.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = S.Machine;
                    cmd.Parameters.Add("@Reason", SqlDbType.NVarChar, 150).Value = S.Reason;
                    cmd.Parameters.Add("@Date", SqlDbType.NVarChar, 150).Value = S.Date;
                    cmd.Parameters.Add("@subassembly", SqlDbType.NVarChar, 150).Value = S.subassembly;

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
        public HttpResponseMessage GetSpecific_Reason_timestamp(Models.specific_timestamp S)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(S.CompanyCode, S.PlantCode, S.line);
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
                    SqlCommand cmd = new SqlCommand("SP_Specific_reason_timestamp", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = S.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = S.PlantCode;
                    cmd.Parameters.Add("@line", SqlDbType.NVarChar, 150).Value = S.line;
                    cmd.Parameters.Add("@Machine", SqlDbType.NVarChar, 150).Value = S.Machine;
                    cmd.Parameters.Add("@Reason", SqlDbType.NVarChar, 150).Value = S.Reason;
                    cmd.Parameters.Add("@Date", SqlDbType.NVarChar, 150).Value = S.Date;
                    cmd.Parameters.Add("@Shift_id", SqlDbType.NVarChar, 150).Value = S.Shift_id;
                    cmd.Parameters.Add("@Report_type", SqlDbType.NVarChar, 150).Value = S.Report_type;

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
