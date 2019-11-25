using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TGIS.Models
{
    //管理員
    [MetadataType(typeof(MetadataAdministrator))]
    public partial class Administrator { }
    public class MetadataAdministrator
    {
        [DisplayName("管理員編號")]
        public string ID { get; set; }
        [DisplayName("管理員帳號"),Required(ErrorMessage = "請輸入帳號")]
        public string Account { get; set; }
        [DisplayName("管理員密碼"), Required(ErrorMessage = "請輸入密碼")]
        public string Password { get; set; }
    }
    //優惠卷主檔
    [MetadataType(typeof(MetadataCoupon))]
    public partial class Coupon { }
    public class MetadataCoupon
    {
        [DisplayName("活動編號"),StringLength(11)]
        public string ID { get; set; }
        [DisplayName("店家會員編號"),StringLength(6)]
        public string ShopID { get; set; }
        [DisplayName("優惠內容"),StringLength(100)]
        public string Content { get; set; }
        [DisplayName("開始使用日期"), DisplayFormat(DataFormatString = "{0:yyyy/MM/DD}", ApplyFormatInEditMode = true)]
        public System.DateTime BeginDate { get; set; }
        [DisplayName("到期日期"), DisplayFormat(DataFormatString = "{0:yyyy/MM/DD}", ApplyFormatInEditMode = true)]
        public System.DateTime ExpireDate { get; set; }
        [DisplayName("需求點數"),Range(0,int.MaxValue)]
        public int PointsRequired { get; set; }
        [DisplayName("可換張數"), Range(0, int.MaxValue)]
        public Nullable<int> LimitedAmount { get; set; }
        [DisplayName("剩餘張數"), Range(0, int.MaxValue)]
        public Nullable<int> RemainedAmount { get; set; }
        [DisplayName("總兌換人數"), Range(0, int.MaxValue)]
        public Nullable<int> ExchangedAmount { get; set; }
        [DisplayName("活動狀態")]
        public bool IsAvailable { get; set; }
    }
    //縣市列表
    [MetadataType(typeof(MetadataCity))]
    public partial class City { }
    public class MetadataCity
    {
        [DisplayName("縣市編號")]
        public int ID { get; set; }
        [DisplayName("縣市名稱"),StringLength(10)]
        public string CityName { get; set; }
    }
    //公告列表
    [MetadataType(typeof(MetadataAnnouncement))]
    public partial class Announcement { }
    public class MetadataAnnouncement
    {
        [DisplayName("公告編號")]
        public string ID { get; set; }
        [DisplayName("發佈管理員"),StringLength(4)]
        public string AdministratorID { get; set; }
        [DisplayName("發佈日期"), DisplayFormat(DataFormatString = "{0:yyyy/MM/DD}", ApplyFormatInEditMode = true)]
        public System.DateTime AnnouncedDate { get; set; }
        [DisplayName("標題"),StringLength(40)]
        public string Title { get; set; }
        [DisplayName("內文")]
        public string Content { get; set; }
    }
    //回報列表
    [MetadataType(typeof(MetadataFeedback))]
    public partial class Feedback { }
    public class MetadataFeedback
    {
        [DisplayName("回報編號")]
        public string ID { get; set; }
        [DisplayName("來信時間")]
        public System.DateTime ReceivedDate { get; set; }
        [DisplayName("檢舉人"),StringLength(6)]
        public string Plaintiff { get; set; }
        [DisplayName("被檢舉人"),StringLength(6)]
        public string Defendent { get; set; }
        [DisplayName("檢舉內容")]
        public string Content { get; set; }
        [DisplayName("回報分類編號")]
        public string TypeTagID { get; set; }
    }
    //行政區列表
    [MetadataType(typeof(MetadataDistrict))]
    public partial class District { }
    public class MetadataDistrict
    {
        [DisplayName("行政區編號")]
        public int ID { get; set; }
        [DisplayName("縣市邊號")]
        public int CityID { get; set; }
        [DisplayName("行政區名稱"),StringLength(10)]
        public string DistrictName { get; set; }
    }
    //訊息列表
    [MetadataType(typeof(MetadataMessage))]
    public partial class Message { }
    public class MetadataMessage
    {
        [DisplayName("訊息編號")]
        public int ID { get; set; }
        [DisplayName("揪桌編號")]
        public string TeamID { get; set; }
        [DisplayName("留言時間"),DisplayFormat(DataFormatString = "{0:yyyy/MM/DD}", ApplyFormatInEditMode = true)]
        public System.DateTime MessageDate { get; set; }
        [DisplayName("留言者"),StringLength(6)]
        public string Speaker { get; set; }
        [DisplayName("留言內容"),StringLength(300)]
        public string Content { get; set; }
        [DisplayName("是否為私人訊息")]
        public bool IsPrivate { get; set; }
    }
    //一般優惠列表
    [MetadataType(typeof(MetadataNormalOffer))]
    public partial class NormalOffer { }
    public class MetadataNormalOffer
    {
        [DisplayName("優惠編號")]
        public long ID { get; set; }
        [DisplayName("店家會員編號")]
        public string ShopID { get; set; }
        [DisplayName("標題"),StringLength(20)]
        public string Title { get; set; }
        [DisplayName("內文"),StringLength(300)]
        public string Content { get; set; }
        [DisplayName("開啟日期"),DisplayFormat(DataFormatString = "{0:yyyy/MM/DD}", ApplyFormatInEditMode = true)]
        public System.DateTime BeginDate { get; set; }
        [DisplayName("關閉日期"),DisplayFormat(DataFormatString = "{0:yyyy/MM/DD}", ApplyFormatInEditMode = true)]
        public System.DateTime EndDate { get; set; }
        [DisplayName("被點閱數")]
        public int Clicks { get; set; }
    }
    //圖片
    [MetadataType(typeof(MetadataPhoto))]
    public partial class Photo { }
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
    [MetadataType(typeof(MetadataPlayer))]
    public partial class Player { }
    public class MetadataPlayer
    {
        [DisplayName("會員編號")]
        public string ID { get; set; }
        [DisplayName("會員帳號"), Required]
        public string Account { get; set; }
        [DisplayName("會員密碼"), Required]
        public string Password { get; set; }
        [DisplayName("暱稱"), Required]
        public string NickName { get; set; }
        [DisplayName("信箱"), EmailAddress, Required]
        public string Email { get; set; }
        [DisplayName("性別"), Required]
        public bool Gender { get; set; }
        [DisplayName("行政區編號")]
        public int DistrictID { get; set; }
        [DisplayName("生日"), DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true), Required]
        public DateTime Birthday { get; set; }
        [DisplayName("持有點數量"),Range(0,int.MaxValue)]
        public int Points { get; set; }
        [DisplayName("是否停權")]
        public bool IsBanned { get; set; }

        [DisplayName("Email驗證狀態")]
        public bool IsEmailValid { get; set; }
    }
    //玩家優惠明細
    [MetadataType(typeof(MetadataPlyerCouponDetail))]
    public partial class PlyerCouponDetail { }
    public class MetadataPlyerCouponDetail
    {
        [DisplayName("會員編號")]
        public string PlayerID { get; set; }
        [DisplayName("活動編號")]
        public string CouponID { get; set; }
        [DisplayName("優惠卷編碼")]
        public long Code { get; set; }
        [DisplayName("兌換時間"), DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public System.DateTime ExchangedDate { get; set; }
        [DisplayName("是否使用")]
        public bool IsUsed { get; set; }
    }
    //玩家點數明細
    [MetadataType(typeof(MetadataPlayerPointDetail))]
    public partial class PlayerPointDetail { }
    public class MetadataPlayerPointDetail
    {
        [DisplayName("流水號")]
        public int ID { get; set; }
        [DisplayName("玩家會員編號")]
        public string PlayerID { get; set; }
        [DisplayName("點數變化"),Range(0,int.MaxValue)]
        public int ChangedAmount { get; set; }
        [DisplayName("變動原因"),StringLength(20)]
        public string Cause { get; set; }
        [DisplayName("獲得時間"), DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public System.DateTime ChangedDate { get; set; }
    }
    //相關教學連結
    [MetadataType(typeof(MetadataRelevantLink))]
    public partial class RelevantLink { }
    public class MetadataRelevantLink
    {
        [DisplayName("流水號")]
        public int ID { get; set; }
        [DisplayName("網址"),StringLength(1000)]
        public string Url { get; set; }
        [DisplayName("桌遊編號"),StringLength(6)]
        public string TableGameID { get; set; }
    }
    //店家主檔
    [MetadataType(typeof(MetadataShop))]
    public partial class Shop { }
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
        [DisplayName("有無低消")]
        public bool IsMinConsumeRequired { get; set; }
        [DisplayName("可否外食")]
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
        [DisplayName("到期日"), DisplayFormat(DataFormatString = "{0:yyyy/MM/DD}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> ExpireDate { get; set; }
        [DisplayName("累積加值時數")]
        public Nullable<int> AccumulatedHours { get; set; }
        [DisplayName("帳號是否啟用")]
        public bool IsAccountEnabled { get; set; }
        [DisplayName("內容描述")]
        public string Description { get; set; }
    }
    //桌遊主檔
    [MetadataType(typeof(MetadataTableGame))]
    public partial class TableGame { }
    public class MetadataTableGame
    {
        [DisplayName("桌遊編號")]
        public string ID { get; set; }
        [DisplayName("中文名稱"),Required(ErrorMessage = "請輸入中文名稱"),StringLength(40)]
        public string ChineseName { get; set; }
        [DisplayName("英文名稱"),Required(ErrorMessage = "請輸入英文名稱"),StringLength(100)]
        public string EnglishName { get; set; }
        [DisplayName("是否為擴充版")]
        public bool IsExtended { get; set; }
        [DisplayName("最小遊玩人數"),Range(1,10)]
        public short MinPlayer { get; set; }
        [DisplayName("最大遊玩人數"),Range(1,30)]
        public short MaxPlayer { get; set; }
        [DisplayName("平均遊戲時間"),Range(0, 240)]
        public short AverageGamePeroid { get; set; }
        [DisplayName("詳細遊戲時間"),StringLength(10)]
        public string DetailGamePeroid { get; set; }
        [DisplayName("桌遊特色")]
        public string Feature { get; set; }
        [DisplayName("桌遊規則簡介")]
        public string Rules { get; set; }
        [DisplayName("上架狀態")]
        public bool IsShown { get; set; }
        [DisplayName("遊戲難度標籤"),StringLength(3)]
        public string DifficultyTagID { get; set; }
        [DisplayName("桌遊品牌標籤"),StringLength(3)]
        public string BrandTagID { get; set; }
    }
    //桌遊評論列表
    [MetadataType(typeof(MetadataTableGameComment))]
    public partial class TableGameComment { }
    public class MetadataTableGameComment
    {
        [DisplayName("流水號")]
        public int ID { get; set; }
        [DisplayName("評論日期"),DisplayFormat(DataFormatString = "{0:yyyy/MM/DD}", ApplyFormatInEditMode = true)]
        public System.DateTime CommentDate { get; set; }
        [DisplayName("評論內容"),StringLength(500)]
        public string Content { get; set; }
        [DisplayName("是否隱藏")]
        public bool IsHidden { get; set; }
        [DisplayName("桌遊編號")]
        public string TableGameID { get; set; }
        [DisplayName("玩家編號")]
        public string PlayerID { get; set; }
    }
    //店內桌遊明細
    [MetadataType(typeof(MetadataTableGameInShopDetail))]
    public partial class TableGameInShopDetail { }
    public class MetadataTableGameInShopDetail
    {
        [DisplayName("桌遊編號"),StringLength(6)]
        public string TableGameID { get; set; }
        [DisplayName("店家會員編號"),StringLength(6)]
        public string ShopID { get; set; }
        [DisplayName("是否販售")]
        public bool IsSale { get; set; }
        [DisplayName("販售價格")]
        public Nullable<decimal> Price { get; set; }

    }
    //桌遊被點閱紀錄
    [MetadataType(typeof(MetadataTableGameVisitedStatistic))]
    public partial class TableGameVisitedStatistic { }
    public class MetadataTableGameVisitedStatistic
    {
        [DisplayName("被閱覽時間"),DisplayFormat(DataFormatString = "{0:yyyy/MM/DD}", ApplyFormatInEditMode = true)]
        public System.DateTime VisitedDate { get; set; }
        [DisplayName("閱覽次數")]
        public int Clicks { get; set; }
        [DisplayName("桌遊編號"),StringLength(6)]
        public string TableGameID { get; set; }

    }
    //標籤列表
    [MetadataType(typeof(MetadataTag))]
    public partial class Tag { }
    public class MetadataTag
    {
        [DisplayName("標籤編號")]
        public string ID { get; set; }
        [DisplayName("標籤名稱"),StringLength(15)]
        public string TagName { get; set; }
    }
    //揪桌主檔
    [MetadataType(typeof(MetadataTeam))]
    public partial class Team
    {
        //以下是資料庫中沒有儲存，僅是推算出來的屬性
        [DisplayName("狀態")]
        public string Status
        {
            get
            {
                int nowPlayers = OtherPlayers.Count + 1;
                if (DateTime.Now > PlayDate)
                    return "已過期";
                else if (IsCanceled)
                    return "已取消出團";
                else if (IsClosed)
                    return "已提前截止報名";
                else if (DateTime.Now > ParticipateEndDate)
                    return "報名時間已過";
                else if (nowPlayers >= MaxPlayer)
                    return "已額滿，僅開放報名候補";
                else if (nowPlayers < MinPlayer)
                    return $"可報名，未達成團人數(目前：{nowPlayers} / 最低：{MinPlayer})";
                else
                    return $"可報名，已達成團人數(目前：{nowPlayers})";
            }
        }
        [DisplayName("是否可報名")]
        public bool IsOpen
        {
            get
            {
                int nowPlayers = OtherPlayers.Count + 1;
                if (nowPlayers >= MinPlayer && nowPlayers <= MaxPlayer && !IsCanceled && !IsClosed && DateTime.Now <= ParticipateEndDate)
                    return true;
                return false;
            }
        }
    }
    public class MetadataTeam
    {
        [DisplayName("揪桌編號")]
        public string ID { get; set; }
        [DisplayName("店家會員編號"),StringLength(6, ErrorMessage = "請選擇一個店家")]
        public string ShopID { get; set; }
        [DisplayName("主揪玩家編號"),StringLength(6)]
        public string LeaderPlayerID { get; set; }
        [DisplayName("揪桌標題"),StringLength(10, ErrorMessage ="標題長度不得大於10個字"), Required(ErrorMessage = "標題為必填")]
        public string Title { get; set; }
        [DisplayName("報名結束時間"),DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "報名結束時間為必填")]
        public System.DateTime ParticipateEndDate { get; set; }
        [DisplayName("其他資訊備註"),StringLength(20)]
        public string Notes { get; set; }
        [DisplayName("遊戲日期"), DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "遊戲日期為必填")]
        public DateTime PlayDate { get; set; }
        [DisplayName("開始時間"), Required(ErrorMessage = "開始時間為必填")]
        public TimeSpan PlayBeginTime { get; set; }
        [DisplayName("結束時間"), Required(ErrorMessage = "結束時間為必填")]
        public TimeSpan PlayEndTime { get; set; }
        [DisplayName("預估花費")]
        public int? EstimatedCost { get; set; }
        [DisplayName("遊玩形式")]
        public string Preference { get; set; }
        [DisplayName("最低人數"), Range(2, 50, ErrorMessage = "必須在2~50之間"), Required(ErrorMessage = "最低人數為必填")]
        public int MinPlayer { get; set; }
        [DisplayName("最高人數"), Range(2, int.MaxValue, ErrorMessage = "必須至少為2"), Required(ErrorMessage = "最高人數為必填")]
        public int MaxPlayer { get; set; }
        [DisplayName("是否已取消")]
        public bool IsCanceled { get; set; }
        [DisplayName("是否已提前截止")]
        public bool IsClosed { get; set; }


    }
}