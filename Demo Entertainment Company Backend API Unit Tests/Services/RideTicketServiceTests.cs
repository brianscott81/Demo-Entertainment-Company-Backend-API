using Demo_Entertainment_Company_Backend_API;
using Demo_Entertainment_Company_Backend_API.Models;
using Demo_Entertainment_Company_Backend_API.Services;
using Microsoft.EntityFrameworkCore;

namespace Demo_Entertainment_Company_Backend_API_Unit_Tests.Services;

public class RideTicketServiceTests
{
    private ApplicationDbContext _context;
    private RideTicketService _ticketService;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        _context = new ApplicationDbContext(options);
        _ticketService = new RideTicketService(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task AddTicketsAsync_WithValidUserAndAmount_UpdatesTickets()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Username = "testuser",
            Email = "test@example.com",
            RideTickets = 5
        };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _ticketService.AddTicketsAsync(1, 3);

        // Assert
        Assert.That(result.RideTickets, Is.EqualTo(8));

        // Verify the changes were saved to the database
        var updatedUser = await _context.Users.FindAsync(1);
        Assert.That(updatedUser.RideTickets, Is.EqualTo(8));
    }

    [Test]
    public void AddTicketsAsync_WithInvalidUser_ThrowsKeyNotFoundException()
    {
        // Act & Assert
        Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _ticketService.AddTicketsAsync(999, 3));
    }

    [Test]
    public async Task DeductTicketsAsync_WithSufficientTickets_UpdatesBalance()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Username = "testuser",
            Email = "test@example.com",
            RideTickets = 5
        };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _ticketService.DeductTicketsAsync(1, 2);

        // Assert
        Assert.That(result.RideTickets, Is.EqualTo(3));

        // Verify the changes were saved to the database
        var updatedUser = await _context.Users.FindAsync(1);
        Assert.That(updatedUser.RideTickets, Is.EqualTo(3));
    }

    [Test]
    public void DeductTicketsAsync_WithInvalidUser_ThrowsKeyNotFoundException()
    {
        // Act & Assert
        Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _ticketService.DeductTicketsAsync(999, 2));
    }

    [Test]
    public async Task DeductTicketsAsync_WithInsufficientTickets_ThrowsInvalidOperationException()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Username = "testuser",
            Email = "test@example.com",
            RideTickets = 1
        };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(() =>
            _ticketService.DeductTicketsAsync(1, 2));
    }
}