using Demo_Entertainment_Company_Backend_API;
using Demo_Entertainment_Company_Backend_API.Models;
using Demo_Entertainment_Company_Backend_API.Models.DTO;
using Demo_Entertainment_Company_Backend_API.Services;
using Microsoft.EntityFrameworkCore;

namespace Demo_Entertainment_Company_Backend_API_Unit_Tests.Services;

public class UserServiceTests
{
    private ApplicationDbContext _context;
    private UserService _userService;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
            .Options;

        _context = new ApplicationDbContext(options);
        _userService = new UserService(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task GetAllUsersAsync_ReturnsAllUsers()
    {
        // Arrange
        var users = new List<User>
        {
            new User { Id = 1, Username = "user1", Email = "user1@example.com" },
            new User { Id = 2, Username = "user2", Email = "user2@example.com" }
        };
        await _context.Users.AddRangeAsync(users);
        await _context.SaveChangesAsync();

        // Act
        var result = await _userService.GetAllUsersAsync();

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(2));
        Assert.That(result.Select(u => u.Username), Does.Contain("user1"));
        Assert.That(result.Select(u => u.Username), Does.Contain("user2"));
    }

    [Test]
    public async Task GetUserByIdAsync_WithValidId_ReturnsUser()
    {
        // Arrange
        var user = new User { Id = 1, Username = "testuser", Email = "test@example.com" };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        // Act
        var result = await _userService.GetUserByIdAsync(1);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Username, Is.EqualTo("testuser"));
        Assert.That(result.Email, Is.EqualTo("test@example.com"));
    }

    [Test]
    public async Task GetUserByIdAsync_WithInvalidId_ReturnsNull()
    {
        // Act
        var result = await _userService.GetUserByIdAsync(999);

        // Assert
        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task CreateUserAsync_WithValidData_CreatesAndReturnsUser()
    {
        // Arrange
        var createUserDto = new CreateUserDto
        {
            Username = "newuser",
            Email = "newuser@example.com"
        };

        // Act
        var result = await _userService.CreateUserAsync(createUserDto);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Username, Is.EqualTo("newuser"));
        Assert.That(result.Email, Is.EqualTo("newuser@example.com"));
        Assert.That(result.Id, Is.GreaterThan(0));

        // Verify the user was actually saved to the database
        var savedUser = await _context.Users.FindAsync(result.Id);
        Assert.That(savedUser, Is.Not.Null);
        Assert.That(savedUser.Username, Is.EqualTo("newuser"));
    }
}
