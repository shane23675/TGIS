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
        [CenterLogin(CenterLogin.UserType.Admin)]
        public ActionResult CityIndex()
        {
            return View(db.Cities.ToList());
        }

        [HttpPost]
        [CenterLogin(CenterLogin.UserType.Admin)]
        public ActionResult CityCreate(string CityName)
        {
            var c = new City();
            c.CityName = CityName;
            db.Cities.Add(c);
            db.SaveChanges();
            return RedirectToAction("_CityList");
        }

        [CenterLogin(CenterLogin.UserType.Admin)]
        public ActionResult CityEdit(int CityID,string NewCityName)
        {
            db.Cities.Find(CityID).CityName= NewCityName;
            db.SaveChanges();
            return RedirectToAction("_CityList");
        }

        [CenterLogin(CenterLogin.UserType.Admin)]
        public ActionResult CityDelete(int CityID)
        {
            try
            {
                var c = db.Cities.Find(CityID);
                db.Cities.Remove(c);
                db.SaveChanges();
            }
            catch
            {
                return JavaScript("alert('此城市的相關資料尚未刪除，無法刪除此資料!!')");
            }
            return RedirectToAction("CityIndex");
        }

        public ActionResult _CityList()
        {
            return PartialView(db.Cities.ToList());
        }
    }
}