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
        public string Index()
        {
            return UsefulTools.GetNextID(db.Shops, 1);
        }
        //衝突測試：
        //寫一些無意義的東西來看看
        //變更變更



        //123132
    }
}