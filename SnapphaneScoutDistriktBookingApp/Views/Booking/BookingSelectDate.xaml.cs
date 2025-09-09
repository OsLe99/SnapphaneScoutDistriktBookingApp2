using CommunityToolkit.Mvvm.ComponentModel;
using SnapphaneScoutDistriktBookingApp.Models;
using SnapphaneScoutDistriktBookingApp.Services.Interface;
using System.ComponentModel;
namespace SnapphaneScoutDistriktBookingApp.Views.Booking;

public partial class BookingSelectDate : ContentPage
{
    private Customer _customer;
    private readonly IBookingService _bookingService;

    public BookingSelectDate(Customer customer, IBookingService bookingService)
    {
        InitializeComponent();
        _customer = customer;
        _bookingService = bookingService;
        BindingContext = _customer;
    }

    private async void OnChangeToBookingCustomerInfo(object sender, EventArgs e)
    {
        _customer.StartDate = StartDatePicker.Date;
        _customer.EndDate = EndDatePicker.Date;
        var validateBookingService = App.Current.Handler.MauiContext.Services.GetService<IValidateBookingService>();
        await Navigation.PushAsync(new BookingCustomerInfo(_customer, _bookingService, validateBookingService));
    }
}