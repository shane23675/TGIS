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
}