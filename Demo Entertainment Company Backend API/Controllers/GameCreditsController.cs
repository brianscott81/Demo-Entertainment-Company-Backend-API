using Demo_Entertainment_Company_Backend_API.Models;
using Demo_Entertainment_Company_Backend_API.Models.DTO;
using Demo_Entertainment_Company_Backend_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Demo_Entertainment_Company_Backend_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GameCreditsController : ControllerBase
    {
        private readonly GameCreditService _creditService;
        private readonly UserService _userService;

        public GameCreditsController(GameCreditService creditService, UserService userService)
        {
            _creditService = creditService;
            _userService = userService;
        }

        [HttpPost("add")]
        public async Task<ActionResult<User>> AddCredits(CreditTransactionDto transaction)
        {
            var staffUser = await GetCurrentUser();
            if (staffUser == null || !staffUser.IsStaff)
                return Forbid();

            try
            {
                var updatedUser = await _creditService.AddCreditsAsync(transaction.UserId, transaction.Amount);
                return Ok(updatedUser);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("deduct")]
        public async Task<ActionResult<User>> DeductCredits(CreditTransactionDto transaction)
        {
            var staffUser = await GetCurrentUser();
            if (staffUser == null || !staffUser.IsStaff)
                return Forbid();

            try
            {
                var updatedUser = await _creditService.DeductCreditsAsync(transaction.UserId, transaction.Amount);
                return Ok(updatedUser);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // TODO: Replace this with proper authentication
        private async Task<User?> GetCurrentUser()
        {
            // This is a placeholder - replace with actual user authentication
            var userId = 1; // Default to user ID 1 for demo purposes
            return await _userService.GetUserByIdAsync(userId);
        }
    }
}