using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGIS.Models;

namespace TGIS.Controllers
{
    public class AdminController : Controller
    {
        TGISDBEntities db = new TGISDBEntities();

        //查看管理員列表
        public ActionResult AdminIndex()
        {
            return View(db.Administrators.ToList());
        }

        //新增管理員
        public ActionResult AdminCreate()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AdminCreate(Administrator administrator, string pwdConfirm)
        {
            //驗證帳號密碼格式
            UsefulTools.RegisterValidate(administrator.Account, msg => ModelState["Account"].Errors.Add(msg), false, false);
            UsefulTools.RegisterValidate(administrator.Password, msg => ModelState["Password"].Errors.Add(msg), true, false);
            if (administrator.Password != pwdConfirm)
            {
                ModelState.AddModelError("Password", "密碼確認和密碼欄位不相符");
            }
            if (ModelState.IsValid)
            {
                administrator.ID = UsefulTools.GetNextID(db.Administrators, 1);
                db.Administrators.Add(administrator);
                db.SaveChanges();
                return RedirectToAction("AdminIndex");
            }
            return View (administrator);
        }

        //更改管理員密碼
        public ActionResult AdminChangePassword(string adminID)
        {
            var admin = db.Administrators.Find(adminID);
            TempData["AdminID"] = adminID;
            return View(admin);
        }
        [HttpPost]
        public ActionResult AdminChangePassword(string pwd, string pwdConfirm)
        {
            var admin = db.Administrators.Find(TempData["AdminID"].ToString());
            UsefulTools.RegisterValidate(pwd, msg => ModelState.AddModelError("Password", msg), true, false);
            if (pwd != pwdConfirm)
            {
                ModelState.AddModelError("Password", "密碼確認和密碼欄位不相符");
            }
            if (ModelState.IsValid)
            {
                admin.Password = pwd;
                db.SaveChanges();
                return RedirectToAction("AdminIndex");
            }
            TempData.Keep();
            return View(admin);
        }

        //刪除管理員
        public ActionResult AdminDelete(string adminID)
        {
            var admin = db.Administrators.Find(adminID);
            //最後一名管理員不得刪除
            if (admin != null && db.Administrators.Count() > 1)
            {
                //刪除公告
                db.Announcements.RemoveRange(admin.Announcements);
                //刪除管理員
                db.Administrators.Remove(admin);
                db.SaveChanges();
            }
            TempData["ErrorMessage"] = "不得刪除最後一名管理員";
            return RedirectToAction("AdminIndex");
        }
    }
}