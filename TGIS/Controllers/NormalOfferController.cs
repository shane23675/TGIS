using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGIS.Models;

namespace TGIS.Controllers
{
    public class NormalOfferController : Controller
    {
        TGISDBEntities db = new TGISDBEntities();
        //查看活動列表
        public ActionResult OfferList()
        {
            //若有店家ID則顯示店家活動列表
            Shop s = db.Shops.Find((string)Session["ShopID"]);
            if (s != null)
                return View("OfferList", "_LayoutShoperCenter", s.NormalOffers.ToList());
            //否則顯示全部活動列表(管理員用)
            return View(db.NormalOffers.ToList());
        }

        //店家新增優惠券
        public ActionResult OfferCreate()
        {
            return View();
        }
        [HttpPost]
        public ActionResult OfferCreate(NormalOffer normalOffer, HttpPostedFileBase[] photos)
        {
            if (ModelState.IsValid)
            {
                //填入預設值
                normalOffer.ID = UsefulTools.GetNextID(db.NormalOffers, 1);
                normalOffer.ShopID = Session["ShopID"].ToString();
                normalOffer.Clicks = 0;

                db.NormalOffers.Add(normalOffer);
                db.SaveChanges();
                //存入圖片
                PhotoManager.Create(normalOffer.ID, photos);
                return RedirectToAction("OfferList");
            }
            return View();
        }

        //店家編輯活動
        public ActionResult OfferEdit(string normalOfferID)
        {
            var offer = db.NormalOffers.Find(normalOfferID);
            //檢查店家ID是否為活動所屬店家
            string shopID = (string)Session["ShopID"];
            if (shopID != offer.ShopID)
                return HttpNotFound();

            //將此活動的圖片數量傳入ViewBag
            ViewBag.photoIDList = PhotoManager.GetPhotoIDList(normalOfferID);
            TempData["Offer"] = offer;
            return View(offer);
        }
        [HttpPost]
        public ActionResult OfferEdit(NormalOffer normalOffer, HttpPostedFileBase[] photos)
        {
            if (ModelState.IsValid)
            {
                //取出原始資料並灌入必要資料
                NormalOffer offer = (NormalOffer)TempData["Offer"];
                normalOffer.ID = offer.ID;
                normalOffer.ShopID = offer.ShopID;
                normalOffer.Clicks = offer.Clicks;

                db.Entry(normalOffer).State = EntityState.Modified;
                db.SaveChanges();
                //存入圖片
                PhotoManager.Create(normalOffer.ID, photos);
                return RedirectToAction("OfferList");
            }
            TempData.Keep("Offer");
            return View();
        }

        //店家刪除活動
        public ActionResult OfferDel(string normalOfferID)
        {
            var offer = db.NormalOffers.Find(normalOfferID);
            //檢查店家ID是否為活動所屬店家
            string shopID = (string)Session["ShopID"];
            if (shopID != offer.ShopID)
                return HttpNotFound();

            //先刪除圖片
            PhotoManager.Delete(normalOfferID);
            db.NormalOffers.Remove(offer);
            db.SaveChanges();
            return RedirectToAction("OfferList");
        }

        //給玩家看的活動列表
        [ChildActionOnly]
        public ActionResult _OfferListForPlayer(string shopID)
        {
            return PartialView(db.Shops.Find(shopID).NormalOffers.ToList());
        }

        //給玩家看的活動詳細資料
        public ActionResult OfferDetail(string normalOfferID)
        {
            var offer = db.NormalOffers.Find(normalOfferID);
            //存入來源店家ID以便返回頁面
            ViewBag.ShopID = offer.ShopID;
            //將此活動的圖片數量傳入ViewBag
            ViewBag.photoIDList = PhotoManager.GetPhotoIDList(normalOfferID);
            return View(offer);
        }

        //查看店家活動列表
        public ActionResult OfferListForAdmin(string shopID)
        {
            var s = db.Shops.Find(shopID).NormalOffers;
            ViewBag.Shop = db.Shops.Find(shopID).ShopName;
            return View(s.ToList());
        }

        public ActionResult OfferListStatusChange(string normalOfferID,string shopID)
        {
            db.NormalOffers.Find(normalOfferID).IsHidden = !(db.NormalOffers.Find(normalOfferID).IsHidden);
            ViewBag.Status = db.NormalOffers.Find(normalOfferID).IsHidden;
            db.SaveChanges();
            return RedirectToAction("OfferListForAdmin","NormalOffer",new{ shopID = shopID });
        }
    }
}