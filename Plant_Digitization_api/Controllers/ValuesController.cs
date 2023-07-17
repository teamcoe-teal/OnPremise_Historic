using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Security.Cryptography;
using System.Text;

namespace Plant_Digitization_api.Controllers
{
    public class ValuesController : ApiController
    {
        static string key { get; set; } = "A!9HHhi%XjjYY4YP2@Nob009X";
        private string connectionstring = ConfigurationManager.ConnectionStrings["con"].ToString();
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
        [HttpPost]
        [Obsolete]
        [CustomAuthenticationFilter]
        public HttpResponseMessage User_settings_details(Models.Setting U)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(U.CompanyCode, U.PlantCode, U.LineCode);
            if (con_string == "0")
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "Coudnot Connect to database", data = "" });

            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    string response = string.Empty;
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("SP_GetSettings_data", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = U.QueryType;
                    cmd.Parameters.Add("@Parameter", SqlDbType.NVarChar, 150).Value = U.Parameter;
                    cmd.Parameters.Add("@Parameter1", SqlDbType.NVarChar, 150).Value = U.Parameter1;
                    cmd.Parameters.Add("@Parameter2", SqlDbType.NVarChar, 150).Value = U.Parameter2;
                    cmd.Parameters.Add("@Parameter4", SqlDbType.NVarChar, 150).Value = U.Parameter4;
                    cmd.Parameters.Add("@Parameter3", SqlDbType.NVarChar, 150).Value = U.Parameter3;
                    cmd.Parameters.Add("@Parameter5", SqlDbType.NVarChar, 150).Value = U.Parameter5;
                    cmd.Parameters.Add("@Parameter6", SqlDbType.NVarChar, 150).Value = U.Parameter6;


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

        public IHttpActionResult settings_details(Models.Setting U)
        {
            try
            {
                database_connection d = new database_connection();
                string con_string = d.Getconnectionstring(U.CompanyCode, U.PlantCode, U.LineCode);
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
                        DataSet ds = new DataSet();
                        SqlCommand cmd = new SqlCommand("SP_GetSettings_data", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = U.QueryType;
                        cmd.Parameters.Add("@Parameter", SqlDbType.NVarChar, 150).Value = U.Parameter;
                        cmd.Parameters.Add("@Parameter1", SqlDbType.NVarChar, 150).Value = U.Parameter1;
                        cmd.Parameters.Add("@Parameter2", SqlDbType.NVarChar, 150).Value = U.Parameter2;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(ds);
                        if (ds.Tables.Count != 0)
                        {
                            return Ok(ds.Tables[0]);
                        }
                        else
                        {
                            return Ok(new string[0]);
                        }
                        //return Ok(ds.Tables[0]);
                    }
                }
            }
            catch(Exception ex)
            {
                return Ok("1");
            }
        }

        [HttpPost]
        [CustomAuthenticationFilter]
        public IHttpActionResult delete_User_settings_details(Models.Setting U)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(U.CompanyCode, U.PlantCode, U.LineCode);
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
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("SP_delete_usersettings", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = U.QueryType;
                    cmd.Parameters.Add("@Parameter", SqlDbType.NVarChar, 150).Value = U.Parameter;
                    cmd.Parameters.Add("@Parameter1", SqlDbType.NVarChar, 150).Value = U.Parameter1;
                    cmd.Parameters.Add("@Parameter2", SqlDbType.NVarChar, 150).Value = U.Parameter2;
                    cmd.Parameters.Add("@Parameter3", SqlDbType.NVarChar, 150).Value = U.Parameter3;
                    cmd.Parameters.Add("@Parameter4", SqlDbType.NVarChar, 150).Value = U.Parameter4;
                    cmd.Parameters.Add("@Parameter5", SqlDbType.NVarChar, 150).Value = U.Parameter5;
                    cmd.Parameters.Add("@Parameter6", SqlDbType.NVarChar, 150).Value = U.Parameter6;
                    cmd.ExecuteNonQuery();
                    return Ok(response);
                }
            }
        }
        [HttpPost]
        [CustomAuthenticationFilter]
        public IHttpActionResult new_delete_User_settings_details(Models.Setting U)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(U.CompanyCode, U.PlantCode, U.LineCode);
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
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("SP_new_delete_usersettings", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = U.QueryType;
                    cmd.Parameters.Add("@Parameter", SqlDbType.NVarChar, 150).Value = U.Parameter;
                    cmd.Parameters.Add("@Parameter1", SqlDbType.NVarChar, 150).Value = U.Parameter1;
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
        public HttpResponseMessage GetSettingdatas1(Models.SettingData tool)
        {
            var fronthash = tool.HashId;
            var string1 = tool.CompanyCode + tool.PlantCode;
            var backhash = Encrypt(string1);
            if (fronthash == backhash)
            {
               
                    using (SqlConnection con = new SqlConnection(connectionstring))
                    {
                        //To Get the response from database and display from user
                        var result = new List<Models.SettingData>();
                        con.Open();
                        SqlCommand cmd = new SqlCommand("SP_Settings_data", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Flag", SqlDbType.NVarChar, 150).Value = tool.Flag;
                        cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = tool.CompanyCode;
                        cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = tool.PlantCode;
                        cmd.Parameters.Add("@LineCode", SqlDbType.NVarChar, 150).Value = tool.LineCode;
                        cmd.Parameters.Add("@MachineCode", SqlDbType.NVarChar, 150).Value = tool.MachineCode;
                    cmd.Parameters.Add("@Parameter", SqlDbType.NVarChar, 150).Value = tool.Parameter;
                    var reader = cmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        DataTable dtData = new DataTable("Data");
                        DataTable dtSchema = new DataTable("Schema");
                        //SqlDataAdapter da = new SqlDataAdapter(cmd);
                        //da.Fill(dt);
                        while (reader.Read())
                        {

                            result.Add(item: new Models.SettingData
                            {
                                //Name = (string)reader["Name"],
                                //Code = (string)reader["Code"],
                                Name = Convert.ToString(reader["Name"] == DBNull.Value ? "" : reader["Name"]),
                                Code = Convert.ToString(reader["Code"] == DBNull.Value ? "" : reader["Code"]),
                            });
                        }

                        if (result.Any())
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new { data = result.ToArray() });
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "No Data Available", data = "" });
                        }
                    }
               

            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "No Data Available", data = "" });
            }
        }
        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage GetSettingdatas(Models.SettingData tool)
        {
           
                using (SqlConnection con = new SqlConnection(connectionstring))
                {
                    //To Get the response from database and display from user
                    var result = new List<Models.SettingData>();
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SP_Settings_data", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@Flag", SqlDbType.NVarChar, 150).Value = tool.Flag;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = tool.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = tool.PlantCode;
                    cmd.Parameters.Add("@LineCode", SqlDbType.NVarChar, 150).Value = tool.LineCode;
                    cmd.Parameters.Add("@MachineCode", SqlDbType.NVarChar, 150).Value = tool.MachineCode;
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        result.Add(item: new Models.SettingData
                        {
                            //Name = (string)reader["Name"],
                            //Code = (string)reader["Code"],
                            Name = Convert.ToString(reader["Name"] == DBNull.Value ? "" : reader["Name"]),
                            Code = Convert.ToString(reader["Code"] == DBNull.Value ? "" : reader["Code"]),

                        });
                    }
                    if (result.Any())
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { status = "Success", msg = "Details Found", data = result });
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "No Data Available", data = "" });
                    }
                
            }
        }
        public static string Encrypt(string text)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {

                using (var tdes = new TripleDESCryptoServiceProvider())

                {
                    tdes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;

                    using (var transform = tdes.CreateEncryptor())
                    {
                        byte[] textBytes = UTF8Encoding.UTF8.GetBytes(text);
                        byte[] bytes = transform.TransformFinalBlock(textBytes, 0, textBytes.Length);
                        return Convert.ToBase64String(bytes, 0, bytes.Length);
                    }
                }
            }
        }
        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage GetSettingdatas_line(Models.SettingData tool)
        {

            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                //To Get the response from database and display from user
                var result = new List<Models.SettingData>();
                DataSet ds = new DataSet();
                con.Open();
                SqlCommand cmd = new SqlCommand("SP_Settings_data", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandTimeout = 0;
                cmd.Parameters.Add("@Flag", SqlDbType.NVarChar, 150).Value = tool.Flag;
                cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = tool.CompanyCode;
                cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = tool.PlantCode;
                cmd.Parameters.Add("@LineCode", SqlDbType.NVarChar, 150).Value = tool.LineCode;
                cmd.Parameters.Add("@MachineCode", SqlDbType.NVarChar, 150).Value = tool.MachineCode;
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



                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    result.Add(item: new Models.SettingData
                    {
                        //Name = (string)reader["Name"],
                        //Code = (string)reader["Code"],
                        Name = Convert.ToString(reader["Name"] == DBNull.Value ? "" : reader["Name"]),
                        Code = Convert.ToString(reader["Code"] == DBNull.Value ? "" : reader["Code"]),

                    });
                }
                if (result.Any())
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = "Success", msg = "Details Found", data = result });
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "No Data Available", data = "" });
                }

            }
        }

        public IHttpActionResult settings_details1(Models.Setting U)
        {
            try
            {
                database_connection d = new database_connection();
                string con_string = d.Getconnectionstring(U.CompanyCode, U.PlantCode, U.LineCode);
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
                        DataSet ds = new DataSet();
                        SqlCommand cmd = new SqlCommand("SP_GetSettings_data", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = U.QueryType;
                        cmd.Parameters.Add("@Parameter", SqlDbType.NVarChar, 150).Value = U.Parameter;
                        cmd.Parameters.Add("@Parameter1", SqlDbType.NVarChar, 150).Value = U.Parameter1;
                        cmd.Parameters.Add("@Parameter2", SqlDbType.NVarChar, 150).Value = U.Parameter2;
                        cmd.Parameters.Add("@Parameter3", SqlDbType.NVarChar, 150).Value = U.Parameter3;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(ds);

                        if (ds.Tables.Count != 0)
                        {

                            return Ok(ds.Tables[0].Rows[0]["alarm_count"].ToString());
                        }
                        else
                        {
                            return Ok("0");
                        }
                        //return Ok(ds.Tables[0]);
                    }
                }
            }
            catch (Exception ex)
            {
                return Ok("1");
            }
        }

        //[CustomAuthenticationFilter]
        //public HttpResponseMessage Search_details(Models.Setting U)
        //{

        //    database_connection d = new database_connection();
        //    string con_string = d.Getconnectionstring(U.CompanyCode, U.PlantCode, U.LineCode);
        //    if (con_string == "0")
        //    {
        //        return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "Coudnot Connect to database", data = "" });

        //    }
        //    else
        //    {
        //        using (SqlConnection con = new SqlConnection(con_string))
        //        {
        //            string response = string.Empty;
        //            con.Open();
        //            DataSet ds = new DataSet();
        //            SqlCommand cmd = new SqlCommand("SP_GetSettings_data", con);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            cmd.CommandTimeout = 0;
        //            cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = U.QueryType;
        //            cmd.Parameters.Add("@Parameter", SqlDbType.NVarChar, 150).Value = U.Parameter;
        //            cmd.Parameters.Add("@Parameter1", SqlDbType.NVarChar, 150).Value = U.Parameter1;
        //            cmd.Parameters.Add("@Parameter2", SqlDbType.NVarChar, 150).Value = U.Parameter2;
        //            cmd.Parameters.Add("@Parameter3", SqlDbType.NVarChar, 150).Value = U.Parameter3;
        //            SqlDataAdapter da = new SqlDataAdapter(cmd);
        //            da.Fill(ds);
        //            if (ds.Tables.Count != 0)
        //            {
        //                return Request.CreateResponse(HttpStatusCode.OK, new { data = ds });
        //            }
        //            else
        //            {
        //                return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "No Data Available", data = "" });
        //            }

        //        }
        //    }

        //}



        [CustomAuthenticationFilter]
        public HttpResponseMessage Search_details(Models.Setting U)
        {

            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(U.CompanyCode, U.PlantCode, U.LineCode);
            if (con_string == "0")
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "Coudnot Connect to database", data = "" });

            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    string response = string.Empty;
                    con.Open();
                    DataSet ds = new DataSet();
                    //SqlCommand cmd = new SqlCommand("SP_GetSettings_data", con);
                    SqlCommand cmd = new SqlCommand("SELECT * FROM AlarmTable_Setting WHERE CompanyCode=@Parameter AND PlantCode=@Parameter1 AND Line_Code=@Parameter2 AND Machine_Code=@Parameter3", con);
                    //cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.CommandTimeout = 0;
                    //cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = U.QueryType;
                    cmd.Parameters.Add("@Parameter", SqlDbType.NVarChar, 150).Value = U.Parameter;
                    cmd.Parameters.Add("@Parameter1", SqlDbType.NVarChar, 150).Value = U.Parameter1;
                    cmd.Parameters.Add("@Parameter2", SqlDbType.NVarChar, 150).Value = U.Parameter2;
                    cmd.Parameters.Add("@Parameter3", SqlDbType.NVarChar, 150).Value = U.Parameter3;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds.Tables.Count != 0)
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
        [Obsolete]
        [CustomAuthenticationFilter]

        public HttpResponseMessage CollectFileName_settings_details(Models.Setting U)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(U.CompanyCode, U.PlantCode, U.LineCode);
            if (con_string == "0")
            {
                // return Ok("Line can not find");
                return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "Coudnot Connect to database", data = "" });

            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    string response = string.Empty;
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("SP_Manualupload", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = U.QueryType;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = U.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = U.PlantCode;
                    cmd.Parameters.Add("@Line_Code", SqlDbType.NVarChar, 150).Value = U.LineCode;
                    cmd.Parameters.Add("@Unique_id", SqlDbType.NVarChar, 150).Value = U.Unique_id;
                    SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                    SQLReturn.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(SQLReturn);
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
        [Obsolete]
        [CustomAuthenticationFilter]
        public HttpResponseMessage File_settings_details(Models.Setting U)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(U.CompanyCode, U.PlantCode, U.LineCode);
            if (con_string == "0")
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "Coudnot Connect to database", data = "" });

            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    string response = string.Empty;
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("SP_Manualupload", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = U.QueryType;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = U.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = U.PlantCode;
                    cmd.Parameters.Add("@Line_Code", SqlDbType.NVarChar, 150).Value = U.LineCode;
                    SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                    SQLReturn.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(SQLReturn);

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


        [CustomAuthenticationFilter]
        public IHttpActionResult Download_Search_details(Models.Setting U)
        {

            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(U.CompanyCode, U.PlantCode, U.LineCode);
            if (con_string == "0")
            {
                //return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "Coudnot Connect to database", data = "" });
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Couldnot connect to database"));
            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    string response = string.Empty;
                    con.Open();
                    DataSet ds = new DataSet();
                    SqlCommand cmd = new SqlCommand("SP_GetSettings_data", con);
                    //SqlCommand cmd = new SqlCommand("SELECT * FROM AlarmTable_Setting WHERE CompanyCode=@Parameter AND PlantCode=@Parameter1 AND Line_Code=@Parameter2 AND Machine_Code=@Parameter3", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = U.QueryType;
                    cmd.Parameters.Add("@Parameter", SqlDbType.NVarChar, 150).Value = U.Parameter;
                    cmd.Parameters.Add("@Parameter1", SqlDbType.NVarChar, 150).Value = U.Parameter1;
                    cmd.Parameters.Add("@Parameter2", SqlDbType.NVarChar, 150).Value = U.Parameter2;
                    cmd.Parameters.Add("@Parameter3", SqlDbType.NVarChar, 150).Value = U.Parameter3;
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    if (ds.Tables.Count != 0)
                    {
                        return Ok(ds.Tables[0]);
                    }
                    else
                    {
                        return Ok(new string[0]);
                    }
                }
            }

        }


    }
}
