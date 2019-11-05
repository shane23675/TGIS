using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace TGIS.Models
{
    //這個類別是用來放一些在各個Controller都可能用到的函數
    //方法請都設定為static
    public static class UsefulTools
    {
        static TGISDBEntities db = new TGISDBEntities();

        //取得字串類型流水號的方法(參數：dataTable為資料表, n為ID前面文字的數量)
        //使用範例：UsefulTools.GetNextID(db.TableGames, 1)  取得桌遊主檔的下一個ID
        public static string GetNextID<T>(IEnumerable<T> dataTable, int n)
        {
            //取得傳入的資料表的最後一筆資料
            object lastData = dataTable.ToList()[dataTable.Count() - 1];
            //使用反射取得該筆資料的ID屬性資訊
            PropertyInfo info = lastData.GetType().GetProperty("ID");
            //取得最後一筆資料的ID
            string lastID = (string)(info.GetValue(lastData));
            //取得前綴文字
            string prefix = lastID.Substring(0, n);
            //取得後面編號字串
            string IDStr = lastID.Substring(n, lastID.Length - prefix.Length);
            //轉換成數字
            int num = int.Parse(IDStr);
            //編到下一號
            num++;
            //取得當前num的位數
            int digit = num.ToString().Length;
            //前面補零的部分
            string zeroFill = "";
            for (int i = 0; i < IDStr.Length-digit; i++)
            {
                zeroFill += "0";
            }
            //拼接成新ID：前綴文字 + 補零 + 編號
            return prefix + zeroFill + num;
        }
        
        //更新資料物件的方法(可在Edit Action使用)
        //參數：objOld 更新前的物件  objNew 更新後的物件
        //使用範例：UsefulTools.Update(oldTableGame, newTableGame)
        public static void Update<T>(T objOld, T objNew)
        {
            Type t = typeof(T);
            PropertyInfo[] infos = t.GetProperties();
            foreach (PropertyInfo info in infos)
            {
                //若為導覽屬性則跳過
                if (info.GetMethod.IsVirtual)
                    continue;
                info.SetValue(objOld, info.GetValue(objNew));
            }
        }


        //新增圖片的方法
        public static void CreatePhoto(string SourceID, HttpPostedFileBase[] photos)
        {
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
                    db.Photos.Add(new Photo { SourceID = SourceID, Content = photoBytes, MIMEType = p.ContentType });
                    db.SaveChanges();
                }
            }
        }


    }
}