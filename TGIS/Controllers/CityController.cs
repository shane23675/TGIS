using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGIS.Models;

namespace TGIS.Controllers
{
    public class CityController : Controller
    {
        TGISDBEntities db = new TGISDBEntities();
        // GET: City
        public ActionResult CityIndex()
        {
            return View(db.Cities.ToList());
        }

        [HttpPost]
        public ActionResult CityCreate(string CityName)
        {
            var c = new City();
            c.CityName = CityName;
            db.Cities.Add(c);
            db.SaveChanges();
            return RedirectToAction("_CityList");
        }

        public ActionResult CityEdit(int CityID,string NewCityName)
        {
            db.Cities.Find(CityID).CityName= NewCityName;
            db.SaveChanges();
            return RedirectToAction("_CityList");
        }

        public ActionResult CityDelete(int CityID)
        {
            var c = db.Cities.Find(CityID);
            db.Cities.Remove(c);
            db.SaveChanges();
            return RedirectToAction("CityIndex");
        }

        public ActionResult _CityList()
        {
            return PartialView(db.Cities.ToList());
        }
    }
}