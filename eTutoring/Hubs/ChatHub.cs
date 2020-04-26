using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace eTutoring.Hubs
{
    public class ChatHub : Hub
    {
        public void Subscribe(int chatRoomId)
        {
            Groups.Add(Context.ConnectionId, chatRoomId.ToString());
        }

        public void Unsubscribe(int chatRoomId)
        {
            Groups.Remove(Context.ConnectionId, chatRoomId.ToString());
        }
    }
}