using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TGIS.Models
{
    public class AccountRepeatAttribute : ValidationAttribute
    {
        TGISDBEntities db = new TGISDBEntities();

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            Shop shop = (Shop)validationContext.ObjectInstance;
            if (db.Shops.Where(m => m.Account == shop.Account) != null)
            {
                return new ValidationResult("帳號重複");
            }
            if (db.Shops.Where(m => m.Email == shop.Email) != null)
            {
                return new ValidationResult("Email重複");
            }
            return ValidationResult.Success;
        }
    }
}