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
        //取得selectList的方法(keyword為要尋找的Tag的ID字首)
        List<SelectListItem> GetSelectList(char keyword)
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            foreach (Tag t in db.Tags)
            {
                if (t.ID[0] == keyword)
                    selectList.Add(new SelectListItem { Text = t.TagName, Value = t.ID });
            }
            return selectList;
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
            return View(db.TableGames.Find(tableGameID));
        }

        ////////////////////////////分隔線：此處以下是只有管理員或店家等編輯人員才能看到的內容///////////////////////////////
        ////////////////////////////分隔線：此處以下是只有管理員或店家等編輯人員才能看到的內容///////////////////////////////
        ////////////////////////////分隔線：此處以下是只有管理員或店家等編輯人員才能看到的內容///////////////////////////////

        //管理員看到的桌遊列表
        public ActionResult ShowTableGameListForAdmin()
        {
            return View(db.TableGames.ToList());
        }
        //新增桌遊
        public ActionResult CreateTableGame()
        {
            //傳入標籤資訊到ViewBag
            ViewBag.difficultySelectList = GetSelectList('D');
            ViewBag.brandSelectList = GetSelectList('B');
            //將遊戲類別標籤製為List傳入ViewBag
            ViewBag.categoryTagsList = db.Tags.Where(t => t.ID.Substring(0, 1) == "C").ToList();
            //傳入自動生成的ID
            ViewBag.tableGameID = UsefulTools.GetNextID(db.TableGames, 1);
            return View();
        }
        [HttpPost]
        public ActionResult CreateTableGame(TableGame newTableGame, string[] selectedCategories)
        {
            db.TableGames.Add(newTableGame);
            db.SaveChanges();
            foreach(string sc in selectedCategories)
            {
                newTableGame.GameCategoryTags.Add(db.Tags.Find(sc));
                db.SaveChanges();
            }
            return RedirectToAction("ShowTableGameListForAdmin");
        }

        //編輯桌遊
        public ActionResult EditTableGame(string tableGameID)
        {
            //傳入標籤資訊到ViewBag
            ViewBag.difficultySelectList = GetSelectList('D');
            ViewBag.brandSelectList = GetSelectList('B');
            //將遊戲類別標籤製為List傳入ViewBag
            ViewBag.categoryTagsList = db.Tags.Where(t => t.ID.Substring(0, 1) == "C").ToList();
            return View(db.TableGames.Find(tableGameID));
        }
        [HttpPost]
        public ActionResult EditTableGame(TableGame tableGame, string[] selectedCategories)
        {
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
            return RedirectToAction("ShowTableGameListForAdmin");
        }

        //刪除桌遊
        public ActionResult DeleteTableGame(string tableGameID)
        {
            TableGame tg = db.TableGames.Find(tableGameID);
            //刪除對應的GameCategoryTags
            tg.GameCategoryTags.Clear();
            db.TableGames.Remove(tg);
            db.SaveChanges();
            return RedirectToAction("ShowTableGameListForAdmin");
        }
    }
}