using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGIS.Models;

namespace TGIS.Controllers
{
    public class HomeController : Controller
    {
        TGISDBEntities db = new TGISDBEntities();
        //首頁
        public ActionResult Index()
        {
            //找到付費店家
            var shops = db.Shops.Where(s => s.IsVIP).ToList();
            //找到其中的優惠券及活動
            List<Coupon> coupons = new List<Coupon>();
            shops.ForEach(s => coupons.AddRange(s.Coupons));
            List<NormalOffer> offers = new List<NormalOffer>();
            shops.ForEach(s => offers.AddRange(s.NormalOffers));
            //找到其中包含圖片者且未過期或被隱藏者
            coupons = coupons.Where(c => db.Photos.Any(p => p.SourceID == c.ID) && c.IsExchangable).ToList();
            offers = offers.Where(o => db.Photos.Any(p => p.SourceID == o.ID) && o.IsAvailable).ToList();
            //傳入ViewBag
            ViewBag.Coupons = coupons;
            ViewBag.Offers = offers;
            return View();
        }

        //玩家取得訊息框(PartialView)
        public ActionResult TeamChatBox()
        {
            string playerID = (string)Session["PlayerID"];
            //如果尚未登入則顯示錯誤訊息
            if (playerID == null)
                return new EmptyResult();

            //取得此登入玩家的資訊
            Player player = db.Players.Find(playerID);
            ViewBag.UserID = player.ID;

            //取得此登入玩家所有尚未到達遊玩時間的揪桌的編號
            List<string> teamIDs = new List<string>();
            List<bool> isTeamPrivateFlags = new List<bool>();
            foreach(Team t in player.TeamsForLeader.Where(t => t.PlayDate >= DateTime.Today).ToList())
            {
                teamIDs.Add(t.ID);
                teamIDs.Add(t.ID);
                isTeamPrivateFlags.Add(false);
                isTeamPrivateFlags.Add(true);
            }
            foreach(Team t in player.TeamsForOtherPlayer.Where(t => t.PlayDate >= DateTime.Today).ToList())
            {
                teamIDs.Add(t.ID);
                isTeamPrivateFlags.Add(false);
            }

            ViewBag.TeamIDs = teamIDs;
            ViewBag.IsTeamPrivateFlags = isTeamPrivateFlags;
            return PartialView();
        }

        //店家取得訊息框(PartialView，和玩家訊息框的邏輯相同)
        public ActionResult TeamChatBoxForShop()
        {
            string shopID = (string)Session["ShopID"];
            if (shopID == null)
                return new EmptyResult();

            ViewBag.UserID = shopID;

            Shop shop = db.Shops.Find(shopID);
            List<string> teamIDs = new List<string>();
            List<bool> isTeamPrivateFlags = new List<bool>();
            foreach(Team t in shop.Teams.Where(t => t.PlayDate > DateTime.Today))
            {
                teamIDs.Add(t.ID);
                isTeamPrivateFlags.Add(true);
            }
            ViewBag.TeamIDs = teamIDs;
            ViewBag.IsTeamPrivateFlags = isTeamPrivateFlags;
            //跟玩家使用同一個PartialView
            return PartialView("TeamChatBox");
        }

    }
}