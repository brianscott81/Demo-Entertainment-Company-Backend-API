using Demo_Entertainment_Company_Backend_API.Controllers;
using Demo_Entertainment_Company_Backend_API.Models;
using Demo_Entertainment_Company_Backend_API.Models.DTO;
using Demo_Entertainment_Company_Backend_API.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Demo_Entertainment_Company_Backend_API_Unit_Tests.Controllers;

public class UserControllerTests
{
    private Mock<IUserService> _mockUserService;
    private UserController _controller;

    [SetUp]
    public void Setup()
    {
        _mockUserService = new Mock<IUserService>();
        _controller = new UserController(_mockUserService.Object);
    }

    [Test]
    public async Task GetUsers_ReturnsOkResultWithUsers()
    {
        // Arrange
        var users = new List<User> {
            new User {
                Id = 1, Username = "user1", Email = "user1@example.com"
            },
            new User {
                Id = 2, Username = "user2", Email = "user2@example.com"
            }
        };
        _mockUserService.Setup(s => s.GetAllUsersAsync())
            .ReturnsAsync(users);

        // Act
        var result = await _controller.GetUsers();

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        var returnedUsers = okResult.Value as IEnumerable<User>;
        Assert.That(returnedUsers, Is.Not.Null);
        Assert.That(returnedUsers.Count(), Is.EqualTo(2));
    }

    [Test]
    public async Task GetUser_WithValidId_ReturnsOkResultWithUser()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Username = "testuser",
            Email = "test@example.com"
        };
        _mockUserService.Setup(s => s.GetUserByIdAsync(1))
            .ReturnsAsync(user);

        // Act
        var result = await _controller.GetUser(1);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        var returnedUser = okResult.Value as User;
        Assert.That(returnedUser, Is.Not.Null);
        Assert.That(returnedUser.Username, Is.EqualTo("testuser"));
        Assert.That(returnedUser.Email, Is.EqualTo("test@example.com"));
    }

    [Test]
    public async Task GetUser_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        _mockUserService.Setup(s => s.GetUserByIdAsync(999))
            .ReturnsAsync((User)null);

        // Act
        var result = await _controller.GetUser(999);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public async Task CreateUser_WithValidData_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var createUserDto = new CreateUserDto
        {
            Username = "newuser",
            Email = "newuser@example.com"
        };
        var createdUser = new User
        {
            Id = 1,
            Username = createUserDto.Username,
            Email = createUserDto.Email
        };
        _mockUserService.Setup(s => s.CreateUserAsync(createUserDto))
            .ReturnsAsync(createdUser);

        // Act
        var result = await _controller.CreateUser(createUserDto);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<CreatedAtActionResult>());
        var createdAtActionResult = result.Result as CreatedAtActionResult;
        Assert.That(createdAtActionResult.ActionName, Is.EqualTo(nameof(UserController.GetUser)));
        Assert.That(createdAtActionResult.RouteValues["id"], Is.EqualTo(1));
        var returnedUser = createdAtActionResult.Value as User;
        Assert.That(returnedUser, Is.Not.Null);
        Assert.That(returnedUser.Username, Is.EqualTo("newuser"));
        Assert.That(returnedUser.Email, Is.EqualTo("newuser@example.com"));
    }
}