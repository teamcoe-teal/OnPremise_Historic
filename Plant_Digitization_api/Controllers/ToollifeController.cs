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
    //[EnableCors(origins: "http://localhost:55974", headers: "*", methods: "Get","Post")]
   
    public class ToollifeController : ApiController
    {
        static string key { get; set; } = "A!9HHhi%XjjYY4YP2@Nob009X";
        private string connectionstring = ConfigurationManager.ConnectionStrings["con"].ToString();
        //[EnableCors(origins: "http://localhost:63627", headers: "*", methods: "GET,POST")]
        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage Gettoolhistorical(Models.historical tool)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(tool.CompanyCode, tool.PlantCode, tool.linename);
            if (con_string == "0")
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "Coudnot Connect to database", data = "" });

            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    //To Get the response from database and display from user
                    var result = new List<Models.historical>();
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SP_toollife", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@Flag", SqlDbType.NVarChar, 150).Value = tool.Flag;
                    cmd.Parameters.Add("@linecode", SqlDbType.NVarChar, 150).Value = tool.linename;
                    cmd.Parameters.Add("@machine_code", SqlDbType.NVarChar, 150).Value = tool.subsystem;
                    cmd.Parameters.Add("@fromdate", SqlDbType.Date, 150).Value = tool.fromdate;
                    cmd.Parameters.Add("@todate", SqlDbType.Date, 150).Value = tool.todate;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = tool.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = tool.PlantCode;
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        result.Add(item: new Models.historical
                        {
                            //linename = (string)reader["Line_code"],
                            //element = (string)reader["ToolName"],
                            //subsystem = (string)reader["Machine_Code"],
                            //make = (string)reader["Make"],
                            //partnumber = (string)reader["Part_number"],
                            //ratedlife = (decimal)reader["ratedlifecle"],
                            //classification = (string)reader["Classification"],
                            //conversionparameter = (string)reader["Conversion_parameter"],
                            //uom = (string)reader["UOM"],
                            //usage = (decimal)reader["usage"],
                            //currentlifecycle = (decimal)reader["currentlifecle"],
                            //lastmain = (string)reader["lastmain"],


                            linename = Convert.ToString(reader["Line_code"] == DBNull.Value ? "" : reader["Line_code"]),
                            element = Convert.ToString(reader["ToolName"] == DBNull.Value ? "" : reader["ToolName"]),
                            subsystem = Convert.ToString(reader["Machine_Code"] == DBNull.Value ? "" : reader["Machine_Code"]),
                            make = Convert.ToString(reader["Make"] == DBNull.Value ? "" : reader["Make"]),
                            partnumber = Convert.ToString(reader["Part_number"] == DBNull.Value ? "" : reader["Part_number"]),
                            ratedlife = Convert.ToDecimal(reader["ratedlifecle"] == DBNull.Value ? "" : reader["ratedlifecle"]),
                            classification = Convert.ToString(reader["Classification"] == DBNull.Value ? "" : reader["Classification"]),
                            conversionparameter = Convert.ToString(reader["Conversion_parameter"] == DBNull.Value ? "" : reader["Conversion_parameter"]),
                            uom = Convert.ToString(reader["UOM"] == DBNull.Value ? "" : reader["UOM"]),
                            //usage = Convert.ToDecimal(reader["usage"] == DBNull.Value ? "" : reader["usage"]),
                            //currentlifecycle = Convert.ToDecimal(reader["currentlifecle"] == DBNull.Value ? "" : reader["currentlifecle"]),
                            lastmain = Convert.ToString(reader["lastmain"] == DBNull.Value ? "" : reader["lastmain"]),
                            RecText = Convert.ToString(reader["RecommendationText"] == DBNull.Value ? "" : reader["RecommendationText"]),
                            SerialNo = Convert.ToString(reader["SerialNo"] == DBNull.Value ? "" : reader["SerialNo"]),
                            lifeatpm = Convert.ToString(reader["lifeatpm"] == DBNull.Value ? "" : reader["lifeatpm"]),


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
        }

        [HttpPost]
        //[CustomAuthenticationFilter]
        public HttpResponseMessage GetPreventiveMain(Models.Toollife tool)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(tool.CompanyCode, tool.PlantCode, tool.linename);
            if (con_string == "0")
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "Coudnot Connect to database", data = "" });

            }
            else
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(con_string))
                    {
                        //To Get the response from database and display from user
                        var result = new List<Models.Toollife>();
                        con.Open();
                        DataSet ds = new DataSet();
                        SqlCommand cmd = new SqlCommand("SP_toollife", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add("@Flag", SqlDbType.NVarChar, 150).Value = tool.Flag;
                        cmd.Parameters.Add("@linecode", SqlDbType.NVarChar, 150).Value = tool.linename;
                        cmd.Parameters.Add("@machine_code", SqlDbType.NVarChar, 150).Value = tool.subsystem;
                        cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = tool.CompanyCode;
                        cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = tool.PlantCode;
                        cmd.Parameters.Add("@RecommendationText", SqlDbType.NVarChar, 150).Value = tool.Rectext;
                        cmd.Parameters.Add("@Status", SqlDbType.NVarChar, 150).Value = tool.Status;
                        cmd.Parameters.Add("@ToolID", SqlDbType.NVarChar, 150).Value = tool.ToolID;

                        var reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            result.Add(item: new Models.Toollife
                            {
                                linename = Convert.ToString(reader["Line_code"] == DBNull.Value ? "" : reader["Line_code"]),
                                element = Convert.ToString(reader["ToolName"] == DBNull.Value ? "" : reader["ToolName"]),
                                subsystem = Convert.ToString(reader["Machine_Code"] == DBNull.Value ? "" : reader["Machine_Code"]),
                                make = Convert.ToString(reader["Make"] == DBNull.Value ? "" : reader["Make"]),
                                partnumber = Convert.ToString(reader["Part_number"] == DBNull.Value ? "" : reader["Part_number"]),
                                classification = Convert.ToString(reader["Classification"] == DBNull.Value ? "" : reader["Classification"]),
                                uom = Convert.ToString(reader["UOM"] == DBNull.Value ? "" : reader["UOM"]),
                                currentusage = Convert.ToDecimal(reader["currentlifecle"] == DBNull.Value ? "" : reader["currentlifecle"]),
                                lastmain = Convert.ToString(reader["lastmain"] == DBNull.Value ? "" : reader["lastmain"]),
                                isreplaced = Convert.ToString(reader["replaced"] == DBNull.Value ? "" : reader["replaced"]),
                                noofreplace = Convert.ToInt32(reader["noofreplce"] == DBNull.Value ? "" : reader["noofreplce"]),
                                remarks = Convert.ToString(reader["remark"] == DBNull.Value ? "" : reader["remark"]),
                                ToolID = Convert.ToString(reader["ToolID"] == DBNull.Value ? "" : reader["ToolID"]),
                                Rectext = Convert.ToString(reader["RecommendationText"] == DBNull.Value ? "" : reader["RecommendationText"]),
                                SerialNo = Convert.ToString(reader["SerialNo"] == DBNull.Value ? "" : reader["SerialNo"]),
                                Status = Convert.ToString(reader["Status"] == DBNull.Value ? "" : reader["Status"]),
                                ratedlifecle = Convert.ToString(reader["ratedlifecle"] == DBNull.Value ? "" : reader["ratedlifecle"]),
                                lifeatpm = Convert.ToString(reader["lifeatpm"] == DBNull.Value ? "" : reader["lifeatpm"]),

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
                catch (Exception ex) {
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "No Data Available", data = "" });

                }
            }
        }


        /// <summary>
        /// Insert & Update Shift Settings Details 
        /// </summary>
        /// <param name="SS"></param>
        /// <returns></returns>
        [CustomAuthenticationFilter]
        [HttpPost]
        public HttpResponseMessage Update_tool(Models.maintenance SS)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(SS.CompanyCode, SS.PlantCode, SS.LineCode);
            if (con_string == "0")
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "Coudnot Connect to database", data = "" });

            }
            else
            {
                try
                {
                    using (SqlConnection con = new SqlConnection(con_string))
                    {
                        var result = new List<Models.maintenance>();
                        con.Open();
                        SqlCommand cmd = new SqlCommand("SP_toollife_maintenance", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 0;
                        cmd.Parameters.Add("@ToolID", SqlDbType.NVarChar, 150).Value = SS.ToolID;

                        //cmd.Parameters.Add("@LastMaintenaceDate", SqlDbType.Date).Value = SS.lastmain;

                        cmd.Parameters.Add("@IsReplaced", SqlDbType.NVarChar, 150).Value = SS.IsReplaced;
                        cmd.Parameters.Add("@No_of_Replacements", SqlDbType.NVarChar, 150).Value = SS.No_of_Replacements;
                        cmd.Parameters.Add("@Currentusage", SqlDbType.NVarChar, 150).Value = SS.Currentusage;

                        cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = SS.CompanyCode;
                        cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar, 150).Value = SS.Remarks;
                        cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = SS.PlantCode;


                        cmd.Parameters.Add("@Status", SqlDbType.NVarChar, 150).Value = SS.Status;
                        cmd.Parameters.Add("@RecommendationText", SqlDbType.NVarChar, 150).Value = SS.RecText;

                        cmd.Parameters.Add("@NSerialNo", SqlDbType.NVarChar, 150).Value = SS.NSerialNo;

                        cmd.Parameters.Add("@SerialNo", SqlDbType.NVarChar, 150).Value = SS.SerialNo;

                        var reader = cmd.ExecuteReader();

                        return Request.CreateResponse(HttpStatusCode.OK, new { status = "Success", msg = "Details Added Successfully", data = "" });
                    }
                }
                catch(Exception ex)
                {
                    var exce = ex;
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "Coudnot Connect to database", data = "" });

                }
            }
        }


        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage Gettoollivedata(Models.Toollifelive tool)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(tool.CompanyCode, tool.PlantCode, tool.linename);
            if (con_string == "0")
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "Coudnot Connect to database", data = "" });

            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    //To Get the response from database and display from user
                    var result = new List<Models.Toollifelive>();
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SP_toollife_live", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = tool.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = tool.PlantCode;
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        result.Add(item: new Models.Toollifelive
                        {
                            linename = Convert.ToString(reader["Line_code"] == DBNull.Value ? "" : reader["Line_code"]),
                            element = Convert.ToString(reader["ToolName"] == DBNull.Value ? "" : reader["ToolName"]),
                            subsystem = Convert.ToString(reader["Machine_Code"] == DBNull.Value ? "" : reader["Machine_Code"]),
                            make = Convert.ToString(reader["Make"] == DBNull.Value ? "" : reader["Make"]),
                            partnumber = Convert.ToString(reader["Part_number"] == DBNull.Value ? "" : reader["Part_number"]),
                            ratedlife = Convert.ToDecimal(reader["ratedlifecle"] == DBNull.Value ? "" : reader["ratedlifecle"]),
                            classification = Convert.ToString(reader["Classification"] == DBNull.Value ? "" : reader["Classification"]),
                            conversionparameter = Convert.ToString(reader["Conversion_parameter"] == DBNull.Value ? "" : reader["Conversion_parameter"]),
                            uom = Convert.ToString(reader["UOM"] == DBNull.Value ? "" : reader["UOM"]),
                            usage = Convert.ToDecimal(reader["usage"] == DBNull.Value ? "" : reader["usage"]),
                            currentlifecycle = Convert.ToDecimal(reader["currentlifecle"] == DBNull.Value ? "" : reader["currentlifecle"]),
                            lastmain = Convert.ToString(reader["lastmain"] == DBNull.Value ? "" : reader["lastmain"]),

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
        }
        
        public static string Decrypt(string cipher)
        {
            using (var md5 = new MD5CryptoServiceProvider())
            {
                using (var tdes = new TripleDESCryptoServiceProvider())
                {
                    tdes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;

                    using (var transform = tdes.CreateDecryptor())
                    {
                        byte[] cipherBytes = Convert.FromBase64String(cipher);
                        byte[] bytes = transform.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                        return UTF8Encoding.UTF8.GetString(bytes);
                    }
                }
            }
        }
        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage GetSettingdatas(Models.SettingData tool)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(tool.CompanyCode, tool.PlantCode, tool.LineCode);
            if (con_string == "0")
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "Coudnot Connect to database", data = "" });

            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
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
        public HttpResponseMessage GetSettingdatas1(Models.SettingData tool)
        {
            var fronthash = tool.HashId;
            var string1 = tool.CompanyCode + tool.PlantCode;
            var backhash = Encrypt(string1);
            if (fronthash == backhash)
            {
                database_connection d = new database_connection();
                string con_string = d.Getconnectionstring(tool.CompanyCode, tool.PlantCode, tool.LineCode);
                if (con_string == "0")
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "Coudnot Connect to database", data = "" });

                }
                else
                {
                    using (SqlConnection con = new SqlConnection(con_string))
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
                
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "No Data Available", data = "" });
            }
        }
        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage GetSettingdatas2(Models.SettingData tool)
        {
            var fronthash = tool.HashId;
            var string1 = tool.CompanyCode + tool.PlantCode;
            var backhash = Encrypt(string1);
            if (fronthash == backhash)
            {
                database_connection d = new database_connection();
                string con_string = d.Getconnectionstring(tool.CompanyCode, tool.PlantCode, tool.LineCode);
                if (con_string == "0")
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "Coudnot Connect to database", data = "" });

                }
                else
                {
                    using (SqlConnection con = new SqlConnection(con_string))
                    {
                        //To Get the response from database and display from user
                        var result = new List<Models.SettingData2>();
                        con.Open();
                        SqlCommand cmd = new SqlCommand("SP_Settings_data", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Flag", SqlDbType.NVarChar, 150).Value = tool.Flag;
                        cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = tool.CompanyCode;
                        cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = tool.PlantCode;
                        cmd.Parameters.Add("@LineCode", SqlDbType.NVarChar, 150).Value = tool.LineCode;
                        var reader = cmd.ExecuteReader();
                        DataTable dt = new DataTable();
                        DataTable dtData = new DataTable("Data");
                        DataTable dtSchema = new DataTable("Schema");
                        //SqlDataAdapter da = new SqlDataAdapter(cmd);
                        //da.Fill(dt);
                        while (reader.Read())
                        {

                            result.Add(item: new Models.SettingData2
                            {
                                //Name = (string)reader["Name"],
                                //Code = (int)reader["Code"],
                                Name = Convert.ToString(reader["Name"] == DBNull.Value ? "" : reader["Name"]),
                                Code = Convert.ToInt32(reader["Code"] == DBNull.Value ? "" : reader["Code"]),
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
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "No Data Available", data = "" });
            }
        }
        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage Getvariablesettings(Models.SettingData_Variable tool)
        {
            var fronthash = tool.HashId;
            var string1 = tool.CompanyCode + tool.PlantCode;
            var backhash = Encrypt(string1);
            if (fronthash == backhash)
            {
                database_connection d = new database_connection();
                string con_string = d.Getconnectionstring(tool.CompanyCode, tool.PlantCode, tool.LineCode);
                if (con_string == "0")
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "Coudnot Connect to database", data = "" });

                }
                else
                {
                    using (SqlConnection con = new SqlConnection(con_string))
                    {
                        //To Get the response from database and display from user
                        var result = new List<Models.SettingData2>();
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

                            result.Add(item: new Models.SettingData2
                            {
                                //Name = (string)reader["Name"],
                                //Code = (int)reader["Code"],
                                Name = Convert.ToString(reader["Name"] == DBNull.Value ? "" : reader["Name"]),
                                Code = Convert.ToInt32(reader["Code"] == DBNull.Value ? "" : reader["Code"]),
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
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "No Data Available", data = "" });
            }
        }


        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage GetAlertSettingdatas1(Models.SettingData tool)
        {
            var fronthash = tool.HashId;
            var string1 = tool.CompanyCode + tool.PlantCode;
            var backhash = Encrypt(string1);
            if (fronthash == backhash)
            {
                database_connection d = new database_connection();
                string con_string = d.Getconnectionstring(tool.CompanyCode, tool.PlantCode, tool.LineCode);
                if (con_string == "0")
                {
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "Coudnot Connect to database", data = "" });

                }
                else
                {
                    using (SqlConnection con = new SqlConnection(con_string))
                    {
                        //To Get the response from database and display from user
                        var result = new List<Models.SettingData>();
                        con.Open();
                        SqlCommand cmd = new SqlCommand("SP_Alert_Settings_data", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Flag", SqlDbType.NVarChar, 150).Value = tool.Flag;
                        cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = tool.CompanyCode;
                        cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = tool.PlantCode;
                        cmd.Parameters.Add("@LineCode", SqlDbType.NVarChar, 150).Value = tool.LineCode;
                        cmd.Parameters.Add("@MachineCode", SqlDbType.NVarChar, 150).Value = tool.MachineCode;
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
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "No Data Available", data = "" });
            }
        }



        [HttpPost]
        [CustomAuthenticationFilter]
        public HttpResponseMessage Addtoolmaintenance(Models.maintenance tool)
        {
            database_connection d = new database_connection();
            string con_string = d.Getconnectionstring(tool.CompanyCode, tool.PlantCode, tool.LineCode);
            if (con_string == "0")
            {
                return Request.CreateResponse(HttpStatusCode.OK, new { status = "Error", msg = "Coudnot Connect to database", data = "" });

            }
            else
            {
                using (SqlConnection con = new SqlConnection(con_string))
                {
                    //To Get the response from database and display from user
                    var result = new List<Models.maintenance>();
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SP_toollife_maintenance", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add("@ToolID", SqlDbType.NVarChar, 150).Value = tool.ToolID;

                    cmd.Parameters.Add("@LastMaintenaceDate", SqlDbType.Date).Value = tool.LastMaintenaceDate;

                    cmd.Parameters.Add("@IsReplaced", SqlDbType.NVarChar, 150).Value = tool.IsReplaced;
                    cmd.Parameters.Add("@No_of_Replacements", SqlDbType.NVarChar, 150).Value = tool.No_of_Replacements;
                    cmd.Parameters.Add("@Currentusage", SqlDbType.NVarChar, 150).Value = tool.Currentusage;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = tool.CompanyCode;
                    cmd.Parameters.Add("@Remarks", SqlDbType.NVarChar, 150).Value = tool.Remarks;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = tool.PlantCode;
                    var reader = cmd.ExecuteReader();
                    return Request.CreateResponse(HttpStatusCode.OK, new { status = "Success", msg = "Details Added Successfully", data = "" });
                }
            }
        }
    }
}
