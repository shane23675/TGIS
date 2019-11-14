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
            System.Net.Mail.SmtpClient smtp = new
                System.Net.Mail.SmtpClient("stmp.gmail.com", 123);

            smtp.Credentials = new System.Net.NetworkCredential();
            smtp.EnableSsl = true;
            smtp.Send("","","","");
            smtp.Dispose();
        }
    }
}