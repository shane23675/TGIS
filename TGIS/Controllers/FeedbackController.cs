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
        // GET: ReturnSystem
        public ActionResult ReturnProblem()
        {
            ViewBag.TypeTagID = new SelectList(db.Tags, "ID", "TagName").Where(m=>m.Value.Substring(0,1)=="R");
            return View();
        }
        [HttpPost]
        public ActionResult ReturnProblem(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                feedback.ReceivedDate = DateTime.Now;
                db.Feedbacks.Add(feedback);
                db.SaveChanges();
                return RedirectToAction("ReturnList");
            }
            ViewBag.TypeTagID = new SelectList(db.Tags, "ID", "TagName").Where(m => m.Value.Substring(0, 1) == "R");
            return View(feedback);
        }
        public ActionResult ReturnList()
        {
            return View(db.Feedbacks.ToList());
        }
        public ActionResult ReturnDel(int id)
        {
            var fd = db.Feedbacks.Find(id);
            db.Feedbacks.Remove(fd);
            db.SaveChanges();
            return RedirectToAction("ReturnList");
        }
        public ActionResult ReturnDetail(int id)
        {
            return View(db.Feedbacks.Find(id));
        }
    }
}