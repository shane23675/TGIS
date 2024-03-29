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
    
    public partial class TableGame
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TableGame()
        {
            this.RelevantLinks = new HashSet<RelevantLink>();
            this.TableGameInShopDetails = new HashSet<TableGameInShopDetail>();
            this.TableGameComments = new HashSet<TableGameComment>();
            this.GameCategoryTags = new HashSet<Tag>();
            this.TableGameVisitedStatistics = new HashSet<TableGameVisitedStatistic>();
        }
    
        public string ID { get; set; }
        public string ChineseName { get; set; }
        public string EnglishName { get; set; }
        public bool IsExtended { get; set; }
        public short MinPlayer { get; set; }
        public short MaxPlayer { get; set; }
        public short AverageGamePeroid { get; set; }
        public string DetailGamePeroid { get; set; }
        public string Feature { get; set; }
        public string Rules { get; set; }
        public bool IsShown { get; set; }
        public string DifficultyTagID { get; set; }
        public string BrandTagID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RelevantLink> RelevantLinks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TableGameInShopDetail> TableGameInShopDetails { get; set; }
        public virtual Tag DifficultyTag { get; set; }
        public virtual Tag BrandTag { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TableGameComment> TableGameComments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tag> GameCategoryTags { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TableGameVisitedStatistic> TableGameVisitedStatistics { get; set; }
    }
}
