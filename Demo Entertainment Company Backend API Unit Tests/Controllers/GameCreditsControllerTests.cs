using Demo_Entertainment_Company_Backend_API.Controllers;
using Demo_Entertainment_Company_Backend_API.Models;
using Demo_Entertainment_Company_Backend_API.Models.DTO;
using Demo_Entertainment_Company_Backend_API.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Demo_Entertainment_Company_Backend_API_Unit_Tests.Controllers;

public class GameCreditsControllerTests
{
    private Mock<IGameCreditService> _mockCreditService;
    private Mock<IUserService> _mockUserService;
    private GameCreditsController _controller;

    [SetUp]
    public void Setup()
    {
        _mockCreditService = new Mock<IGameCreditService>();
        _mockUserService = new Mock<IUserService>();
        _controller = new GameCreditsController(_mockCreditService.Object, _mockUserService.Object);
    }

    [Test]
    public async Task AddCredits_WithValidStaffUser_ReturnsOkResult()
    {
        // Arrange
        var staffUser = new User { Id = 1, IsStaff = true };
        var transaction = new CreditTransactionDto { UserId = 2, Amount = 100 };
        var updatedUser = new User { Id = 2, GameCredits = 100 };

        _mockUserService.Setup(s => s.GetUserByIdAsync(1))
            .ReturnsAsync(staffUser);
        _mockCreditService.Setup(s => s.AddCreditsAsync(transaction.UserId, transaction.Amount))
            .ReturnsAsync(updatedUser);

        // Act
        var result = await _controller.AddCredits(transaction);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        var returnedUser = okResult.Value as User;
        Assert.That(returnedUser.GameCredits, Is.EqualTo(100));
    }

    [Test]
    public async Task AddCredits_WithNonStaffUser_ReturnsForbid()
    {
        // Arrange
        var nonStaffUser = new User { Id = 1, IsStaff = false };
        var transaction = new CreditTransactionDto { UserId = 2, Amount = 100 };

        _mockUserService.Setup(s => s.GetUserByIdAsync(1))
            .ReturnsAsync(nonStaffUser);

        // Act
        var result = await _controller.AddCredits(transaction);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<ForbidResult>());
    }

    [Test]
    public async Task AddCredits_WithInvalidUser_ReturnsNotFound()
    {
        // Arrange
        var staffUser = new User { Id = 1, IsStaff = true };
        var transaction = new CreditTransactionDto { UserId = 2, Amount = 100 };

        _mockUserService.Setup(s => s.GetUserByIdAsync(1))
            .ReturnsAsync(staffUser);
        _mockCreditService.Setup(s => s.AddCreditsAsync(transaction.UserId, transaction.Amount))
            .ThrowsAsync(new KeyNotFoundException());

        // Act
        var result = await _controller.AddCredits(transaction);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public async Task DeductCredits_WithValidStaffUser_ReturnsOkResult()
    {
        // Arrange
        var staffUser = new User { Id = 1, IsStaff = true };
        var transaction = new CreditTransactionDto { UserId = 2, Amount = 50 };
        var updatedUser = new User { Id = 2, GameCredits = 50 };

        _mockUserService.Setup(s => s.GetUserByIdAsync(1))
            .ReturnsAsync(staffUser);
        _mockCreditService.Setup(s => s.DeductCreditsAsync(transaction.UserId, transaction.Amount))
            .ReturnsAsync(updatedUser);

        // Act
        var result = await _controller.DeductCredits(transaction);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        var returnedUser = okResult.Value as User;
        Assert.That(returnedUser.GameCredits, Is.EqualTo(50));
    }

    [Test]
    public async Task DeductCredits_WithNonStaffUser_ReturnsForbid()
    {
        // Arrange
        var nonStaffUser = new User { Id = 1, IsStaff = false };
        var transaction = new CreditTransactionDto { UserId = 2, Amount = 50 };

        _mockUserService.Setup(s => s.GetUserByIdAsync(1))
            .ReturnsAsync(nonStaffUser);

        // Act
        var result = await _controller.DeductCredits(transaction);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<ForbidResult>());
    }

    [Test]
    public async Task DeductCredits_WithInvalidUser_ReturnsNotFound()
    {
        // Arrange
        var staffUser = new User { Id = 1, IsStaff = true };
        var transaction = new CreditTransactionDto { UserId = 2, Amount = 50 };

        _mockUserService.Setup(s => s.GetUserByIdAsync(1))
            .ReturnsAsync(staffUser);
        _mockCreditService.Setup(s => s.DeductCreditsAsync(transaction.UserId, transaction.Amount))
            .ThrowsAsync(new KeyNotFoundException());

        // Act
        var result = await _controller.DeductCredits(transaction);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public async Task DeductCredits_WithInsufficientCredits_ReturnsBadRequest()
    {
        // Arrange
        var staffUser = new User { Id = 1, IsStaff = true };
        var transaction = new CreditTransactionDto { UserId = 2, Amount = 50 };

        _mockUserService.Setup(s => s.GetUserByIdAsync(1))
            .ReturnsAsync(staffUser);
        _mockCreditService.Setup(s => s.DeductCreditsAsync(transaction.UserId, transaction.Amount))
            .ThrowsAsync(new InvalidOperationException("Insufficient game credits"));

        // Act
        var result = await _controller.DeductCredits(transaction);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.That(badRequestResult.Value, Is.EqualTo("Insufficient game credits"));
    }
}
