using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Microsoft.Web.WebSockets;
using Newtonsoft.Json;
using TGIS.Models;

namespace TGIS.Controllers
{
    //處理揪桌聊天室的控制器
    public class TeamChatController : ApiController
    {
        public HttpResponseMessage Get(string userName, string teamID, string userID, bool isPrivate)
        {
            HttpContext.Current.AcceptWebSocketRequest(new ChatWebSocketHandler(userName, teamID, userID, isPrivate));
            return Request.CreateResponse(HttpStatusCode.SwitchingProtocols);
        }
        //WebSocket處理器
        class ChatWebSocketHandler : WebSocketHandler
        {
            string _userName;
            string _teamID;
            string _userID;
            string _roomKey;
            bool _isPrivate;
            //字典: key為揪桌ID，value為WebSocketCollection
            static Dictionary<string, WebSocketCollection> _chatRooms = new Dictionary<string, WebSocketCollection>();
            //構造器
            public ChatWebSocketHandler(string userName, string teamID, string userID, bool isPrivate)
            {
                _userName = userName;
                _teamID = teamID;
                _userID = userID;
                _isPrivate = isPrivate;
                //使用_roomKey來區分房間，如果是跟店家聯繫的房間則在_roomKey後加入S
                _roomKey = isPrivate ? teamID + "S" : teamID;
            }
            //覆寫OnOpen事件，鑄造新的ChatWebSocketHandler時觸發
            public override void OnOpen()
            {
                //如果從teamID能找到對應的WebSocketCollection(也就是房間)則加入，否則新開一間房
                if (_chatRooms.ContainsKey(_roomKey))
                {
                     _chatRooms[_roomKey].Add(this);
                }
                else
                    _chatRooms[_roomKey] = new WebSocketCollection() { this };
            }
            //覆寫OnMessage事件，前端send時觸發，被觸發後會回頭觸發前端的onmessage事件
            public override void OnMessage(string message)
            {
                //將訊息的相關資訊製成Json檔後傳出
                _chatRooms[_roomKey].Broadcast(
                    JsonConvert.SerializeObject(new {
                        Message = message,
                        Time = DateTime.Now.ToString("MM/dd hh:mm"),
                        UserID = _userID,
                        UserName = _userName
                    })
                );

                //將此訊息儲存至訊息列表
                Message m = new Message
                {
                    TeamID = _teamID,
                    MessageDate = DateTime.Now,
                    Speaker = _userID,
                    IsPrivate = _isPrivate,
                    Content = message.Length > 300 ? message.Substring(0, 300) : message
                };
                var db = new TGISDBEntities();
                db.Messages.Add(m);
                db.SaveChanges();
            }
        }
    }
}
