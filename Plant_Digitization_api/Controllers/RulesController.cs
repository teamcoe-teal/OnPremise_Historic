using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Text;
using System.Web.Http;

namespace Plant_Digitization_api.Controllers
{
    public class RulesController : ApiController
    {
        private string connectionstring = ConfigurationManager.ConnectionStrings["con"].ToString();

        public IHttpActionResult Rules_details(Models.Rules R)
        {
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                string response = string.Empty;
                con.Open();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("SELECT * FROM tbl_rules", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return Ok(ds.Tables[0]);
            }
        }


        [HttpPost]
        public IHttpActionResult Add_Rules(Models.Rules R)
        {
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                string response = string.Empty;
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_rules", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@QueryType", SqlDbType.NVarChar, 150).Value = R.QueryType;
                cmd.Parameters.Add("@id", SqlDbType.NVarChar, 150).Value = R.id;
                cmd.Parameters.Add("@value1", SqlDbType.NVarChar, 150).Value = R.value1;
                cmd.Parameters.Add("@value2", SqlDbType.NVarChar, 150).Value = R.value2;
                cmd.Parameters.Add("@condition", SqlDbType.NVarChar, 150).Value = R.condition;
                SqlParameter SQLReturn = new SqlParameter("@SQLReturn", SqlDbType.NVarChar, 50);
                SQLReturn.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(SQLReturn);
                cmd.ExecuteNonQuery();
                response = SQLReturn.Value.ToString().Trim();
                return Ok(response);
            }
        }

        public HttpResponseMessage Setting_details()
        {
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                string response = string.Empty;
                con.Open();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("SELECT * FROM tbl_settings", con);
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

        public IHttpActionResult Active_Rules_details(Models.Rules R)
        {
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                string response = string.Empty;
                con.Open();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("SELECT * FROM tbl_rules where status='Y' ", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                return Ok(ds.Tables[0]);
            }
        }

        [HttpPost]
        public HttpResponseMessage Edit_rule(Models.Rules R)
        {
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                string response = string.Empty;
                con.Open();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand("SELECT * FROM tbl_rules where id=@id", con);
                cmd.Parameters.AddWithValue("id", R.id);
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

        [HttpPost]
        public IHttpActionResult Send_mail(Models.Rules R)
        {
            using (SqlConnection con = new SqlConnection(connectionstring))
            {
                con.Open();
                DataTable dt = new DataTable();
                SqlCommand cmd_mail = new SqlCommand("SELECT * FROM tbl_gmail_settings", con);
                SqlDataAdapter da = new SqlDataAdapter(cmd_mail);
                da.Fill(dt);

                MailMessage mail = new MailMessage();
                mail.To.Add(dt.Rows[0]["HRGmail"].ToString());
                mail.From = new MailAddress(dt.Rows[0]["Smtp_user"].ToString());
                var dateee = DateTime.Now.ToString();
                mail.Subject = "Reg : Notification Mail";
                mail.Body = string.Format("This is test mail...!");
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = dt.Rows[0]["Smtp_host"].ToString();
                smtp.Port = Convert.ToInt32(dt.Rows[0]["Smtp_port"].ToString());
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = new System.Net.NetworkCredential(dt.Rows[0]["Smtp_user"].ToString(), dt.Rows[0]["Smtp_pass"].ToString());
                smtp.EnableSsl = true;
                smtp.Send(mail);
            }
            return Ok();
        }
    }
}