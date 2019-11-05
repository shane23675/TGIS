using System;
using System.Collections.Generic;
using System.IO;
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
        public string Index(string SourceID, HttpPostedFileBase Content)
        {
            if (Content.ContentLength > 0)
            {
                byte[] photoBytes;
                using (MemoryStream ms = new MemoryStream())
                {
                    Content.InputStream.CopyTo(ms);
                    photoBytes = ms.GetBuffer();
                }
                db.Photos.Add(new Photo { SourceID = SourceID, Content = photoBytes });
                db.SaveChanges();
                return "Success";
            }
            return "Failed";
        }

        //顯示圖片
        public ActionResult GetPhoto()
        {
            //還要再修改
            return View();
        }

    }
}