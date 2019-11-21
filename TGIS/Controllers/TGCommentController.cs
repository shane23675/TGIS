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
                    tgc.CommentDate = DateTime.Today;
                    tgc.Content = comment;
                    tgc.IsHidden = false;
                    db.TableGameComments.Add(tgc);
                    PlayerPointDetail ppd = new PlayerPointDetail();
                    ppd.PlayerID = pId;
                    ppd.Cause = "評論桌遊";
                    ppd.ChangedAmount = 1;
                    ppd.ChangedDate = DateTime.Now;
                    db.PlayerPointDetails.Add(ppd);
                    Player player = db.Players.Find(pId);
                    player.Points += 1;
                    db.SaveChanges();
                }
                return RedirectToAction("ShowTableGameDetail", "TableGame", new { tableGameID = tId });
            }
            return RedirectToAction("LoginForPlayer","Login");
        }
        public ActionResult CommentDelete(int commentID)
        {
            var cmt = db.TableGameComments.Find(commentID);
            var tld = cmt.TableGameID;
            if (cmt != null)
            {
                db.TableGameComments.Remove(cmt);
                db.SaveChanges();
                return RedirectToAction("ShowTableGameDetail", "TableGame", new { tableGameID = tld });
            }
            return Content("查無此評論");
        }
    }
}