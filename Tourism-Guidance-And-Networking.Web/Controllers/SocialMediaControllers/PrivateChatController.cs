
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Tourism_Guidance_And_Networking.Core.DTOs.SocialMediaDTOs;
using Tourism_Guidance_And_Networking.Core.Models.SocialMedia;
using Tourism_Guidance_And_Networking.Web.Services.Hubs;

namespace Tourism_Guidance_And_Networking.Web.Controllers.SocialMediaControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrivateChatController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<ChatHub> _chatHub;

        public PrivateChatController(IUnitOfWork unitOfWork, IHubContext<ChatHub> chatHub)
        {
            _unitOfWork = unitOfWork;
            _chatHub = chatHub;
        }
        [HttpPost("/getPrivateChatMessages")]
        public async Task<IActionResult> GetPrivateChatMessages([FromBody] PrivateChatDTO privateChatDTO)
        {
            if (privateChatDTO is null || privateChatDTO.SenderEmail is null || privateChatDTO.ReceiverEmail is null)
                return BadRequest();

            var sender = await _unitOfWork.ApplicationUsers.FindAsync(x => x.Email == privateChatDTO.SenderEmail);
            var receiver = await _unitOfWork.ApplicationUsers.FindAsync(x => x.Email == privateChatDTO.ReceiverEmail);

            if (sender is null || receiver is null)
                return NotFound();

            var chat = await _unitOfWork.PrivateChats.GetChatAsync(sender.Id, receiver.Id);
            
            if (chat == null) 
            {
                PrivateChat privateChat = new()
                {
                    SenderId = sender.Id,
                    ReceiverId = receiver.Id
                };

                await _unitOfWork.PrivateChats.AddAsync(privateChat);

                if (!(_unitOfWork.Complete() > 0))
                {
                    ModelState.AddModelError("", "Something Went Wrong While Saving");
                    return StatusCode(500, ModelState);
                }
                privateChatDTO.ChatId = privateChat.Id;
            }
            else
            {
                privateChatDTO.ChatId = chat.Id;
                privateChatDTO.Messages = await _unitOfWork.PrivateChats.GetMessagesAsync(chat.Id);
            }
            return StatusCode(200, privateChatDTO);
        }

        [HttpPost("/sendPrivateMessage")]
        public async Task<IActionResult> SendPrivateMessasgeAsync([FromBody] MessageDTO messageDTO)
        {
            if(messageDTO is null)
            {
                return BadRequest();
            }

            var sender = await _unitOfWork.ApplicationUsers.FindAsync(x => x.Email == messageDTO.SenderEmail);
            var receiver = await _unitOfWork.ApplicationUsers.FindAsync(x => x.Email == messageDTO.ReceiverEmail);

            if ((await _unitOfWork.PrivateChats.GetByIdAsync(messageDTO.ChatId) is null) ||
                sender is null || receiver is null)
            {
                return NotFound();
            }

            Message newMessage = new() { 
                Text = messageDTO.Text,
                ApplicationUserId = sender.Id,
                ChatId = messageDTO.ChatId

            };

            await _unitOfWork.Messages.AddAsync(newMessage);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something Went Wrong While Saving");
                return StatusCode(500, ModelState);
            }

            await _chatHub.Clients
                .Users(sender.Id, receiver.Id)
                .SendAsync("sendPrivateMessage", messageDTO.Text);

            return Ok(newMessage);
        }
        [HttpDelete("{chatId}")]
        public IActionResult DeletePrivateChat([FromRoute] int chatId)
        {

            var chat = _unitOfWork.PrivateChats.GetById(chatId);

            if (chat is null)
            {
                return NotFound();
            }
            _unitOfWork.PrivateChats.DeletePrivateChat(chatId);

            if (!(_unitOfWork.Complete() > 0))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Deleted Successfully");
        }
    }
}
