using CommunityToolkit.Mvvm.ComponentModel;
using SnapphaneScoutDistriktBookingApp.Models;
using System.ComponentModel;
namespace SnapphaneScoutDistriktBookingApp.Views.Booking;

public partial class BookingSelectDate : ContentPage
{
    private Customer _customer;

    public BookingSelectDate(Customer customer)
    {
        InitializeComponent();
        _customer = customer;

        BindingContext = _customer;
    }

    private async void OnChangeToBookingCustomerInfo(object sender, EventArgs e)
    {
        _customer.StartDate = StartDatePicker.Date;
        _customer.EndDate = EndDatePicker.Date;
        await Navigation.PushAsync(new BookingCustomerInfo(_customer));
    }
}