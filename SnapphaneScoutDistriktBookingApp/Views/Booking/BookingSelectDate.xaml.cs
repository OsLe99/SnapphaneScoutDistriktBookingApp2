using CommunityToolkit.Mvvm.ComponentModel;
using SnapphaneScoutDistriktBookingApp.Models;
using System.ComponentModel;
namespace SnapphaneScoutDistriktBookingApp.Views.Booking;

public partial class BookingSelectDate : ContentPage, INotifyPropertyChanged
{
    private Customer _customer;
    private DateTime _startDate = DateTime.Now;
    private DateTime _endDate = DateTime.Now.AddDays(1);

    public DateTime StartDate
    {
        get => _startDate;
        set
        {
            if (_startDate != value)
            {
                _startDate = value;
                OnPropertyChanged();
            }
        }
    }
    public DateTime EndDate
    {
        get => _endDate;
        set
        {
            if (_endDate != value)
            {
                _endDate = value;
                OnPropertyChanged();
            }
        }
    }

    public BookingSelectDate(Customer customer)
    {
        InitializeComponent();
        customer.StartDate = StartDate;
        customer.EndDate = EndDate;
        _customer = customer;

        BindingContext = this;
    }

    private async void OnChangeToBookingCustomerInfo(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new BookingCustomerInfo(_customer));
    }

    // Psuedo code
    // 1. Find booking type of current customer.
    // 2. Blackout dates where amount left is smaller than whats left. If campgrounds or cabin, blackout entirely between dates already used.
    // 3. Set new startdate and enddate for current customer.
}