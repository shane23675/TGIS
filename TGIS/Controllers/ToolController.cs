using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGIS.Models;

namespace TGIS.Controllers
{
    //此控制器為測試中的工具包，請先不要使用其中的任何功能
    public class ToolController : Controller
    {
        TGISDBEntities db = new TGISDBEntities();
        //取得填入時間的PartialView的方法
        public ActionResult _GetTimeInput()
        {
            return PartialView();
        }

        //測試專區
        public ActionResult Test()
        {
            return View();
        }
        //修正RelevantLinks網址的方法
        public ActionResult RelevantLinksReplace()
        {
            //var links = db.RelevantLinks.ToList();
            //foreach (var link in links)
            //{
            //    link.Url = link.Url.Replace("watch?v=", "embed/");
            //}
            //db.SaveChanges();
            return RedirectToAction("Test");
        }
    }
}