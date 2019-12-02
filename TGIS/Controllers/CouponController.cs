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
            return View(db.Coupons.Find(couponID));
        }
        [HttpPost]
        public ActionResult CouponEdit(Coupon coupon)
        {
            CouponCheck(coupon);
            if (ModelState.IsValid)
            {
                db.Entry(coupon).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("CouponIndexForShop");
            }
            return View(coupon);
        }
    }
}