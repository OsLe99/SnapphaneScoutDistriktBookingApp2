using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using SnapphaneScoutDistriktBookingApp.Services;
using SnapphaneScoutDistriktBookingApp.Services.Interface;
using Clerk.BackendAPI;
using Clerk.BackendAPI.Models.Components;
using Clerk.BackendAPI.Models.Operations;
using Clerk.BackendAPI.Models;
using System.Net.Http.Json;
using SnapphaneScoutDistriktBookingApp.Models;
using System.Text.Json;
using System.Diagnostics;
using SnapphaneScoutDistriktBookingApp.Helpers;

namespace SnapphaneScoutDistriktBookingApp.Services
{
    public class ClerkUserSessionService : IClerkUserSessionService
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly ClerkBackendApi _sdk;

        private string _userName = string.Empty;
        private string _userEmail = string.Empty;
        private bool _isAdmin = false;

        public ClerkUserSessionService(string bearerToken)
        {
            LoadUserData();
            _sdk = new ClerkBackendApi(bearerAuth: bearerToken);
        }
        #region Setters
        public string UserName
        {
            get => _userName;
            set
            {
                if (_userName != value)
                {
                    _userName = value;
                    OnPropertyChanged(nameof(UserName));
                }
            }
        }

        public string UserEmail
        {
            get => _userEmail;
            set
            {
                if (_userEmail != value)
                {
                    _userEmail = value;
                    OnPropertyChanged(nameof(UserEmail));
                }
            }
        }

        public bool IsAdmin
        {
            get => _isAdmin;
            private set
            {
                if (_isAdmin != value)
                {
                    _isAdmin = value;
                    OnPropertyChanged(nameof(IsAdmin));
                }
            }
        }
        #endregion

        public async Task<ClerkUser?> CreateUserAsync(string firstName, string lastName, string email, string password)
        {
            var req = new CreateUserRequestBody()
            {
                FirstName = firstName,
                LastName = lastName,
                EmailAddress = new List<string> { email },
                Password = password,
                
            };

            try
            {
                var response = await _sdk.Users.CreateAsync(req);

                if (response?.User != null)
                {
                    return new ClerkUser
                    {
                        Id = response.User.Id,
                        FirstName = response.User.FirstName,
                        LastName = response.User.LastName,
                        EmailAddress = response.User.PrimaryEmailAddressId ?? string.Empty
                    };
                }
            }
            catch (Exception ex)
            {
                Debug.Write($"Error creating user: {ex.Message}");
            }

            return null;
        }

        public async void SetUserToken(string token)
        {
            await TokenStorage.SaveTokenAsync(token);
        }

        public void LoadUserData()
        {
            UserName = Preferences.Get("Användarnamn", string.Empty);
            UserEmail = Preferences.Get("Användarmail", string.Empty);
            IsAdmin = Preferences.Get("IsAdmin", false);
        }

        public void SetUser(string userName, string userEmail)
        {
            UserName = userName;
            UserEmail = userEmail;
            Preferences.Set("Användarnamn", userName);
            Preferences.Set("Användarmail", userEmail);
        }

        public bool SetAdmin(bool isAdmin)
        {
            IsAdmin = isAdmin;
            Preferences.Set("IsAdmin", isAdmin);
            return IsAdmin;
        }

        public bool IsUserSet()
        {
            return !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(UserEmail);
        }

        public void ResetUser()
        {
            Preferences.Remove("Användarnamn");
            Preferences.Remove("Användarmail");
            Preferences.Remove("IsAdmin");
            UserName = string.Empty;
            UserEmail = string.Empty;
            IsAdmin = false;
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}