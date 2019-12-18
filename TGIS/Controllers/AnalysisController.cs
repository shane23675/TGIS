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
        public ActionResult TeamStatistic(int moonsFromNow)
        {
            Shop s = db.Shops.Find(Session["ShopID"].ToString());
            //只取有被接受訂位的團
            var teams = s.Teams.Where(t => t.IsConfirmedByShop != null && (bool)t.IsConfirmedByShop).ToList();
            //篩選出距今指定月數的資料(0表示30天內，1表示31~60天...)
            teams = teams.Where(t => DateTime.Now.Subtract(t.PlayDate).Days / 30 == moonsFromNow).ToList();
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
            public int Month { get; set; }
            public int Clicks { get; set; }
        }
        //桌遊趨勢分析(Ajax)
        [HttpPost]
        [CenterLogin(CenterLogin.UserType.Shop)]
        public ActionResult TableGameTrend(string[] tableGameIDs)
        {
            /*
             *  店家選擇桌遊
             *  取得這些桌遊前一年份的閱覽紀錄
             *  回傳Json：月份、點擊量
             */
            //建立裝資料的容器
            var data = new Dictionary<string, List<TGStatistic>>();

            foreach(string id in tableGameIDs)
            {
                data[id] = new List<TGStatistic>();
                //找到對應桌遊的統計資料
                var statistics = db.TableGames.Find(id).TableGameVisitedStatistics.ToList();
                //依據月份遞減找到對應的資料
                for (int i = 0; i < 12; i++)
                {
                    var statistic = statistics.Where(s => s.VisitedDate.ToString("yyyy/MM") == DateTime.Today.AddMonths(-i).ToString("yyyy/MM")).FirstOrDefault();
                    int clicks = 0;
                    if (statistic != null)
                        clicks = statistic.Clicks;
                    //將統計資料填入小容器
                    data[id].Add(new TGStatistic{ Month = DateTime.Today.AddMonths(-i).Month, Clicks = clicks });
                }
            }
            return Json(data);
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