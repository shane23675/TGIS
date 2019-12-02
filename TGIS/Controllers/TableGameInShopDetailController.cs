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
        //更新TableGameInShopDetail
        public ActionResult UpdateTableGameInShopDetail(string shopID)
        {
            ViewBag.shopID = shopID;
            return View(db.TableGames.ToList());
        }
        [HttpPost]
        public ActionResult UpdateTableGameInShopDetail(string shopID, string[] tableGameIDs, bool[] isContainedFlags, bool[] isSaleFlags, int[] Price)
        {
            TableGame tg;
            TableGameInShopDetail detail;
            //依據索引值逐個檢查每個桌遊
            for (int i = 0; i < tableGameIDs.Length; i++)
            {
                //查找此店家是否有此桌遊
                tg = db.TableGames.Find(tableGameIDs[i]);
                detail = tg.TableGameInShopDetails.Where(m => m.ShopID == shopID).FirstOrDefault();
                //有此桌遊，進一步判斷此桌遊是否有被刪除
                if (detail != null)
                {
                    if (isContainedFlags[i])
                    {
                        detail.IsSale = isSaleFlags[i];
                        detail.Price = Price[i];
                    }
                    else
                    {
                        db.TableGameInShopDetails.Remove(detail);
                    }
                }
                //無此桌遊，若後來有被新增則新增至TableGameInShopDetails
                else if (isContainedFlags[i])
                {
                    TableGameInShopDetail d = new TableGameInShopDetail
                    {
                        ShopID = shopID,
                        TableGameID = tableGameIDs[i],
                        IsSale = isSaleFlags[i],
                        Price = Price[i]
                    };
                    db.TableGameInShopDetails.Add(d);
                }
                db.SaveChanges();
            }
            return RedirectToAction("UpdateTableGameInShopDetail", new { shopID = shopID });
        }

        //取得TableGameInShopDetail的partialView
        [ChildActionOnly]
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

        //以桌遊名稱查詢店內桌遊(Ajax)
        public ActionResult SearchTableGameInShopByName(string shopID, string name)
        {
            //建立一匿名型別的集合來準備製成JSON檔案
            var data = new[]
            {
                new
                {
                    ChineseName = "",
                    EnglishName = "",
                    IsContained = ""
                }
            }.ToList();
            data.RemoveAt(0);

            //先找到可能的桌遊
            TableGame[] games = db.TableGames.Where(m => m.ChineseName.Contains(name)).ToArray();
            //中文找不到再找英文
            if (games.Length == 0)
                games = db.TableGames.Where(m => m.EnglishName.Contains(name)).ToArray();
            if (games.Length == 0)
            {
                data.Add(new { ChineseName = "找不到相符的項目", EnglishName = "No Possible Matches", IsContained = "" });
                return Json(data.ToList());
            }

            //填入資料
            foreach (var game in games)
            {
                data.Add(new
                {
                    ChineseName = game.ChineseName,
                    EnglishName = game.EnglishName,
                    IsContained = game.TableGameInShopDetails.Any(m=>m.ShopID == shopID) ? "有" : "無"
                });
            }
            return Json(data.ToList());
        }
    }
}