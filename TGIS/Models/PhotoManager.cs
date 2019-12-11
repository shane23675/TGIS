using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace TGIS.Models
{
    public class PhotoManager
    {
        static TGISDBEntities db = new TGISDBEntities();
        //新增圖片的方法
        public static void Create(string sourceID, HttpPostedFileBase[] photos)
        {
            //若傳入空陣列則返回
            if (photos[0] == null)
                return;
            foreach (HttpPostedFileBase p in photos)
            {
                if (p.ContentLength > 0)
                {
                    byte[] photoBytes;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        //將檔案資訊複製至ms
                        p.InputStream.CopyTo(ms);
                        //將ms轉為二進位資料
                        photoBytes = ms.GetBuffer();
                    }
                    db.Photos.Add(new Photo { SourceID = sourceID, Content = photoBytes, MIMEType = p.ContentType });
                    db.SaveChanges();
                }
            }
        }

        //刪除圖片的方法

        //多載一：以Photo.ID刪除
        public static void Delete(int photoID)
        {
            Photo p = db.Photos.Find(photoID);
            if (p != null)
                db.Photos.Remove(p);
            db.SaveChanges();
        }
        //多載二：以Photo.ID組成的陣列刪除
        public static void Delete(int[] photoIDs)
        {
            if (photoIDs == null)
                return;
            //以迴圈呼叫多載一進行刪除
            foreach (int id in photoIDs)
            {
                Delete(id);
            }
        }
        //多載三：以Photo.SourceID刪除
        public static void Delete(string sourceID)
        {
            List<Photo> photos = db.Photos.Where(m => m.SourceID == sourceID).ToList();
            foreach (Photo p in photos)
            {
                db.Photos.Remove(p);
                db.SaveChanges();
            }
        }


        //取得某來源擁有的圖片的ID的方法
        public static List<int> GetPhotoIDList(string sourceID)
        {
            List<int> photoIDList = new List<int>();
            foreach (Photo p in db.Photos.Where(m => m.SourceID == sourceID))
            {
                photoIDList.Add(p.ID);
            }
            return photoIDList;
        }

    }
}