using SnapphaneScoutDistriktBookingApp.Services.Interface;
using SnapphaneScoutDistriktBookingApp.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Clerk.BackendAPI.Models.Components;
using SnapphaneScoutDistriktBookingApp.Helpers;

namespace SnapphaneScoutDistriktBookingApp.ViewModels
{
    public class AdminPageViewModel
    {
        private readonly IDbService _db;
        private readonly IRoleAuthService _roleAuthService;
        private readonly IClerkAuthService _clerkAuthService;
        private readonly IClerkUserSessionService _userSessionService;
        public event PropertyChangedEventHandler? PropertyChanged;

        private ObservableCollection<Booking> _bookings;
        public ObservableCollection<Booking> Bookings
        {
            get { return _bookings; }
            set
            {
                _bookings = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<DisplayUser> _users;
        public ObservableCollection<DisplayUser> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged();
            }
        }

        public ICommand ListAllBookingsCommand { get; }
        public ICommand ListAllNewBookingsCommand { get; }
        public ICommand ListAllUsersCommand { get; }
        public ICommand PromoteToAdminCommand { get; }
        public AdminPageViewModel(IDbService db, IClerkAuthService clerkAuthService, IRoleAuthService roleAuthService, IClerkUserSessionService clerkUserSession)
        {
            _db = db;
            _clerkAuthService = clerkAuthService;
            _roleAuthService = roleAuthService;
            _userSessionService = clerkUserSession;

            Users = new ObservableCollection<DisplayUser>();

            Bookings = new ObservableCollection<Booking>();

            ListAllBookingsCommand = new Command(async () =>
                await _db.LoadAllBookingsAsync(Bookings));

            ListAllNewBookingsCommand = new Command(async () =>
                await _db.LoadAllNewBookingsAsync(Bookings));

            ListAllUsersCommand = new Command(async () =>
            await LoadAllUsersAsync());
            
            PromoteToAdminCommand = new Command<DisplayUser>(async (displayUser) =>
            await PromoteToAdminAsync(displayUser));
        }

        private async Task LoadAllUsersAsync()
        {
            var users = await _userSessionService.GetAllUsersAsync();
            Users.Clear();
            foreach (var user in users)
            {
                Users.Add(new DisplayUser(user));
            }
        }

        private async Task PromoteToAdminAsync(DisplayUser displayUser)
        {
            if (!await _roleAuthService.IsAdminAsync())
            {
                return;
            }

            bool success = await _userSessionService.UpdateUserRoleAsync(displayUser.User.Id, "admin");

            if (!success)
            {
                await Shell.Current.DisplayAlert("Fel", "Det gick inte att uppdatera användaren", "OK");
            }

            await LoadAllUsersAsync();
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
