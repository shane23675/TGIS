using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGIS.Models;

namespace TGIS.Controllers
{
    public class TagController : Controller
    {
        TGISDBEntities db = new TGISDBEntities();
        // GET: Tag
        public ActionResult TagIndex()
        {
            return View();
        }

        public ActionResult TagCreate(string TagName,string TagID)
        {
            var tag = new Tag
            {
                TagName = TagName,
                ID = UsefulTools.GetNextID(db.Tags.ToList().Where(t => t.ID[0].ToString() == TagID), 1)
            };
            db.Tags.Add(tag);
            db.SaveChanges();
            return RedirectToAction("_TagList");
        }

        public ActionResult _TagList()
        {
            ViewBag.brand = db.Tags.ToList().Where(t => t.ID[0] == 'B');
            ViewBag.category = db.Tags.ToList().Where(t => t.ID[0] == 'C');
            ViewBag.difficulty = db.Tags.ToList().Where(t => t.ID[0] == 'D');
            ViewBag.report = db.Tags.ToList().Where(t => t.ID[0] == 'R');
            return PartialView();
        }

        public ActionResult TagDelete(string TagID)
        {
            var t = db.Tags.Find(TagID);
            db.Tags.Remove(t);
            db.SaveChanges();
            return RedirectToAction("_TagList");
        }

        public ActionResult TagEdit(string TagID,string NewTagName)
        {
            var t = db.Tags.Find(TagID);
            t.TagName = NewTagName;
            db.SaveChanges();
            return RedirectToAction("_TagList");
        }
    }
}