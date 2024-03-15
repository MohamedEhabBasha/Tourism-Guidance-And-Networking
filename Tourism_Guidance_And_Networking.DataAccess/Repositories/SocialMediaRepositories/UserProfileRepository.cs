﻿
using System.Linq.Expressions;
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
                    user = await _context.Users.SingleAsync(x => x.Id == contact.AppFriendId);
                }else
                    user = await _context.Users.SingleAsync(x => x.Id == contact.AppUserId);

                UserDTO userDTO = new()
                {
                    FirstName = user.FirstName, 
                    LastName = user.LastName,
                    Address = user.Address,
                    Email = user.Email!
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
                    user = await _context.Users.SingleAsync(x => x.Id == friend.AppFriendId);
                }
                else
                    user = await _context.Users.SingleAsync(x => x.Id == friend.AppUserId);

                UserDTO userDTO = new()
                {
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Address = user.Address,
                    Email = user.Email!
                };

                users.Add(userDTO);
            }
            return users;
        }

        public async Task CreateFriendAsync(Friend friend)
        {
            await _context.Friends.AddAsync(friend);
        }
        public async Task CreateContact(Contact contact)
        {
            await _context.Contacts.AddAsync(contact);
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
