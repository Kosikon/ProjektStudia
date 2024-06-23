using Application.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Threading.Tasks;
using WebAPI.Controllers;
using WebAPI.Models;
using Xunit;

public class UsersControllerTests
{
    private readonly Mock<IUserService> _userServiceMock;
    private readonly UsersController _controller;

    public UsersControllerTests()
    {
        _userServiceMock = new Mock<IUserService>();
        _controller = new UsersController(_userServiceMock.Object);
    }

    [Fact]
    public async Task Authenticate_ReturnsBadRequest_WhenUserIsNull()
    {
        // Arrange
        _userServiceMock.Setup(x => x.Authenticate(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((User)null);

        // Act
        var result = await _controller.Authenticate(new AuthenticateRequest { Username = "test", Password = "test" });

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }
}