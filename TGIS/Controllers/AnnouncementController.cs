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
                ann.AdministratorID = Session["AdminID"].ToString();
                ann.AnnouncedDate = DateTime.Today;
                db.Announcements.Add(ann);
                db.SaveChanges();
            }
            return RedirectToAction("AnnouncementList");
        }
        public ActionResult EditAnnoun(string id)
        {
            return View(db.Announcements.Find(id));
        }
        [HttpPost]
        public ActionResult EditAnnoun(Announcement ann)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ann).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("AnnouncementList");
            }
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
    }
}