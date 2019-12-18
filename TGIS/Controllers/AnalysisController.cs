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
        //揪桌客群分析
        [CenterLogin(CenterLogin.UserType.Shop)]
        public ActionResult TeamStatistic(int monthsFromNow = 0)
        {
            Shop shop = db.Shops.Find(Session["ShopID"].ToString());
            //只取有被接受訂位的團
            var teams = shop.Teams.Where(t => t.IsConfirmedByShop != null && (bool)t.IsConfirmedByShop).ToList();
            //篩選出距今指定月數的資料(0表示30天內，1表示31~60天...)
            teams = teams.Where(t => DateTime.Now.Subtract(t.PlayDate).Days / 30 == monthsFromNow).ToList();
            //如果都沒有人就直接導向View
            if (teams.Count == 0)
            {
                ViewBag.NoTeam = true;
                return View();
            }
            //先取出所有的玩家
            List<Player> players = new List<Player>();
            foreach(Team t in teams)
            {
                players.AddRange(t.OtherPlayers);
                players.Add(t.LeaderPlayer);
            }

            //資料淨化器(可以去除資料中為0的部分)
            void DataCleaner(ref List<string> labels, ref List<int> data)
            {
                for (int i = 0; i < data.Count; i++)
                {
                    if (data[i] == 0)
                    {
                        labels.RemoveAt(i);
                        data.RemoveAt(i);
                        i--;
                    }
                }
            }

            //性別比資料
            List<string> genderLabels = new List<string> { "男", "女" };
            List<int> genderData = new List<int>{ players.Count(p => p.Gender), players.Count(p => !p.Gender) };
            DataCleaner(ref genderLabels, ref genderData);
            ViewBag.GenderLabels = JsonConvert.SerializeObject(genderLabels);
            ViewBag.GenderData = JsonConvert.SerializeObject(genderData);
            //年齡資料
            List<string> ageLabels = new List<string> { "10歲以下", "11-15歲", "16-20歲", "21-25歲", "26-30歲", "31-35歲", "36-40歲", "41歲以上" };
            List<int> ageData = new List<int>{ 0, 0, 0, 0, 0, 0, 0, 0 };
            foreach(Player p in players)
            {
                int age = p.Age;
                if (age <= 10)
                    ageData[0]++;
                else if (age <= 40)
                    ageData[(age - 11) / 5 + 1]++;
                else
                    ageData[7]++;
            }
            DataCleaner(ref ageLabels, ref ageData);
            ViewBag.AgeLabels = JsonConvert.SerializeObject(ageLabels);
            ViewBag.AgeData = JsonConvert.SerializeObject(ageData);

            //行政區資料
            List<string> districtLabels = new List<string>();
            List<int> districtData = new List<int>();
            foreach(District d in shop.District.City.Districts)
            {
                districtLabels.Add(d.DistrictName);
                districtData.Add(players.Count(p => p.DistrictID == d.ID));
            }
            districtLabels.Add("外縣市");
            districtData.Add(players.Count(p => p.District.CityID != shop.District.CityID));
            DataCleaner(ref districtLabels, ref districtData);
            ViewBag.DistrictLabels = JsonConvert.SerializeObject(districtLabels);
            ViewBag.DistrictData = JsonConvert.SerializeObject(districtData);

            return View();
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
            ViewBag.Months = "0";
            ViewBag.Data = "0";
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

    }
}