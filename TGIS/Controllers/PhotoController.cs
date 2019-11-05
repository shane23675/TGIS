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

        //取得圖片的方法(參數：sourceID 來源ID, index 索引值，也就是第幾張圖片)
        public ActionResult GetFirstPhoto(string sourceID)
        {
            Photo photo = db.Photos.Where(m => m.SourceID == sourceID).FirstOrDefault();
            MemoryStream ms = new MemoryStream(photo.Content);
            return File(ms.ToArray(), photo.MIMEType);
        }
        
        //取得所有圖片的方法
        //使用此方法時需將返回的photoUrls逐一取出後填入圖片的url中
        public List<FileContentResult> GetAllPhotos(string sourceID)
        {
            //建立容器
            List<FileContentResult> photoUrls = new List<FileContentResult>();
            MemoryStream ms;
            //找到對應sourceID的圖片物件
            Photo[] photos = db.Photos.Where(m => m.SourceID == sourceID).ToArray();
            //將這些圖片讀取後的結果(FileContentResult)加入photoUrls中
            foreach(Photo p in photos)
            {
                ms = new MemoryStream(p.Content);
                photoUrls.Add(File(ms.ToArray(), p.MIMEType));
            }
            return photoUrls;
        }
    }
}