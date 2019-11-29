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

        //創建玩家會員(註冊)
        public ActionResult PlayerCreate()
        {
            ViewBag.CityID = new SelectList(db.Cities, "ID", "CityName");
            ViewBag.DistrictID = new SelectList(db.Districts, "ID", "DistrictName");
            ViewBag.PlayerID = UsefulTools.GetNextID(db.Players, 2);
            return View();
        }
        [HttpPost]
        public ActionResult PlayerCreate(Player player, string passwordConfirm, int CityID, HttpPostedFileBase photo)
        {
            //檢查確認密碼是否輸入正確
            if (passwordConfirm != player.Password)
            {
                ModelState["Password"].Errors.Add("輸入的密碼和確認密碼不相符");
            }
            //檢查帳號及密碼是否符合規則
            UsefulTools.RegisterValidate(player.Account, ModelState["Account"].Errors.Add, false, false);
            UsefulTools.RegisterValidate(player.Password, ModelState["Password"].Errors.Add, true, false);

            if (ModelState.IsValid)
            {
                db.Players.Add(player);
                db.SaveChanges();
                //儲存圖片(先將photo變為陣列再傳入)
                PhotoManager.Create(player.ID, new HttpPostedFileBase[] { photo });

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
        //玩家變更暱稱(Ajax)
        public ActionResult ChangeNickName(string playerID, string nickname)
        {
            Player p = db.Players.Find(playerID);
            p.NickName = nickname;
            db.SaveChanges();
            return Content(nickname);
        }
        //玩家變更圖片
        /*
            --玩家點擊「變更圖片」並選擇圖片
            --出現「上傳並變更」按鈕
            --點選「上傳並變更」，將圖片送至控制器處理
            --依據玩家ID刪除原來的圖片並新增新圖片
            --重新導向至PlayerDetail
        */
        public ActionResult ChangePlayerPhoto(HttpPostedFileBase[] photo)
        {
            string playerID = (string)Session["PlayerID"];
            //若登入時間已過則跳轉至登入頁
            if (playerID == null)
                return RedirectToAction("LoginForPlayer", "Login");
            if (photo.Length != 0)
            {
                //先刪除原先圖片再新增圖片
                PhotoManager.Delete(playerID);
                PhotoManager.Create(playerID, photo);
            }
            return RedirectToAction("PlayerDetail", new { playerID });
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