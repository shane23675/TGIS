using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TGIS.Models;

namespace TGIS.Controllers
{

    public class ajaxController : Controller
    {
        TGISDBEntities db = new TGISDBEntities();
        bool RepeatCheck<T>(IEnumerable<T> table, string val,string property)
        {
            PropertyInfo info = typeof(T).GetProperty(property);
            foreach (T item in table)
            {
                string value = info.GetValue(item).ToString();
                if (value == val)
                    return true;
            }
            return false;
        }
        // GET: District
        //連動式列表(行政區)
        public ActionResult generateStateList(int CId, int? Did)
        {
            StringBuilder sb = new StringBuilder();
            var c = db.Districts.Where(m => m.CityID == CId).ToList();
            foreach (var item in c)
            {
                if (Did == item.ID)
                {
                    sb.Append($"<option value='{item.ID}' Selected>{item.DistrictName}</option>");
                }
                else
                {
                    sb.Append($"<option value='{item.ID}'>{item.DistrictName}</option>");
                }
            }
            return Content(sb.ToString());
        }
        public  ActionResult AccountRepeat(string val, string tableName,string property)
        {
            bool isRepeated = false;
            switch (tableName)
            {
                case "Shops":
                    isRepeated = RepeatCheck(db.Shops, val, property);
                    break;
                case "Players":
                    isRepeated = RepeatCheck(db.Players, val, property);
                    break;
            }
            if (!isRepeated)
            {
                return Content("可使用".ToString());
            }
            return Content("已使用過".ToString());
        }

        //取得店家列表
        public ActionResult GetShopSelectList(int districtID)
        {
            Shop[] shops = db.Districts.Find(districtID).Shops.ToArray();
            //找不到任何店家則返回錯誤選項
            if (shops.Length == 0)
                return Content("<option value=\"ErrorMessage\">此地區無任何店家</option>");
            string result = "";
            foreach (Shop s in shops)
            {
                result += $"<option value=\"{s.ID}\">{s.ShopName}</option>";
            }
            return Content(result);
        }
    }
}