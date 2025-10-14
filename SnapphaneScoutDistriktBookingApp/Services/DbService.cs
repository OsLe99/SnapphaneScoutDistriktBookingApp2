using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.ApplicationModel.Communication;
using SnapphaneScoutDistriktBookingApp.Models;
using SnapphaneScoutDistriktBookingApp.Services.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;
using SnapphaneScoutDistriktBookingApp.Helpers;
using Supabase.Postgrest;
using System.Diagnostics;

namespace SnapphaneScoutDistriktBookingApp.Services
{
    public class DbService : IDbService
    {
        #region variables
        private readonly Supabase.Client _client;
        private readonly IEmailService _emailService;

        #endregion

        #region ctor
        public DbService(Supabase.Client supabaseClient)
        {
            _client = supabaseClient;
            _emailService = new EmailService();
        }
        #endregion

        #region private methods
        private List<Booking> BookingCollection()
        {
            var database = _client.From<Booking>().Get();
            return database.Result.Models;
        }
        private List<Models.Contact> ContactCollection()
        {
            var database = _client.From<Models.Contact>().Get();
            return database.Result.Models;
        }
        private List<Models.Info> InfoCollection()
        {
            var database = _client.From<Models.Info>().Get();
            return database.Result.Models;
        }

        private Booking ConvertCustomerToSwedishTime(Booking booking)
        {
            if (booking == null) 
                return null!;

            booking.StartDate = TimeZoneHelper.ToSwedishTime(booking.StartDate);
            booking.EndDate = TimeZoneHelper.ToSwedishTime(booking.EndDate);
            return booking;
        }
        private List<Booking> ConvertCustomersToSwedishTime(List<Booking> customers)
        {
            foreach (var c in customers)
            {
                ConvertCustomerToSwedishTime(c);
            }
            return customers;
        }
        #endregion

        #region CRUD booking
        public async Task<Booking> AddCustomerAsync(Booking booking)
        {
            await _client.InitializeAsync();
            try
            {
                booking.StartDate = TimeZoneHelper.FromSwedishTime(booking.StartDate);
                booking.EndDate = TimeZoneHelper.FromSwedishTime(booking.EndDate);
                await _client.From<Booking>().Insert(booking);
                return booking;
            }
            catch (Supabase.Postgrest.Exceptions.PostgrestException ex)
            {
                Debug.WriteLine($"Supabase error: {ex.Message}");
                Debug.WriteLine($"Status: {ex.StatusCode}");
                Debug.WriteLine($"Content: {ex.Content}");
                throw;
            }
        }

        public async Task UpdateBookingAsync(Booking booking)
        {
            await _client.InitializeAsync();
            booking.StartDate = TimeZoneHelper.FromSwedishTime(booking.StartDate);
            booking.EndDate = TimeZoneHelper.FromSwedishTime (booking.EndDate);
            var update = await _client
                .From<Booking>()
                .Where(x => x.Id == booking.Id)
                .Upsert(booking);
        }

        #endregion

        #region CRUD contact
        public async Task<Models.Contact> AddContactAsync(Models.Contact contact)
        {
            await _client.InitializeAsync();
            await _client.From<Models.Contact>().Insert(contact);
            return contact;
        }

        #endregion

        #region CRUD info
        public async Task<Models.Info> UpdateInfoAsync(Models.Info info, Guid Id)
        {
            var updatedInfo = await _client
                .From<Models.Info>()
                .Where(x => x.Id == Id)
                .Upsert(info);
            return info;
        }

        #endregion

        #region admin methods
        //public async Task<bool> RegisterAdminAsync(string userName, string userEmail, string password) // Byta till ex. SignInUser
        //{
        //    var collection = AdminUserCollection();

        //    var existingUser = await collection.Find(x => x.Name == userName).FirstOrDefaultAsync();

        //    if (existingUser != null)
        //    {
        //        return false;
        //    }

        //    string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        //    var newAdmin = new Models.Admin
        //    {
        //        Name = userName,
        //        Email = userEmail,
        //        PasswordHashed = hashedPassword
        //    };

        //    await collection.InsertOneAsync(newAdmin);
        //    return true;
        //}

        //public async Task<bool> CheckIfAdminAsync(string userName, string userEmail)
        //{
        //    var collection = AdminUserCollection();
        //    var filter = Builders<Admin>.Filter.And(
        //        Builders<Admin>.Filter.Eq(x => x.Name, userName),
        //        Builders<Admin>.Filter.Eq(x => x.Email, userEmail)
        //    );

        //    var adminUser = await collection.Find(filter).FirstOrDefaultAsync();

        //    return adminUser != null;
        //}
        //public async Task<bool> CheckAdminCredentialsAsync(string userEmail, string password)
        //{
        //    var collection = AdminUserCollection();
        //    var filter = Builders<Admin>.Filter.And(
        //        Builders<Admin>.Filter.Eq(x => x.Email, userEmail)
        //        );
        //    var adminUser = await collection.Find(filter).FirstOrDefaultAsync();

        //    if (adminUser == null )
        //    {
        //        return false;
        //    }
        //    return BCrypt.Net.BCrypt.Verify(password, adminUser.PasswordHashed);
        //}

        #endregion

        public async Task<List<Models.Info>> GetAllInfoAsync()
        {
            var result = await _client.From<Models.Info>().Get();
            return result.Models.ToList();
        }
        public async Task<List<Booking>> GetAllBookingsAsync()
        {
            var result = await _client.From<Booking>().Get();
            return ConvertCustomersToSwedishTime(result.Models);
        }

        public async Task<List<Models.Contact>> GetAllContactsAsync()
        {
            var result = await _client.From<Models.Contact>().Get();
            return result.Models;
        }
        public async Task<ObservableCollection<Booking>> LoadAllBookingsAsync(ObservableCollection<Booking> bookings)
        {
            var data = await GetAllBookingsAsync();
            foreach (var booking in data)
            {
                bookings.Add(booking);
            }
            return bookings;
        }
        public async Task<ObservableCollection<Booking>> LoadAllNewBookingsAsync(ObservableCollection<Booking> bookings)
        {
            var data = await GetAllBookingsAsync();
            var newData = data.Where(x => x.StartDate.Date >= DateTime.Today).ToList();
            bookings.Clear();
            foreach (var newBookings in newData)
            {
                bookings.Add(newBookings);
            }
            return bookings;
        }
        public async Task<Booking?> FindBookingByIdAsync(Booking booking)
        {
            var result = await _client.From<Booking>().Get();
            return booking != null ? ConvertCustomerToSwedishTime(booking) : null;
        }
        public async Task<Booking?> FindBookingByIdAndEmailAsync(Guid id, string email)
        {
            var booking = await _client.From<Booking>().Where(b => b.Id == id && b.Email == email).Get();
            return booking != null ? ConvertCustomerToSwedishTime(booking.Model) : null;
        }
    }
}

