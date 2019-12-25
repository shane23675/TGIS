using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web.Mvc;
using System.Text.RegularExpressions;

namespace TGIS.Models
{
    //這個類別是用來放一些在各個Controller都可能用到的函數
    //方法請都設定為static
    public static class UsefulTools
    {
        static TGISDBEntities db = new TGISDBEntities();

        //取得字串類型流水號的方法(參數：dataTable為資料表, n為ID前面文字的數量)
        //使用範例：UsefulTools.GetNextID(db.TableGames, 1)  取得桌遊主檔的下一個ID
        public static string GetNextID<T>(IEnumerable<T> dataTable, int n)
        {
            //取得傳入的資料表的最後一筆資料
            object lastData = dataTable.ToList()[dataTable.Count() - 1];
            //使用反射取得該筆資料的ID屬性資訊
            PropertyInfo info = lastData.GetType().GetProperty("ID");
            //取得最後一筆資料的ID
            string lastID = (string)(info.GetValue(lastData));
            //取得前綴文字
            string prefix = lastID.Substring(0, n);
            //取得後面編號字串
            string IDStr = lastID.Substring(n, lastID.Length - prefix.Length);
            //轉換成數字
            int num = int.Parse(IDStr);
            //編到下一號
            num++;
            //取得當前num的位數
            int digit = num.ToString().Length;
            //前面補零的部分
            string zeroFill = "";
            for (int i = 0; i < IDStr.Length-digit; i++)
            {
                zeroFill += "0";
            }
            //拼接成新ID：前綴文字 + 補零 + 編號
            return prefix + zeroFill + num;
        }
        
        //更新資料物件的方法(可在Edit Action使用)
        //參數：objOld 更新前的物件  objNew 更新後的物件
        //使用範例：UsefulTools.Update(oldTableGame, newTableGame)
        public static void Update<T>(T objOld, T objNew)
        {
            Type t = typeof(T);
            PropertyInfo[] infos = t.GetProperties();
            foreach (PropertyInfo info in infos)
            {
                //若為導覽屬性則跳過
                if (info.GetMethod.IsVirtual)
                    continue;
                info.SetValue(objOld, info.GetValue(objNew));
            }
        }


        //從db.Tags取得selectList的方法(參數keyword為要尋找的Tag的ID字首)
        public static List<SelectListItem> GetSelectListFromTags(char keyword)
        {
            List<SelectListItem> selectList = new List<SelectListItem>();
            foreach (Tag t in db.Tags)
            {
                if (t.ID[0] == keyword)
                    selectList.Add(new SelectListItem { Text = t.TagName, Value = t.ID });
            }
            return selectList;
        }

        /// <summary>
        /// 註冊時驗證帳號或密碼的方法
        /// </summary>
        /// <param name="target">驗證目標(帳號或密碼)</param>
        /// <param name="callback">回調函數，會將錯誤訊息傳入其中</param>
        /// <param name="isPassword">驗證目標為密碼則為true</param>
        /// <param name="isStrictMode">嚴格模式：需要大小寫字母</param>
        /// <returns>是否通過驗證</returns>
        public static bool RegisterValidate(string target, Action<string> callback, bool isPassword, bool isStrictMode)
        {
            bool isValid = true;
            int minLength = isPassword ? 8 : 6;
            Regex notWord = new Regex(@"[^A-Za-z0-9_]");

            if (target.Length < minLength || target.Length > 20)
            {
                isValid = false;
                callback($"長度需在{minLength}-20位之間");
            }
            if (notWord.IsMatch(target))
            {
                isValid = false;
                callback("只能由英數字及底線符號組成，請勿輸入中文字元或特殊符號、空白等等");
            }
            //傳入的是密碼則進行下列判斷
            if (isPassword)
            {
                Regex hasAlphabet = new Regex(@"[a-zA-Z]");
                Regex hasNumber = new Regex(@"\d");
                Regex hasLowerAlphabet = new Regex(@"[a-z]");
                Regex hasUpperAlphabet = new Regex(@"[A-Z]");

                if (!hasAlphabet.IsMatch(target))
                {
                    isValid = false;
                    callback("必須包含英文字母");
                }
                if (!hasNumber.IsMatch(target))
                {
                    isValid = false;
                    callback("必須包含數字");
                }
                if (isStrictMode && (!hasUpperAlphabet.IsMatch(target) || !hasLowerAlphabet.IsMatch(target)))
                {
                    isValid = false;
                    callback("必須包含大小寫英文字母");
                }
            }
            return isValid;
        }
        /// <summary>
        /// 新增一筆PlayerPointDetail資料
        /// </summary>
        /// <param name="playerID">玩家ID</param>
        /// <param name="cause">變動原因</param>
        /// <param name="amount">變動數量(增加為正，減少為負)</param>
        public static void PointRecord(string playerID,string cause,int amount)
        {
            PlayerPointDetail ppd = new PlayerPointDetail();
            ppd.PlayerID = playerID;
            ppd.Cause = cause;
            ppd.ChangedAmount = amount;
            ppd.ChangedDate = DateTime.Now;
            db.PlayerPointDetails.Add(ppd);
            db.SaveChanges();
        }

        //忘記密碼時的臨時密碼
        public static string CreateNewPwd() 
        {
            string refWord = "abcdefghijkmnpqrstuvwxyzABCDEFGHJKLMNPQRSTUVWXYZ";
            string refNumber = "123456789";
            char[] newPwd= new char[8];
            Random r = new Random();
            for(int i = 0; i < 4; i++)
            {
                newPwd[i] = refWord[r.Next(0, refWord.Length - 1)];
                newPwd[i+4] = refNumber[r.Next(0, refNumber.Length - 1)];
            }
            string pwd = new string(newPwd);
            return pwd;
        }

        /// <summary>
        /// 從使用者ID取得使用者名稱
        /// </summary>
        /// <param name="ID">使用者ID</param>
        /// <returns>使用者名稱</returns>
        public static string GetUserNameByID(string ID)
        {
            Player p = db.Players.Find(ID);
            Shop s;
            if (p == null)
                s = db.Shops.Find(ID);
            else
                return p.NickName;
            if (s == null)
                return "未知使用者";
            else
                return s.ShopName;
        }
    }
}