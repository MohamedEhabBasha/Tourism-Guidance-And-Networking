﻿
using System.Linq.Expressions;
using Tourism_Guidance_And_Networking.Core.Models.SocialMedia;

namespace Tourism_Guidance_And_Networking.Core.Interfaces.SocialMedia
{
    public interface IUserProfileRepository
    {
        Task<ICollection<UserDTO>> GetAllFriends(string id);
        Task<ICollection<UserDTO>> GetAllContacts(string id);
        Task CreateFriendAsync(Friend friend);
        Task CreateContact(Contact contact);
        void DeleteFriend(string userId, string friendId);
        void DeleteContact(string userId, string friendId);
        Task<Friend?> GetFriendAsync(string userId, string friendId);
    }
}
