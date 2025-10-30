using Demo_Entertainment_Company_Backend_API.Models;
using Demo_Entertainment_Company_Backend_API.Models.DTO;
using Demo_Entertainment_Company_Backend_API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Demo_Entertainment_Company_Backend_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RideTicketsController : ControllerBase
    {
        private readonly IRideTicketService _ticketService;
        private readonly IUserService _userService;

        public RideTicketsController(IRideTicketService ticketService, IUserService userService)
        {
            _ticketService = ticketService;
            _userService = userService;
        }

        [HttpPost("add")]
        public async Task<ActionResult<User>> AddTickets(TicketTransactionDto transaction)
        {
            var staffUser = await GetCurrentUser();
            if (staffUser == null || !staffUser.IsStaff)
                return Forbid();

            try
            {
                var updatedUser = await _ticketService.AddTicketsAsync(transaction.UserId, transaction.Amount);
                return Ok(updatedUser);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost("deduct")]
        public async Task<ActionResult<User>> DeductTickets(TicketTransactionDto transaction)
        {
            var staffUser = await GetCurrentUser();
            if (staffUser == null || !staffUser.IsStaff)
                return Forbid();

            try
            {
                var updatedUser = await _ticketService.DeductTicketsAsync(transaction.UserId, transaction.Amount);
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