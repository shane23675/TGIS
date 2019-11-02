using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TGIS.Models
{
    //管理員
    public class MetadataAdministrator
    {
        [DisplayName("管理員編號")]
        public string ID { get; set; }
        [DisplayName("管理員帳號")]
        [Required(ErrorMessage = "請輸入帳號")]
        public string Account { get; set; }
        [DisplayName("管理員密碼")]
        [Required(ErrorMessage = "請輸入密碼")]
        public string Password { get; set; }
    }
    //優惠卷主檔
    public class MetadataCoupon
    {
        [DisplayName("活動編號")]
        public string ID { get; set; }
        [DisplayName("店家會員編號")]
        public string ShopID { get; set; }
        [DisplayName("優惠內容")]
        public string Content { get; set; }
        [DisplayName("開始使用日期")]
        public System.DateTime BeginDate { get; set; }
        [DisplayName("到期日期")]
        public System.DateTime ExpireDate { get; set; }
        [DisplayName("需求點數")]
        public int PointsRequired { get; set; }
        [DisplayName("可換張數")]
        public Nullable<int> LimitedAmount { get; set; }
        [DisplayName("剩餘張數")]
        public Nullable<int> RemainedAmount { get; set; }
        [DisplayName("總兌換人數")]
        public Nullable<int> ExchangedAmount { get; set; }
        [DisplayName("活動狀態")]
        public bool IsAvailable { get; set; }
    }
    //縣市列表
    public class MetadataCity
    {
        [DisplayName("縣市編號")]
        public int ID { get; set; }
        [DisplayName("縣市名稱")]
        public string CityName { get; set; }
    }
    //公告列表
    public class MetadataAnnouncement
    {
        [DisplayName("公告編號")]
        public string ID { get; set; }
        [DisplayName("發佈管理員")]
        public string AdministratorID { get; set; }
        [DisplayName("發佈日期")]
        public System.DateTime AnnouncedDate { get; set; }
        [DisplayName("標題")]
        public string Title { get; set; }
        [DisplayName("內文")]
        public string Content { get; set; }
    }
    //回報列表
    public class MetadataFeedback
    {
        [DisplayName("回報編號")]
        public string ID { get; set; }
        [DisplayName("來信時間")]
        public System.DateTime ReceivedDate { get; set; }
        [DisplayName("檢舉人")]
        public string Plaintiff { get; set; }
        [DisplayName("被檢舉人")]
        public string Defendent { get; set; }
        [DisplayName("檢舉內容")]
        public string Content { get; set; }
        [DisplayName("回報分類編號")]
        public string TypeTagID { get; set; }
    }
    //行政區列表
    public class MetadataDistrict
    {
        [DisplayName("行政區編號")]
        public int ID { get; set; }
        [DisplayName("縣市邊號")]
        public int CityID { get; set; }
        [DisplayName("行政區名稱")]
        public string DistrictName { get; set; }
    }
    //訊息列表
    public class MetadataMessage
    {
        [DisplayName("訊息編號")]
        public int ID { get; set; }
        [DisplayName("揪桌編號")]
        public string TeamID { get; set; }
        [DisplayName("留言時間")]
        public System.DateTime MessageDate { get; set; }
        [DisplayName("留言者")]
        public string Speaker { get; set; }
        [DisplayName("留言內容")]
        public string Content { get; set; }
        [DisplayName("是否為私人訊息")]
        public bool IsPrivate { get; set; }
    }
    //一般優惠列表
    public class MetadataNormalOffer
    {
        [DisplayName("優惠編號")]
        public long ID { get; set; }
        [DisplayName("店家會員編號")]
        public string ShopID { get; set; }
        [DisplayName("宣傳圖")]
        public byte[] ADImage { get; set; }
        [DisplayName("標題")]
        public string Title { get; set; }
        [DisplayName("內文")]
        public string Content { get; set; }
        [DisplayName("開啟日期")]
        public System.DateTime BeginDate { get; set; }
        [DisplayName("關閉日期")]
        public System.DateTime EndDate { get; set; }
        [DisplayName("被點閱數")]
        public int Clicks { get; set; }
    }
    //圖片
    public class MetadataPhoto
    {
        [DisplayName("流水號")]
        public int ID { get; set; }
        [DisplayName("來源編號")]
        public string SourceID { get; set; }
        [DisplayName("圖片資料")]
        public byte[] Content { get; set; }
    }
    //玩家主檔
    public class MetadataPlyer
    {
        [DisplayName("會員編號")]
        public string ID { get; set; }
        [DisplayName("會員帳號")]
        public string Account { get; set; }
        [DisplayName("會員密碼")]
        public string Password { get; set; }
        [DisplayName("暱稱")]
        public string NickName { get; set; }
        [DisplayName("照片")]
        public byte[] PlayerImage { get; set; }
        [DisplayName("信箱")]
        public string Email { get; set; }
        [DisplayName("性別")]
        public bool Gender { get; set; }
        [DisplayName("行政區編號")]
        public int DistrictID { get; set; }
        [DisplayName("生日")]
        public System.DateTime Birthday { get; set; }
        [DisplayName("持有點數量")]
        public int Points { get; set; }
        [DisplayName("是否停權")]
        public bool IsBanned { get; set; }

    }
    //玩家優惠明細
    public class MetadataPlyerCouponDetail
    {
        [DisplayName("會員編號")]
        public string PlayerID { get; set; }
        [DisplayName("活動編號")]
        public string CouponID { get; set; }
        [DisplayName("優惠卷編碼")]
        public long Code { get; set; }
        [DisplayName("兌換時間")]
        public System.DateTime ExchangedDate { get; set; }
        [DisplayName("是否使用")]
        public bool IsUsed { get; set; }
    }
    //玩家點數明細
    public class MetadataPlayerPointDetail
    {
        [DisplayName("流水號")]
        public int ID { get; set; }
        [DisplayName("玩家會員編號")]
        public string PlayerID { get; set; }
        [DisplayName("點數變化")]
        public int ChangedAmount { get; set; }
        [DisplayName("變動原因")]
        public string Cause { get; set; }
        [DisplayName("獲得時間")]
        public System.DateTime ChangedDate { get; set; }
    }
    //相關教學連結
    public class MetadataRelevantLink
    {
        [DisplayName("流水號")]
        public int ID { get; set; }
        [DisplayName("網址")]
        public string Url { get; set; }
        [DisplayName("桌遊編號")]
        public string TableGameID { get; set; }
    }
    //店家主檔
    public class MetadataShop
    {
        [DisplayName("會員編號")]
        public string ID { get; set; }
        [DisplayName("帳號")]
        public string Account { get; set; }
        [DisplayName("密碼")]
        public string Password { get; set; }
        [DisplayName("店家名稱")]
        public string ShopName { get; set; }
        [DisplayName("營業時間")]
        public string OpeningHours { get; set; }
        [DisplayName("行政區編號")]
        public int DistrictID { get; set; }
        [DisplayName("地址")]
        public string Address { get; set; }
        [DisplayName("有無抵銷")]
        public bool IsMinConsumeRequired { get; set; }
        [DisplayName("飲食規則")]
        public bool IsFoodAcceptable { get; set; }
        [DisplayName("場地規模")]
        public string AreaScale { get; set; }
        [DisplayName("電話")]
        public string Tel { get; set; }
        [DisplayName("信箱")]
        public string Email { get; set; }
        [DisplayName("Line")]
        public string Line { get; set; }
        [DisplayName("官網")]
        public string Website { get; set; }
        [DisplayName("是否啟用VIP")]
        public bool IsVIP { get; set; }
        [DisplayName("到期日")]
        public Nullable<System.DateTime> ExpireDate { get; set; }
        [DisplayName("累積加值時數")]
        public Nullable<int> AccumulatedHours { get; set; }
        [DisplayName("帳號狀態")]
        public bool IsAccountEnabled { get; set; }
        [DisplayName("內容描述")]
        public string Description { get; set; }
    }
    //桌遊主檔
    public class MetadataTableGame
    {
        [DisplayName("桌遊編號")]
        [Required(ErrorMessage = "請輸入桌遊編號")]
        [StringLength(100)]
        public string ID { get; set; }
        [DisplayName("中文名稱")]
        [Required(ErrorMessage = "請輸入中文名稱")]
        [StringLength(40)]
        public string ChineseName { get; set; }
        [Required(ErrorMessage = "請輸入英文名稱")]
        [StringLength(100)]
        [DisplayName("英文名稱")]
        public string EnglishName { get; set; }
        [DisplayName("是否為擴充版")]
        public bool IsExtended { get; set; }
        [DisplayName("最小遊玩人數")]
        [Range(1,10)]
        public short MinPlayer { get; set; }
        [DisplayName("最大遊玩人數")]
        [Range(1,30)]
        public short MaxPlayer { get; set; }
        [DisplayName("平均遊戲時間")]
        [Range(0, 240)]
        public short AverageGamePeroid { get; set; }
        [DisplayName("詳細遊戲時間")]
        [StringLength(10)]
        public string DetailGamePeroid { get; set; }
        [DisplayName("桌遊特色")]
        public string Feature { get; set; }
        [DisplayName("桌遊規則簡介")]
        public string Rules { get; set; }
        [DisplayName("上架狀態")]
        public bool IsShown { get; set; }
        [DisplayName("遊戲難度標籤")]
        public string DifficultyTagID { get; set; }
        [DisplayName("桌遊品牌標籤")]
        public string BrandTagID { get; set; }
    }
    //桌遊評論列表
    public class MetadataTableGameComment
    {
        [DisplayName("流水號")]
        public int ID { get; set; }
        [DisplayName("評論日期")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/DD}", ApplyFormatInEditMode = true)]
        public System.DateTime CommentDate { get; set; }
        [DisplayName("評論內容")]
        [StringLength(500)]
        public string Content { get; set; }
        [DisplayName("是否隱藏")]
        public bool IsHidden { get; set; }
        [DisplayName("桌遊編號")]
        public string TableGameID { get; set; }
        [DisplayName("玩家編號")]
        public string PlayerID { get; set; }
    }
    //店內桌遊明細
    public class MetadataTableGameInShopDetail
    {
        [DisplayName("桌遊編號")]
        public string TableGameID { get; set; }
        [DisplayName("店家會員編號")]
        public string ShopID { get; set; }
        [DisplayName("是否販售")]
        public bool IsSale { get; set; }
        [DisplayName("販售價格")]
        public Nullable<decimal> Price { get; set; }

    }
    //桌遊被點閱紀錄
    public class MetadataTableGameVisitedStatistic
    {
        [DisplayName("被閱覽時間")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/DD}", ApplyFormatInEditMode = true)]
        public System.DateTime VisitedDate { get; set; }
        [DisplayName("閱覽次數")]
        public int Clicks { get; set; }
        [DisplayName("桌遊編號")]
        public string TableGameID { get; set; }

    }
    //標籤列表
    public class MetadataTag
    {
        [DisplayName("標籤編號")]
        public string ID { get; set; }
        [DisplayName("標籤名稱")]
        public string TagName { get; set; }
    }
    //揪桌主檔
    public class MetadataTeam
    {
        [DisplayName("揪桌編號")]
        public string ID { get; set; }
        [DisplayName("店家會員編號")]
        public string ShopID { get; set; }
        [DisplayName("主揪玩家編號")]
        public string LeaderPlayerID { get; set; }
        [DisplayName("揪桌標題")]
        public string Title { get; set; }
        [DisplayName("報名開始時間")]
        public System.DateTime ParticipateBeginDate { get; set; }
        [DisplayName("報名結束時間")]
        public System.DateTime ParticipateEndDate { get; set; }
        [DisplayName("遊玩開始時間")]
        public System.DateTime PlayBeginTime { get; set; }
        [DisplayName("遊玩結束時間")]
        public System.DateTime PlayEndTime { get; set; }
        [DisplayName("成團人數")]
        public string MinPlayers { get; set; }
        [DisplayName("遊玩形式")]
        public string PlayPreference { get; set; }
        [DisplayName("預估花費")]
        public Nullable<decimal> EstimatedCost { get; set; }
        [DisplayName("其他資訊備註")]
        public string Notes { get; set; }
        [DisplayName("團務狀態")]
        public string Status { get; set; }
    }
}