using MongoDB;
using MongoDB.Driver;
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

        public async Task<Models.Customer> AddBookingAsync(Models.Customer customer)
        {
            // Add the booking to the database
            await _db.AddCustomerAsync(customer);
            await _emailService.SendEmailAsync("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", "emil.berg@campusnykoping.se", customer.Email, customer);
            var newBooking = await FindAddedBookingByIdAsync(customer);
            return newBooking;
        }

        // Find placed booking based on Id and return
        public async Task<Models.Customer> FindAddedBookingByIdAsync(Models.Customer customer)
        {
            var newBooking = await _db.FindBookingByIdAsync(customer);
            return newBooking;
        }
    }
}
