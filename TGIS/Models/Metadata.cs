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
    public partial class Coupon
    {
        //以下為推導出的屬性，在資料庫中不會儲存
        [DisplayName("已兌換人數")]
        public int ExchangedAmount
        {
            get
            {
                return PlayerCouponDetails.Count;
            }
        }
        [DisplayName("已使用人數")]
        public int UsedAmount
        {
            get
            {
                return PlayerCouponDetails.Count(d => d.IsUsed);
            }
        }
        [DisplayName("是否可兌換")]
        public bool IsExchangable
        {
            get
            {
                return IsAvailable && DateTime.Today <= ExpireDate 
                    && ExchangedAmount < LimitedAmount;
            }
        }
    }
    public class MetadataCoupon
    {
        [DisplayName("活動編號"),StringLength(11)]
        public string ID { get; set; }
        [DisplayName("店家會員編號"),StringLength(6)]
        public string ShopID { get; set; }
        [DisplayName("優惠內容"),StringLength(100), Required]
        public string Content { get; set; }
        [DisplayName("開始使用日期"), DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true), Required, DataType(DataType.Date)]
        public System.DateTime BeginDate { get; set; }
        [DisplayName("到期日期"), DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true), Required, DataType(DataType.Date)]
        public System.DateTime ExpireDate { get; set; }
        [DisplayName("需求點數"),Range(1, int.MaxValue), Required]
        public int PointsRequired { get; set; }
        [DisplayName("可換張數"), Range(0, int.MaxValue)]
        public Nullable<int> LimitedAmount { get; set; }
        [DisplayName("啟用確認")]
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
        [DisplayName("發佈日期"), DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true), Required, DataType(DataType.Date)]
        public System.DateTime AnnouncedDate { get; set; }
        [DisplayName("標題"),StringLength(40), Required]
        public string Title { get; set; }
        [DisplayName("內文"), Required]
        public string Content { get; set; }
    }
    //回報列表
    [MetadataType(typeof(MetadataFeedback))]
    public partial class Feedback { }
    public class MetadataFeedback
    {
        [DisplayName("回報編號")]
        public string ID { get; set; }
        [DisplayName("來信時間"), DisplayFormat(DataFormatString = "{0:yyyy/MM/dd  hh:mm}", ApplyFormatInEditMode = true)]
        public System.DateTime ReceivedDate { get; set; }
        [DisplayName("檢舉人"),StringLength(6)]
        public string Plaintiff { get; set; }
        [DisplayName("被檢舉人"),StringLength(6)]
        public string Defendent { get; set; }
        [DisplayName("檢舉內容"), Required]
        public string Content { get; set; }
        [DisplayName("回報分類編號")]
        public string TypeTagID { get; set; }
        [DisplayName("已讀取")]
        public string IsRead { get; set; }
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
        [DisplayName("留言時間"),DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}", ApplyFormatInEditMode = true)]
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
    public partial class NormalOffer
    {
        //以下屬性為推導出來的屬性，沒有儲存在資料庫中
        [DisplayName("是否會展示")]
        public bool IsAvailable
        {
            get
            {
                return !IsHidden && DateTime.Now < EndDate;
            }
        }
    }
    public class MetadataNormalOffer
    {
        [DisplayName("優惠編號")]
        public long ID { get; set; }
        [DisplayName("店家會員編號")]
        public string ShopID { get; set; }
        [DisplayName("標題"),StringLength(20), Required]
        public string Title { get; set; }
        [DisplayName("活動內容"),StringLength(300), Required]
        public string Content { get; set; }
        [DisplayName("開始時間"),DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}", ApplyFormatInEditMode = true), Required]
        public System.DateTime BeginDate { get; set; }
        [DisplayName("結束時間"),DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}", ApplyFormatInEditMode = true), Required]
        public System.DateTime EndDate { get; set; }
        [DisplayName("被點閱數")]
        public int Clicks { get; set; }
        [DisplayName("已隱藏")]
        public bool IsHidden { get; set; }
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
    public partial class Player 
    {
        [DisplayName("年齡")]
        public int Age
        {
            get
            {
                return DateTime.Now.Subtract(Birthday).Days / 365;
            }
        }
    }
    public class MetadataPlayer
    {
        [DisplayName("會員編號")]
        public string ID { get; set; }
        [DisplayName("會員帳號"), Required]
        public string Account { get; set; }
        [DisplayName("會員密碼"), Required]
        public string Password { get; set; }
        [DisplayName("暱稱"), Required, StringLength(10)]
        public string NickName { get; set; }
        [DisplayName("信箱"), EmailAddress, Required]
        public string Email { get; set; }
        [DisplayName("性別"), Required]
        public bool Gender { get; set; }
        [DisplayName("行政區編號")]
        public int DistrictID { get; set; }
        [DisplayName("生日"), DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true), Required, DataType(DataType.Date)]
        [BirthdayValidation]
        public DateTime Birthday { get; set; }
        [DisplayName("持有點數量"),Range(0,int.MaxValue)]
        public int Points { get; set; }
        [DisplayName("是否停權")]
        public bool IsBanned { get; set; }

        [DisplayName("Email驗證狀態")]
        public bool IsEmailValid { get; set; }
    }
    //玩家優惠明細
    [MetadataType(typeof(MetadataPlayerCouponDetail))]
    public partial class PlayerCouponDetail { }
    public class MetadataPlayerCouponDetail
    {
        [DisplayName("會員編號")]
        public string PlayerID { get; set; }
        [DisplayName("活動編號")]
        public string CouponID { get; set; }
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
        [DisplayName("點數變化"),Range(int.MinValue,int.MaxValue)]
        public int ChangedAmount { get; set; }
        [DisplayName("變動原因"),StringLength(20)]
        public string Cause { get; set; }
        [DisplayName("變動時間"), DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
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
    public partial class Shop
    {
        //以下為推算出的衍生屬性，沒有儲存在資料庫中
        [DisplayName("詳細地址")]
        public string FullAddress
        {
            get
            {
                return District.City.CityName + District.DistrictName + Address;
            }
        }
    }
    public class MetadataShop
    {
        [DisplayName("會員編號")]
        public string ID { get; set; }
        [DisplayName("帳號"), Required]
        public string Account { get; set; }
        [DisplayName("密碼"), Required]
        public string Password { get; set; }
        [DisplayName("店家名稱"), Required, StringLength(40)]
        public string ShopName { get; set; }
        [DisplayName("營業時間"), Required, StringLength(100)]
        public string OpeningHours { get; set; }
        [DisplayName("行政區編號")]
        public int DistrictID { get; set; }
        [DisplayName("地址"), Required, StringLength(60)]
        public string Address { get; set; }
        [DisplayName("有無低消")]
        public bool IsMinConsumeRequired { get; set; }
        [DisplayName("可否外食")]
        public bool IsFoodAcceptable { get; set; }
        [DisplayName("場地規模"), Required, StringLength(1)]
        public string AreaScale { get; set; }
        [DisplayName("電話"), Required, StringLength(25)]
        public string Tel { get; set; }
        [DisplayName("信箱"), StringLength(60)]
        public string Email { get; set; }
        [DisplayName("Line"), StringLength(30)]
        public string Line { get; set; }
        [DisplayName("官網")]
        public string Website { get; set; }
        [DisplayName("是否啟用VIP")]
        public bool IsVIP { get; set; }
        [DisplayName("到期日"), DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true), DataType(DataType.Date)]
        public Nullable<DateTime> ExpireDate { get; set; }
        [DisplayName("累積加值月數"), Range(0, int.MaxValue)]
        public Nullable<int> AccumulatedHours { get; set; }
        [DisplayName("帳號是否啟用")]
        public bool IsAccountEnabled { get; set; }
        [DisplayName("店家介紹"), StringLength(1000)]
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
        [DisplayName("是否上架")]
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
        [DisplayName("評論日期"),DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
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
        [DisplayName("被閱覽時間"),DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
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
        /// <summary>
        /// 該團的目前狀態
        /// </summary>
        [DisplayName("狀態")]
        public string Status
        {
            get
            {
                if (DateTime.Today > PlayDate)
                    return "已過期";
                else if (IsCanceled)
                    return "已取消出團";
                else if (IsConfirmedByShop != null)
                {
                    if ((bool)IsConfirmedByShop)
                        return "訂位成功";
                    return "訂位失敗";
                }
                else if (IsRequestSent)
                    return "已送出訂位請求";
                else if (IsClosed)
                    return "已成團";
                else if (DateTime.Now > ParticipateEndDate)
                {
                    if (CurrentPlayers >= MinPlayer)
                        return "已成團";
                    return "已流團(人數不足)";
                }
                else if (CurrentPlayers >= MaxPlayer)
                    return "已額滿";
                else if (CurrentPlayers < MinPlayer)
                    return "可報名，未達成團人數";
                else
                    return "可報名，已達成團人數";
            }
        }
        [DisplayName("人數限制")]
        public string PlayerLimit
        {
            get
            {
                if (MaxPlayer == MinPlayer)
                    return $"正好{MaxPlayer}人";
                else
                    return $"{MinPlayer} ~ {MaxPlayer} 人";
            }
        }
        [DisplayName("遊戲時間")]
        public string PlayPeriod
        {
            get
            {
                return PlayDate.ToString("yyyy/MM/dd") + "<br/>" +
                    PlayBeginTime.ToString(@"hh\:mm") + "至" + PlayEndTime.ToString(@"hh\:mm");
            }
        }
        [DisplayName("當前人數")]
        public int CurrentPlayers
        {
            get
            {
                return OtherPlayers.Count + 1;
            }
        }
        /// <summary>
        /// 是否已經截止報名
        /// </summary>
        public bool IsParticipateEnded
        {
            get
            {
                return DateTime.Now > ParticipateEndDate || IsCanceled || IsClosed;
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
        [DisplayName("揪桌標題"),StringLength(15, ErrorMessage ="標題長度不得大於15個字"), Required(ErrorMessage = "標題為必填")]
        public string Title { get; set; }
        [DisplayName("報名結束時間"),DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}", ApplyFormatInEditMode = true), DataType(DataType.DateTime)]
        [Required(ErrorMessage = "報名結束時間為必填")]
        public System.DateTime ParticipateEndDate { get; set; }
        [DisplayName("其他資訊備註"),StringLength(100)]
        public string Notes { get; set; }
        [DisplayName("遊戲日期"), DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true), DataType(DataType.Date)]
        [Required(ErrorMessage = "遊戲日期為必填")]
        public DateTime PlayDate { get; set; }
        [DisplayName("開始時間"), Required(ErrorMessage = "開始時間為必填"), DataType(DataType.Time)]
        public TimeSpan PlayBeginTime { get; set; }
        [DisplayName("結束時間"), Required(ErrorMessage = "結束時間為必填"), DataType(DataType.Time)]
        public TimeSpan PlayEndTime { get; set; }
        [DisplayName("預估花費")]
        public int? EstimatedCost { get; set; }
        [DisplayName("要玩的桌遊")]
        public string Preference { get; set; }
        [DisplayName("最低人數"), Range(2, 50, ErrorMessage = "必須在2~50之間"), Required(ErrorMessage = "最低人數為必填")]
        public int MinPlayer { get; set; }
        [DisplayName("最高人數"), Range(2, int.MaxValue, ErrorMessage = "必須至少為2"), Required(ErrorMessage = "最高人數為必填")]
        public int MaxPlayer { get; set; }
        [DisplayName("是否已取消")]
        public bool IsCanceled { get; set; }
        [DisplayName("是否已提前截止")]
        public bool IsClosed { get; set; }
        [DisplayName("是否送出訂位請求")]
        public bool IsRequestSent { get; set; }
        [DisplayName("是否訂位成功")]
        public bool IsConfirmedByShop { get; set; }


    }
}