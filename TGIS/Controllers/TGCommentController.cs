using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGIS.Models;


namespace TGIS.Controllers
{
    public class TGCommentController : Controller
    {
        TGISDBEntities db = new TGISDBEntities();
        // GET: TGComment
        public ActionResult CreateTGComment(string tId,string comment)
        {
            var pId =Session["PlayerID"].ToString();
            if (ModelState.IsValid)
            {
                TableGameComment tgc = new TableGameComment();
                tgc.PlayerID = pId;
                tgc.TableGameID = tId;
                tgc.CommentDate = DateTime.Now;
                tgc.Content = comment;
                tgc.IsHidden = false;
                db.TableGameComments.Add(tgc);
                db.SaveChanges();

                return RedirectToAction("ShowTableGameDetail","TableGame",new { tableGameID = tId });
            }
            return View();
        }
    }
}