using eTutoring.Repositories;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace eTutoring.Hubs
{
    public class ChatHub : Hub
    {
        private readonly AuthRepository _repo = new AuthRepository();

        public async Task Send(string receiverId, string message)
        {
            var senderId = Context.User.Identity.GetUserId();
            var senderName = await _repo.FindUserById(senderId);
            var receiverName = await _repo.FindUserById(receiverId);
            await Clients.All.SendAsync("newMessage", senderName, receiverName, message);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repo.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}