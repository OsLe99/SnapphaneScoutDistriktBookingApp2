using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.ApplicationModel.Communication;
using SnapphaneScoutDistriktBookingApp.Models;
using ScoutContact = SnapphaneScoutDistriktBookingApp.Models.Contact;
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
        private readonly AppSettings appSettings;

        #endregion

        #region ctor
        public DbService(Supabase.Client supabaseClient, IEmailService emailService)
        {
            _client = supabaseClient;
            _emailService = emailService;
        }
        #endregion

        #region private methods

        // Hämta alla bookings i en lista
        private async Task<List<Booking>> BookingCollection()
        {
            await _client.InitializeAsync();
            var database = await _client.From<Booking>().Get();
            return database.Models;
        }
        private async Task<List<ScoutContact>> ContactCollection()
        {
            await _client.InitializeAsync();
            var database = _client.From<ScoutContact>().Get();
            return database.Result.Models;
        }
        private async Task<List<Info>> InfoCollection()
        {
            await _client.InitializeAsync();
            var database = _client.From<Info>().Get();
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

            booking.StartDate = TimeZoneHelper.FromSwedishTime(booking.StartDate);
            booking.EndDate = TimeZoneHelper.FromSwedishTime(booking.EndDate);
            await _client.From<Booking>().Insert(booking);
            return booking;
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
        public async Task<ScoutContact> AddContactAsync(ScoutContact contact)
        {
            await _client.InitializeAsync();
            await _client.From<ScoutContact>().Insert(contact);
            return contact;
        }

        #endregion

        #region CRUD info
        public async Task<Info> UpdateInfoAsync(Info info, Guid Id)
        {
            await _client.InitializeAsync();
            var updatedInfo = await _client
                .From<Info>()
                .Where(x => x.Id == Id)
                .Upsert(info);
            return info;
        }

        #endregion

        #region admin methods

        #endregion

        public async Task<List<Info>> GetAllInfoAsync()
        {
            await _client.InitializeAsync();
            var result = await _client.From<Info>().Get();
            return result.Models.OrderBy(i => i.CreatedAt).ToList();
        }
        public async Task<List<Booking>> GetAllBookingsAsync()
        {
            await _client.InitializeAsync();
            var result = await _client.From<Booking>().Get();
            return ConvertCustomersToSwedishTime(result.Models);
        }

        public async Task<List<ScoutContact>> GetAllContactsAsync()
        {
            await _client.InitializeAsync();
            var result = await _client.From<ScoutContact>().Get();
            Debug.WriteLine("Number of contacts: " + result.Models.Count);
            return result.Models.OrderBy(b => b.Id).ToList();
        }
        public async Task<ObservableCollection<Booking>> LoadAllBookingsAsync(ObservableCollection<Booking> bookings)
        {
            var data = await GetAllBookingsAsync();

            MainThread.BeginInvokeOnMainThread(() =>
            {
                bookings.Clear(); 
                foreach (var booking in data)
                    bookings.Add(booking);
            });
            return bookings;
        }
        public async Task<ObservableCollection<Booking>> LoadAllNewBookingsAsync(ObservableCollection<Booking> bookings)
        {
            var data = await GetAllBookingsAsync();
            var newData = data.Where(x => x.IsConfirmed == false).ToList();
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

