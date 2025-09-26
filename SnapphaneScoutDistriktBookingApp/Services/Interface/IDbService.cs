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
        Task UpdateCheckBoxDatabaseAsync(Customer costumer);
        Task<bool> RegisterAdminAsync(string userName, string userEmail, string password);
        Task<bool> CheckIfAdminAsync(string username, string userEmail);
        Task<bool> CheckAdminCredentialsAsync(string userEmail, string password);
        Task<List<Models.Customer>> GetAllBookingsAsync();
        Task<List<Models.Contact>> GetAllContactsAsync();
        Task<Models.Contact> AddContactAsync(Models.Contact contact);
        Task<ObservableCollection<Customer>> LoadAllBookingsAsync(ObservableCollection<Customer> bookings);
        Task<ObservableCollection<Customer>> LoadAllNewBookingsAsync(ObservableCollection<Customer> bookings);
        Task UpdateBookingAsync(Customer booking);
        Task<Customer> AddCustomerAsync(Customer customer);
        Task<Customer?> FindBookingByIdAsync(Customer customer);
        Task<Customer?> FindBookingByIdAndEmailAsync(ObjectId id, string email);
        Task<Models.Info> UpdateInfoAsync(Models.Info info, string Id);
        Task<List<Models.Info>> GetAllInfoAsync();
    }
}
