using SnapphaneScoutDistriktBookingApp.Models;
using SnapphaneScoutDistriktBookingApp.Services.Interface;
namespace SnapphaneScoutDistriktBookingApp.Views.Booking;

public partial class BookingCustomerInfo : ContentPage
{
    private Customer _customer;
    private readonly IBookingService _bookingService;
    private readonly IValidateBookingService _validateBookingService;
    public BookingCustomerInfo(Customer customer, IBookingService bookingService, IValidateBookingService validateBookingService)
    {
        InitializeComponent();
        _customer = customer;
        _bookingService = bookingService;
        _validateBookingService = validateBookingService;
        BindingContext = _customer;
    }

    private async void OnChangeToBookingConfirmationAsync(object sender, EventArgs e)
    {
        var errors = _validateBookingService.ValidateBookingDetails(_customer);

        if(errors.Any())
        {
            string message = string.Join("\n", errors);
            await DisplayAlert("Fel", message, "OK");
            return;
        }
        await Navigation.PushAsync(new BookingConfirmation(_customer, _bookingService));
    }

    private void OnCheckChange(object sender, CheckedChangedEventArgs e)
    {
        if (e.Value)
        {
            statusLabel.Text = "Scoutmedlem";
            hiddenLabel.IsVisible = true;
            orgNameInput.IsVisible = true;
        }
        else
        {
            hiddenLabel.IsVisible = false;
            orgNameInput.IsVisible = false;
        }
    }
}