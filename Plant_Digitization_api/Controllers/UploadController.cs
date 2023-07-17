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
    public class UploadController : ApiController
    {

        private string connectionstring = ConfigurationManager.ConnectionStrings["con"].ToString();

        // GET: Upload
        //not used
        [HttpPost]
        [CustomAuthenticationFilter]
        public string upload_diagnosticDetails(Models.Diagnostics_settings A)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionstring))
                {
                    string response = string.Empty;
                    con.Open();
                    SqlCommand cmd = new SqlCommand("sp_Diagnostics", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = "Insert";
                    cmd.Parameters.Add("@deviceid", SqlDbType.NVarChar, 150).Value = A.DeviceID;
                    cmd.Parameters.Add("@deviceref", SqlDbType.NVarChar, 150).Value = A.DeviceRef;
                    cmd.Parameters.Add("@make", SqlDbType.NVarChar, 150).Value = A.make;
                    cmd.Parameters.Add("@gateway", SqlDbType.NVarChar, 150).Value = A.gateway;
                    cmd.Parameters.Add("@ioserver", SqlDbType.NVarChar, 150).Value = A.ioserver;
                    cmd.Parameters.Add("@macaddress", SqlDbType.NVarChar, 150).Value = A.macaddress;
                    cmd.Parameters.Add("@connectedTo", SqlDbType.NVarChar, 150).Value = A.connectedto;
                    cmd.Parameters.Add("@installed", SqlDbType.NVarChar, 150).Value = A.installed;
                    cmd.Parameters.Add("@remarks", SqlDbType.NVarChar, 500).Value = A.remarks;
                    cmd.Parameters.Add("@partnumber", SqlDbType.NVarChar, 150).Value = A.partnumber;
                    cmd.Parameters.Add("@unique_id", SqlDbType.NVarChar, 150).Value = A.Unique_ID;
                    cmd.Parameters.Add("@CompanyCode", SqlDbType.NVarChar, 150).Value = A.CompanyCode;
                    cmd.Parameters.Add("@PlantCode", SqlDbType.NVarChar, 150).Value = A.PlantCode;
                    cmd.Parameters.Add("@LineCode", SqlDbType.NVarChar, 150).Value = A.LineCode;

                    SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                    SQLReturn.Direction = ParameterDirection.Output;
                    cmd.Parameters.Add(SQLReturn);
                    cmd.ExecuteNonQuery();
                    response = SQLReturn.Value.ToString().Trim();
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}