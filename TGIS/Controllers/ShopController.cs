using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TGIS.Models;
using PagedList;

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
            ViewBag.CityID = new SelectList(db.Cities, "ID", "CityName");
            ViewBag.DistrictID = new SelectList(db.Districts, "ID", "DistrictName");
            //傳入自動生成的ID
            ViewBag.shopID = UsefulTools.GetNextID(db.Shops, 1);
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
        public ActionResult MgShopDetail(string id)
        {

            return View(db.Shops.Find(id));
        }

        //管理員編輯店家
        public ActionResult MgShopEdit(string id)
        {
            ViewBag.CityID = new SelectList(db.Cities, "ID", "CityName");
            ViewBag.DistrictID = new SelectList(db.Districts, "ID", "DistrictName");
            return View(db.Shops.Find(id));
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
        //玩家看到的店家列表
        public ActionResult ShopIndexForPlayer()
        {
            return View(db.Shops.ToList());
        }
        //玩家看到店家詳細資料
        public ActionResult ShopDetailForPlayer(string id)
        {
            return View(db.Shops.Find(id));
        }
        //店家編輯店家資料
        public ActionResult ShopEditForStore(string id)
        {
            ViewBag.CityID = new SelectList(db.Cities, "ID", "CityName");
            ViewBag.DistrictID = new SelectList(db.Districts, "ID", "DistrictName");
            return View(db.Shops.Find(id));
        }
        [HttpPost]
        public ActionResult ShopEditForStore(Shop shop)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shop).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("ShopDetailForStore",new {id=shop.ID});
            }
            return View(shop);
        }
        //店家看到店家詳細資料
        public ActionResult ShopDetailForStore(string id)
        {
            return View(db.Shops.Find(id));
        }
    }
}