using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TGIS.Models;

//請將自定義的驗證Attribute寫在這裡並標註功能
namespace TGIS.Models
{
    //檢查輸入日期是否是在未來
    public class AfterNowAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value != null)
            {
                return (DateTime)value > DateTime.Now;
            }
            return false;
        }
    }
}