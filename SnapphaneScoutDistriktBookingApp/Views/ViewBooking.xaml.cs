using MongoDB.Bson;
using SnapphaneScoutDistriktBookingApp.Models;
using SnapphaneScoutDistriktBookingApp.Services.Interface;
using System.Windows.Input;
using BookingModel = SnapphaneScoutDistriktBookingApp.Models.Booking;

namespace SnapphaneScoutDistriktBookingApp.Views;

public partial class ViewBooking : ContentPage
{
	private readonly IBookingService _bookingService;
    private readonly IValidateBookingService _validateBookingService;
    private readonly IDbService _dbService;
    public ICommand EditBookingCommand { get; }
	public ViewBooking(IBookingService bookingService, IValidateBookingService validateBookingService, IDbService dbService)
	{
		InitializeComponent();
		_bookingService = bookingService;
        _validateBookingService = validateBookingService;
        _dbService = dbService;

        EditBookingCommand = new Command<BookingModel>(async booking =>
        {
            if (booking != null)
            {
                await Navigation.PushAsync(new EditBookingPage(booking, _bookingService, _validateBookingService, dbService));
            }
        });
        BindingContext = this;
	}

    protected override void OnAppearing()
    {
        BookingIdEntry.Text = string.Empty;
        EmailEntry.Text = string.Empty;
        ResultsCollectionView.ItemsSource = null;
        ResultsCollectionView.SelectedItem = null;
    }

	private async void OnSearchClicked(object sender, EventArgs e)
	{
		var email = EmailEntry.Text?.Trim();
		var idText = BookingIdEntry.Text?.Trim();

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(idText))
        {
            await DisplayAlert("Fel", "Ange både bokningsnummer och email.", "OK");
            return;
        }

        try
        {
            var id = new Guid(idText);
            var booking = await _bookingService.GetBookingByIdAndEmailAsync(id, email);

            if (booking != null)
            {
                ResultsCollectionView.ItemsSource = new List<BookingModel> { booking };
            }
            else
            {
                ResultsCollectionView.ItemsSource = null;
                await DisplayAlert("Kunde ej hitta", "Ingen bokning hittad.", "OK");
            }
        }
        catch (FormatException)
        {
            await DisplayAlert("Error", "Något gick fel.", "OK");
        }
    }

    //private async void OnBookingSelected(object sender, SelectionChangedEventArgs e)
    //{
    //    if (e.CurrentSelection.FirstOrDefault() is Customer selectedBooking)
    //    {
    //        await Navigation.PushAsync(new EditBookingPage(selectedBooking, _bookingService, _validateBookingService));
    //    }
    //}
}