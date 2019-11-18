using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Configuration;

namespace TGIS.Controllers
{
    public class EmailValidateController : Controller
    {
        SqlConnection Conn = new SqlConnection(@"data source=(LocalDB)\MSSQLLocalDB;attachdbfilename=|DataDirectory|\TGISDB.mdf;integrated security=True;connect timeout=30;MultipleActiveResultSets=True;App=EntityFramework&quot;");
        // GET: Default
        public ActionResult Index()
        {
            return View();
        }
    }
}