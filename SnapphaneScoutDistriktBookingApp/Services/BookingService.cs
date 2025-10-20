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
using static Microsoft.Maui.ApplicationModel.Permissions;

namespace SnapphaneScoutDistriktBookingApp.Services
{
    public class BookingService : IBookingService
    {
        private readonly IDbService _db;
        private readonly IEmailService _emailService;
        public BookingService(IDbService db, IEmailService emailService)
        {
            _db = db;
            _emailService = emailService;
        }

        public async Task<Booking?> AddBookingAsync(Booking booking)
        {
            // Add the booking to the database
            await _db.AddCustomerAsync(booking);
            await _emailService.SendEmailAsync(Environment.GetEnvironmentVariable("SENDGRID_API_KEY"), Environment.GetEnvironmentVariable("SENDGRID_EMAIL"), booking.Email, booking);
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
