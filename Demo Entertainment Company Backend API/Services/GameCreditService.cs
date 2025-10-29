using Demo_Entertainment_Company_Backend_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Demo_Entertainment_Company_Backend_API.Services
{
    public class GameCreditService
  {
        private readonly ApplicationDbContext _context;

      public GameCreditService(ApplicationDbContext context)
        {
      _context = context;
        }

        public async Task<User> AddCreditsAsync(int userId, int amount)
     {
       var user = await _context.Users.FindAsync(userId);
       if (user == null)
       throw new KeyNotFoundException("User not found");

      user.GameCredits += amount;
 await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> DeductCreditsAsync(int userId, int amount)
        {
 var user = await _context.Users.FindAsync(userId);
      if (user == null)
        throw new KeyNotFoundException("User not found");

            if (user.GameCredits < amount)
           throw new InvalidOperationException("Insufficient game credits");

    user.GameCredits -= amount;
          await _context.SaveChangesAsync();
            return user;
        }
    }
}