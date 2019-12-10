using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGIS.Models;

namespace TGIS.Controllers
{
    public class HomeController : Controller
    {
        TGISDBEntities db = new TGISDBEntities();
        //首頁
        public ActionResult Index()
        {
            return View();
        }

        //測試區(請勿刪除)
        public ActionResult Test()
        {
            string playerID = Session["PlayerID"].ToString();
            //如果尚未登入則顯示錯誤訊息
            if (playerID == null)
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);

            //取得此登入玩家的資訊
            Player player = db.Players.Find(playerID);
            ViewBag.Player = player;

            //取得此登入玩家所有揪桌的編號
            List<string> teamIDs = new List<string>();
            player.TeamsForLeader.ToList().ForEach(t => teamIDs.Add(t.ID));
            player.TeamsForOtherPlayer.ToList().ForEach(t => teamIDs.Add(t.ID));
            ViewBag.TeamIDs = teamIDs;
            return View();
        }
    }
}