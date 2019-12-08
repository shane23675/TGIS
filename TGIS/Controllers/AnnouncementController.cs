using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGIS.Models;

namespace TGIS.Controllers
{
    public class AnnouncementController : Controller
    {
        TGISDBEntities db = new TGISDBEntities();
        // GET: Announcement
        public ActionResult AnnouncementList()
        {
            return View(db.Announcements.ToList());
        }
        public ActionResult CreateAnnoun()
        {
            if (Session["AdminID"] == null)
            {
                return RedirectToAction("LoginForAdmin", "LoginForAdmin");
            }
            return View();
        }
        [HttpPost]
        public ActionResult CreateAnnoun(Announcement ann)
        {
            if (ModelState.IsValid)
            {
                //填入必要資料
                ann.ID = UsefulTools.GetNextID(db.Announcements, 2);
                ann.AdministratorID = Session["AdminID"].ToString();
                ann.AnnouncedDate = DateTime.Today;
                db.Announcements.Add(ann);
                db.SaveChanges();
            }
            return RedirectToAction("AnnouncementList");
        }
        public ActionResult EditAnnoun(string id)
        {
            if (Session["AdminID"] == null)
            {
                return RedirectToAction("LoginForAdmin", "LoginForAdmin");
            }
            TempData["AnnouncementID"] = id;
            return View(db.Announcements.Find(id));
        }
        [HttpPost]
        public ActionResult EditAnnoun(Announcement ann)
        {
            if (ModelState.IsValid)
            {
                //取出原始資料進行修改
                var announcement = db.Announcements.Find((string)TempData["AnnouncementID"]);
                announcement.AdministratorID = (string)Session["AdminID"];
                announcement.AnnouncedDate = DateTime.Today;
                announcement.Title = ann.Title;
                announcement.Content = ann.Content;

                db.SaveChanges();
                return RedirectToAction("AnnouncementList");
            }
            TempData.Keep("AnnouncementID");
            return View(ann);
        }
        public ActionResult AnnounDel(string id)
        {
            var ann = db.Announcements.Find(id);
            db.Announcements.Remove(ann);
            db.SaveChanges();
            return RedirectToAction("AnnouncementList");
        }
        public ActionResult AnnounDetail(string id)
        {
            return View(db.Announcements.Find(id));
        }
        public ActionResult _AnnouncementPartail(int number)
        {
            List<Announcement> Ann;
            Ann = db.Announcements.OrderByDescending(m => m.AnnouncedDate).Take(number).ToList();
            return PartialView(Ann);
        }
    }
}