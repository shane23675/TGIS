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

    [MetadataType(typeof(MetadataRelevantLink))]
    public partial class RelevantLink
    {
        public int ID { get; set; }
        public string Url { get; set; }
        public string TableGameID { get; set; }
    
        public virtual TableGame TableGame { get; set; }
    }
}