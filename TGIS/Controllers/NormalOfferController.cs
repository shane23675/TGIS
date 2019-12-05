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
        // GET: NormalOffer
        public ActionResult OfferList()
        {
            return View(db.NormalOffers.ToList());
        }
        public ActionResult OfferCreate()
        {
            return View();
        }
        [HttpPost]
        public ActionResult OfferCreate(NormalOffer normalOffer)
        {
            if (ModelState.IsValid)
            {
                db.NormalOffers.Add(normalOffer);
                db.SaveChanges();
                return View();
            }
            return View();
        }
        public ActionResult OfferEdit(string id)
        {
            return View(db.NormalOffers.Find(id));
        }
        [HttpPost]
        public ActionResult OfferEdit(NormalOffer normalOffer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(normalOffer).State = EntityState.Modified;
                db.SaveChanges();
                return View();
            }
            return View();
        }
        public ActionResult OfferDel(string id)
        {
            var offer = db.NormalOffers.Find(id);
            db.NormalOffers.Remove(offer);
            db.SaveChanges();
            return View();
        }
    }
}