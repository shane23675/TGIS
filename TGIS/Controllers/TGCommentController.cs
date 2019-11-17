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
            if (Session["PlayerID"] != null)
            {
                if (ModelState.IsValid)
                    {
                    var pId = Session["PlayerID"].ToString();
                    TableGameComment tgc = new TableGameComment();
                    tgc.PlayerID = pId;
                    tgc.TableGameID = tId;
                    tgc.CommentDate = DateTime.Now;
                    tgc.Content = comment.Replace("\n", "<br />");
                    tgc.IsHidden = false;
                    db.TableGameComments.Add(tgc);
                    Player player = db.Players.Where(m => m.ID == pId).SingleOrDefault();
                    player.Points += 1;
                    db.SaveChanges();
                    }
                return RedirectToAction("ShowTableGameDetail", "TableGame", new { tableGameID = tId });
            }
            return RedirectToAction("LoginForPlayer","Login");
        }
    }
}