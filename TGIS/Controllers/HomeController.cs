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

        //測試區(請勿刪除)
        public ActionResult TeamChatBox()
        {
            string playerID = (string)Session["PlayerID"];
            //如果尚未登入則顯示錯誤訊息
            if (playerID == null)
                return new EmptyResult();

            //取得此登入玩家的資訊
            Player player = db.Players.Find(playerID);
            ViewBag.Player = player;

            //取得此登入玩家所有揪桌的編號
            List<string> teamIDs = new List<string>();
            player.TeamsForLeader.ToList().ForEach(t => teamIDs.Add(t.ID));
            player.TeamsForOtherPlayer.ToList().ForEach(t => teamIDs.Add(t.ID));
            ViewBag.TeamIDs = teamIDs;
            return PartialView();
        }

    }
}