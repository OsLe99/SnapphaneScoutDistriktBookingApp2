using Xunit;
using Moq;
using SnapphaneScoutDistriktBookingApp.Services.Interface;
using SnapphaneScoutDistriktBookingApp.Models;
using SnapphaneScoutDistriktBookingApp.Services;
using System.Runtime.CompilerServices;
using MongoDB.Driver;
namespace SnapphaneScoutDistrikBookingApp.Test;

public class AdminServiceTest
{
    private readonly Mock<IClerkUserSessionService> _userSessionServiceMock;
    private readonly Mock<IDbService> _dbServiceMock;

    public AdminServiceTest()
    {
        _userSessionServiceMock = new Mock<IClerkUserSessionService>();
        _dbServiceMock = new Mock<IDbService>();
    }

    [Fact]
    public async Task TryLoginAdminAsync_ReturnBoolCorrectly()
    {
        string username = "AdminUser";
        string email = "admin@email.com";
        string password = "password";

        _dbServiceMock.Setup(d => d.CheckIfAdminAsync(username, email).Result).Returns(true);

        var adminService = new AdminService(_userSessionServiceMock.Object, _dbServiceMock.Object);
        var result = await adminService.TryLoginAdminAsync(username, email, password);

        Assert.True(result);
    }
}
