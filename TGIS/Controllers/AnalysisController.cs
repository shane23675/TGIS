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
    }
}