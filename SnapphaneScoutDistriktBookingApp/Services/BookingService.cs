using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;
using SnapphaneScoutDistriktBookingApp.Models;
using SnapphaneScoutDistriktBookingApp.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.Extensions.Options;
using SnapphaneScoutDistriktBookingApp.Helpers;
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace SnapphaneScoutDistriktBookingApp.Services
{
    public class BookingService : IBookingService
    {
        private readonly IDbService _db;
        private readonly IEmailService _emailService;
        private readonly AppSettings _appSettings;
        public BookingService(IDbService db, IEmailService emailService, IOptions<AppSettings> appSettings)
        {
            _db = db;
            _emailService = emailService;
            _appSettings = appSettings.Value;
        }

        public async Task<Booking?> AddBookingAsync(Booking booking)
        {
            // Add the booking to the database
            await _db.AddCustomerAsync(booking);
            await _emailService.SendEmailAsync(_appSettings.SENDGRID_API_KEY, _appSettings.SENDGRID_EMAIL, booking.Email, booking);
            var newBooking = await FindAddedBookingByIdAsync(booking);
            return newBooking;
        }

        // Find placed booking based on Id and return
        public async Task<Booking?> FindAddedBookingByIdAsync(Booking customer)
        {
            var newBooking = await _db.FindBookingByIdAsync(customer);
            return newBooking;
        }

        public async Task<Booking?> GetBookingByIdAndEmailAsync(Guid id, string email)
        {
            return await _db.FindBookingByIdAndEmailAsync(id, email);
        }

        public async Task UpdateBookingAsync(Booking booking)
        {
            await _db.UpdateBookingAsync(booking);
        }
    }
}
