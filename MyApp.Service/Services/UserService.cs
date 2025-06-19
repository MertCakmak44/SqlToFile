using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyAppCore.Dtos;
using MyAppCore.Entities;
using MyAppCore.Interfaces;
using MyAppData.Context;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyAppService.Services
{
    public class UserService : IUserService
    {
        private readonly BilnexDbContext _context;
        private readonly IMapper _mapper;

        public UserService(BilnexDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            var users = await _context.Users.ToListAsync();
            return _mapper.Map<List<UserDto>>(users);
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

            _mapper.Map(updatedUser, user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}
