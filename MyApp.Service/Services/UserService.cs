using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
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
        private readonly ILogger<UserService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(BilnexDbContext context, IMapper mapper, ILogger<UserService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            var users = await _context.Users.ToListAsync();
            var username = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Bilinmeyen";
            _logger.LogInformation("{Username} adlı kullanıcı tüm kullanıcıları getirdi. Toplam: {Count}", username, users.Count);
            return _mapper.Map<List<UserDto>>(users);
        }

        public async Task<User> AddAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            var username = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Bilinmeyen";
            _logger.LogInformation("{Username} adlı kullanıcı yeni kullanıcı ekledi: {NewUser}", username, user.Username);
            return user;
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            var username = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Bilinmeyen";
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                _logger.LogInformation("{Username} adlı kullanıcı başka bir kullanıcıyı sildi. ID: {Id}, Username: {DeletedUser}", username, user.Id, user.Username);
            }
            else
            {
                _logger.LogWarning("{Username} adlı kullanıcı silinmek istenen kullanıcıyı bulamadı. ID: {Id}", username, id);
            }
        }

        public async Task<User> UpdateAsync(UserUpdateDto updatedUser)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == updatedUser.Username);
            var username = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Bilinmeyen";
            if (user == null)
            {
                _logger.LogWarning("{Username} adlı kullanıcı güncellenmek istenen kullanıcıyı bulamadı. Username: {TargetUsername}", username, updatedUser.Username);
                return null;
            }

            _mapper.Map(updatedUser, user);
            await _context.SaveChangesAsync();
            _logger.LogInformation("{Username} adlı kullanıcı bir kullanıcıyı güncelledi: {TargetUsername}", username, user.Username);
            return user;
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            var caller = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Bilinmeyen";
            if (user == null)
                _logger.LogWarning("{Username} adlı kullanıcı başka bir kullanıcıyı bulamadı. Aranan: {Target}", caller, username);
            return user;
        }
    }
}