using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGIS.Models;

namespace TGIS.Controllers
{
    public class TableGameInShopDetailController : Controller
    {
        TGISDBEntities db = new TGISDBEntities();
        public ActionResult UpdateTableGameInShopDetail(string shopID)
        {
            ViewBag.shopID = shopID;
            return View(db.TableGames.ToList());
        }

        //取得TableGameInShopDetail的partialView
        public ActionResult _GetOneDetail(string tableGameID, string shopID)
        {
            TableGame tg = db.TableGames.Find(tableGameID);
            //從傳入的桌遊，找到店家ID為shopID的店內桌遊明細
            TableGameInShopDetail detail = tg.TableGameInShopDetails.Where(m => m.ShopID == shopID).FirstOrDefault();
            //如果有找到則作為Model，沒找到則傳入新鑄造的TableGameInShopDetail
            if (detail != null)
            {
                ViewBag.isContained = true;
                return PartialView(detail);
            }
            else
            {
                ViewBag.isContained = false;
                return PartialView(new TableGameInShopDetail { TableGameID = tableGameID, TableGame=tg ,ShopID = shopID, IsSale = false, Price=0 });
            }
        }
    }
}