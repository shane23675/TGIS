using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
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
            ViewBag.CityID = new SelectList(db.Cities, "ID", "CityName");
            ViewBag.DistrictID = new SelectList(db.Districts, "ID", "DistrictName");
            //傳入自動生成的ID
            ViewBag.shopID = UsefulTools.GetNextID(db.Shops, 1);
            return View();
        }
        [HttpPost]
        public ActionResult MgShopCreate(Shop shop, HttpPostedFileBase[] photos)
        {
            if (ModelState.IsValid)
            {
                shop.Password = Hash.PwdHash(shop.Password);
                db.Shops.Add(shop);
                db.SaveChanges();

                //添加圖片
                PhotoManager.Create(shop.ID, photos);

                return RedirectToAction("ShopIndex");
            }
            ViewBag.DistrictID = new SelectList(db.Districts, "ID", "DistrictName");
            ViewBag.CityID = new SelectList(db.Cities, "ID", "CityName",shop.District.CityID);
            return View(shop);
        }

        //刪除店家資料
        public ActionResult ShopDelete(string id)
        {
            //刪除該店家的圖片
            PhotoManager.Delete(id);

            //最後再刪除店家本身
            Shop s = db.Shops.Find(id);
            db.Shops.Remove(s);
            db.SaveChanges();

            return RedirectToAction("ShopIndex");
        }

        //管理員編輯店家
        public ActionResult MgShopEdit(string id)
        {
            var shop = db.Shops.Find(id);
            ViewBag.CityID = new SelectList(db.Cities, "ID", "CityName",shop.District.CityID);
            ViewBag.DistrictID = new SelectList(db.Districts, "ID", "DistrictName");
            ViewBag.photoIDList = PhotoManager.GetPhotoIDList(id);
            return View(db.Shops.Find(id));
        }
        [HttpPost]
        public ActionResult MgShopEdit(Shop shop, int[] deletedPhotoID, HttpPostedFileBase[] newPhoto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(shop).State = EntityState.Modified;
                db.SaveChanges();

                //刪除被勾選的圖片
                if (deletedPhotoID != null)
                {
                    foreach (int id in deletedPhotoID)
                    {
                        PhotoManager.Delete(id);
                    }
                }
                //加入新圖片
                PhotoManager.Create(shop.ID, newPhoto);

                return RedirectToAction("ShopIndex");
            }
            return View(shop);
        }
        //玩家看到的店家列表
        public ActionResult ShopIndexForPlayer()
        {
            ViewBag.CityID = new SelectList(db.Cities, "ID", "CityName");
            ViewBag.DistrictID = new SelectList(db.Districts, "ID", "DistrictName");
            return View(db.Shops.ToList());
        }
        //玩家看到店家詳細資料
        public ActionResult ShopDetailForPlayer(string id)
        {
            ViewBag.photoIDList = PhotoManager.GetPhotoIDList(id);
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
        public ActionResult ShopDetailForStore()
        {
            var id = Session["ShopID"].ToString();
            return View(db.Shops.Find(id));
        }
        public ActionResult ShopPasswordChange()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ShopPasswordChange(string pwd,string newpwd,string pwdrepeat)
        {
            var id = Session["ShopID"].ToString();
            Shop shop = db.Shops.Find(id);
            if (ModelState.IsValid)
            {
                if (shop.Password == Hash.PwdHash(pwd))
                {
                    if (newpwd == pwdrepeat)
                    {
                        shop.Password = Hash.PwdHash(newpwd);
                        db.SaveChanges();

                        return RedirectToAction("ShopDetailForStore",new {id});
                    }
                    ViewBag.Error = "新密碼不符";
                    return View();
                }
                ViewBag.Error = "舊密碼不符";
                return View();
            }
            return View();
        }

        //取得店家列表(Ajax)
        public ActionResult GetShopSelectList(int districtID)
        {
            Shop[] shops = db.Districts.Find(districtID).Shops.ToArray();
            //找不到任何店家則返回錯誤選項
            if (shops.Length == 0)
                return Content("<option value=\"ErrorMessage\">此地區無任何店家</option>");
            string result = "";
            foreach (Shop s in shops)
            {
                result += $"<option value=\"{s.ID}\">{s.ShopName}</option>";
            }
            return Content(result);
        }

    }
}