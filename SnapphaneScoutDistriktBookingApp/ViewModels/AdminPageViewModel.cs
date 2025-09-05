using MongoDB.Driver;
using SnapphaneScoutDistriktBookingApp.Services.Interface;
using SnapphaneScoutDistriktBookingApp.Services;
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
    class AdminPageViewModel : INotifyPropertyChanged
    {
        private readonly IDbService _db = new DbService();
        public event PropertyChangedEventHandler? PropertyChanged;
        private ObservableCollection<Models.Customer> _bookings;
        public ObservableCollection<Models.Customer> Bookings { get { return _bookings; }
            set
            {
                _bookings = value;
                OnPropertyChanged();
            }
        }
        public ICommand ListAllBookingsCommand { get; }
        public ICommand ListAllNewBookingsCommand { get; }
        public AdminPageViewModel()
        {
            Bookings = new ObservableCollection<Models.Customer>();
            ListAllBookingsCommand = new Command(async () => await LoadAllBookingsAsync());
            ListAllNewBookingsCommand = new Command(async () => await LoadAllNewBookingsAsync());

        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private async Task<List<Models.Customer>> GetAllBookingsFromDB()
        {
            List<Models.Customer> bookings = await _db.BookingCollection().Find(_ => true).ToListAsync();
            return bookings;
        }
        private async Task LoadAllBookingsAsync()
        {
            var data = await GetAllBookingsFromDB();
            foreach(var booking in data)
            {
                Bookings.Add(booking);
            }
        }
        private async Task LoadAllNewBookingsAsync()
        {
            var data = await GetAllBookingsFromDB();
            var newData = data.Where(x => x.StartDate.Date >= DateTime.Today).ToList();
            Bookings.Clear();
            foreach(var newBookings in newData)
            {
                Bookings.Add(newBookings);
            }
        }
    }
}
