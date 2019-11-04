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
        //做了一些修改
        //改改改
        //同一個地方做其他修改
    }
}