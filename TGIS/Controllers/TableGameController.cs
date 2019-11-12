using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGIS.Models;

namespace TGIS.Controllers
{
    public class TableGameController : Controller
    {
         TGISDBEntities db = new TGISDBEntities();

        //玩家看到的桌遊百科(列表形式)
        public ActionResult ShowTableGameListForPlayer()
        {
            ViewBag.DifficultyTagList = db.Tags.ToList().Where(m => m.ID[0] == 'D');
            ViewBag.CategoryTagList = db.Tags.ToList().Where(m => m.ID[0] == 'C');
            return View(db.TableGames.ToList());
        }
        [HttpPost]
        public ActionResult ShowTableGameListForPlayer(string difficultTagID, string[] categoryTagIDs)
        {
            //選出符合此難度標籤的桌遊
            TableGame[] tableGames = db.Tags.Find(difficultTagID).TableGamesForDifficulty.ToArray();
            //選出對應的所有分類標籤
            List<Tag> categoryTags = new List<Tag>();
            foreach (string id in categoryTagIDs)
            {
                categoryTags.Add(db.Tags.Find(id));
            }
            //目標桌遊的容器
            List<TableGame> targetTableGames = new List<TableGame>();
            //將含有categoryTags中任何一個標籤的桌遊加入targetTableGames
            foreach (TableGame item in tableGames)
            {
                foreach (Tag tag in categoryTags)
                {
                    if (item.GameCategoryTags.Contains(tag))
                    {
                        targetTableGames.Add(item);
                        break;
                    }
                }
            }
            //一樣的操作
            ViewBag.DifficultyTagList = db.Tags.ToList().Where(m => m.ID[0] == 'D');
            ViewBag.CategoryTagList = db.Tags.ToList().Where(m => m.ID[0] == 'C');
            return View(targetTableGames);
        }

        //顯示單個桌遊詳細內容
        public ActionResult ShowTableGameDetail(string tableGameID)
        {
            //將此桌遊的相關連結傳入ViewBag
            ViewBag.relevantLinks = db.RelevantLinks.Where(m => m.TableGameID == tableGameID).ToList();
            //將此桌遊的圖片數量傳入ViewBag
            ViewBag.photoIDList = PhotoManager.GetPhotoIDList(tableGameID);
            return View(db.TableGames.Find(tableGameID));
        }

        ////////////////////////////分隔線：此處以下是只有管理員或店家等編輯人員才能看到的內容///////////////////////////////
        ////////////////////////////分隔線：此處以下是只有管理員或店家等編輯人員才能看到的內容///////////////////////////////
        ////////////////////////////分隔線：此處以下是只有管理員或店家等編輯人員才能看到的內容///////////////////////////////
        //更新前的準備
        private void UpdateTableGamePreparaion()
        {
            //傳入標籤資訊到ViewBag
            ViewBag.difficultySelectList = UsefulTools.GetSelectListFromTags('D');
            ViewBag.brandSelectList = UsefulTools.GetSelectListFromTags('B');
            //將遊戲類別標籤製為List傳入ViewBag
            ViewBag.categoryTagsList = db.Tags.Where(t => t.ID.Substring(0, 1) == "C").ToList();
        }

        //管理員看到的桌遊列表
        public ActionResult ShowTableGameListForAdmin()
        {
            return View(db.TableGames.ToList());
        }
        //新增桌遊
        public ActionResult CreateTableGame()
        {
            UpdateTableGamePreparaion();
            //傳入自動生成的ID
            ViewBag.tableGameID = UsefulTools.GetNextID(db.TableGames, 1);
            return View();
        }
        [HttpPost]
        public ActionResult CreateTableGame(TableGame newTableGame, string[] selectedCategories, HttpPostedFileBase[] photos, string[] links)
        {
            //無法通過驗證則顯示錯誤訊息
            if (!ModelState.IsValid)
            {
                UpdateTableGamePreparaion();
                //傳入自動生成的ID
                ViewBag.tableGameID = UsefulTools.GetNextID(db.TableGames, 1);
                return View();
            }
            //儲存newTableGame
            db.TableGames.Add(newTableGame);
            db.SaveChanges();
            foreach(string sc in selectedCategories)
            {
                newTableGame.GameCategoryTags.Add(db.Tags.Find(sc));
                db.SaveChanges();
            }
            //調用PhotoManager中的方法來儲存傳入的圖片
            PhotoManager.Create(newTableGame.ID, photos);
            //儲存相關教學連結
            RelevantLinkManager.Create(newTableGame.ID, links);

            return RedirectToAction("ShowTableGameListForAdmin");
        }

        //編輯桌遊
        public ActionResult EditTableGame(string tableGameID)
        {
            UpdateTableGamePreparaion();
            //將圖片的ID的List傳入ViewBag
            ViewBag.photoIDList = PhotoManager.GetPhotoIDList(tableGameID);
            return View(db.TableGames.Find(tableGameID));
        }
        [HttpPost]
        public ActionResult EditTableGame(TableGame tableGame, string[] selectedCategories, 
            int[] deletedPhotoID, HttpPostedFileBase[] newPhoto, int[] deletedLinkIDs, string[] links)
        {
            //無法通過驗證則顯示錯誤訊息
            if (!ModelState.IsValid)
            {
                UpdateTableGamePreparaion();
                //將圖片的ID的List傳入ViewBag
                ViewBag.photoIDList = PhotoManager.GetPhotoIDList(tableGame.ID);
                return View(db.TableGames.Find(tableGame.ID));
            }
            //通過ID找到舊的tableGame資料
            TableGame oldTableGame = db.TableGames.Find(tableGame.ID);
            //使用自訂方法更新
            UsefulTools.Update(oldTableGame, tableGame);
            db.SaveChanges();

            //通過selectedCategories解析出對應的Tags並加入newTagList
            List<Tag> newTagList = new List<Tag>();
            foreach(string sc in selectedCategories)
            {
                Tag t = db.Tags.Find(sc);
                newTagList.Add(t);
            }
            //全部的GameCategoryTag
            List<Tag> allTagList = db.Tags.Where(t => t.ID.Substring(0, 1) == "C").ToList();
            //遍歷所有GameCategoryTag
            foreach (Tag t in allTagList)
            {
                if (oldTableGame.GameCategoryTags.Contains(t))
                {
                    //這個GameCategoryTag有在這個桌遊的GameCategoryTags之中
                    //進一步判斷是否有在newTagList中，如果沒有就從這個桌遊的GameCategoryTags刪除
                    if (!newTagList.Contains(t))
                    {
                        oldTableGame.GameCategoryTags.Remove(t);
                        db.SaveChanges();
                    }
                }
                else
                {
                    //這個GameCategoryTag不在這個桌遊的GameCategoryTags之中
                    //進一步判斷是否有在newTagList中，如果有就加入這個桌遊的GameCategoryTags
                    if (newTagList.Contains(t))
                    {
                        oldTableGame.GameCategoryTags.Add(t);
                        db.SaveChanges();
                    }
                }
            }

            //刪除被勾選的圖片
            if (deletedPhotoID != null)
            {
                foreach (int id in deletedPhotoID)
                {
                    PhotoManager.Delete(id);
                }
            }
            //加入新圖片
            PhotoManager.Create(tableGame.ID, newPhoto);
            //刪除連結
            if (deletedLinkIDs != null)
                RelevantLinkManager.Delete(deletedLinkIDs);
            //加入新連結
            RelevantLinkManager.Create(tableGame.ID, links);

            return RedirectToAction("ShowTableGameListForAdmin");
        }

        //刪除桌遊
        public ActionResult DeleteTableGame(string tableGameID)
        {
            TableGame tg = db.TableGames.Find(tableGameID);
            //刪除對應的GameCategoryTags
            tg.GameCategoryTags.Clear();
            //刪除對應的圖片
            PhotoManager.Delete(tableGameID);
            //刪除相關教學連結
            db.RelevantLinks.Where(m => m.TableGameID == tableGameID).ToList().ForEach(m => db.RelevantLinks.Remove(m));
            //刪除店內桌遊明細
            List<TableGameInShopDetail> details = db.TableGameInShopDetails.Where(m => m.TableGameID == tableGameID).ToList();
            details.ForEach(m => db.TableGameInShopDetails.Remove(m));
            //最後再刪除桌遊本身
            db.TableGames.Remove(tg);
            db.SaveChanges();
            return RedirectToAction("ShowTableGameListForAdmin");
        }
    }
}