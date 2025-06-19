using Microsoft.EntityFrameworkCore;
using MyAppCore.Dtos;
using MyAppCore.Entities;
using MyAppCore.Interfaces;
using MyAppData.Context;

namespace MyAppService.Services
{
    public class UserService : IUserService
    {
        private readonly BilnexDbContext _context;

        public UserService(BilnexDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<User> UpdateAsync(UserUpdateDto updatedUser)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == updatedUser.Username);
            if (user == null)
                return null;

            user.Password = updatedUser.Password;
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return user;
        }


        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}
