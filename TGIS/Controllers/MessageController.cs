using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGIS.Models;

namespace TGIS.Controllers
{
    public class MessageController : Controller
    {
        TGISDBEntities db = new TGISDBEntities();
        //管理員查看某揪桌訊息
        public ActionResult MessageIndex(string teamID)
        {
            Team t = db.Teams.Find(teamID);
            ViewBag.TeamTitle = t.Title;
            return View(t.Messages.ToList().OrderByDescending(m=>m.ID));
        }
    }
}