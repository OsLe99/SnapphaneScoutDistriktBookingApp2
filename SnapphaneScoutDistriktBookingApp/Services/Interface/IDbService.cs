using CommunityToolkit.Mvvm.ComponentModel;
using MongoDB.Bson;
using MongoDB.Driver;
using SnapphaneScoutDistriktBookingApp.Models;
using ScoutContact = SnapphaneScoutDistriktBookingApp.Models.Contact;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapphaneScoutDistriktBookingApp.Services.Interface
{
    public interface IDbService
    {
        //Task UpdateCheckBoxDatabaseAsync(Booking booking);
        //Task<bool> RegisterAdminAsync(string userName, string userEmail, string password);
        //Task<bool> CheckIfAdminAsync(string username, string userEmail);
        //Task<bool> CheckAdminCredentialsAsync(string userEmail, string password);
        Task<List<Booking>> GetAllBookingsAsync();
        Task<List<ScoutContact>> GetAllContactsAsync();
        Task<ScoutContact> AddContactAsync(ScoutContact contact);
        Task<ObservableCollection<Booking>> LoadAllBookingsAsync(ObservableCollection<Booking> bookings);
        Task<ObservableCollection<Booking>> LoadAllNewBookingsAsync(ObservableCollection<Booking> bookings);
        Task UpdateBookingAsync(Booking booking);
        Task<Booking> AddCustomerAsync(Booking customer);
        Task<Booking?> FindBookingByIdAsync(Booking customer);
        Task<Booking?> FindBookingByIdAndEmailAsync(Guid id, string email);
        Task<Info> UpdateInfoAsync(Info info, Guid Id);
        Task<List<Info>> GetAllInfoAsync();
    }
}
