using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnapphaneScoutDistriktBookingApp.Models;
using Clerk.BackendAPI.Models.Components;

namespace SnapphaneScoutDistriktBookingApp.Services.Interface
{
    public interface IClerkUserSessionService : INotifyPropertyChanged
    {
        string UserName { get; set; }
        string UserEmail { get; set; }
        bool IsAdmin { get; }
        bool IsLoggedIn { get; }
        void SetLoggedIn(bool isLoggedIn);
        Task LoadUserData();
        void SetUserAsync(string userName, string userEmail, string sessionId);
        bool IsUserSet();
        Task ResetUser();

        Task<ClerkUser?> CreateUserAsync(string firstName, string lastName, string email, string password);
        Task<string> GetCurrentUserIdAsync();
        Task<User?> GetUserAsync(string userId);
        Task<List<User>> GetAllUsersAsync();
        Task<bool> UpdateUserRoleAsync(string userId, string role);
        Task CheckLoginStateAsync();
    }
}
