using Demo_Entertainment_Company_Backend_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo_Entertainment_Company_Backend_API.Services
{
    public class RideTicketService
    {
        private readonly ApplicationDbContext _context;

        public RideTicketService(ApplicationDbContext context)
        {
   _context = context;
     }

 public async Task<User> AddTicketsAsync(int userId, int amount)
        {
        var user = await _context.Users.FindAsync(userId);
            if (user == null)
              throw new KeyNotFoundException("User not found");

      user.RideTickets += amount;
         await _context.SaveChangesAsync();
    return user;
        }

        public async Task<User> DeductTicketsAsync(int userId, int amount)
        {
      var user = await _context.Users.FindAsync(userId);
            if (user == null)
         throw new KeyNotFoundException("User not found");

            if (user.RideTickets < amount)
      throw new InvalidOperationException("Insufficient ride tickets");

        user.RideTickets -= amount;
  await _context.SaveChangesAsync();
    return user;
  }
    }
}