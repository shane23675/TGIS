using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGIS.Models;

namespace TGIS.Controllers
{
    public class PlayerCouponDetailController : Controller
    {
        TGISDBEntities db = new TGISDBEntities();
        //玩家查看「我的優惠券」
        [CenterLogin(CenterLogin.UserType.Player)]
        public ActionResult MyCoupons()
        {
            Player p = db.Players.Find((string)Session["PlayerID"]);
            Session["nowPage"] = "myCoupon";
            return View(p.PlayerCouponDetails.ToList());
        }
    }
}