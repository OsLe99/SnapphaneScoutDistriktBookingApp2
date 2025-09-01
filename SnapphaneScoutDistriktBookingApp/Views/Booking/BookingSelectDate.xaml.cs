namespace SnapphaneScoutDistriktBookingApp.Views.Booking;

public partial class BookingSelectDate : ContentPage
{
	public BookingSelectDate()
	{
		InitializeComponent();
	}
    private async void OnChangeToBookingCustomerInfo(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new BookingCustomerInfo());
    }
}