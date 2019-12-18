using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGIS.Models;

namespace TGIS.Controllers
{
    //處理數據分析的控制器
    public class AnalysisController : Controller
    {
        TGISDBEntities db = new TGISDBEntities();
        //揪桌客群分析(Ajax)
        [HttpPost]
        [CenterLogin(CenterLogin.UserType.Shop)]
        public ActionResult TeamStatistic(int monthsFromNow)
        {
            Shop s = db.Shops.Find(Session["ShopID"].ToString());
            //只取有被接受訂位的團
            var teams = s.Teams.Where(t => t.IsConfirmedByShop != null && (bool)t.IsConfirmedByShop).ToList();
            //篩選出距今指定月數的資料(0表示30天內，1表示31~60天...)
            teams = teams.Where(t => DateTime.Now.Subtract(t.PlayDate).Days / 30 == monthsFromNow).ToList();
            /*
             *  將過去一個月的揪桌資料統計出來：
             *  需要性別、年齡層、行政區
             */
            //先取出所有的玩家
            List<Player> players = new List<Player>();
            foreach(Team t in teams)
            {
                players.AddRange(t.OtherPlayers);
                players.Add(t.LeaderPlayer);
            }
            //取得行政區資料
            Dictionary<string, int> district = new Dictionary<string, int>();
            foreach(District d in s.District.City.Districts)
            {
                district[d.DistrictName] = players.Count(p => p.DistrictID == d.ID);
            }
            district["外縣市"] = players.Count(p => p.District.CityID != s.District.CityID);

            //最終資料
            var data = new
            {
                Total = players.Count,
                Male = players.Count(p => p.Gender),
                Female = players.Count(p => !p.Gender),
                Age0_10 = players.Count(p => p.Age <= 10),
                Age11_15 = players.Count(p => p.Age > 10 && p.Age <= 15),
                Age16_20 = players.Count(p => p.Age > 15 && p.Age <= 20),
                Age21_25 = players.Count(p => p.Age > 20 && p.Age <= 25),
                Age26_30 = players.Count(p => p.Age > 25 && p.Age <= 30),
                Age31_35 = players.Count(p => p.Age > 30 && p.Age <= 35),
                Age36_40 = players.Count(p => p.Age > 35 && p.Age <= 40),
                Age40Up = players.Count(p => p.Age > 40),
                District = district
            };
            return Json(data);
        }

        //桌遊趨勢分析需要的類別
        class TGStatistic
        {
            public string Name { get; set; }
            public int[] Clicks { get; set; } = new int[12];
        }
        //桌遊趨勢分析
        public ActionResult TableGameTrend()
        {
            return View();
        }
        [HttpPost]
        //[CenterLogin(CenterLogin.UserType.Shop)]
        public ActionResult TableGameTrend(string[] tableGameIDs)
        {
            //取得一年內的月份資料(如果現在是6月，則輸出為[7, 8, 9, 10, 11, 12, 1, 2, 3, 4, 5, 6] )
            int nowMonth = DateTime.Now.Month;
            int[] months = new int[12];
            for (int i = 0; i < 12; i++)
            {
                int m = nowMonth - i;
                months[11 - i] = m > 0 ? m : m + 12;
            }

            //取得桌遊統計資料(每筆資料形態皆如TGStatistic)
            TGStatistic[] data = new TGStatistic[tableGameIDs.Length];
            int c = 0;
            foreach (string id in tableGameIDs)
            {
                var game = db.TableGames.Find(id);
                //往資料集合添加一個TGStatistic物件
                data[c] = new TGStatistic { Name = game.ChineseName, Clicks = new int[12] };
                //依據月份順序往TGStatistic物件中的Clicks填入對應的點擊量
                for (int i = 0; i < 12; i++)
                {
                    var statistic = game.TableGameVisitedStatistics.Where(s => s.VisitedDate.ToString("yyyy/MM") == DateTime.Today.AddMonths(-i).ToString("yyyy/MM")).FirstOrDefault();
                    data[c].Clicks[11 - i] = statistic == null ? 0 : statistic.Clicks;
                }
                c++;
            }
            //將資料傳入ViewBag
            ViewBag.Months = JsonConvert.SerializeObject(months);
            ViewBag.Data = JsonConvert.SerializeObject(data);
            return View();
        }

        //優惠券行銷分析(Ajax)
        [HttpPost]
        [CenterLogin(CenterLogin.UserType.Shop)]
        public ActionResult CouponUsage()
        {
            var coupons = db.Shops.Find(Session["ShopID"].ToString()).Coupons.ToList();
            var data = coupons.Select(c => new { c.Content, c.LimitedAmount, c.ExchangedAmount, c.UsedAmount });
            return Json(data);
        }

        //優惠券分析測試
        [CenterLogin(CenterLogin.UserType.Shop)]
        public ActionResult CouponUsageDisplay()
        {
            string shopID = Session["ShopID"].ToString();
            return View();
        }

        [CenterLogin(CenterLogin.UserType.Shop)]
        public ActionResult TeamStatisticDisplay()
        {
            string shopID = Session["ShopID"].ToString();
            return View();
        }

        [CenterLogin(CenterLogin.UserType.Shop)]
        public ActionResult TableGameTrendDisplay()
        {
            string shopID = Session["ShopID"].ToString();
            return View();
        }
    }
}