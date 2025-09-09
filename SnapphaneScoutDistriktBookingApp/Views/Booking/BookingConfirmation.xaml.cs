using SnapphaneScoutDistriktBookingApp.Models;
using SnapphaneScoutDistriktBookingApp.Services.Interface;
using System.ComponentModel;

namespace SnapphaneScoutDistriktBookingApp.Views.Booking;

public partial class BookingConfirmation : ContentPage, INotifyPropertyChanged
{
	private readonly Customer _customer;
    private readonly IBookingService _bookingService;
    public BookingConfirmation(Customer customer, IBookingService bookingService)
	{
        InitializeComponent();
        _customer = customer;
        _bookingService = bookingService;
        BindingContext = _customer;
    }

    private async void OnConfirmBookingClicked(object sender, EventArgs e)
	{
        await _bookingService.AddBookingAsync(_customer);
        await Shell.Current.GoToAsync("//MainPage");
	}
}