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

        public void Test()
        {
            Dictionary<int, string> d = new Dictionary<int, string>();
        }

        //玩家看到的桌遊百科(列表形式)
        public ActionResult ShowTableGameListForPlayer()
        {
            db.TableGameComments.Add(new TableGameComment { ID = 1, PlayerID = "TP0001", TableGameID = "T00005", CommentDate = new DateTime(2018, 6, 15), Content = "Nice", IsHidden = false });
            return View(db.TableGames.ToList());
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
        public ActionResult CreateTableGame(TableGame newTableGame, string[] selectedCategories, HttpPostedFileBase[] photos)
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
            PhotoManager.CreatePhoto(newTableGame.ID, photos);

            return RedirectToAction("ShowTableGameListForAdmin");
        }

        //編輯桌遊
        public ActionResult EditTableGame(string tableGameID)
        {
            ////傳入標籤資訊到ViewBag
            //ViewBag.difficultySelectList = UsefulTools.GetSelectListFromTags('D');
            //ViewBag.brandSelectList = UsefulTools.GetSelectListFromTags('B');
            ////將遊戲類別標籤製為List傳入ViewBag
            //ViewBag.categoryTagsList = db.Tags.Where(t => t.ID.Substring(0, 1) == "C").ToList();
            UpdateTableGamePreparaion();
            //將圖片的ID的List傳入ViewBag
            ViewBag.photoIDList = PhotoManager.GetPhotoIDList(tableGameID);
            return View(db.TableGames.Find(tableGameID));
        }
        [HttpPost]
        public ActionResult EditTableGame(TableGame tableGame, string[] selectedCategories, int[] deletedPhotoID, HttpPostedFileBase[] newPhoto)
        {
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
                    PhotoManager.DeletePhoto(id);
                }
            }
            //加入新圖片
            PhotoManager.CreatePhoto(tableGame.ID, newPhoto);

            return RedirectToAction("ShowTableGameListForAdmin");
        }

        //刪除桌遊
        public ActionResult DeleteTableGame(string tableGameID)
        {
            TableGame tg = db.TableGames.Find(tableGameID);
            //刪除對應的GameCategoryTags
            tg.GameCategoryTags.Clear();
            //刪除對應的圖片
            PhotoManager.DeletePhoto(tableGameID);
            //最後再刪除桌遊本身
            db.TableGames.Remove(tg);
            db.SaveChanges();
            return RedirectToAction("ShowTableGameListForAdmin");
        }
    }
}