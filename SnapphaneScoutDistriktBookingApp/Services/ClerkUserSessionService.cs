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
    public class ClerkUserSessionService : IClerkUserSessionService, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private readonly ClerkBackendApi _sdk;
        private readonly IClerkAuthService _authService;

        private string _userName = string.Empty;
        private string _userEmail = string.Empty;
        private bool _isAdmin = false;
        private bool _isLoggedIn = false;
        public string UserToken { get; private set; } = string.Empty;
        public string WelcomeText => string.IsNullOrWhiteSpace(UserName) ? "Välkommen!" : $"Välkommen {UserName}!";
        public ClerkUserSessionService(string bearerToken, IClerkAuthService authService)
        {
            _sdk = new ClerkBackendApi(bearerAuth: bearerToken);
            _authService = authService;
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
                    OnPropertyChanged(nameof(WelcomeText));
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

        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            private set
            {
                if (_isLoggedIn != value)
                {
                    _isLoggedIn = value;
                    OnPropertyChanged(nameof(IsLoggedIn));
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

        public async Task<string> GetCurrentUserIdAsync()
        {
            var sessionId = await SecureStorage.Default.GetAsync("sessionId");
            if (string.IsNullOrEmpty(sessionId))
            {
                return string.Empty;
            }

            var sessionResponse = await _sdk.Sessions.GetAsync(sessionId);
            return sessionResponse?.Session?.UserId ?? string.Empty;
        }

        public async Task<User?> GetUserAsync(string userId)
        {
            try
            {
                var response = await _sdk.Users.GetAsync(userId);
                return response?.User;
            }
            catch (Exception ex)
            {
                // Future logging potential?
                Debug.WriteLine($"Error getting user: {ex.Message}");
                return null;
            }
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            try
            {
                var usersResponse = await _sdk.Users.ListAsync();
                return usersResponse?.UserList?.ToList() ?? new List<User>();
            }
            catch(Exception ex)
            {
                // Future logging potential?
                Debug.WriteLine($"Error fetching users: {ex.Message}");
                return new List<User>();
            }
        }

        public async Task<bool> UpdateUserRoleAsync(string userId, string role)
        {
            try
            {
                var response = await _sdk.Users.UpdateMetadataAsync(userId, new UpdateUserMetadataRequestBody
                {
                    PrivateMetadata = new Dictionary<string, object>
                    {
                        {"role", role }
                    }
                });

                return response?.User != null;
            }
            catch (Exception ex)
            {
                // Future logging potential?
                Debug.WriteLine($"Error updating user: {ex.Message}");
                return false;
            }
        }

        public void SetLoggedIn(bool isLoggedIn)
        {
            IsLoggedIn = isLoggedIn;
        }

        public async Task CheckLoginStateAsync()
        {
            bool isLoggedIn = await _authService.IsUserLoggedInAsync();
            SetLoggedIn(isLoggedIn);
        }

        public Task LoadUserData()
        {
            UserName = Preferences.Get("userName", string.Empty);
            UserEmail = Preferences.Get("userEmail", string.Empty);

            return Task.CompletedTask;
        }

        public async void SetUserAsync(string userName, string userEmail, string sessionId)
        {
            Preferences.Set("userName", userName);
            Preferences.Set("userEmail", userEmail);

            await SecureStorage.SetAsync("sessionId", sessionId);

            UserName = userName;
            UserEmail = userEmail;
        }

        public bool IsUserSet()
        {
            return !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(UserEmail);
        }

        public async Task ResetUser()
        {
            try
            {
                Preferences.Set("userName", string.Empty);
                Preferences.Set("userEmail", string.Empty);

                SecureStorage.Remove("sessionId");
                await TokenStorage.RemoveTokenAsync();

                UserName = string.Empty;
                UserEmail = string.Empty;
                Debug.WriteLine("User data cleared safely");
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error on user reset: {ex.Message}");
            }
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}