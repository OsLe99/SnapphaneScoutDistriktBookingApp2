using SnapphaneScoutDistriktBookingApp.Models;
using System.ComponentModel;

namespace SnapphaneScoutDistriktBookingApp.Views.Booking;

public partial class BookingConfirmation : ContentPage, INotifyPropertyChanged
{
	private readonly Customer _customer;
    public BookingConfirmation(Customer customer)
	{
        InitializeComponent();
        _customer = customer;
        BindingContext = _customer;
    }

    private async void OnConfirmBookingClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("//MainPage");
	}
}