using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGIS.Models;

namespace TGIS.Controllers
{
    public class ShopController : Controller
    {
        TGISDBEntities db = new TGISDBEntities();
        // GET: Shop
        //店家主檔

        //店家頁面
        public ActionResult ShopIndex()
        {
            return View(db.Shops.ToList());
        }

        //管理員創建店家會員
        public ActionResult MgShopCreate()
        {
            ViewBag.DistrictID = new SelectList(db.Districts, "ID", "DistrictName");
            return View();
        }
        [HttpPost]
        public ActionResult MgShopCreate(Shop shop)
        {
            if (ModelState.IsValid)
            {
                db.Shops.Add(shop);
                db.SaveChanges();

                return RedirectToAction("ShopIndex");
            }
            return View(shop);
        }

        //刪除店家資料
        public ActionResult ShopDelete(string id)
        {
            var str = db.Shops.Find(id);
            db.Shops.Remove(str);
            db.SaveChanges();

            return RedirectToAction("ShopIndex");
        }

        //店家詳細資料
        public ActionResult MgShopDetail (string id)
        {
            Shop s = db.Shops.Find(id);
            return View(s);
        }

        //管理員編輯店家
        public ActionResult MgShopEdit(string id)
        {
            ViewBag.DistrictID = new SelectList(db.Districts, "ID", "DistrictName");
            Shop s = db.Shops.Find(id);
            return View(s);
        }
        [HttpPost]
        public ActionResult MgShopEdit(Shop shop)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shop).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("ShopIndex");
            }
            return View(shop);
        }

    }
}