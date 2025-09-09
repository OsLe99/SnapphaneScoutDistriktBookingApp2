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

        public async Task AddBookingAsync(Models.Customer customer)
        {
            // Add the booking to the database
            await _db.BookingCollection().InsertOneAsync(customer);
            await _emailService.SendEmail("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", "emil.berg@campusnykoping.se", customer.Email, customer);
        }

        //public async Task UpdateBookingAsync(Models.Customer customer)
        //{
        //    var collection = _db.BookingCollection();
        //}
    }
}
