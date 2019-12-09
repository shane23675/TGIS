using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGIS.Models;

namespace TGIS.Controllers
{
    public class CouponController : Controller
    {
        TGISDBEntities db = new TGISDBEntities();
        //管理員查看優惠券列表
        public ActionResult CouponIndexForAdmin()
        {
            return View(db.Coupons.ToList());
        }
        
        //管理員啟用優惠券
        public ActionResult CouponActivate(string couponID)
        {
            Coupon c = db.Coupons.Find(couponID);
            c.IsAvailable = true;
            db.SaveChanges();
            return RedirectToAction("CouponIndexForAdmin");
        }

        //店家查看自己的優惠券列表
        public ActionResult CouponIndexForShop()
        {
            Shop s = db.Shops.Find((string)Session["ShopID"]);
            return View(s.Coupons.ToList());
        }

        //新增及修改優惠券時檢查的方法
        void CouponCheck(Coupon coupon)
        {
            //檢查到期日期是否在開始日期以及今天之後
            if (coupon.BeginDate > coupon.ExpireDate)
                ModelState["ExpireDate"].Errors.Add("到期日期必須在開始使用日期之後");
            else if (DateTime.Today > coupon.ExpireDate)
                ModelState["ExpireDate"].Errors.Add("到期日期必須在今天之後");
        }
        //店家新增優惠券
        public ActionResult CouponCreate()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CouponCreate(Coupon coupon)
        {
            //填入預設值
            coupon.ID = UsefulTools.GetNextID(db.Coupons, 1);
            coupon.ShopID = (string)Session["ShopID"];
            coupon.IsAvailable = false;

            CouponCheck(coupon);
            if (ModelState.IsValid)
            {
                db.Coupons.Add(coupon);
                db.SaveChanges();
                return RedirectToAction("CouponIndexForShop");
            }
            return View();
        }

        //店家修改優惠券
        public ActionResult CouponEdit(string couponID)
        {
            Coupon c = db.Coupons.Find(couponID);
            TempData["Coupon"] = c;
            return View(c);
        }
        [HttpPost]
        public ActionResult CouponEdit(Coupon coupon)
        {
            //從TempData取出原始資料並存入必要欄位
            Coupon c = (Coupon)TempData["Coupon"];
            coupon.ID = c.ID;
            coupon.ShopID = c.ShopID;
            //優惠券只要有修改過就必須再經審核
            coupon.IsAvailable = false;
            CouponCheck(coupon);
            if (ModelState.IsValid)
            {
                db.Entry(coupon).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("CouponIndexForShop");
            }
            return View(coupon);
        }

        //玩家查看店家優惠券列表
        public ActionResult _CouponInShopIndex(string shopID)
        {
            //僅傳入可兌換的優惠券
            return PartialView(db.Shops.Find(shopID).Coupons.Where(m=>m.IsExchangable).ToList());
        }

        //玩家兌換優惠券
        public ActionResult ExchangeCoupon(string couponID)
        {
            Coupon c = db.Coupons.Find(couponID);
            Player p = db.Players.Find(Session["PlayerID"]);
            //排除不能兌換、已經兌換過的優惠券
            if (c.IsExchangable && !p.PlayerCouponDetails.Any(m=>m.Coupon.Equals(c)))
            {
                //從玩家扣除使用的點數
                p.Points -= c.PointsRequired;
                //產生點數使用明細
                UsefulTools.PointRecord(p.ID, "兌換優惠券", -c.PointsRequired);
                //產生玩家優惠券明細
                PlayerCouponDetail couponDetail = new PlayerCouponDetail
                {
                    CouponID = c.ID,
                    PlayerID = p.ID,
                    ExchangedDate = DateTime.Now,
                    IsUsed = false
                };
                db.PlayerCouponDetails.Add(couponDetail);
                db.SaveChanges();

                return RedirectToAction("CouponInShopIndex", new { shopID = c.ShopID });
            }
            //優惠券已無法兌換則顯示錯誤訊息
            return HttpNotFound();
        }

        //玩家使用優惠券
        public ActionResult UseCoupon(string couponID)
        {
            Player p = db.Players.Find((string)Session["PlayerID"]);
            var detail = db.PlayerCouponDetails.Find(p.ID, couponID);
            detail.IsUsed = true;
            db.SaveChanges();
            return RedirectToAction("MyCoupons", "PlayerCouponDetail");
        }

    }
}