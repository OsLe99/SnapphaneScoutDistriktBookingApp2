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
    class AdminPageViewModel
    {
        //private readonly IDbService _db = new DbService();
        //public event PropertyChangedEventHandler? PropertyChanged;
        //private ObservableCollection<Models.Customer> _bookings;
        //public ObservableCollection<Models.Customer> Bookings { get { return _bookings; }
        //    set
        //    {
        //        _bookings = value;
        //        OnPropertyChanged();
        //    }
        //}
        //public ICommand ListAllBookingsCommand { get; }
        //public ICommand ListAllNewBookingsCommand { get; }
        //public AdminPageViewModel()
        //{
        //    Bookings = new ObservableCollection<Models.Customer>();
        //    ListAllBookingsCommand = new Command(async () => await _db.LoadAllBookingsAsync(Bookings));
        //    ListAllNewBookingsCommand = new Command(async () => await _db.LoadAllNewBookingsAsync(Bookings));

        //}
        //protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        //{
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //}
    }
}
