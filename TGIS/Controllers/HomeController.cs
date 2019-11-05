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
        public ActionResult Index()
        {
            return View();
        }


        //上傳圖片測試
    }
}