using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGIS.Models;

namespace TGIS.Controllers
{
    //此控制器為測試中的工具包，請先不要使用其中的任何功能
    public class ToolController : Controller
    {
        TGISDBEntities db = new TGISDBEntities();
        //取得填入時間的PartialView的方法
        public ActionResult _GetTimeInput()
        {
            return PartialView();
        }

        //測試專區
        public ActionResult Test()
        {
            return View();
        }
        //修正RelevantLinks網址的方法
        public ActionResult RelevantLinksReplace()
        {
            var links = db.RelevantLinks.ToList();
            foreach (var link in links)
            {
                link.Url = link.Url.Replace("watch?v=", "embed/");
            }
            db.SaveChanges();
            return RedirectToAction("Test");
        }

        //新增揪桌測試資料的方法
        public string CreateTeamTestData(int n)
        {
            Random r = new Random();
            var players = db.Players.ToList();
            //製造出對應筆數的Team及其相關資料
            for (int i = 0; i < n; i++)
            {
                //將玩家集合打亂
                players = players.OrderBy(p => Guid.NewGuid()).ToList();
                string teamID = UsefulTools.GetNextID(db.Teams, 1);
                int playerAmount = r.Next(2, 6);
                DateTime playDate = DateTime.Today.AddDays(r.Next(-90, -1)).Date;
                Team team = new Team
                {
                    ID = teamID,
                    ShopID = "S00014",
                    Title = "測試用資料",
                    MinPlayer = playerAmount,
                    MaxPlayer = playerAmount,
                    EstimatedCost = 0,
                    IsCanceled = false,
                    IsClosed = false,
                    IsConfirmedByShop = true,
                    IsRequestSent = true,
                    LeaderPlayerID = players[0].ID,
                    PlayDate = playDate,
                    ParticipateEndDate = playDate.AddDays(-1),
                    PlayBeginTime = new TimeSpan(8, 0, 0),
                    PlayEndTime = new TimeSpan(10, 0, 0),
                    Notes = "測試用"
                };
                db.Teams.Add(team);
                db.SaveChanges();

                //存入後立刻取出並填入其他玩家資料
                Team hotNewTeam = db.Teams.Find(teamID);
                for (int j = 1; j < playerAmount; j++)
                {
                    hotNewTeam.OtherPlayers.Add(players[j]);
                }
                db.SaveChanges();
            }
            return "Success";
        }
    }
}