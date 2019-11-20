﻿using System;
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
        TGISDBEntities db = new TGISDBEntities();
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
            return Content("以使用過".ToString());
        }
    }
}