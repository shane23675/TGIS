using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGIS.Models;

namespace TGIS.Controllers
{
    public class PlayerController : Controller
    {
        TGISDBEntities db = new TGISDBEntities();
       
        //玩家頁面
        public ActionResult PlayerIndex()
        {
            return View(db.Players.ToList());
        }

        //創建玩家會員(註冊)
        public ActionResult PlayerCreate()
        {
            ViewBag.CityID = new SelectList(db.Cities, "ID", "CityName");
            ViewBag.DistrictID = new SelectList(db.Districts, "ID", "DistrictName");
            ViewBag.PlayerID = UsefulTools.GetNextID(db.Players, 2);
            return View();
        }
        [HttpPost]
        public ActionResult PlayerCreate(Player player, int CityID)
        {
            if (ModelState.IsValid)
            {
                db.Players.Add(player);
                db.SaveChanges();

                return RedirectToAction("EmailValidate", "EmailValidate",new {Email = player.Email,id=player.ID});
            }
            ViewBag.CityID = new SelectList(db.Cities, "ID", "CityName", CityID);
            ViewBag.DistrictID = new SelectList(db.Districts, "ID", "DistrictName", player.DistrictID);
            ViewBag.PlayerID = player.ID;
            return View(player);
        }
        //刪除玩家資料
        public ActionResult PlayerDelete(string playerID)
        {
            Player p  = db.Players.Find(playerID);
            db.Players.Remove(p);
            db.SaveChanges();

            return RedirectToAction("PlayerIndex");
        }
        //玩家詳細資料
        public ActionResult PlayerDetail(string playerID)
        {
            Player p = db.Players.Find(playerID);
            return View(p);
        }
        //玩家變更暱稱
        public ActionResult ChangeNickName(string playerID, string nickname)
        {
            Player p = db.Players.Find(playerID);
            p.NickName = nickname;
            db.SaveChanges();
            return Content(nickname);
        }
        //管理員編輯玩家
        public ActionResult PlayerEdit(string playerID)
        {
            ViewBag.DistrictID = new SelectList(db.Districts, "ID", "DistrictName");
            Player p  = db.Players.Find(playerID);
            return View(p);
        }
        [HttpPost]
        public ActionResult PlayerEdit(Player player)
        {
            if (ModelState.IsValid)
            {
                db.Entry(player).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("PlayerIndex");
            }
            return View(player);
        }
    }
}