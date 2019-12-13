using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGIS.Models;

namespace TGIS.Controllers
{
    public class DistrictController : Controller
    {
        TGISDBEntities db = new TGISDBEntities();
        // GET: District
        public ActionResult DistrictIndex(int CityID)
        {
            ViewBag.CityID = CityID;
            ViewBag.CityName = db.Cities.Find(CityID).CityName;
            return View(db.Districts.Where(m=>m.CityID==CityID).ToList());
        }

        public ActionResult DistrictCreate(string DistrictName,int CityID)
        {
            var d = new District();
            d.DistrictName = DistrictName;
            d.CityID = CityID;
            db.Districts.Add(d);
            db.SaveChanges();
            return RedirectToAction("_DistrictList",new { CityID= CityID });
        }

        public ActionResult DistrictEdit(int CityID, string NewDistrictName,int DistrictID)
        {
            db.Districts.Find(DistrictID).DistrictName = NewDistrictName;
            db.SaveChanges();
            return RedirectToAction("_DistrictList", new { CityID = CityID });
        }

        public ActionResult DistrictDelete(int DistrictID,int CityID) {
            var d = db.Districts.Find(DistrictID);
            db.Districts.Remove(d);
            db.SaveChanges();
            return RedirectToAction("DistrictIndex", new { CityID = CityID });
        }

        public ActionResult _DistrictList(int CityID) {
            return PartialView(db.Districts.Where(m => m.CityID == CityID).ToList());
        }
    }
}