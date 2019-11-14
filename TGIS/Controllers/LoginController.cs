using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGIS.Models;

namespace TGIS.Controllers
{
    public class LoginController : Controller
    {
        TGISDBEntities db = new TGISDBEntities();
        // GET: Login
        public ActionResult LoginForPlayer(string account,string pwd)
        {
            var user = db.Players.Where(m => m.Account == account).Where(m => m.Password == pwd).FirstOrDefault();
            if (user != null)
            {
                return RedirectToAction("", "");
            }
            ViewBag.Error = "帳號密碼錯誤";
            return View();
        }
        public ActionResult LoginForShop()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginForShop (string account,string pwd)
        {
            var password = Hash.PwdHash(pwd);
            Shop user = db.Shops.Where(m => m.Account == account).Where(m => m.Password == password).SingleOrDefault();
            if (user != null)
            {
                var id = user.ID;
                return RedirectToAction("ShopDetailForStore", "Shop",new { id });
            }
            ViewBag.Error = "帳號密碼錯誤";
            return View();
        }
    }
}