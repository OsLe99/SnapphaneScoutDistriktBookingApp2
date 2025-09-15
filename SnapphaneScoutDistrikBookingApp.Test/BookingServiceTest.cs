using Castle.Core.Resource;
using EllipticCurve;
using Microsoft.UI.Xaml.Documents;
using MongoDB.Driver;
using Moq;
using SnapphaneScoutDistriktBookingApp.Models;
using SnapphaneScoutDistriktBookingApp.Services;
using SnapphaneScoutDistriktBookingApp.Services.Interface;
using System.Reflection;
using Xunit;
using ZstdSharp.Unsafe;

namespace SnapphaneScoutDistrikBookingApp.Test;

public class BookingServiceTest
{
    private readonly Mock<IBookingService> _mockBooking;
    private readonly Mock<IDbService> _mockDb;
    private readonly Mock<IEmailService> _mockEmailService;
    private readonly Customer _customer;
    private readonly Mock<IMongoCollection<Customer>> _mockCollection;
    public BookingServiceTest()
    {
        _mockEmailService = new Mock<IEmailService>();
        _mockDb = new Mock<IDbService>();
        _mockBooking = new Mock<IBookingService>();
        _mockCollection = new Mock<IMongoCollection<Customer>>();

        _customer = new Customer
        {
            Name = "Oscar",
            Phone = "1234567",
            Email = "test@gmail.com",
            IsOrg = true,
            OrgName = "Testkår",
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(1),
            BookingType = Customer.TypeOfBooking.Canoe,
            NumberOfCabin = null,
            NumberOfCampground = null,
            NumberOfCanoes = 2,
            NumberOfLeanTo = null,
            IsConfirmed = false
        };

        _mockBooking.Setup(book => book.FindAddedBookingByIdAsync(_customer).Result).Returns(_customer);
        _mockEmailService.Setup(e => e.SendEmailAsync(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<SnapphaneScoutDistriktBookingApp.Models.Customer>())).Returns(Task.CompletedTask);

        _mockDb.Setup(d => d.AddCustomerAsync(_customer).Result).Returns(_customer);
        _mockDb.Setup(d => d.FindBookingByIdAsync(_customer).Result).Returns(_customer);
    }
    [Fact]
    public async Task AddBookingAsync_ShouldInsertBookingAndSendEmail()
    {
        var mockBooking = new BookingService(_mockDb.Object, _mockEmailService.Object);
        var newCustomer = await mockBooking.AddBookingAsync(_customer);
        // Assert
        Assert.Equal(_customer.Id, newCustomer.Id);
    }
}
