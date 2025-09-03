using SnapphaneScoutDistriktBookingApp.Models;

namespace SnapphaneScoutDistriktBookingApp.Views.Booking;

public partial class BookingConfirmation : ContentPage
{
	private readonly Customer _customer;
	private int _amountOf;
    public BookingConfirmation(Customer customer)
	{
		InitializeComponent();
        _customer = customer;
        BindingContext = customer;
    }
	public void FindAmountOf(Customer customer)
	{
		switch(_customer.BookingType)
		{
			case Customer.TypeOfBooking.Canoe:
                _amountOf = _customer.NumberOfCanoes ?? 0;
				break;
			case Customer.TypeOfBooking.Cabin:
				_amountOf = _customer.NumberOfCabin ?? 0;
				break;
			case Customer.TypeOfBooking.LeanTo:
				_amountOf = _customer.NumberOfLeanTo ?? 0;
				break;
			case Customer.TypeOfBooking.CampGrounds:
				_amountOf = _customer.NumberOfCampground ?? 0;
				break;
        }
    }

    private async void OnConfirmBookingClicked(object sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("//MainPage");
	}
}