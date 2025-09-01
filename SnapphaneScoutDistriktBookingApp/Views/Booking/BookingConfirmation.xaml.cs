namespace SnapphaneScoutDistriktBookingApp.Views.Booking;

public partial class BookingConfirmation : ContentPage
{
	public BookingConfirmation()
	{
		InitializeComponent();
	}

	private async void OnConfirmBookingClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("//MainPage");
	}
}