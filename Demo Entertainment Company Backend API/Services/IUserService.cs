using Demo_Entertainment_Company_Backend_API.Models;
using Demo_Entertainment_Company_Backend_API.Models.DTO;

namespace Demo_Entertainment_Company_Backend_API.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<User> CreateUserAsync(CreateUserDto createUserDto);
    }
}