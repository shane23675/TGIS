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
        //管理員查看某揪桌訊息(需指定是否為私人訊息)
        [CenterLogin(CenterLogin.UserType.Admin)]
        public ActionResult MessageIndex(string teamID, bool isPrivate)
        {
            Team t = db.Teams.Find(teamID);
            ViewBag.TeamTitle = t.Title;
            return View(t.Messages.Where(m => m.IsPrivate == isPrivate).ToList().OrderByDescending(m=>m.ID));
        }
    }
}