using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TGIS.Models
{
    public class AutoEmail
    {
        public static void AutoEmailSend(string recipient,string subject,string content)
        {
            System.Net.Mail.SmtpClient smtp =
            new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);

            smtp.Credentials = new System.Net.NetworkCredential("kevin0930024496@gmail.com", "bavvjfsnecmjnhhy");
            smtp.EnableSsl = true;
            smtp.Send("kevin0930024496@gmail.com", $"{recipient}", $"{subject}", $"{content}");
            smtp.Dispose();
        }
    }
}