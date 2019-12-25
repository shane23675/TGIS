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
        //測試專區
        public ActionResult Test()
        {
            return View();
        }
        //修正RelevantLinks網址
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

        //新增揪桌測試資料的
        public string CreateTeamTestData(string shopID, int n)
        {
            Random r = new Random();
            var players = db.Players.ToList();
            //製造出對應筆數的Team及其相關資料
            for (int i = 0; i < n; i++)
            {
                //將玩家集合打亂
                players = players.OrderBy(p => Guid.NewGuid()).ToList();
                string teamID = UsefulTools.GetNextID(db.Teams, 1);
                int playerAmount = r.Next(2, 11);
                DateTime playDate = DateTime.Today.AddDays(r.Next(-90, -1)).Date;
                Team team = new Team
                {
                    ID = teamID,
                    ShopID = shopID,
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

        //新增優惠券測試資料
        public string CreateCouponTestData(string shopID, int n)
        {
            var players = db.Players.ToList();
            Random r = new Random();
            for (int i = 0; i < n; i++)
            {
                int limitedAmount = r.Next(10, 30);
                int exchangedAmount = limitedAmount * r.Next(1, 10) / 10;
                int usedAmount = exchangedAmount * r.Next(3, 10) / 10;
                //產生一張優惠券
                Coupon coupon = new Coupon
                {
                    ID = UsefulTools.GetNextID(db.Coupons, 1),
                    BeginDate = DateTime.Parse("2010-10-10"),
                    ExpireDate = DateTime.Parse("2010-10-11"),
                    ShopID = shopID,
                    Content = "測試用資料",
                    IsAvailable = false,
                    PointsRequired = 1,
                    LimitedAmount = limitedAmount
                };
                db.Coupons.Add(coupon);
                db.SaveChanges();

                //產生隨機排序的玩家列表
                players = players.OrderBy(p => Guid.NewGuid()).ToList();
                //依序兌換及使用優惠券
                for (int j = 0; j < exchangedAmount; j++)
                {
                    //產生優惠券兌換明細
                    var p = players[j];
                    var detail = new PlayerCouponDetail
                    {
                        CouponID = coupon.ID,
                        ExchangedDate = DateTime.Parse("2010-10-11"),
                        PlayerID = p.ID,
                        //只使用指定數量
                        IsUsed = j <= usedAmount
                    };
                    db.PlayerCouponDetails.Add(detail);
                }
                db.SaveChanges();
            }
            return "Success";
        }

        //新增玩家測試資料
        public string CreatePlayerTestData(int n)
        {
            var districts = db.Districts.ToList();
            Random r = new Random();
            for (int i = 0; i < n; i++)
            {
                db.Players.Add(new Player
                {
                    ID = UsefulTools.GetNextID(db.Players, 2),
                    Account = "testPlayer999",
                    Password = "testPlayer999",
                    Birthday = DateTime.Today.AddYears(-r.Next(8, 55)),
                    Points = 0,
                    IsBanned = false,
                    IsEmailValid = true,
                    Email = "test@gmail.com",
                    DistrictID = districts[r.Next(0, districts.Count)].ID,
                    Gender = r.Next(0, 2) == 1,
                    NickName = "測試用資料"
                });
                db.SaveChanges();
            }
            return "Success";
        }

        //新增桌遊閱覽紀錄測試資料
        public string CreateTGVisitedStatisticData()
        {
            DateTime today = DateTime.Today;
            Random r = new Random();
            foreach (var game in db.TableGames.ToList())
            {
                //隨機產生基本點擊量和浮動點擊量，實現擬真數據
                int baseClicks = r.Next(0, 500);
                int floatClicks = r.Next(100, 1000);
                for (int i = 0; i < 12; i++)
                {
                    db.TableGameVisitedStatistics.Add(new TableGameVisitedStatistic
                    {
                        TableGameID = game.ID,
                        VisitedDate = today.AddMonths(-i).Date,
                        Clicks = r.Next(baseClicks, baseClicks+floatClicks)
                    }) ;
                }
            }
            db.SaveChanges();
            return "Success";
        }
    
    }
}