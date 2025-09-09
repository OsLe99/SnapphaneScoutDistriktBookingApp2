using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Moq;
using SnapphaneScoutDistriktBookingApp.Services;
using SnapphaneScoutDistriktBookingApp.Services.Interface;

namespace SnapphaneScoutDistrikBookingApp.Test
{
    public class ValidationTest
    {
        private readonly ValidateBookingService _mockValidate;

        public ValidationTest()
        {
            _mockValidate = new ValidateBookingService();
        }

        [Theory]
        [InlineData("testemail@com", false)]
        [InlineData("testmail.com", false)]
        [InlineData("testgmaill@.com", false)]
        [InlineData("test@gmail.com", true)]

        public void TestEmailValidation(string email, bool expected)
        {
            // Act
            var result = _mockValidate.ValidateEmail(email);
            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("123456", false)]
        [InlineData("1234567890123456", false)]
        [InlineData("1234567", true)]
        [InlineData("123456789012345", true)]

        public void TestPhoneNumberValidation(string phoneNumber, bool expected)
        {
            // Act
            var result = _mockValidate.ValidatePhoneNumber(phoneNumber);
            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("John123", false)]
        [InlineData("John_Doe", false)]
        [InlineData("John Doe", true)]
        [InlineData("John", true)]

        public void TestNameValidation(string name, bool expected)
        {
            // Act
            var result = _mockValidate.ValidateName(name);
            // Assert
            Assert.Equal(expected, result);
        }
    }
}
