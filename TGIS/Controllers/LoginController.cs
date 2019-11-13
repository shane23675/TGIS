using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Configuration;

namespace TGIS.Controllers
{
    public class LoginController : Controller
    {
        SqlConnection Conn = new SqlConnection(ConfigurationManager.ConnectionStrings["TGISDBEntities"].ConnectionString);
        // GET: Login
        public ActionResult LoginForPlayer(string acc,string pwd)
        {
            string sql = "select ID from Player where Account=@acc and Password=@pwd";
            SqlCommand cmd = new SqlCommand(sql,Conn);
            cmd.Parameters.AddWithValue("Account", acc);
            cmd.Parameters.AddWithValue("Password", pwd);
            SqlDataReader rd;

            Conn.Open();
            rd = cmd.ExecuteReader();
            if (rd.Read())
            {
                Conn.Close();
                return RedirectToAction("PlayerIndex","Player");
            }
            Conn.Close();
            ViewBag.Error = "帳號密碼錯誤";
            return View(acc);
        }
    }
}