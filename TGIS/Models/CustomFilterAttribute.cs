using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;


//這裡放的是各種自定義Filter
namespace TGIS.Models
{
    //玩家中心的登入檢查
    public class CenterLogin : ActionFilterAttribute
    {
        private UserType _userType;
        //導向網址預設為首頁
        private string _redirectURL = "/Home/Index";
        public CenterLogin(UserType t)
        {
            _userType = t;
        }

        //主要的檢查程序
        void LoginCheck(HttpContext context)
        {
            //通過_userType判斷要檢查的目標
            string target = "";
            switch (_userType)
            {
                case UserType.Player:
                    target = "PlayerID";
                    break;
                case UserType.Shop:
                    target = "ShopID";
                    break;
                case UserType.Admin:
                    target = "AdminID";
                    _redirectURL = "/LoginForAdmin/LoginForAdmin";
                    break;
            }
            if (context.Session[target] == null)
                context.Response.Redirect(_redirectURL);
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            LoginCheck(HttpContext.Current);
        }

        //判斷使用者類型的列舉
        public enum UserType
        {
            Shop, Player, Admin
        }
    }

    //VIP權限檢查
    public class VIPOnly : ActionFilterAttribute
    {
        TGISDBEntities db = new TGISDBEntities();
        //主檢查程序
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var context = HttpContext.Current;
            Shop shop = db.Shops.Find((string)context.Session["ShopID"]);
            //尚未登入則跳過
            if (shop == null)
                return;

            if (!shop.IsVIP)
                context.Response.Redirect("/Shop/NoVIPHint");
        }
    }
}