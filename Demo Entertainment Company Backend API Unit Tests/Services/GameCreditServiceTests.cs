using Demo_Entertainment_Company_Backend_API;
using Demo_Entertainment_Company_Backend_API.Models;
using Demo_Entertainment_Company_Backend_API.Services;
using Microsoft.EntityFrameworkCore;

namespace Demo_Entertainment_Company_Backend_API_Unit_Tests.Services;

public class GameCreditServiceTests
{
    private ApplicationDbContext _context;
    private GameCreditService _creditService;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        _context = new ApplicationDbContext(options);
        _creditService = new GameCreditService(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task AddCreditsAsync_WithValidUserAndAmount_UpdatesCredits()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Username = "testuser",
            Email = "test@example.com",
            GameCredits = 100
        };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _creditService.AddCreditsAsync(1, 50);

        // Assert
        Assert.That(result.GameCredits, Is.EqualTo(150));

        // Verify the changes were saved to the database
        var updatedUser = await _context.Users.FindAsync(1);
        Assert.That(updatedUser.GameCredits, Is.EqualTo(150));
    }

    [Test]
    public void AddCreditsAsync_WithInvalidUser_ThrowsKeyNotFoundException()
    {
        // Act & Assert
        Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _creditService.AddCreditsAsync(999, 50));
    }

    [Test]
    public async Task DeductCreditsAsync_WithSufficientCredits_UpdatesBalance()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Username = "testuser",
            Email = "test@example.com",
            GameCredits = 100
        };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _creditService.DeductCreditsAsync(1, 50);

        // Assert
        Assert.That(result.GameCredits, Is.EqualTo(50));

        // Verify the changes were saved to the database
        var updatedUser = await _context.Users.FindAsync(1);
        Assert.That(updatedUser.GameCredits, Is.EqualTo(50));
    }

    [Test]
    public void DeductCreditsAsync_WithInvalidUser_ThrowsKeyNotFoundException()
    {
        // Act & Assert
        Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _creditService.DeductCreditsAsync(999, 50));
    }

    [Test]
    public async Task DeductCreditsAsync_WithInsufficientCredits_ThrowsInvalidOperationException()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Username = "testuser",
            Email = "test@example.com",
            GameCredits = 30
        };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(() =>
            _creditService.DeductCreditsAsync(1, 50));
    }
}
