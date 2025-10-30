using Demo_Entertainment_Company_Backend_API.Controllers;
using Demo_Entertainment_Company_Backend_API.Models;
using Demo_Entertainment_Company_Backend_API.Models.DTO;
using Demo_Entertainment_Company_Backend_API.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Demo_Entertainment_Company_Backend_API_Unit_Tests.Controllers;

public class RideTicketsControllerTests
{
    private Mock<IRideTicketService> _mockTicketService;
    private Mock<IUserService> _mockUserService;
    private RideTicketsController _controller;

    [SetUp]
    public void Setup()
    {
        _mockTicketService = new Mock<IRideTicketService>();
        _mockUserService = new Mock<IUserService>();
        _controller = new RideTicketsController(_mockTicketService.Object, _mockUserService.Object);
    }

    [Test]
    public async Task AddTickets_WithValidStaffUser_ReturnsOkResult()
    {
        // Arrange
        var staffUser = new User
        {
            Id = 1,
            IsStaff = true
        };
        var transaction = new TicketTransactionDto
        {
            UserId = 2,
            Amount = 5
        };
        var updatedUser = new User
        {
            Id = 2,
            RideTickets = 5
        };

        _mockUserService.Setup(s => s.GetUserByIdAsync(1))
            .ReturnsAsync(staffUser);
        _mockTicketService.Setup(s => s.AddTicketsAsync(transaction.UserId, transaction.Amount))
            .ReturnsAsync(updatedUser);

        // Act
        var result = await _controller.AddTickets(transaction);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        var returnedUser = okResult.Value as User;
        Assert.That(returnedUser.RideTickets, Is.EqualTo(5));
    }

    [Test]
    public async Task AddTickets_WithNonStaffUser_ReturnsForbid()
    {
        // Arrange
        var nonStaffUser = new User
        {
            Id = 1,
            IsStaff = false
        };
        var transaction = new TicketTransactionDto
        {
            UserId = 2,
            Amount = 5
        };

        _mockUserService.Setup(s => s.GetUserByIdAsync(1))
            .ReturnsAsync(nonStaffUser);

        // Act
        var result = await _controller.AddTickets(transaction);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<ForbidResult>());
    }

    [Test]
    public async Task AddTickets_WithInvalidUser_ReturnsNotFound()
    {
        // Arrange
        var staffUser = new User
        {
            Id = 1,
            IsStaff = true
        };
        var transaction = new TicketTransactionDto
        {
            UserId = 2,
            Amount = 5
        };

        _mockUserService.Setup(s => s.GetUserByIdAsync(1))
            .ReturnsAsync(staffUser);
        _mockTicketService.Setup(s => s.AddTicketsAsync(transaction.UserId, transaction.Amount))
            .ThrowsAsync(new KeyNotFoundException());

        // Act
        var result = await _controller.AddTickets(transaction);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public async Task DeductTickets_WithValidStaffUser_ReturnsOkResult()
    {
        // Arrange
        var staffUser = new User
        {
            Id = 1,
            IsStaff = true
        };
        var transaction = new TicketTransactionDto
        {
            UserId = 2,
            Amount = 3
        };
        var updatedUser = new User
        {
            Id = 2,
            RideTickets = 2
        };

        _mockUserService.Setup(s => s.GetUserByIdAsync(1))
            .ReturnsAsync(staffUser);
        _mockTicketService.Setup(s => s.DeductTicketsAsync(transaction.UserId, transaction.Amount))
            .ReturnsAsync(updatedUser);

        // Act
        var result = await _controller.DeductTickets(transaction);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<OkObjectResult>());
        var okResult = result.Result as OkObjectResult;
        var returnedUser = okResult.Value as User;
        Assert.That(returnedUser.RideTickets, Is.EqualTo(2));
    }

    [Test]
    public async Task DeductTickets_WithNonStaffUser_ReturnsForbid()
    {
        // Arrange
        var nonStaffUser = new User
        {
            Id = 1,
            IsStaff = false
        };
        var transaction = new TicketTransactionDto
        {
            UserId = 2,
            Amount = 3
        };

        _mockUserService.Setup(s => s.GetUserByIdAsync(1))
            .ReturnsAsync(nonStaffUser);

        // Act
        var result = await _controller.DeductTickets(transaction);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<ForbidResult>());
    }

    [Test]
    public async Task DeductTickets_WithInvalidUser_ReturnsNotFound()
    {
        // Arrange
        var staffUser = new User
        {
            Id = 1,
            IsStaff = true
        };
        var transaction = new TicketTransactionDto
        {
            UserId = 2,
            Amount = 3
        };

        _mockUserService.Setup(s => s.GetUserByIdAsync(1))
            .ReturnsAsync(staffUser);
        _mockTicketService.Setup(s => s.DeductTicketsAsync(transaction.UserId, transaction.Amount))
            .ThrowsAsync(new KeyNotFoundException());

        // Act
        var result = await _controller.DeductTickets(transaction);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<NotFoundResult>());
    }

    [Test]
    public async Task DeductTickets_WithInsufficientTickets_ReturnsBadRequest()
    {
        // Arrange
        var staffUser = new User
        {
            Id = 1,
            IsStaff = true
        };
        var transaction = new TicketTransactionDto
        {
            UserId = 2,
            Amount = 3
        };

        _mockUserService.Setup(s => s.GetUserByIdAsync(1))
            .ReturnsAsync(staffUser);
        _mockTicketService.Setup(s => s.DeductTicketsAsync(transaction.UserId, transaction.Amount))
            .ThrowsAsync(new InvalidOperationException("Insufficient ride tickets"));

        // Act
        var result = await _controller.DeductTickets(transaction);

        // Assert
        Assert.That(result.Result, Is.InstanceOf<BadRequestObjectResult>());
        var badRequestResult = result.Result as BadRequestObjectResult;
        Assert.That(badRequestResult.Value, Is.EqualTo("Insufficient ride tickets"));
    }
}