using Demo_Entertainment_Company_Backend_API.Models;

namespace Demo_Entertainment_Company_Backend_API.Services
{
    public interface IRideTicketService
    {
        Task<User> AddTicketsAsync(int userId, int amount);
        Task<User> DeductTicketsAsync(int userId, int amount);
    }
}