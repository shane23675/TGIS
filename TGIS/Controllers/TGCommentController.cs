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
        // 玩家新增評論
        public ActionResult CreateTGComment(string tId,string comment)
        {
            if (Session["PlayerID"] != null)
            {
                if (ModelState.IsValid && comment.Trim() != "")
                {
                    var pId = Session["PlayerID"].ToString();
                    TableGameComment tgc = new TableGameComment();
                    tgc.PlayerID = pId;
                    tgc.TableGameID = tId;
                    tgc.CommentDate = DateTime.Today;
                    tgc.Content = comment;
                    tgc.IsHidden = false;
                    db.TableGameComments.Add(tgc);
                    UsefulTools.PointRecord((string)Session["PlayerID"],"評論桌遊",1);
                    Player player = db.Players.Find(pId);
                    player.Points += 1;
                    db.SaveChanges();
                }
                return RedirectToAction("ShowTableGameDetail", "TableGame", new { tableGameID = tId });
            }
            return RedirectToAction("LoginForPlayer","Login");
        }

        //玩家刪除評論
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

        //管理員查看評論列表
        public ActionResult TGCommentIndexForAdmin()
        {
            return View(db.TableGameComments.OrderByDescending(c => c.ID).ToList());
        }

        //管理員隱藏評論
        public ActionResult HideTGComment(int commentID)
        {
            db.TableGameComments.Find(commentID).IsHidden = true;
            db.SaveChanges();
            return RedirectToAction("TGCommentIndexForAdmin");
        }
    }
}