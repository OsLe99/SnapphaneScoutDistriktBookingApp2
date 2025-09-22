using SnapphaneScoutDistriktBookingApp.Models;
using SnapphaneScoutDistriktBookingApp.Services.Interface;
using SnapphaneScoutDistriktBookingApp.ViewModels;

namespace SnapphaneScoutDistriktBookingApp.Views;

public partial class EditBookingPage : ContentPage
{
	private readonly EditBookingViewModel _viewModel;
	private readonly IBookingService _bookingService;
	private readonly IValidateBookingService _validateBookingService;
	public EditBookingPage(Customer booking, IBookingService bookingService, IValidateBookingService validateBookingService)
	{
		InitializeComponent();
		_bookingService = bookingService;
		_validateBookingService = validateBookingService;
		_viewModel = new EditBookingViewModel(booking);
		BindingContext = _viewModel;
	}

	private async void OnSaveClicked(object sender, EventArgs e)
	{
        var updatedBooking = _viewModel.GetBooking();

        var errors = _validateBookingService.ValidateBookingDetails(updatedBooking);

        if (errors.Any())
        {
            var errorMessage = string.Join("\n", errors);
            await DisplayAlert("Fel", errorMessage, "OK");
            return;
        }

        await _bookingService.UpdateBookingAsync(updatedBooking);

        await DisplayAlert("Sparat", "Bokningen har uppdaterats.", "OK");

        await Navigation.PopAsync();
    }
}