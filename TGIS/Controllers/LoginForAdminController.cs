﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGIS.Models;

namespace TGIS.Controllers
{
    
    public class LoginForAdminController : Controller
    {
        TGISDBEntities db = new TGISDBEntities();
        // GET: Login
        //管理員登入
        public ActionResult LoginForAdmin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginForAdmin(string ac, string pwd)
        {
            var user = db.Administrators.Where(m => m.Account == ac).Where(m=>m.Password==pwd).FirstOrDefault();
            if (user !=null)
            {
                Session["AdminID"] = user.ID;
                return RedirectToAction("ReturnList", "Feedback");
            }
            ViewBag.Error = "帳號密碼錯誤";
            return View();
        }
    }
}