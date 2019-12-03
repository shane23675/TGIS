using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using TGIS.Models;

//請將自定義的驗證Attribute寫在這裡並標註功能
namespace TGIS.Models
{
    //驗證生日是否在今天以前
    public class BirthdayValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                ErrorMessage = "生日為必填欄位";
                return false;
            }
            ErrorMessage = "生日日期不合法";
            return (DateTime)value < DateTime.Now;
        }
    }

    //驗證玩家帳號是否重複
    public class PlayerAccountRepeatCheck : ValidationAttribute
    {
        TGISDBEntities db = new TGISDBEntities();
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;
            else if (db.Players.Any(m => m.Account == (string)value))
            {
                ErrorMessage = "該帳號已經有人使用";
                return false;
            }
            return true;
        }
    }

    //驗證店家帳號是否重複
    public class ShopAccountRepeatCheck : ValidationAttribute
    {
        TGISDBEntities db = new TGISDBEntities();
        public override bool IsValid(object value)
        {
            if (value == null)
                return false;
            else if (db.Shops.Any(m => m.Account == (string)value))
            {
                ErrorMessage = "該帳號已經有人使用";
                return false;
            }
            return true;
        }
    }
}