using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGIS.Models;

namespace TGIS.Controllers
{
    public class PhotoController : Controller
    {
        TGISDBEntities db = new TGISDBEntities();

        //取得圖片的方法
        
        //方法一(參數：sourceID 來源ID, index 索引值，也就是第幾張圖片)
        public ActionResult GetPhotoBySourceID(string sourceID, int index)
        {
            Photo photo;
            try
            {
                photo = db.Photos.Where(m => m.SourceID == sourceID).ToList()[index];
            }
            catch
            {
                //找不到圖片(報錯)則返回預設圖片(來源ID：D00000)
                photo = db.Photos.Where(m => m.SourceID == "D00000").ToList()[0];
            }
            MemoryStream ms = new MemoryStream(photo.Content);
            return File(ms.ToArray(), photo.MIMEType);
        }
        //方法二(參數：photoID 圖片ID)
        public ActionResult GetPhotoByID(int photoID)
        {
            Photo photo = db.Photos.Where(m => m.ID == photoID).FirstOrDefault();
            MemoryStream ms = new MemoryStream(photo.Content);
            return File(ms.ToArray(), photo.MIMEType);
        }


        //用來新增預設圖片的方法(目前僅能由網址進入此頁面)
        public ActionResult CreateDefaultPhoto()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateDefaultPhoto(string sourceID, HttpPostedFileBase[] photos)
        {
            PhotoManager.Create(sourceID, photos);
            return Content("success");
        }

    }
}