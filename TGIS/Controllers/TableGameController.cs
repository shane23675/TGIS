using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGIS.Models;
using PagedList;
using PagedList.Mvc;

namespace TGIS.Controllers
{
    public class TableGameController : Controller
    {
         TGISDBEntities db = new TGISDBEntities();

        //玩家看到的桌遊百科(列表形式)
        public ActionResult ShowTableGameListForPlayer(int page = 1)
        {
            ViewBag.DifficultyTagList = db.Tags.ToList().Where(m => m.ID[0] == 'D');
            ViewBag.CategoryTagList = db.Tags.ToList().Where(m => m.ID[0] == 'C');
            ViewBag.BrandTagList = db.Tags.ToList().Where(m => m.ID[0] == 'B');
            ViewBag.AverageGamePeroid = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Value = "30", Text = "30分鐘以內"},
                new SelectListItem { Value = "60", Text = "30分鐘 ~ 1小時"},
                new SelectListItem { Value = "90", Text = "1小時 ~ 1.5小時"},
                new SelectListItem { Value = "120", Text = "1.5小時以上"}
            }, "Value", "Text");
            ViewBag.difficultTagIDs = new string[0];
            ViewBag.categoryTagIDs = new string[0];
            ViewBag.brandTagIDs = new string[0];
            ViewBag.IsFilterOn = false;
            return View(db.TableGames.OrderBy(g => g.ID).ToPagedList(page, 12));
        }
        [HttpPost]
        public ActionResult ShowTableGameListForPlayer(int? minPlayer, int? maxPlayer, short? AverageGamePeroid, string[] difficultTagIDs, string[] categoryTagIDs, string[] brandTagIDs, int page = 1, bool notExtended = false)
        {
            //進行篩選
            var games = db.TableGames.ToList();
            if (minPlayer != null)
                games = games.Where(g => g.MinPlayer <= minPlayer).ToList();
            if (maxPlayer != null)
                games = games.Where(g => g.MaxPlayer >= maxPlayer).ToList();
            if (difficultTagIDs != null)
                games = games.Where(g => difficultTagIDs.Contains(g.DifficultyTagID)).ToList();
            if (categoryTagIDs != null)
                games = games.Where(g => g.GameCategoryTags.Any(t => categoryTagIDs.Contains(t.ID))).ToList();
            if (brandTagIDs != null)
                games = games.Where(g => brandTagIDs.Contains(g.BrandTagID)).ToList();
            if (notExtended)
                games = games.Where(g => !g.IsExtended).ToList();
            if (AverageGamePeroid != null)
            {
                if (AverageGamePeroid < 91)
                    games = games.Where(g => g.AverageGamePeroid <= AverageGamePeroid && g.AverageGamePeroid > AverageGamePeroid - 29).ToList();
                else
                    games = games.Where(g => g.AverageGamePeroid >= 91).ToList();
            }


                //一樣的操作
                ViewBag.DifficultyTagList = db.Tags.ToList().Where(m => m.ID[0] == 'D');
            ViewBag.CategoryTagList = db.Tags.ToList().Where(m => m.ID[0] == 'C');
            ViewBag.BrandTagList = db.Tags.ToList().Where(m => m.ID[0] == 'B');
            ViewBag.AverageGamePeroid = new SelectList(new List<SelectListItem>
            {
                new SelectListItem { Value = "30", Text = "30分鐘以內"},
                new SelectListItem { Value = "60", Text = "30分鐘 ~ 1小時"},
                new SelectListItem { Value = "90", Text = "1小時 ~ 1.5小時"},
                new SelectListItem { Value = "120", Text = "1.5小時以上"}
            }, "Value", "Text");
            ViewBag.difficultTagIDs = difficultTagIDs == null ? new string[0] : difficultTagIDs;
            ViewBag.categoryTagIDs = categoryTagIDs == null ? new string[0] : categoryTagIDs;
            ViewBag.brandTagIDs = brandTagIDs == null ? new string[0] : brandTagIDs;
            ViewBag.IsFilterOn = true;
            return View(games.OrderBy(g => g.ID).ToPagedList(page, 12));
        }

        //顯示單個桌遊詳細內容
        public ActionResult ShowTableGameDetail(string tableGameID)
        {
            //將此桌遊的相關連結傳入ViewBag
            ViewBag.relevantLinks = db.RelevantLinks.Where(m => m.TableGameID == tableGameID).ToList();
            //將此桌遊的圖片數量傳入ViewBag
            ViewBag.photoIDList = PhotoManager.GetPhotoIDList(tableGameID);
            //新增一筆桌遊閱覽紀錄(每月至多紀錄一筆)
            var statistic = db.TableGameVisitedStatistics.ToList().Where(s => s.VisitedDate.ToString("yyyy/MM") == DateTime.Today.ToString("yyyy/MM")).FirstOrDefault();
            if (statistic == null)
            {
                db.TableGameVisitedStatistics.Add(new TableGameVisitedStatistic
                {
                    VisitedDate = DateTime.Today,
                    Clicks = 1,
                    TableGameID = tableGameID
                });
            }
            else
            {
                statistic.Clicks++;
            }
            db.SaveChanges();
            return View(db.TableGames.Find(tableGameID));
        }


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
        [CenterLogin(CenterLogin.UserType.Admin)]
        public ActionResult ShowTableGameListForAdmin(int page = 1)
        {
            return View(db.TableGames.OrderBy(t => t.ID).ToPagedList(page, 20));
        }
        //管理員新增桌遊
        [CenterLogin(CenterLogin.UserType.Admin)]
        public ActionResult CreateTableGame()
        {
            UpdateTableGamePreparaion();
            return View();
        }
        [HttpPost]
        [CenterLogin(CenterLogin.UserType.Admin)]
        public ActionResult CreateTableGame(TableGame newTableGame, string[] selectedCategories, HttpPostedFileBase[] photos, string[] links)
        {
            //無法通過驗證則顯示錯誤訊息
            if (!ModelState.IsValid)
            {
                UpdateTableGamePreparaion();
                return View();
            }
            //儲存newTableGame
            newTableGame.ID = UsefulTools.GetNextID(db.TableGames, 1);
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

        //管理員編輯桌遊
        [CenterLogin(CenterLogin.UserType.Admin)]
        public ActionResult EditTableGame(string tableGameID)
        {
            UpdateTableGamePreparaion();
            TempData["TableGame_ID"] = tableGameID;
            //將圖片的ID的List傳入ViewBag
            ViewBag.photoIDList = PhotoManager.GetPhotoIDList(tableGameID);
            return View(db.TableGames.Find(tableGameID));
        }
        [HttpPost]
        [CenterLogin(CenterLogin.UserType.Admin)]
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
            tableGame.ID = (string)TempData["TableGame_ID"];
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

        //管理員刪除桌遊
        [CenterLogin(CenterLogin.UserType.Admin)]
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
            //刪除桌遊評論
            db.TableGameComments.RemoveRange(tg.TableGameComments);
            //刪除桌遊閱覽紀錄
            db.TableGameVisitedStatistics.RemoveRange(tg.TableGameVisitedStatistics);
            //最後再刪除桌遊本身
            db.TableGames.Remove(tg);
            db.SaveChanges();
            return RedirectToAction("ShowTableGameListForAdmin");
        }

        //以桌遊分類隨機取得桌遊項目的PartialView(卡片方式呈現)
        public ActionResult _GetTableGameCards(TableGame tableGame)
        {
            Tag[] tags = tableGame.GameCategoryTags.ToArray();
            //將對應分類標籤下的TableGame全部裝入games
            List<TableGame> games = new List<TableGame>();
            foreach (Tag t in tags)
            {
                foreach (TableGame tg in t.TableGamesForGameCategory)
                {
                    if (!games.Contains(tg))
                        games.Add(tg);
                }
            }
            //若games包含傳入的桌遊本身則移除
            if (games.Contains(tableGame))
                games.Remove(tableGame);
            //若games不大於於四項則全丟入Model
            if (games.Count <= 4)
                return PartialView(games);
            //games大於四項則隨機選出四項裝入selectedGames
            List<TableGame> selectedGames = new List<TableGame>();
            Random r = new Random();
            TableGame selectedGame;
            for (int i = 0; i < 4; i++)
            {
                int index = r.Next(games.Count);
                selectedGame = games[index];
                games.Remove(selectedGame);
                selectedGames.Add(selectedGame);
            }
            return PartialView(selectedGames);
        }

        //以名稱搜尋桌遊(Ajax)
        //$.post("/TableGame/SearchTableGameByName", { name: 桌遊搜尋輸入框的值(字串) }, function(data){})
        [HttpPost]
        public ActionResult SearchTableGameByName(string name)
        {
            var games = db.TableGames.ToArray();
            //從中英文名稱查找桌遊(英文不分大小寫)
            var result = games.Where(g => g.ChineseName.Contains(name) || g.EnglishName.IndexOf(name, StringComparison.CurrentCultureIgnoreCase) != -1);
            //選出需要的資料
            var data = result.Select(g => new { g.ID, g.ChineseName, g.EnglishName, Link = Url.Action("ShowTableGameDetail", "TableGame", new { tableGameID = g.ID }) });

            return Json(data);
        }
    }
}