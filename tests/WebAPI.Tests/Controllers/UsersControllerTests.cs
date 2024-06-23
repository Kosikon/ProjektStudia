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
        _userServiceMock.Setup(x => x.Authenticate(It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync((User)null);

        var result = await _controller.Authenticate(new AuthenticateRequest { Username = "test", Password = "test" });

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task GetAll_ReturnsOkResult_WithListOfUsers()
    {
        _userServiceMock.Setup(x => x.GetAll())
            .ReturnsAsync(new List<User> { new User { Id = 1, Username = "testuser" } });

        var result = await _controller.GetAll();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var users = Assert.IsType<List<User>>(okResult.Value);
        Assert.Single(users);
    }

    [Fact]
    public async Task GetById_ReturnsOkResult_WhenUserExists()
    {
        var userId = 1;
        _userServiceMock.Setup(x => x.GetById(userId))
            .ReturnsAsync(new User { Id = userId, Username = "testuser" });

        var result = await _controller.GetById(userId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var user = Assert.IsType<User>(okResult.Value);
        Assert.Equal(userId, user.Id);
    }

    [Fact]
    public async Task Create_ReturnsOkResult_WhenUserIsCreated()
    {
        var createUserRequest = new CreateUserRequest { Username = "newuser", Password = "password", Role = "User" };
        var user = new User { Id = 1, Username = createUserRequest.Username, Role = createUserRequest.Role };

        _userServiceMock.Setup(x => x.Create(It.IsAny<User>(), createUserRequest.Password))
            .ReturnsAsync(user);

        var result = await _controller.Create(createUserRequest);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var createdUser = Assert.IsType<User>(okResult.Value);
        Assert.Equal(createUserRequest.Username, createdUser.Username);
    }

    [Fact]
    public async Task Update_ReturnsOkResult_WhenUserIsUpdated()
    {
        var updateUserRequest = new UpdateUserRequest { Username = "updateduser", Password = "newpassword", Role = "User" };
        var user = new User { Id = 1, Username = updateUserRequest.Username, Role = updateUserRequest.Role };

        _userServiceMock.Setup(x => x.Update(It.IsAny<User>(), updateUserRequest.Password))
            .Returns(Task.CompletedTask);

        var result = await _controller.Update(1, updateUserRequest);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var updatedUser = Assert.IsType<User>(okResult.Value);
        Assert.Equal(updateUserRequest.Username, updatedUser.Username);
    }

    [Fact]
    public async Task Delete_ReturnsOkResult_WhenUserIsDeleted()
    {
        _userServiceMock.Setup(x => x.Delete(It.IsAny<int>()))
            .Returns(Task.CompletedTask);

        var result = await _controller.Delete(1);

        Assert.IsType<OkResult>(result);
    }
}
