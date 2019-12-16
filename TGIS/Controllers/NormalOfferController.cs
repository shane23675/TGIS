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
        //店家查看活動列表
        [CenterLogin(CenterLogin.UserType.Shop)]
        public ActionResult OfferListForShop()
        {
            Shop s = db.Shops.Find((string)Session["ShopID"]);
            return View(s.NormalOffers.ToList());
        }

        //檢查活動時間是否正確的方法
        void OfferTimeCheck(NormalOffer normalOffer)
        {
            if (normalOffer.BeginDate > normalOffer.EndDate)
                ModelState["BeginDate"].Errors.Add("開始時間必須在結束時間之前");
            if (normalOffer.EndDate < DateTime.Now)
                ModelState["EndDate"].Errors.Add("結束時間必須在現在時間之後");
        }

        //店家新增優惠券
        public ActionResult OfferCreate()
        {
            return View();
        }
        [HttpPost]
        public ActionResult OfferCreate(NormalOffer normalOffer, HttpPostedFileBase[] photos)
        {
            OfferTimeCheck(normalOffer);
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
            OfferTimeCheck(normalOffer);
            NormalOffer offer = (NormalOffer)TempData["Offer"];
            if (ModelState.IsValid)
            {
                //取出原始資料並灌入必要資料
                normalOffer.ID = offer.ID;
                normalOffer.ShopID = offer.ShopID;
                normalOffer.Clicks = offer.Clicks;

                db.Entry(normalOffer).State = EntityState.Modified;
                db.SaveChanges();
                //存入圖片
                PhotoManager.Create(normalOffer.ID, photos);
                return RedirectToAction("OfferList");
            }
            ViewBag.photoIDList = PhotoManager.GetPhotoIDList(offer.ID);
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
            //增加被點擊次數
            offer.Clicks++;
            db.SaveChanges();
            //存入來源店家ID以便返回頁面
            ViewBag.ShopID = offer.ShopID;
            //將此活動的圖片數量傳入ViewBag
            ViewBag.photoIDList = PhotoManager.GetPhotoIDList(normalOfferID);
            return View(offer);
        }

        //管理員查看店家活動列表
        public ActionResult OfferListForAdmin(string shopID)
        {
            var s = db.Shops.Find(shopID).NormalOffers;
            ViewBag.Shop = db.Shops.Find(shopID).ShopName;
            return View(s.ToList());
        }

        //變更活動狀態
        public ActionResult OfferListStatusChange(string normalOfferID,string shopID)
        {
            db.NormalOffers.Find(normalOfferID).IsHidden = !(db.NormalOffers.Find(normalOfferID).IsHidden);
            ViewBag.Status = db.NormalOffers.Find(normalOfferID).IsHidden;
            db.SaveChanges();
            return RedirectToAction("OfferListForAdmin","NormalOffer",new{ shopID = shopID });
        }
    }
}