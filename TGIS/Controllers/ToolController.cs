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
        // GET: Tool
        public ActionResult GetNextID<T>(IEnumerable<T> dataTable, int n)
        {
            return Content(UsefulTools.GetNextID(dataTable, n));
        }
        public ActionResult TestMethod()
        {
            return Content("ABCDEF");
        }
    }
}