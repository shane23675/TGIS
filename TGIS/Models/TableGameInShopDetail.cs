//------------------------------------------------------------------------------
// <auto-generated>
//     ?�個�?式碼?�由範本?��???
//
//     對這個�?案進�??��?變更?�能導致?��??�用程�??��??��??��?行為??
//     如�??�新?��?程�?碼�?將�?覆寫對這個�?案�??��?變更??
// </auto-generated>
//------------------------------------------------------------------------------

namespace TGIS.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    [MetadataType(typeof(MetadataTableGameInShopDetail))]
    public partial class TableGameInShopDetail
    {
        public string TableGameID { get; set; }
        public string ShopID { get; set; }
        public bool IsSale { get; set; }
        public Nullable<decimal> Price { get; set; }
    
        public virtual Shop Shop { get; set; }
        public virtual TableGame TableGame { get; set; }
    }
}