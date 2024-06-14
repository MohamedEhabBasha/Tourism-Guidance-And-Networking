using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Tourism_Guidance_And_Networking.Web.Services.Hubs
{
    [EnableCors("AllowAnyOrigin")]
    public class ChatHub : Hub
    {
        private readonly ApplicationDbContext _context;

        public ChatHub(ApplicationDbContext context)
        {
            _context = context;
        }
        /* public async Task SendPrivateMessageAsync(string receiverId,string message)
         {
             var senderId = Context.User!.FindFirstValue(ClaimTypes.NameIdentifier);
             var senderName = _context.Users.FirstOrDefault(u => u.Id == senderId)!.UserName;
             await Clients.Users(senderId!, receiverId).SendAsync("sendMessage",senderName,message);
         }*/
        public string GetConnectionId() => Context.ConnectionId;
    }
}
