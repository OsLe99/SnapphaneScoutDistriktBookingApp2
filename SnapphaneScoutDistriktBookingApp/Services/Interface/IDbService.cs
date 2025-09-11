using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using SnapphaneScoutDistriktBookingApp.Models;
using MongoDB.Driver;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SnapphaneScoutDistriktBookingApp.Services.Interface
{
    public interface IDbService
    {
        Task UpdateCheckBoxDatabaseAsync(Customer costumer);
        Task<bool> RegisterAdminAsync(string userName, string userEmail, string password);
        Task<bool> CheckIfAdminAsync(string username, string userEmail);
        Task<List<Models.Customer>> GetAllBookingsAsync();
        Task<List<Models.Contact>> GetAllContactsAsync();
        Task<Models.Contact> AddContactAsync(Models.Contact contact);
        Task<ObservableCollection<Customer>> LoadAllBookingsAsync(ObservableCollection<Customer> bookings);
        Task<ObservableCollection<Customer>> LoadAllNewBookingsAsync(ObservableCollection<Customer> bookings);
        Task<Customer> AddCustomerAsync(Customer customer);
        Task<Customer> FindBookingByIdAsync(Customer customer);
        Task<Models.Info> UpdateInfoAsync(Models.Info info, string Id);
        Task<List<Models.Info>> GetAllInfoAsync();
    }
}
