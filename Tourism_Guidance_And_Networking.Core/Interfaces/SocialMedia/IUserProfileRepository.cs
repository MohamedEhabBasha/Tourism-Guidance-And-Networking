
using System.Linq.Expressions;
using Tourism_Guidance_And_Networking.Core.DTOs.SocialMediaDTOs;
using Tourism_Guidance_And_Networking.Core.Models.SocialMedia;

namespace Tourism_Guidance_And_Networking.Core.Interfaces.SocialMedia
{
    public interface IUserProfileRepository
    {
        Task<ICollection<UserDTO>> GetAllFriends(string id);
        Task<ICollection<UserDTO>> GetAllContacts(string id);
        Task<UserProfileDTO> GetUserProfileDTOAsync(string id);
        Task<bool> IsFriendAsync(string userId, string friendId);
        Task<Friend> CreateFriendAsync(FriendDTO friendDTO);
        Task<Contact> CreateContactAsync(Contact contact);
        void DeleteFriend(string userId, string friendId);
        void DeleteContact(string userId, string friendId);
        Task<Friend?> GetFriendAsync(string userId, string friendId);
    }
}
