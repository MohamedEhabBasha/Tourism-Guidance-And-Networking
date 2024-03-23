
using Tourism_Guidance_And_Networking.Core.Interfaces.SocialMedia;
using Tourism_Guidance_And_Networking.Core.Models.SocialMedia;

namespace Tourism_Guidance_And_Networking.DataAccess.Repositories.SocialMediaRepositories
{
    public class PrivateChatRepository : BaseRepository<PrivateChat>, IPrivateChatRepository
    {
        private new readonly ApplicationDbContext _context;
        public PrivateChatRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<PrivateChat?> GetChatAsync (string senderId, string receiveId)
        {
            return await FindAsync(c => ((c.SenderId == senderId && c.ReceiverId == receiveId) || (c.SenderId == receiveId && c.ReceiverId == senderId)))?? null;
        }
        public PrivateChat? GetChat(string senderId, string receiveId)
        {
            return _context.PrivateChats.SingleOrDefault(c => c.SenderId == senderId && c.ReceiverId == receiveId) ?? null;
        }
        public bool DeletePrivateChat(int chatId)
        {
            var chat = _context.PrivateChats.SingleOrDefault(x => x.Id ==  chatId);

            if (chat is null) {
                return false;
            }
            Delete(chat);

            return true;
        }
        public async Task<ICollection<Message>> GetMessagesAsync(int ChatId)
        {
            return await _context.Messages
                .Where(c => c.ChatId == ChatId)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
