using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TGIS.Models;

namespace TGIS.Controllers
{
    //此控制器為測試中的工具包，請先不要使用其中的任何功能
    public class ToolController : Controller
    {
        //取得填入時間的PartialView的方法
        public ActionResult _GetTimeInput()
        {
            return PartialView();
        }
    }
}