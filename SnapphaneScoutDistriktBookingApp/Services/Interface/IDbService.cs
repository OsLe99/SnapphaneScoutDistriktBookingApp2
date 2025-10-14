using CommunityToolkit.Mvvm.ComponentModel;
using MongoDB.Bson;
using MongoDB.Driver;
using SnapphaneScoutDistriktBookingApp.Models;
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
        Task<List<Models.Booking>> GetAllBookingsAsync();
        Task<List<Models.Contact>> GetAllContactsAsync();
        Task<Models.Contact> AddContactAsync(Models.Contact contact);
        Task<ObservableCollection<Booking>> LoadAllBookingsAsync(ObservableCollection<Booking> bookings);
        Task<ObservableCollection<Booking>> LoadAllNewBookingsAsync(ObservableCollection<Booking> bookings);
        Task UpdateBookingAsync(Booking booking);
        Task<Booking> AddCustomerAsync(Booking customer);
        Task<Booking?> FindBookingByIdAsync(Booking customer);
        Task<Booking?> FindBookingByIdAndEmailAsync(Guid id, string email);
        Task<Models.Info> UpdateInfoAsync(Models.Info info, Guid Id);
        Task<List<Models.Info>> GetAllInfoAsync();
    }
}
