using Demo_Entertainment_Company_Backend_API.Models;

namespace Demo_Entertainment_Company_Backend_API.Services
{
    public interface IGameCreditService
    {
        Task<User> AddCreditsAsync(int userId, int amount);
        Task<User> DeductCreditsAsync(int userId, int amount);
    }
}