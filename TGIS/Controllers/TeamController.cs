using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGIS.Models;

namespace TGIS.Controllers
{
    public class TeamController : Controller
    {
        TGISDBEntities db = new TGISDBEntities();
        // 揪桌列表主頁
        public ActionResult TeamIndexForPlayer()
        {
            return View(db.Teams.ToList());
        }

        //揪桌詳細內容(玩家用)
        public ActionResult TeamDetailForPlayer(string teamID)
        {
            return View(db.Teams.Find(teamID));
        }

        //新開一桌(玩家用)
        public ActionResult TeamCreate()
        {
            //若尚未登入則重新導向至登入頁
            if (Session["PlayerID"] == null)
                return RedirectToAction("LoginForPlayer", "Login");

            ViewBag.teamID = UsefulTools.GetNextID(db.Teams, 1);
            ViewBag.citySelectList = 123; 
            //這裡不傳PlayerID，直接在View中通過Session取得
            return View("TeamCreate");
        }
        [HttpPost]
        public ActionResult TeamCreate(Team team)
        {
            if (ModelState.IsValid)
            {
                db.Teams.Add(team);
                db.SaveChanges();
                return RedirectToAction("TeamIndexForPlayer");
            }
            ViewBag.teamID = team.ID;
            return View();
        }
    }
}