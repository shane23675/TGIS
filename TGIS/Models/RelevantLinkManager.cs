using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TGIS.Models
{
    public static class RelevantLinkManager
    {
        static TGISDBEntities db = new TGISDBEntities();

        //新增連結的方法
        public static void Create(string tableGameID, string[] links)
        {
            if (links == null)
                return;
            foreach (string url in links)
            {
                if (url != "")
                {
                    db.RelevantLinks.Add(new RelevantLink { TableGameID = tableGameID, Url = url });
                    db.SaveChanges();
                }
            }
        }

        //刪除連結的方法
        public static void Delete(int[] deletedLinkIDs)
        {
            RelevantLink rl;
            foreach (int id in deletedLinkIDs)
            {
                rl = db.RelevantLinks.Find(id);
                db.RelevantLinks.Remove(rl);
                db.SaveChanges();
            }
        }
    }
}