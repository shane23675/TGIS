﻿using System;
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

        //管理創建玩家會員
        public ActionResult PlayerCreate()
        {
            return View();
        }
        [HttpPost]
        public ActionResult PlayerCreate(Player player)
        {
            if (ModelState.IsValid)
            {
                db.Players.Add(player);
                db.SaveChanges();

                return RedirectToAction("PlayerIndex");
            }
            return View(player);
        }
        //刪除玩家資料
        public ActionResult PlayerDelete(string id)
        {
            var str = db.Players.Find(id);
            db.Players.Remove(str);
            db.SaveChanges();

            return RedirectToAction("PlayerIndex");
        }
        //玩家詳細資料
        public ActionResult PlayerDetail(string id)
        {
            Player p = db.Players.Find(id);
            return View(p);
        }
        //管理員編輯玩家
        public ActionResult PlayerEdit(string id)
        {
            ViewBag.DistrictID = new SelectList(db.Districts, "ID", "DistrictName");
            Player p  = db.Players.Find(id);
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