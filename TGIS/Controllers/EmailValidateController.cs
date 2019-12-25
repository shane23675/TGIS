using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGIS.Models;

namespace TGIS.Controllers
{
    public class EmailValidateController : Controller
    {
        TGISDBEntities db = new TGISDBEntities();
        // GET: Default
        public ActionResult EmailValidate(string Email,string id)
        {
            var content = $"您好，感謝您註冊成為有桌方遊的會員。\n請點擊網址進行信箱驗證：http://localhost:55525/EmailValidate/EmailRepeat?id={id} ，祝您找到同遊好夥伴～\n\n有桌方遊資訊網";
            AutoEmail.AutoEmailSend(Email, "有桌方遊－註冊驗證信",content);
            return View();
        }
        public ActionResult EmailRepeat(string id)
        {
            Player player = db.Players.Find(id);
            player.IsEmailValid = true;
            db.SaveChanges();
            return View();
        }

        //信箱尚未驗證頁面
        public ActionResult EmailValidateRequired()
        {
            return View("EmailValidate");
        }
    }


}