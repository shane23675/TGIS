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
        [HttpPost]
        public ActionResult Index(string SourceID, HttpPostedFileBase Content)
        {
            if (Content.ContentLength > 0)
        }

        //上傳圖片測試
    }
}