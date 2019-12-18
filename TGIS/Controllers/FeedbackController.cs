using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGIS.Models;

namespace TGIS.Controllers
{
    public class FeedbackController : Controller
    {
        TGISDBEntities db = new TGISDBEntities();
        //聯絡我們
        public ActionResult ContactUs()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ContactUs(Feedback feedback)
        {
            //不需要檢舉人和被檢舉人，只確定回報類型
            feedback.Plaintiff = "";
            feedback.Defendent = "";
            feedback.ReceivedDate = DateTime.Now;
            feedback.TypeTagID = "R03";
            feedback.IsRead = false;
            db.Feedbacks.Add(feedback);
            db.SaveChanges();
            return View("ThanksForFeedback");
        }
        // 檢舉玩家或店家
        public ActionResult Report(string defendent)
        {
            //尚未登入則導向登入頁
            if (Session["PlayerID"] == null)
                return RedirectToAction("LoginForPlayer", "Login");

            //判斷被檢舉人類型
            if (defendent.Substring(0, 2) == "TP")
            {
                //判斷為玩家
                Player p = db.Players.Find(defendent);
                if (p != null)
                {
                    ViewBag.PlaintiffName = p.NickName;
                    TempData["PlaintiffName"] = p.NickName;
                    TempData["TypeTagID"] = "R01";
                }
            }
            else if (defendent.Substring(0, 1) == "S")
            {
                //判斷為店家
                Shop s = db.Shops.Find(defendent);
                if (s != null)
                {
                    ViewBag.PlaintiffName = s.ShopName;
                    TempData["PlaintiffName"] = s.ShopName;
                    TempData["TypeTagID"] = "R02";
                }
            }
            else
            {
                return HttpNotFound();
            }
            TempData["Defendent"]= defendent;
            return View();
        }
        [HttpPost]
        public ActionResult Report(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                //填入自動取得的資料
                feedback.Plaintiff = (string)Session["PlayerID"];
                feedback.Defendent = (string)TempData["Defendent"];
                feedback.ReceivedDate = DateTime.Now;
                feedback.TypeTagID = (string)TempData["TypeTagID"];
                feedback.IsRead = false;
                TempData.Clear();

                db.Feedbacks.Add(feedback);
                db.SaveChanges();
                return View("ThanksForFeedback");
            }
            ViewBag.PlaintiffName = (string)TempData["PlaintiffName"];
            TempData.Keep();
            return View(feedback);
        }

        //管理員查看回報列表
        public ActionResult ReturnList()
        {
            return View(db.Feedbacks.ToList());
        }
        
        //管理員刪除回報
        public ActionResult ReturnDel(int id)
        {
            var fd = db.Feedbacks.Find(id);
            db.Feedbacks.Remove(fd);
            db.SaveChanges();
            return RedirectToAction("ReturnList");
        }

        //管理員查看回報詳細
        public ActionResult ReturnDetail(int id)
        {
            Feedback f = db.Feedbacks.Find(id);
            f.IsRead = true;
            db.SaveChanges();
            return View(f);
        }

        //關於我們
        public ActionResult AboutUs()
        {
            return View();
        }
    }
}