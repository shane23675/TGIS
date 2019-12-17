using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using TGIS.Models;

namespace TGIS.Controllers
{
    public class TableGameInShopDetailController : Controller
    {
        TGISDBEntities db = new TGISDBEntities();

        //更新TableGameInShopDetail
        [CenterLogin(CenterLogin.UserType.Shop)]
        public ActionResult UpdateTableGameInShopDetail()
        {
            return View(db.TableGames.ToList());
        }
        [HttpPost, CenterLogin(CenterLogin.UserType.Shop)]
        public ActionResult UpdateTableGameInShopDetail(string[] tableGameIDs, bool[] isContainedFlags, bool[] isSaleFlags, int[] Price)
        {
            string shopID = Session["ShopID"].ToString();
            TableGame tg;
            TableGameInShopDetail detail;
            //CheckBox陣列整理(o:舊陣列)
            Func<bool[], List<bool>> arrange = (o) =>
            {
                List<bool> n = new List<bool>();
                for (int i = 0; i < o.Length; i++)
                {
                    if (o[i] == true)
                    {
                        n.Add(true);
                        i++;
                    }
                    else
                        n.Add(false);
                }
                return n;
            };
             for (int i = 0; i < tableGameIDs.Length; i++)
            {
                //查找此店家是否有此桌遊
                tg = db.TableGames.Find(tableGameIDs[i]);
                detail = tg.TableGameInShopDetails.Where(m => m.ShopID == shopID).FirstOrDefault();
                //有此桌遊，進一步判斷此桌遊是否有被刪除
                if (detail != null)
                {
                    if (arrange(isContainedFlags)[i])
                    {
                        detail.IsSale = arrange(isSaleFlags)[i];
                        detail.Price = Price[i];
                    }
                    else
                    {
                        db.TableGameInShopDetails.Remove(detail);
                    }
                }
                //無此桌遊，若後來有被新增則新增至TableGameInShopDetails
                else if (arrange(isContainedFlags)[i])
                {
                    TableGameInShopDetail d = new TableGameInShopDetail
                    {
                        ShopID = shopID,
                        TableGameID = tableGameIDs[i],
                        IsSale = arrange(isSaleFlags)[i],
                        Price = Price[i]
                    };
                    db.TableGameInShopDetails.Add(d);
                }
                db.SaveChanges();
            }
            return RedirectToAction("UpdateTableGameInShopDetail");
        }

        //取得店內桌遊明細的partialView(店家編輯用)
        [ChildActionOnly, CenterLogin(CenterLogin.UserType.Shop)]
        public ActionResult _GetOneDetail(string tableGameID)
        {
            string shopID = Session["ShopID"].ToString();
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

        //查看店家內的所有桌遊
        public ActionResult _AllTableGamesInTheShop(string shopID)
        {
            Shop s = db.Shops.Find(shopID);
            return PartialView(s.TableGameInShopDetails.ToList());
        }

        //查詢店家內的所有桌遊(揪桌用，Ajax)
        //$.post("/TableGameInShopDetail/GetTableGameInShopJson", { shopID: $("#ShopID").val() }, function (data){})
        public ActionResult GetTableGameInShopJson(string shopID)
        {
            //取得該店家的所有店內桌遊明細
            var details = db.Shops.Find(shopID).TableGameInShopDetails.ToArray();
            //建構一個Json檔案的匿名類別容器
            var data = new[]
            {
                new { ChineseName = ""}
            }.ToList();
            data.Clear();
            //將明細中的資料填入data
            foreach(var d in details)
            {
                data.Add(new { ChineseName = d.TableGame.ChineseName });
            }
            return Json(data);
        }
    }
}