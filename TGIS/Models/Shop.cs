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
    
    public partial class Shop
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Shop()
        {
            this.Coupons = new HashSet<Coupon>();
            this.TableGameInShopDetails = new HashSet<TableGameInShopDetail>();
            this.Teams = new HashSet<Team>();
            this.NormalOffers = new HashSet<NormalOffer>();
        }
    
        public string ID { get; set; }
        public string Account { get; set; }
        public string Password { get; set; }
        public string ShopName { get; set; }
        public string OpeningHours { get; set; }
        public int DistrictID { get; set; }
        public string Address { get; set; }
        public bool IsMinConsumeRequired { get; set; }
        public bool IsFoodAcceptable { get; set; }
        public string AreaScale { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }
        public string Line { get; set; }
        public string Website { get; set; }
        public bool IsVIP { get; set; }
        public Nullable<System.DateTime> ExpireDate { get; set; }
        public Nullable<int> AccumulatedHours { get; set; }
        public bool IsAccountEnabled { get; set; }
        public string Description { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Coupon> Coupons { get; set; }
        public virtual District District { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TableGameInShopDetail> TableGameInShopDetails { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Team> Teams { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<NormalOffer> NormalOffers { get; set; }
    }
}
