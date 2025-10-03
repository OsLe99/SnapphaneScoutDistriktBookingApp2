using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnapphaneScoutDistriktBookingApp.Models;

namespace SnapphaneScoutDistriktBookingApp.Services.Interface
{
    public interface IClerkUserSessionService : INotifyPropertyChanged
    {
        string UserName { get; set; }
        string UserEmail { get; set; }
        bool IsAdmin { get; }

        void SetUser(string userName, string userEmail);
        bool SetAdmin(bool isAdmin);
        bool IsUserSet();
        void ResetUser();

        Task<ClerkUser?> CreateUserAsync(string firstName, string lastName, string email, string password);
        void SetUserToken(string token);
    }
}
