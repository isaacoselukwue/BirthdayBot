using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BirthdayBot
{
    public class Birthday
    {
        public IConfiguration _configuration;
        public Birthday(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task WishBirthday()
        {
            string constr = _configuration.GetConnectionString("Constring");

            DataTable dt = new DataTable();
            string query = "SELECT Id,Name,DOB,Email FROM Employee WHERE DATEPART(DAY, DOB) = @Day AND DATEPART(MONTH, DOB) = @Month";
            //string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
            using (SqlConnection con = new(constr))
            {
                using SqlCommand cmd = new(query);
                cmd.Connection = con;
                cmd.Parameters.AddWithValue("@Day", DateTime.Today.Day);
                cmd.Parameters.AddWithValue("@Month", DateTime.Today.Month);
                using SqlDataAdapter sda = new SqlDataAdapter(cmd);
                sda.Fill(dt);
            }
            foreach (DataRow dr in dt.Rows)
            {
                using MailMessage mm = new("princeisaac8@gmail.com", dr["Email"].ToString());
                mm.Subject = "Birthday Greetings to OUR STAR";
                mm.Body = string.Format("<b>Happy Birthday </b>{0}<br /><br />Many happy returns of the day. You are valued.", dr["Name"].ToString());
                mm.IsBodyHtml = true;
                SmtpClient smtp = new();
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                NetworkCredential credentials = new(mm.From.Address, "mytestcredentials");
                
                smtp.Credentials = credentials;
                smtp.Port = 587;
                await Task.Run(()=>smtp.Send(mm));
            }
        }
    }
}
