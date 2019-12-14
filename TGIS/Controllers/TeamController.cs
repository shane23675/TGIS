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

        //揪桌詳細內容(玩家用)
        public ActionResult TeamDetailForPlayer(string teamID)
        {
            return View(db.Teams.Find(teamID));
        }
        [HttpPost]
        public ActionResult TeamDetailForPlayer(string teamID, string playerID, string action, bool fromMyTeam=false)
        {
            //先找到對應的team、player
            Team t = db.Teams.Find(teamID);
            Player p = db.Players.Find(playerID);
            //通過action判斷要參加、退出、取消出團或提前截止
            switch (action)
            {
                case "exit":
                    t.OtherPlayers.Remove(p);
                    break;
                case "join":
                    t.OtherPlayers.Add(p);
                    break;
                case "cancel":
                    t.IsCanceled = true;
                    break;
                case "close":
                    t.IsClosed = true;
                    break;
            }
            db.SaveChanges();

            //若此請求來自「我的揪桌」則導回
            if (fromMyTeam)
                return RedirectToAction("MyTeam");
            return View(t);
        }

        //新開一桌(玩家用)
        public ActionResult TeamCreate()
        {
            //若尚未登入則重新導向至登入頁
            if (Session["PlayerID"] == null)
                return RedirectToAction("LoginForPlayer", "Login");

            ViewBag.teamID = UsefulTools.GetNextID(db.Teams, 1);
            ViewBag.CityID = new SelectList(db.Cities, "ID", "CityName");
            ViewBag.DistrictID = new SelectList(db.Districts, "ID", "DistrictName");
            //這裡不傳PlayerID，直接在View中通過Session取得

            //若傳入team表示要重開之前流團的揪桌
            return View();
        }
        [HttpPost]
        public ActionResult TeamCreate(Team team, int CityID, int DistrictID)
        {
            //若登入已超時導致無法取得玩家ID則重新導向至登入頁
            if (Session["PlayerID"] == null)
                return RedirectToAction("LoginForPlayer", "Login");

            //檢查報名截止日期是否在現在時間之後
            if (team.ParticipateEndDate <= DateTime.Now)
                ModelState["ParticipateEndDate"].Errors.Add("報名截止日期必須在現在時間之後");
            //檢查遊戲日期是否在報名截止日期之後
            if (team.PlayDate <= team.ParticipateEndDate)
                ModelState["PlayDate"].Errors.Add("遊戲日期必須在報名截止日期之後");
            //檢查結束時間是否在開始時間之後
            if (team.PlayBeginTime >= team.PlayEndTime)
                ModelState["PlayEndTime"].Errors.Add("結束時間必須在開始時間之後");
            //檢查最高人數是否不小於最低人數
            if (team.MaxPlayer < team.MinPlayer)
                ModelState["MaxPlayer"].Errors.Add("最高人數不得小於最低人數");

            //驗證主體
            if (ModelState.IsValid)
            {
                db.Teams.Add(team);
                db.SaveChanges();
                return RedirectToAction("GetTeamList", new { usage = "TeamIndex"});
            }
            //驗證失敗
            ViewBag.teamID = team.ID;
            ViewBag.CityID = new SelectList(db.Cities, "ID", "CityName", CityID);
            ViewBag.DistrictID = new SelectList(db.Districts, "ID", "DistrictName", DistrictID);
            return View(team);
        }

        //我的揪桌
        public ActionResult MyTeam()
        {
            //尚未登入則跳轉至登入頁面
            if ((string)Session["PlayerID"] == null)
                return RedirectToAction("LoginForPlayer", "Login");

            Session["nowPage"] = "myTeam";
            return View();
        }

        //各種揪桌列表都會導向這裡(首頁、所有揪桌、我的揪桌)
        public ActionResult GetTeamList(string usage)
        {
            var allTeams = db.Teams.ToList();
            ViewBag.usage = usage;
            Player p = null;
            if (Session["PlayerID"] != null)
                p = db.Players.Find((string)Session["PlayerID"]);

            //通過usage來判斷用途
            switch (usage)
            {
                case "Home":
                    return PartialView(allTeams.Where(m => !m.IsExpired).OrderBy(m => m.ParticipateEndDate).Take(10));
                case "TeamIndex":
                    return View(allTeams.Where(m => !m.IsExpired).OrderBy(m => m.ParticipateEndDate));
                case "Leader":
                    return PartialView(p.TeamsForLeader.ToList());
                case "Other":
                    return PartialView(p.TeamsForOtherPlayer.ToList());
            }
            return HttpNotFound();
        }

        //管理員的揪桌管理
        public ActionResult GetTeamListForAdmin()
        {
            if (Session["AdminID"] == null)
                return RedirectToAction("LoginForAdmin", "LoginForAdmin");
            return View(db.Teams.ToList().OrderByDescending(t => t.ID));
        }
        //店家的預約管理
        public ActionResult TeamReservationIndex()
        {
            Shop s = db.Shops.Find(Session["ShopID"].ToString());
            //找出已成團且未過期的Team
            var teams = s.Teams.Where(t => t.Status == "已成團" && t.PlayDate > DateTime.Today).ToList();
            return View(teams);
        }
    }
}