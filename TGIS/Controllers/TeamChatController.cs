using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Microsoft.Web.WebSockets;
using TGIS.Models;

namespace TGIS.Controllers
{
    //處理揪桌聊天室的控制器
    public class TeamChatController : ApiController
    {
        public HttpResponseMessage Get(string userName, string teamID, string userID)
        {
            HttpContext.Current.AcceptWebSocketRequest(new ChatWebSocketHandler(userName, teamID, userID));
            return Request.CreateResponse(HttpStatusCode.SwitchingProtocols);
        }
        //WebSocket處理器
        class ChatWebSocketHandler : WebSocketHandler
        {
            string _userName;
            string _teamID;
            string _userID;
            //字典: key為揪桌ID，value為WebSocketCollection
            static Dictionary<string, WebSocketCollection> _chatRooms = new Dictionary<string, WebSocketCollection>();
            //構造器
            public ChatWebSocketHandler(string userName, string teamID, string userID)
            {
                _userName = userName;
                _teamID = teamID;
                _userID = userID;
            }
            //覆寫OnOpen事件，鑄造新的ChatWebSocketHandler時觸發
            public override void OnOpen()
            {
                //如果從teamID能找到對應的WebSocketCollection(也就是房間)則加入，否則新開一間房
                if (_chatRooms.ContainsKey(_teamID))
                {
                     _chatRooms[_teamID].Add(this);
                }
                else
                    _chatRooms[_teamID] = new WebSocketCollection() { this };
            }
            //覆寫OnMessage事件，前端send時觸發，被觸發後會回頭觸發前端的onmessage事件
            public override void OnMessage(string message)
            {
                _chatRooms[_teamID].Broadcast(_userName + "：" + message);

                //將此訊息儲存至訊息列表
                Message m = new Message
                {
                    TeamID = _teamID,
                    MessageDate = DateTime.Now,
                    Speaker = _userID,
                    IsPrivate = true,
                    Content = message.Length > 300 ? message.Substring(0, 300) : message
                };
                var db = new TGISDBEntities();
                db.Messages.Add(m);
                db.SaveChanges();
            }
        }
    }
}
