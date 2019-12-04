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
       
        //創建玩家會員(註冊)
        public ActionResult PlayerCreate()
        {
            ViewBag.CityID = new SelectList(db.Cities, "ID", "CityName");
            ViewBag.DistrictID = new SelectList(db.Districts, "ID", "DistrictName");
            return View();
        }
        [HttpPost]
        public ActionResult PlayerCreate(Player player, string passwordConfirm, int CityID, HttpPostedFileBase photo)
        {
            //檢查確認密碼是否輸入正確
            if (passwordConfirm != player.Password)
                ModelState["Password"].Errors.Add("輸入的密碼和確認密碼不相符");
            //檢查帳號是否重複
            if (db.Players.Any(m=>m.Account == player.Account))
                ModelState["Account"].Errors.Add("此帳號已經有人使用");
            //檢查帳號及密碼是否符合規則
            UsefulTools.RegisterValidate(player.Account, ModelState["Account"].Errors.Add, false, false);
            UsefulTools.RegisterValidate(player.Password, ModelState["Password"].Errors.Add, true, false);
            //填入預設值
            player.ID = UsefulTools.GetNextID(db.Players, 2);
            player.Points = 0;
            player.IsBanned = false;
            player.IsEmailValid = false;

            if (ModelState.IsValid)
            {
                player.Password = Hash.PwdHash(player.Password);
                db.Players.Add(player);
                db.SaveChanges();
                //儲存圖片(先將photo變為陣列再傳入)
                PhotoManager.Create(player.ID, new HttpPostedFileBase[] { photo });

                return RedirectToAction("EmailValidate", "EmailValidate",new {Email = player.Email,id=player.ID});
            }
            ViewBag.CityID = new SelectList(db.Cities, "ID", "CityName", CityID);
            ViewBag.DistrictID = new SelectList(db.Districts, "ID", "DistrictName", player.DistrictID);
            return View(player);
        }
        //玩家詳細資料(玩家會員中心)
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

        //管理員查看玩家列表
        public ActionResult PlayerIndex()
        {
            return View(db.Players.ToList());
        }

        //管理員編輯玩家
        public ActionResult PlayerEdit(string playerID)
        {
            Player p = db.Players.Find(playerID);
            TempData["Player"] = p;
            ViewBag.CityID = new SelectList(db.Cities, "ID", "CityName", p.District.CityID);
            ViewBag.DistrictID = new SelectList(db.Districts, "ID", "DistrictName", p.DistrictID);
            return View(p);
        }
        [HttpPost]
        public ActionResult PlayerEdit(Player player, int CityID, int DistrictID)
        {
            //從TempData中取出原始資料並存入
            Player p = (Player)TempData["Player"];
            player.ID = p.ID;
            player.Account = p.Account;
            player.Password = p.Password;
            player.NickName = p.NickName;
            //將帳號相關檢查移除以免觸發錯誤
            List<string> exceptions = new List<string>{ "Account", "Password", "NickName" };
            exceptions.ForEach(m => ModelState[m].Errors.Clear());

            if (ModelState.IsValid)
            {
                db.Entry(player).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("PlayerIndex");
            }
            ViewBag.CityID = new SelectList(db.Cities, "ID", "CityName", CityID);
            ViewBag.DistrictID = new SelectList(db.Districts, "ID", "DistrictName", DistrictID);
            TempData.Keep("Player_ID");
            return View(player);
        }
        //管理員刪除玩家資料
        public ActionResult PlayerDelete(string playerID)
        {
            //Player p = db.Players.Find(playerID);
            //db.Players.Remove(p);
            //db.SaveChanges();
            //return RedirectToAction("PlayerIndex");
            //尚未完成
            return HttpNotFound();
        }
        public ActionResult PointRecord(string playerId)
        {
            var ppd = db.PlayerPointDetails.Where(m => m.PlayerID == playerId).ToList();
            return View(ppd);
        }

    }
}