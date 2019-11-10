using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGIS.Models;

namespace TGIS.Controllers
{
    public class AdminController : Controller
    {
        TGISDBEntities db = new TGISDBEntities();
        // GET: Admin
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Administrator administrator)
        {
            administrator.Password = Hash.PwdHash(administrator.Password);
            if (ModelState.IsValid)
            {
                db.Administrators.Add(administrator);
                db.SaveChanges();
                return RedirectToAction("ShopIndex","Shop");
            }
            return View (administrator);
        }
    }
}