//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace TGIS.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Coupon
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Coupon()
        {
            this.PlayerCouponDetails = new HashSet<PlayerCouponDetail>();
        }
    
        public string ID { get; set; }
        public string ShopID { get; set; }
        public string Content { get; set; }
        public System.DateTime BeginDate { get; set; }
        public System.DateTime ExpireDate { get; set; }
        public int PointsRequired { get; set; }
        public Nullable<int> LimitedAmount { get; set; }
        public Nullable<int> RemainedAmount { get; set; }
        public Nullable<int> ExchangedAmount { get; set; }
        public bool IsAvailable { get; set; }
    
        public virtual Shop Shop { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlayerCouponDetail> PlayerCouponDetails { get; set; }
    }
}
