using Plant_Digitization_api.Models;
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
    public class OperatorallocationController : ApiController
    {
        //private string connectionstring = ConfigurationManager.ConnectionStrings["con"].ToString();

//-----------------Skill category--------------------

        [HttpPost]
        [CustomAuthenticationFilter]
        public IHttpActionResult Add_Update_Category(Models.Skill_Category R)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(R.Companycode, R.Plantcode, R.LineCode);
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
                    SqlCommand cmd = new SqlCommand("sp_skillcategory", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Id", SqlDbType.NVarChar, 150).Value = R.Id;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = R.QueryType;
                    cmd.Parameters.Add("@Companycode", SqlDbType.NVarChar, 150).Value = R.Companycode;
                    cmd.Parameters.Add("@Plantcode", SqlDbType.NVarChar, 150).Value = R.Plantcode;
                    cmd.Parameters.Add("@Category_name", SqlDbType.NVarChar, 150).Value = R.Category_name;
                    SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                    SQLReturn.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(SQLReturn);
                    cmd.ExecuteNonQuery();
                    response = SQLReturn.Value.ToString().Trim();
                    return Ok(response);

                }

            }

        }

//--------------------Skill Set----------------------

        [HttpPost]
        [CustomAuthenticationFilter]
        public IHttpActionResult Add_Update_Skill_set(Models.Skill_Set R)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(R.Companycode, R.Plantcode, R.LineCode);
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
                    SqlCommand cmd = new SqlCommand("sp_skill_set", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Id", SqlDbType.NVarChar, 150).Value = R.Id;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = R.QueryType;
                    cmd.Parameters.Add("@Companycode", SqlDbType.NVarChar, 150).Value = R.Companycode;
                    cmd.Parameters.Add("@Plantcode", SqlDbType.NVarChar, 150).Value = R.Plantcode;
                    cmd.Parameters.Add("@Category_id", SqlDbType.NVarChar, 150).Value = R.Category_id;
                    cmd.Parameters.Add("@Skill", SqlDbType.NVarChar, 150).Value = R.Skill;
                    SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                    SQLReturn.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(SQLReturn);
                    cmd.ExecuteNonQuery();
                    response = SQLReturn.Value.ToString().Trim();
                    return Ok(response);

                }

            }

        }

//--------------------Machine wise------------------

        [HttpPost]
        [CustomAuthenticationFilter]
        public IHttpActionResult Add_Update_Machinewise_Skill(Machinewise_Skill R)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(R.Companycode, R.Plantcode, R.LineCode);
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
                    SqlCommand cmd = new SqlCommand("sp_machinewise_skill", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add("@Id", SqlDbType.NVarChar, 150).Value = R.Id;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = R.QueryType;
                    cmd.Parameters.Add("@Companycode", SqlDbType.NVarChar, 150).Value = R.Companycode;
                    cmd.Parameters.Add("@Plantcode", SqlDbType.NVarChar, 150).Value = R.Plantcode;
                    cmd.Parameters.Add("@Category_id", SqlDbType.NVarChar, 150).Value = R.Category_id;
                    cmd.Parameters.Add("@Machine_id", SqlDbType.NVarChar, 150).Value = R.Machine_id;
                    SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                    SQLReturn.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(SQLReturn);
                    cmd.ExecuteNonQuery();
                    response = SQLReturn.Value.ToString().Trim();
                    return Ok(response);

                }

            }

        }


        [HttpPost]
        [CustomAuthenticationFilter]
        public IHttpActionResult Add_Update_Machinewise_Skill_list(Machinewise_Skill_list R)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(R.Companycode, R.Plantcode, R.LineCode);
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
                    SqlCommand cmd = new SqlCommand("sp_machinewise_skill_list", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Id", SqlDbType.NVarChar, 150).Value = R.Id;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = R.QueryType;
                    cmd.Parameters.Add("@Companycode", SqlDbType.NVarChar, 150).Value = R.Companycode;
                    cmd.Parameters.Add("@Plantcode", SqlDbType.NVarChar, 150).Value = R.Plantcode;
                    //cmd.Parameters.Add("@Ref_id", SqlDbType.NVarChar, 150).Value = R.Ref_id; //-----changed
                    cmd.Parameters.Add("@Category_id", SqlDbType.NVarChar, 150).Value = R.Category_id;
                    cmd.Parameters.Add("@Machine_id", SqlDbType.NVarChar, 150).Value = R.Machine_id;
                    cmd.Parameters.Add("@Skill_id", SqlDbType.NVarChar, 150).Value = R.Skill_id;
                    cmd.Parameters.Add("@Skill_rank", SqlDbType.NVarChar, 150).Value = R.Skill_rank;
                    SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                    SQLReturn.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(SQLReturn);
                    cmd.ExecuteNonQuery();
                    response = SQLReturn.Value.ToString().Trim();
                    return Ok(response);

                }

            }

        }

//---------------Operator Wise--------------------

        [HttpPost]
        [CustomAuthenticationFilter]
        public IHttpActionResult Add_Update_Operatorwise_skill(Operatorwise_Skill R)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(R.Companycode, R.Plantcode, R.LineCode);
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
                    SqlCommand cmd = new SqlCommand("sp_operatorwise_skill", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ID", SqlDbType.NVarChar, 150).Value = R.ID;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = R.QueryType;
                    cmd.Parameters.Add("@Companycode", SqlDbType.NVarChar, 150).Value = R.Companycode;
                    cmd.Parameters.Add("@Plantcode", SqlDbType.NVarChar, 150).Value = R.Plantcode;
                    cmd.Parameters.Add("@Operator_Id", SqlDbType.NVarChar, 150).Value = R.Operator_Id;
                    cmd.Parameters.Add("@Machine_id", SqlDbType.NVarChar, 150).Value = R.Machine_Id;
                    cmd.Parameters.Add("@Category_Id", SqlDbType.NVarChar, 150).Value = R.Category_Id;
                    SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                    SQLReturn.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(SQLReturn);
                    cmd.ExecuteNonQuery();
                    response = SQLReturn.Value.ToString().Trim();
                    return Ok(response);

                }

            }

        }


        [HttpPost]
        [CustomAuthenticationFilter]
        public IHttpActionResult Add_Update_Operatorwise_skill_list(Operatorwise_Skill_list R)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(R.Companycode, R.Plantcode, R.LineCode);
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
                    SqlCommand cmd = new SqlCommand("sp_operatorwise_skill_list", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ID", SqlDbType.NVarChar, 150).Value = R.ID;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = R.QueryType;
                    cmd.Parameters.Add("@Companycode", SqlDbType.NVarChar, 150).Value = R.Companycode;
                    cmd.Parameters.Add("@Plantcode", SqlDbType.NVarChar, 150).Value = R.Plantcode;
                    cmd.Parameters.Add("@Operator_Id", SqlDbType.NVarChar, 150).Value = R.Operator_Id;

                    cmd.Parameters.Add("@Category_Id", SqlDbType.NVarChar, 150).Value = R.Category_Id;
                    //cmd.Parameters.Add("@Ref_Id", SqlDbType.NVarChar, 150).Value = R.Ref_id;
                    cmd.Parameters.Add("@Skill_Id", SqlDbType.NVarChar, 150).Value = R.Skill_Id;
                    cmd.Parameters.Add("@Skill_Rank", SqlDbType.NVarChar, 150).Value = R.Skill_Rank;
                    SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                    SQLReturn.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(SQLReturn);
                    cmd.ExecuteNonQuery();
                    response = SQLReturn.Value.ToString().Trim();
                    return Ok(response);

                }

            }

        }

//---------------Operator Schedule-------------------

        [HttpPost]
        [CustomAuthenticationFilter]
        public IHttpActionResult Add_Update_Operator_Schedule(Operator_Schedule R)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(R.Companycode, R.Plantcode, R.LineCode);
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
                    SqlCommand cmd = new SqlCommand("sp_operator_schedule", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ID", SqlDbType.Int, 150).Value = R.Id;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = R.QueryType;
                    cmd.Parameters.Add("@Companycode", SqlDbType.NVarChar, 150).Value = R.Companycode;
                    cmd.Parameters.Add("@Plantcode", SqlDbType.NVarChar, 150).Value = R.Plantcode;
                    cmd.Parameters.Add("@Machine_Id", SqlDbType.NVarChar, 150).Value = R.Machine;
                    cmd.Parameters.Add("@Operator_Id", SqlDbType.NVarChar, 150).Value = R.Operator;
                    cmd.Parameters.Add("@Shift", SqlDbType.NVarChar, 150).Value = R.Shift;
                    cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar, 150).Value = R.Remarks;
                    cmd.Parameters.Add("@Ref_id", SqlDbType.Int, 150).Value = R.Ref_id;
                    cmd.Parameters.Add("@FDate", SqlDbType.NVarChar, 150).Value = R.FromDate;
                    cmd.Parameters.Add("@TDate", SqlDbType.NVarChar, 150).Value = R.ToDate;
                    cmd.Parameters.Add("@Days", SqlDbType.NVarChar, 150).Value = R.Days;
                    cmd.Parameters.Add("@Repeat", SqlDbType.NVarChar, 150).Value = R.Repeat;
                    SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                    SQLReturn.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(SQLReturn);
                    cmd.ExecuteNonQuery();
                    response = SQLReturn.Value.ToString().Trim();
                    return Ok(response);

                }

            }

        }



//----------------Operator Allocation Slots---------------------

        //[HttpPost]
        //[CustomAuthenticationFilter]
        //public IHttpActionResult Add_Operator_allocation_slots(Operator_Allocation_Slots R)
        //{
        //    using (SqlConnection con = new SqlConnection(connectionstring))
        //    {
        //        string response = string.Empty;
        //        con.Open();
        //        SqlCommand cmd = new SqlCommand("", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = R.QueryType;
        //        cmd.Parameters.Add("@Companycode", SqlDbType.NVarChar, 150).Value = R.Companycode;
        //        cmd.Parameters.Add("@Plantcode", SqlDbType.NVarChar, 150).Value = R.Plantcode;
        //        cmd.Parameters.Add("@Shift_Id", SqlDbType.NVarChar, 150).Value = R.Shift;
        //        cmd.Parameters.Add("@FDate", SqlDbType.Date, 150).Value = R.FDate;
        //        cmd.Parameters.Add("@TDate", SqlDbType.Date, 150).Value = R.TDate;
        //        cmd.Parameters.Add("@Days", SqlDbType.NVarChar, 150).Value = R.Days;
        //        cmd.Parameters.Add("@Absent_Date", SqlDbType.NVarChar, 150).Value = R.Absent_Date;
        //        SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
        //        SQLReturn.Direction = ParameterDirection.Output;
        //        cmd.Parameters.Add(SQLReturn);
        //        cmd.ExecuteNonQuery();
        //        response = SQLReturn.Value.ToString().Trim();
        //        return Ok(response);

        //    }

        //}


//-------------------List of Absent Details---------------------

        [HttpPost]
        [CustomAuthenticationFilter]
        public IHttpActionResult Add_Update_Absent_Details(Absent_Data R)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(R.Companycode, R.Plantcode, R.LineCode);
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
                    SqlCommand cmd = new SqlCommand("sp_operator_allocation_slots", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ID", SqlDbType.Int, 150).Value = R.ID;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = R.QueryType;
                    cmd.Parameters.Add("@Companycode", SqlDbType.NVarChar, 150).Value = R.Companycode;
                    cmd.Parameters.Add("@Plantcode", SqlDbType.NVarChar, 150).Value = R.Plantcode;
                    //cmd.Parameters.Add("@Shift", SqlDbType.NVarChar, 150).Value = R.Shift;
                    cmd.Parameters.Add("@Operator_id", SqlDbType.NVarChar, 150).Value = R.Operator;
                    cmd.Parameters.Add("@Absent_Date", SqlDbType.Date, 150).Value = R.Absent_Date;
                    SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                    SQLReturn.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(SQLReturn);
                    cmd.ExecuteNonQuery();
                    response = SQLReturn.Value.ToString().Trim();
                    return Ok(response);

                }

            }

        }


//-----------------Skill wise Report--------------------

        [HttpPost]
        [CustomAuthenticationFilter]
        public IHttpActionResult Select_Skillwise_Report(Skill_Wise_Report R)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(R.Companycode, R.Plantcode, R.LineCode);
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
                    SqlCommand cmd = new SqlCommand("", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = R.QueryType;
                    cmd.Parameters.Add("@Companycode", SqlDbType.NVarChar, 150).Value = R.Companycode;
                    cmd.Parameters.Add("@Plantcode", SqlDbType.NVarChar, 150).Value = R.Plantcode;
                    cmd.Parameters.Add("@Machine_Id", SqlDbType.NVarChar, 150).Value = R.Machine;
                    cmd.Parameters.Add("@Operator_Id", SqlDbType.NVarChar, 150).Value = R.Operator;
                    cmd.Parameters.Add("@Skill", SqlDbType.NVarChar, 150).Value = R.Skill_id;
                    cmd.Parameters.Add("@Skill_rank", SqlDbType.NVarChar, 150).Value = R.Skill_Rank;
                    SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                    SQLReturn.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(SQLReturn);
                    cmd.ExecuteNonQuery();
                    response = SQLReturn.Value.ToString().Trim();
                    return Ok(response);

                }
            }
            
        }

        //[HttpPost]
        //[CustomAuthenticationFilter]

        //public IHttpActionResult Skillwise_Report(Skill_Wise_Report S)
        //{
        //    using(SqlConnection con = new SqlConnection(connectionstring))
        //    {
        //        string response = string.Empty;
        //        DataSet ds = new DataSet();
        //        con.Open();
        //        SqlCommand cmd = new SqlCommand("", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(ds);


        //    }


        //}





    }
}
