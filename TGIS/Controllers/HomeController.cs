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
        public ActionResult Index(string SourceID, HttpPostedFileBase Content)
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
                return RedirectToAction("GetPhoto", new { sourceID = SourceID });
            }
            return new EmptyResult();
        }

        //取得圖片的方法
        public ActionResult GetPhoto(string sourceID)
        {
            byte[] photo = db.Photos.Where(m => m.SourceID == sourceID).FirstOrDefault().Content;
            MemoryStream ms = new MemoryStream(photo);
            return File(ms.ToArray(), "jpeg");
        }

    }
}