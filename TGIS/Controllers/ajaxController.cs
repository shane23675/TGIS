using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TGIS.Models;

namespace TGIS.Controllers
{

    public class ajaxController : Controller
    {
        TGISDBEntities db = new TGISDBEntities();
        
        // GET: District
        //連動式列表(行政區)
        public ActionResult generateStateList(int CId, int? Did)
        {
            StringBuilder sb = new StringBuilder();
            var c = db.Districts.Where(m => m.CityID == CId).ToList();
            foreach (var item in c)
            {
                if (Did == item.ID)
                {
                    sb.Append($"<option value='{item.ID}' Selected>{item.DistrictName}</option>");
                }
                else
                {
                    sb.Append($"<option value='{item.ID}'>{item.DistrictName}</option>");
                }
            }
            return Content(sb.ToString());
        }
        [HttpPost]
        public ActionResult PasswordChange(string oldpwd, string newpwd, string pwdrepeat,string userId)
        {
            if (newpwd == pwdrepeat)
            {
                if (userId.Substring(0, 1) == "S") 
                    {
                        Shop shop = db.Shops.Find(userId);
                        if (shop.Password == Hash.PwdHash(oldpwd))
                        {
                            shop.Password = Hash.PwdHash(newpwd);
                            db.SaveChanges();

                            return Content("2");
                        }
                        return Content("1");
                    }
                else if (userId.Substring(0, 1) == "T")
                {
                    Player player = db.Players.Find(userId);
                    if (player.Password == Hash.PwdHash(oldpwd))
                    {
                        player.Password = Hash.PwdHash(newpwd);
                        db.SaveChanges();

                        return Content("2");
                    }
                    return Content("1");
                }
                return Content("3");
            }
            return Content("0");
        }
    }
}