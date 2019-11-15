using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TGIS.Controllers
{
    public class AutoEmailController : Controller
    {
        // GET: AutoEmail
        public void Index()
        {
            System.Net.Mail.SmtpClient smtp = 
                new System.Net.Mail.SmtpClient("smtp.gmail.com", 587);

            smtp.Credentials = new System.Net.NetworkCredential("kevin0930024496@gmail.com", "bavvjfsnecmjnhhy");
            smtp.EnableSsl = true;
            smtp.Send("kevin0930024496@gmail.com", "kevin0976184751@gmail.com","C#測試","hello you motherfucker");
            smtp.Dispose();
        }
    }
}