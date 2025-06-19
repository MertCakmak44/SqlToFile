using MyAppCore.Dtos;
using MyAppCore.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyAppCore.Interfaces
{
    public interface IUserService
    {   
        Task<List<UserDto>> GetAllAsync();
        Task<User> AddAsync(User user);
        Task DeleteAsync(int id);
        Task<User> GetByUsernameAsync(string username);
        Task<User> UpdateAsync(UserUpdateDto updatedUser);
    }
}
