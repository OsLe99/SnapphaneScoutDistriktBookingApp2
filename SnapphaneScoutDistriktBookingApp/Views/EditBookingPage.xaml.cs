using SnapphaneScoutDistriktBookingApp.Models;
using SnapphaneScoutDistriktBookingApp.Services.Interface;
using SnapphaneScoutDistriktBookingApp.ViewModels;
using Syncfusion.Maui.Calendar;
using Syncfusion.Maui.Picker;

namespace SnapphaneScoutDistriktBookingApp.Views;

public partial class EditBookingPage : ContentPage
{
	private readonly EditBookingViewModel _viewModel;
	private readonly IBookingService _bookingService;
	private readonly IValidateBookingService _validateBookingService;
    private readonly IDbService _dbService;
    private List<Models.Booking> _relevantBookings = new();
    public EditBookingPage(Models.Booking booking, IBookingService bookingService, IValidateBookingService validateBookingService, IDbService dbService)
	{
		InitializeComponent();
		_bookingService = bookingService;
		_validateBookingService = validateBookingService;
		_viewModel = new EditBookingViewModel(booking);
        _dbService = dbService;
		BindingContext = _viewModel;

        BookingCalendar.SelectedDateRange = new Syncfusion.Maui.Calendar.CalendarDateRange(
            booking.StartDate.Date,
            booking.EndDate.Date
            );

        StartTimePicker.SelectedTime = _viewModel.StartTime;
        EndTimePicker.SelectedTime = _viewModel.EndTime;
	}

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadBlackoutDatesAsync();
    }

    private void OnStartClicked(object sender, EventArgs e)
    {
        EndTimePicker.IsOpen = false;
        StartTimePicker.IsOpen = true;
    }

    private void OnEndClicked(object sender, EventArgs e)
    {
        StartTimePicker.IsOpen = false;
        EndTimePicker.IsOpen = true;
    }

    private async Task LoadBlackoutDatesAsync()
    {
        var allBookings = await _dbService.GetAllBookingsAsync();
        _relevantBookings = allBookings
            .Where(b => b.BookingType == _viewModel.GetBooking().BookingType && b.IsConfirmed)
            .ToList();

        BookingCalendar.SelectableDayPredicate = IsSelectableDates;
	}

    private bool IsSelectableDates(DateTime date)
    {
        return !_relevantBookings.Any(b => date.Date >= b.StartDate && date.Date <= b.EndDate);
    }

    private void OnCalendarSelectionChanged(object sender, Syncfusion.Maui.Calendar.CalendarSelectionChangedEventArgs e)
    {
        if (!TryGetSelectedRange(out var start, out var end))
            return;

        if (IsOverlappingWithExistingBookings(start, end))
            DisplayAlert("Otillgängligt", "Det finns redan en bokning under dessa datum", "OK");

        _viewModel.StartDate = start;
        _viewModel.EndDate = end;
    }

    private bool TryGetSelectedRange(out DateTime start, out DateTime end)
    {
        start = DateTime.MinValue;
        end = DateTime.MinValue;

        var range = BookingCalendar.SelectedDateRange;
        if (range?.StartDate == null)
            return false;

        start = range.StartDate.Value.Date;
        end = range.EndDate?.Date ?? start;
        return true;
    }

    private bool IsOverlappingWithExistingBookings(DateTime start, DateTime end)
    {
        return _relevantBookings.Any(b => start <= b.EndDate.Date && end >= b.StartDate.Date);
    }

	private async void OnSaveClicked(object sender, EventArgs e)
	{
        var updatedBooking = _viewModel.GetBooking();

        if (updatedBooking.EndDate < updatedBooking.StartDate)
        {
            await DisplayAlert("Fel", "Sluttid kan inte vara före starttid.", "OK");
            return;
        }

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