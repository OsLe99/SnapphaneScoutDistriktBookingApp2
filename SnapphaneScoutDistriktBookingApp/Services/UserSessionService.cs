using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Storage;
using SnapphaneScoutDistriktBookingApp.Services;
using SnapphaneScoutDistriktBookingApp.Services.Interface;

namespace SnapphaneScoutDistriktBookingApp.Services
{
    public class UserSessionService : IUserSessionService
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private string _userName = string.Empty;
        private string _userEmail = string.Empty;
        private bool _isAdmin = false;

        public UserSessionService()
        {
            LoadUserData();
        }

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

        //public static UserSession Instance => instance.Value;
        //private string _userName;
        //public string UserName
        //{
        //    get { return _userName; }
        //    set
        //    {
        //        _userName = value;
        //        OnPropertyChanged();
        //    }
        //}
        //public string UserEmail { get; set; } = string.Empty;

        //public bool IsAdmin { get; private set; } = false;

        //protected void OnPropertyChanged()
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("UserName"));
        //}
        //private void LoadUserData()
        //{
        //    UserName = Preferences.Get("Användarnamn", string.Empty);
        //    UserEmail = Preferences.Get("Användarmail", string.Empty);
        //    IsAdmin = Preferences.Get("IsAdmin", false);
        //}

        //public void SetUser(string userName, string userEmail)
        //{
        //    UserName = userName;
        //    UserEmail = userEmail;
        //    Preferences.Set("Användarnamn", userName);
        //    Preferences.Set("Användarmail", userEmail);
        //}

        //public bool IsUserSet()
        //{
        //    return !string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(UserEmail);
        //}

        //public void ResetUser()
        //{
        //    Preferences.Remove("Användarnamn");
        //    Preferences.Remove("Användarmail");
        //    Preferences.Remove("IsAdmin");
        //    UserName = string.Empty;
        //    UserEmail = string.Empty;
        //    IsAdmin = false;
        //}

        //public void SetAdmin(bool isAdmin)
        //{
        //    IsAdmin = isAdmin;
        //}
    }
}