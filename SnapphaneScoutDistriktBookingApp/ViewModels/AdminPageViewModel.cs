using MongoDB.Driver;
using SnapphaneScoutDistriktBookingApp.Services.Interface;
using SnapphaneScoutDistriktBookingApp.Services;
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

namespace SnapphaneScoutDistriktBookingApp.ViewModels
{
    public class AdminPageViewModel
    {
        private readonly IDbService _db;
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
        public ICommand ListAllBookingsCommand { get; }
        public ICommand ListAllNewBookingsCommand { get; }
        public AdminPageViewModel(IDbService db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));

            Bookings = new ObservableCollection<Booking>();

            ListAllBookingsCommand = new Command(async () =>
                await _db.LoadAllBookingsAsync(Bookings));

            ListAllNewBookingsCommand = new Command(async () =>
                await _db.LoadAllNewBookingsAsync(Bookings));
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
