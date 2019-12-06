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
        //玩家登入
        public ActionResult LoginForPlayer()
        {
            return View();
        }
        [HttpPost]
        public ActionResult LoginForPlayer(string account,string pwd)
        {
            var password = Hash.PwdHash(pwd);
            Player user = db.Players.Where(m => m.Account == account).Where(m => m.Password == password).SingleOrDefault();
            if (user != null)
            {
                Session["PlayerID"] = user.ID.ToString(); //$$$$$$
                return Content("2");
            }
            return Content("1");
        }
        //店家登入
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
                Session["ShopID"] =$"{user.ID}";
                return Content("2");
            }
            return Content("1");
        }
        //登出
        //Session["sessionName"]
        public ActionResult Logout(string sessionName)
        {
            Session.Contents.Remove(sessionName);
            return RedirectToAction("Index","Home");
        }
        //忘記密碼（寄信）
        public ActionResult ForgetPwd()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgetPwd(string account,string Email)
        {
            Player user =  db.Players.Where(m => m.Account == account).SingleOrDefault();
            if (user != null)
            {
                if (Email == user.Email)
                {
                    var id = Hash.PwdHash(user.ID);
                    var content = $"http://localhost:55525/Login/ForgetPwdChange?fdew={account}&aswe={id}";
                    AutoEmail.AutoEmailSend(Email, "有桌方遊:忘記密碼", content);
                    return RedirectToAction("LoginForPlayer");
                }
                ViewBag.Error = "信箱錯誤";
                return View();
            }
            ViewBag.Error = "查無此帳號";
            return View();
        }
        //忘記密碼(變更)
        public ActionResult ForgetPwdChange(string fdew,string aswe)
        {
            ViewBag.Account = fdew;
            ViewBag.ID = aswe;
            return View();
        }
        [HttpPost]
        public ActionResult ForgetPwdChange(string fdew,string aswe,string newPwd,string pwdRepeat)
        {
            Player user = db.Players.Where(w => w.Account == fdew).SingleOrDefault();
                if (Hash.PwdHash(user.ID) == aswe)
                {
                    if (newPwd == pwdRepeat)
                    {
                        user.Password = newPwd;
                        db.SaveChanges();
                        return RedirectToAction("LoginForPlayer");
                    }
                    return Content("<script>alert('網址有誤')</script>");
                }
            return View();
        }
    }
}