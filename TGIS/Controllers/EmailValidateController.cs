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
            var content = $"http://localhost:55525/EmailValidate/EmailRepeat?id={id}";
            AutoEmail.AutoEmailSend(Email, "有桌方遊:信箱驗證",content);
            return View();
        }
        public ActionResult EmailRepeat(string id)
        {
            Player player = db.Players.Where(m => m.ID == id).FirstOrDefault();
            player.IsEmailValid = true;
            db.SaveChanges();
            return View();
        }
    }
}