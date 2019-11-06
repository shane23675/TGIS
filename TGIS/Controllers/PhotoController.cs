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
        public ActionResult GetPhoto(string sourceID, int index)
        {
            Photo photo = db.Photos.Where(m => m.SourceID == sourceID).ToList()[index];
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
    }
}