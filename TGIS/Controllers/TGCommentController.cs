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
            if (Session["Player"] != null)
            {
                if (ModelState.IsValid)
                {
                    var pId = ((Player)Session["Player"]).ID.ToString();
                    TableGameComment tgc = new TableGameComment();
                    tgc.PlayerID = pId;
                    tgc.TableGameID = tId;
                    tgc.CommentDate = DateTime.Today;
                    tgc.Content = comment;
                    tgc.IsHidden = false;
                    db.TableGameComments.Add(tgc);
                    Player player = db.Players.Find(pId);
                    player.Points += 1;
                    db.SaveChanges();
                }
                return RedirectToAction("ShowTableGameDetail", "TableGame", new { tableGameID = tId });
            }
            return RedirectToAction("LoginForPlayer","Login");
        }
    }
}