﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGIS.Models;

namespace TGIS.Controllers
{
    public class TeamController : Controller
    {
        TGISDBEntities db = new TGISDBEntities();
        // 揪桌列表主頁
        public ActionResult TeamIndexForPlayer()
        {
            return View(db.Teams.ToList());
        }

        //揪桌詳細內容(玩家用)
        public ActionResult TeamDetailForPlayer(string teamID)
        {
            return View(db.Teams.Find(teamID));
        }

        //新開一桌(玩家用)
        public ActionResult TeamCreate()
        {
            //若尚未登入則重新導向至登入頁
            if (Session["PlayerID"] == null)
                return RedirectToAction("LoginForPlayer", "Login");

            ViewBag.teamID = UsefulTools.GetNextID(db.Teams, 1);
            ViewBag.CityID = new SelectList(db.Cities, "ID", "CityName");
            ViewBag.DistrictID = new SelectList(db.Districts, "ID", "DistrictName");
            //這裡不傳PlayerID，直接在View中通過Session取得
            return View();
        }
        [HttpPost]
        public ActionResult TeamCreate(Team team)
        {
            //檢查輸入的時間訊息是否有效的函數，否則加入錯誤訊息並返回
            bool isInputValid()
            {
                bool flag = true;
                //檢查遊戲日期是否在報名截止日期之後
                if (team.PlayDate <= team.ParticipateEndDate)
                {
                    ModelState["PlayDate"].Errors.Add("遊戲日期必須在報名截止日期之後");
                    flag =  false;
                }
                //檢查結束時間是否在開始時間之後
                if (team.PlayBeginTime >= team.PlayEndTime)
                {
                    ModelState["PlayEndTime"].Errors.Add("結束時間必須在開始時間之後");
                    flag = false;
                }
                //檢查最高人數是否不小於最低人數
                if (team.MaxPlayer < team.MinPlayer)
                {
                    ModelState["MaxPlayer"].Errors.Add("最高人數不得小於最低人數");
                    flag = false;
                }
                return flag;
            }
            //驗證主體
            if (ModelState.IsValid && isInputValid())
            {
                db.Teams.Add(team);
                db.SaveChanges();
                return RedirectToAction("TeamIndexForPlayer");
            }
            //驗證失敗
            ViewBag.teamID = team.ID;
            ViewBag.CityID = new SelectList(db.Cities, "ID", "CityName");
            ViewBag.DistrictID = new SelectList(db.Districts, "ID", "DistrictName");
            return View(team);
        }
    }
}