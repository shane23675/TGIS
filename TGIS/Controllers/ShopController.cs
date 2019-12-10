using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TGIS.Models;

namespace TGIS.Controllers
{
    public class ShopController : Controller
    {
        TGISDBEntities db = new TGISDBEntities();
        //管理員查看店家列表
        public ActionResult ShopIndex()
        {
            return View(db.Shops.ToList());
        }

        //管理員創建店家會員
        public ActionResult MgShopCreate()
        {
            ViewBag.CityID = new SelectList(db.Cities, "ID", "CityName");
            ViewBag.DistrictID = new SelectList(db.Districts, "ID", "DistrictName");
            return View();
        }
        [HttpPost]
        public ActionResult MgShopCreate(Shop shop, HttpPostedFileBase[] photos)
        {
            //檢查帳號是否重複
            if (db.Shops.Any(m => m.Account == shop.Account))
                ModelState["Account"].Errors.Add("此帳號已經有人使用");
            //驗證帳號密碼是否符合規則
            UsefulTools.RegisterValidate(shop.Account, ModelState["Account"].Errors.Add, false, false);
            UsefulTools.RegisterValidate(shop.Password, ModelState["Password"].Errors.Add, true, true);
            //填入自動生成的ID
            shop.ID = UsefulTools.GetNextID(db.Shops, 1);
            if (ModelState.IsValid)
            {
                shop.Password = Hash.PwdHash(shop.Password);
                db.Shops.Add(shop);
                db.SaveChanges();

                //添加圖片
                PhotoManager.Create(shop.ID, photos);

                return RedirectToAction("ShopIndex");
            }
            ViewBag.DistrictID = new SelectList(db.Districts, "ID", "DistrictName");
            ViewBag.CityID = new SelectList(db.Cities, "ID", "CityName", db.Districts.Find(shop.DistrictID).CityID);
            return View(shop);
        }

        //刪除店家資料
        public ActionResult ShopDelete(string id)
        {
            Shop s = db.Shops.Find(id);
            //刪除該店家的圖片
            PhotoManager.Delete(id);
            //刪除店內桌遊明細
            db.TableGameInShopDetails.RemoveRange(s.TableGameInShopDetails);
            //刪除一般優惠(活動)
            db.NormalOffers.RemoveRange(s.NormalOffers);
            //刪除優惠券主檔及其明細
            s.Coupons.ToList().ForEach(m => db.PlayerCouponDetails.RemoveRange(m.PlayerCouponDetails));
            db.Coupons.RemoveRange(s.Coupons);
            //刪除揪桌主檔及其相關資料
            foreach(Team t in s.Teams)
            {
                t.OtherPlayers.Clear();
                t.Messages.Clear();
                db.Teams.Remove(t);
            }
            //最後再刪除店家本身
            db.Shops.Remove(s);
            db.SaveChanges();

            return RedirectToAction("ShopIndex");
        }

        //管理員編輯店家
        public ActionResult MgShopEdit(string id)
        {
            var shop = db.Shops.Find(id);
            ViewBag.CityID = new SelectList(db.Cities, "ID", "CityName",shop.District.CityID);
            ViewBag.DistrictID = new SelectList(db.Districts, "ID", "DistrictName", shop.DistrictID);
            ViewBag.photoIDList = PhotoManager.GetPhotoIDList(id);
            TempData["Shop_ID"] = id;
            return View(db.Shops.Find(id));
        }
        [HttpPost]
        public ActionResult MgShopEdit(Shop shop)
        {
            //移除不能修改部分的ModelState錯誤
            List<string> exceptions = new List<string>{ "ID", "Account", "Password", "ShopName", "OpeningHours", 
                "Address", "AreaScale", "Tel"};
            exceptions.ForEach(m => ModelState[m].Errors.Clear());

            if (ModelState.IsValid)
            {
                //找到原始店家資料並修改變動項目
                Shop s = db.Shops.Find((string)TempData["Shop_ID"]);
                s.IsVIP = shop.IsVIP;
                s.ExpireDate = shop.ExpireDate;
                s.AccumulatedHours = shop.AccumulatedHours;
                s.IsAccountEnabled = shop.IsAccountEnabled;

                db.Entry(s).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ShopIndex");
            }
            return View(shop);
        }
        //玩家看到的店家列表
        public ActionResult ShopIndexForPlayer(int? CityID, int? DistrictID, string searchedTableGameID, 
            string AreaScale, string IsFoodAcceptable, string IsMinConsumeRequired)
        {
            ViewBag.CityID = new SelectList(db.Cities, "ID", "CityName");
            ViewBag.DistrictID = new SelectList(db.Districts, "ID", "DistrictName", -1);
            ViewBag.AreaScale = new SelectList(new []{
                new { Text = "大" },
                new { Text = "中" },
                new { Text = "小" },
                new { Text = "不限" }
            }, "Text", "Text", "不限");
            ViewBag.IsFoodAcceptable = new SelectList(new[]{
                new { Text = "可外食" },
                new { Text = "不可外食" },
                new { Text = "不限" }
            }, "Text", "Text", "不限");
            ViewBag.IsMinConsumeRequired = new SelectList(new[]{
                new { Text = "有" },
                new { Text = "無" },
                new { Text = "不限" }
            }, "Text", "Text", "不限");
            //店家查詢結果的容器
            var shops = db.Shops.ToList();
            if (searchedTableGameID == "" || searchedTableGameID == null)
            {
                //桌遊搜尋ID為空，啟用一般篩選功能
                if (CityID != null)
                {
                    if (DistrictID != -1)
                    {
                        shops = db.Districts.Find(DistrictID).Shops.ToList();
                    }
                    else
                    {
                        shops.Clear();
                        foreach (District d in db.Cities.Find(CityID).Districts)
                        {
                            shops.AddRange(d.Shops);
                        }
                    }
                }
            }
            //在哪裡可以玩此桌遊的查詢
            else
            {
                //先找到有此桌遊的店家
                TableGame game = db.TableGames.Find(searchedTableGameID);
                //傳送搜尋相關資訊
                ViewBag.SearchingTGInfo = game;
                //將包含此桌遊的店家找出
                shops.Clear();
                foreach (var detail in game.TableGameInShopDetails)
                {
                    shops.Add(detail.Shop);
                }
                //地區篩選
                if (CityID != null)
                {
                    if (DistrictID != -1)
                        shops = shops.Where(s => s.DistrictID == DistrictID).ToList();
                    else
                        shops = shops.Where(s => s.District.CityID == CityID).ToList();
                }
            }
            //店家規模篩選
            if (AreaScale != "不限" && AreaScale != null)
            {
                shops = shops.Where(s => s.AreaScale == AreaScale).ToList();
            }
            //可否外食篩選
            if (IsFoodAcceptable != "不限" && IsFoodAcceptable != null)
            {
                bool acceptable = IsFoodAcceptable == "可外食";
                shops = shops.Where(s => s.IsFoodAcceptable == acceptable).ToList();
            }
            //有無低消篩選
            if (IsMinConsumeRequired != "不限" && IsMinConsumeRequired != null)
            {
                bool required = IsMinConsumeRequired == "有";
                shops = shops.Where(s => s.IsMinConsumeRequired == required).ToList();
            }
            return View(shops.OrderByDescending(s=>s.IsVIP).ThenByDescending(s=>s.AccumulatedHours));
        }
        //玩家看到店家詳細資料
        public ActionResult ShopDetailForPlayer(string id)
        {
            ViewBag.photoIDList = PhotoManager.GetPhotoIDList(id);
            return View(db.Shops.Find(id));
        }
        //店家編輯店家資料
        public ActionResult ShopEditForStore(string id)
        {
            var shop = db.Shops.Find(id);
            ViewBag.CityID = new SelectList(db.Cities, "ID", "CityName", shop.District.CityID);
            ViewBag.DistrictID = new SelectList(db.Districts, "ID", "DistrictName", shop.DistrictID);
            ViewBag.photoIDList = PhotoManager.GetPhotoIDList(id);
            ViewBag.AreaScale = new SelectList(new List<SelectListItem>{
                new SelectListItem { Text = "大", Value = "大" },
                new SelectListItem { Text = "中", Value = "中" },
                new SelectListItem { Text = "小", Value = "小" }
            }, "Value", "Text", shop.AreaScale);
            TempData["Shop_ID"] = id;
            return View(db.Shops.Find(id));
        }
        [HttpPost]
        public ActionResult ShopEditForStore(Shop shop, int[] deletedPhotoID, HttpPostedFileBase[] newPhoto)
        {
            Shop s = db.Shops.Find((string)TempData["Shop_ID"]);

            //清除ModelState中的錯誤以免無法通過
            List<string> exceptions = new List<string> { "Account", "Password"};
            exceptions.ForEach(m => ModelState[m].Errors.Clear());

            if (ModelState.IsValid)
            {
                //取得原始檔案並將必要資料存入
                shop.ID = s.ID;
                shop.Account = s.Account;
                shop.Password = s.Password;
                shop.IsVIP = s.IsVIP;
                shop.ExpireDate = s.ExpireDate;
                shop.AccumulatedHours = s.AccumulatedHours;
                
                //取消追蹤變量s，以免儲存時發生錯誤
                db.Entry(s).State = EntityState.Detached;
                db.Entry(shop).State = EntityState.Modified;
                db.SaveChanges();

                //刪除被勾選的圖片
                if (deletedPhotoID != null)
                {
                    foreach (int id in deletedPhotoID)
                    {
                        PhotoManager.Delete(id);
                    }
                }
                //加入新圖片
                PhotoManager.Create(shop.ID, newPhoto);

                return RedirectToAction("ShopDetailForStore");
            }
            ViewBag.CityID = new SelectList(db.Cities, "ID", "CityName", s.District.CityID);
            ViewBag.DistrictID = new SelectList(db.Districts, "ID", "DistrictName", s.DistrictID);
            ViewBag.photoIDList = PhotoManager.GetPhotoIDList(s.ID);
            TempData.Keep("Shop_ID");
            ViewBag.AreaScale = new SelectList(new List<SelectListItem>{
                new SelectListItem { Text = "大", Value = "大" },
                new SelectListItem { Text = "中", Value = "中" },
                new SelectListItem { Text = "小", Value = "小" }
            }, "Value", "Text", shop.AreaScale);
            return View(shop);
        }
        //店家看到店家詳細資料
        public ActionResult ShopDetailForStore()
        {
            var id = Session["ShopID"].ToString();
            if (id == null)
                return HttpNotFound();
            return View(db.Shops.Find(id));
        }
 

        //取得店家列表(Ajax)
        public ActionResult GetShopSelectList(int districtID, string shopID)
        {
            Shop[] shops = db.Districts.Find(districtID).Shops.ToArray();
            //找不到任何店家則返回錯誤選項
            if (shops.Length == 0)
                return Content("<option value=\"ErrorMessage\">此地區無任何店家</option>");
            string result = "";
            foreach (Shop s in shops)
            {
                //通過shopID判斷是否有已經選擇的項目
                if (s.ID == shopID)
                    result += $"<option value=\"{s.ID}\" selected>{s.ShopName}</option>";
                else
                    result += $"<option value=\"{s.ID}\">{s.ShopName}</option>";
            }
            return Content(result);
        }

        //從名稱搜尋店家(Ajax)
        //$.post("/Shop/SearchShopByName", { name: 店家搜尋框的值 }, function(data){})
        [HttpPost]
        public ActionResult SearchShopByName(string name)
        {
            //找到可能的店家
            var result = db.Shops.Where(s => s.ShopName.Contains(name)).ToArray();
            //選出需要的資料
            var data = result.Select(s => new { s.ShopName, Link = Url.Action("ShopDetailForPlayer", "Shop", new { id = s.ID }) });

            return Json(data);
        }

        //查詢店家帳號是否重複(Ajax)
        //$.post("/Shop/ShopAccountRepeatCheck", { account: 帳號值 }, function(data){})
        public ActionResult ShopAccountRepeatCheck(string account)
        {
            if (db.Shops.Any(s => s.Account == account))
                return Content("true");
            return Content("false");
        }
    }
}