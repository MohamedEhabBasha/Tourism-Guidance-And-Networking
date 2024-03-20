using Tourism_Guidance_And_Networking.Core.Models.SocialMedia;

namespace Tourism_Guidance_And_Networking.Core.Interfaces.SocialMedia
{
    public interface IPrivateChatRepository : IBaseRepository<PrivateChat>
    {
        Task<PrivateChat?> GetChatAsync(string senderId, string receiveId);
        PrivateChat? GetChat(string senderId, string receiveId);
        bool DeletePrivateChat(int chatId);
        Task<ICollection<Message>> GetMessagesAsync(int ChatId);
    }
}
