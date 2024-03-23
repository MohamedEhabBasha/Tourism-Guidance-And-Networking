
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Linq.Expressions;
using Tourism_Guidance_And_Networking.Core.DTOs.SocialMediaDTOs;
using Tourism_Guidance_And_Networking.Core.Interfaces.SocialMedia;
using Tourism_Guidance_And_Networking.Core.Models.SocialMedia;

namespace Tourism_Guidance_And_Networking.DataAccess.Repositories.SocialMediaRepositories
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private  readonly ApplicationDbContext _context;
        public UserProfileRepository(ApplicationDbContext context) 
        {
            _context = context;
        }
        public async Task<ICollection<UserDTO>> GetAllContacts(string id)
        {
            var contacts = await _context.Contacts.Where(x => (x.AppUserId == id || x.AppFriendId == id)).AsNoTracking().ToListAsync();
            List<UserDTO> users = new ();
            foreach (var contact in contacts)
            {
                var user = new ApplicationUser();

                if(contact.AppUserId == id)
                {
                    user = await _context.ApplicationUsers.SingleAsync(x => x.Id == contact.AppFriendId);
                }else
                    user = await _context.ApplicationUsers.SingleAsync(x => x.Id == contact.AppUserId);

                UserDTO userDTO = new()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Address = user.Address,
                    Email = user.Email!,
                    PhoneNumber = user.PhoneNumber,
                    UserName = user.UserName
                };
                users.Add(userDTO);
            }
            return users;
        }

        public async Task<ICollection<UserDTO>> GetAllFriends(string id)
        {
            var friends = await _context.Friends.Where(x => (x.AppUserId == id || x.AppFriendId == id)).AsNoTracking().ToListAsync();
            List<UserDTO> users = new();
            foreach (var friend in friends)
            {
                var user = new ApplicationUser();

                if (friend.AppUserId == id)
                {
                    user = await _context.ApplicationUsers.SingleAsync(x => x.Id == friend.AppFriendId);
                }
                else
                    user = await _context.ApplicationUsers.SingleAsync(x => x.Id == friend.AppUserId);

                UserDTO userDTO = new()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Address = user.Address,
                    Email = user.Email!,
                    PhoneNumber = user.PhoneNumber,
                    UserName = user.UserName
                };

                users.Add(userDTO);
            }
            return users;
        }
        public async Task<UserProfileDTO> GetUserProfileDTOAsync(string userId)
        {
            var user = await _context.ApplicationUsers.SingleAsync(x=> x.Id == userId);
            UserDTO userDTO = new()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Address = user.Address,
                Email = user.Email!,
                PhoneNumber = user.PhoneNumber,
                UserName = user.UserName
            };
            var friends = await GetAllFriends(user.Id);

            UserProfileDTO userProfileDTO = new() { 
                User = userDTO,
                Friends = friends.ToList()
            };
            return userProfileDTO;
        }        
        public async Task<bool> IsFriendAsync(string userId,string friendId)

        {
            var user = await _context.ApplicationUsers.SingleAsync(x => x.Id == userId);
            var friend = await _context.ApplicationUsers.SingleAsync(x => x.Id == friendId);
            var isFriend = await _context.Friends.SingleOrDefaultAsync(x => ((x.AppUserId == user.Id && x.AppFriendId == friend.Id) || (x.AppUserId == friend.Id && x.AppFriendId == user.Id)));
            if(isFriend is null)
            {
                return false;
            }

            return true;
        }
        public async Task<Friend> CreateFriendAsync(FriendDTO friendDTO)
        {
            Friend friend = new()
            {
                AppUserId = friendDTO.UserId,
                AppFriendId = friendDTO.FriendId
            };
            //await _context.Friends.AddAsync(friend);
            return await AddAsync(friend);
        }
        private async Task<Friend> AddAsync(Friend entity)
        {
            await _context.Friends.AddAsync(entity);
            return entity;
        }
        public async Task<Contact> CreateContactAsync(Contact contact)
        {
            await _context.Contacts.AddAsync(contact);
            return contact;
        }
        public void DeleteFriend(string userId,string friendId)
        {
            var friend = _context.Friends.Single(x => ((x.AppUserId == userId && x.AppFriendId == friendId) || (x.AppUserId == friendId && x.AppFriendId == userId)));

            _context.Friends.Remove(friend);
        }
        public void DeleteContact(string userId, string friendId)
        {
            var contact = _context.Contacts.Single(x => ((x.AppUserId == userId && x.AppFriendId == friendId) || (x.AppUserId == friendId && x.AppFriendId == userId)));

            _context.Contacts.Remove(contact);
        }
        public async Task<Friend?> GetFriendAsync(string userId,string friendId)
        {
            return await _context.Friends.SingleOrDefaultAsync(x => ((x.AppUserId == userId && x.AppFriendId == friendId) || (x.AppUserId == friendId && x.AppFriendId == userId)));
        }
    }
}
