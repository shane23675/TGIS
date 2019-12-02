using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGIS.Models;

namespace TGIS.Controllers
{
    /*
        優惠券：
        店家要新增活動必須先以書面形式提出專案，也就是說優惠券只能由管理員增刪修
        所以...就是正常的增刪修
        店家本身應該只能查看/提前結束活動等等(需要再確認)
    */
    public class CouponController : Controller
    {
        TGISDBEntities db = new TGISDBEntities();
        //管理員查看優惠券列表
        public ActionResult CouponIndexForAdmin()
        {
            return View(db.Coupons.ToList());
        }

        //管理員新增優惠券
        public ActionResult CouponCreateForAdmin(string shopID)
        {
            ViewBag.shopID = shopID;
            return View();
        }
        [HttpPost]
        public ActionResult CouponCreateForAdmin(Coupon coupon)
        {
            if (ModelState.IsValid)
            {
                db.Coupons.Add(coupon);
                db.SaveChanges();
                return RedirectToAction("CouponIndexForAdmin");
            }
            ViewBag.shopID = coupon.ShopID;
            return View();
        }
    }
}