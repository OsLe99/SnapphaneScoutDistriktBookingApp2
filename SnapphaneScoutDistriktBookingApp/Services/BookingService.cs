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

        public async Task<Customer?> AddBookingAsync(Customer customer)
        {
            // Add the booking to the database
            await _db.AddCustomerAsync(customer);
            await _emailService.SendEmailAsync("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", "emil.berg@campusnykoping.se", customer.Email, customer);
            var newBooking = await FindAddedBookingByIdAsync(customer);
            return newBooking;
        }

        // Find placed booking based on Id and return
        public async Task<Customer?> FindAddedBookingByIdAsync(Customer customer)
        {
            var newBooking = await _db.FindBookingByIdAsync(customer);
            return newBooking;
        }

        public async Task<Customer?> GetBookingByIdAndEmailAsync(ObjectId id, string email)
        {
            return await _db.FindBookingByIdAndEmailAsync(id, email);
        }

        public async Task UpdateBookingAsync(Customer booking)
        {
            await _db.UpdateBookingAsync(booking);
        }
    }
}
