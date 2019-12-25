using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGIS.Models;

namespace TGIS.Controllers
{
    public class PlayerPointDetailController : Controller
    {
        TGISDBEntities db = new TGISDBEntities();
        //玩家查看點數明細
        [CenterLogin(CenterLogin.UserType.Player)]
        public ActionResult MyPoints()
        {
            Player p = db.Players.Find((string)Session["PlayerID"]);
            Session["nowPage"] = "myPoints";
            ViewBag.PointAmount = p.Points;
            return View(p.PlayerPointDetails.ToList());
        }
    }
}