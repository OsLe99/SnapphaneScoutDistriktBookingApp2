using SnapphaneScoutDistriktBookingApp.Views.Booking;
using Microsoft.Maui.Controls;
namespace SnapphaneScoutDistriktBookingApp.Views;

public partial class BookingExtraInfo : ContentPage
{
	public BookingExtraInfo()
	{
		InitializeComponent();
	}

	private async void OnChangeToBookingDate(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new BookingSelectDate());
    }
}