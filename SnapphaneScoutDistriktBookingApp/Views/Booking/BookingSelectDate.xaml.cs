using SnapphaneScoutDistriktBookingApp.Models;
using SnapphaneScoutDistriktBookingApp.Services.Interface;
using Syncfusion.Maui.Calendar;
using Syncfusion.Maui.Picker;
namespace SnapphaneScoutDistriktBookingApp.Views.Booking;

public partial class BookingSelectDate : ContentPage
{
    private Customer _customer;
    private readonly IBookingService _bookingService;
    private readonly IValidateBookingService _validateBookingService;
    private readonly IDbService _dbService;
    private List<Customer> _relevantBookings = new();

    public BookingSelectDate(Customer customer, IBookingService bookingService, IDbService dbService, IValidateBookingService validateBookingService)
    {
        InitializeComponent();

        _customer = customer;
        _bookingService = bookingService;
        _dbService = dbService;
        _validateBookingService = validateBookingService;
        BindingContext = _customer;

        LoadBlackoutDatesAsync();
        UpdateTimeButtons();
    }

    private async void LoadBlackoutDatesAsync()
    {
        var allBookings = await _dbService.GetAllBookingsAsync();
        _relevantBookings = allBookings.Where(b => b.BookingType == _customer.BookingType && b.IsConfirmed).ToList();
        BookingCalendar.SelectableDayPredicate = IsSelectableDates;
    }

    private bool IsSelectableDates(DateTime date)
    {
        return !_relevantBookings.Any(b => date.Date >= b.StartDate && date.Date <= b.EndDate);
    }

    private void OnCalendarSelectionChanged(object sender, CalendarSelectionChangedEventArgs e)
    {
        if (!TryGetSelectedRange(out var start, out var end))
            return;

        if (IsOverlappingWithExistingBookings(start, end))
        {
            DisplayAlert("Otillgängligt", "Det finns redan en bokning under dessa datum", "OK");
        }
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

    private void OnStartTimeChanged(object sender, EventArgs e)
    {
        UpdateTimeButtons();
        if (EndTimePicker.SelectedTime < StartTimePicker.SelectedTime)
            EndTimePicker.SelectedTime = StartTimePicker.SelectedTime;
    }

    private void OnEndTimeChanged(object sender, EventArgs e)
    {
        UpdateTimeButtons();
        if (EndTimePicker.SelectedTime < StartTimePicker.SelectedTime)
            EndTimePicker.SelectedTime = StartTimePicker.SelectedTime;
    }

    private void UpdateTimeButtons()
    {
        StartTimeButton.Text = $"Starttid: {StartTimePicker.SelectedTime?.ToString("hh\\:mm")}";
        EndTimeButton.Text = $"Sluttid: {EndTimePicker.SelectedTime?.ToString("hh\\:mm")}";
    }

    private async void OnChangeToBookingCustomerInfoAsync(object sender, EventArgs e)
    {
        if (!TryGetSelectedRange(out var startDate, out var endDate) ||
            !StartTimePicker.SelectedTime.HasValue || !EndTimePicker.SelectedTime.HasValue)
        {
            await DisplayAlert("Fel", "Välj datum och tid först.", "OK");
            return;
        }

        if (!await _validateBookingService.CheckIfValidTime(StartTimePicker.SelectedTime, EndTimePicker.SelectedTime))
        {
            await DisplayAlert("Fel", "Starttid kan inte vara senare än sluttid.", "OK");
            return;
        }

        _customer.StartDate = CombineDateAndTime(startDate, StartTimePicker.SelectedTime.Value);
        _customer.EndDate = CombineDateAndTime(endDate, EndTimePicker.SelectedTime.Value);

        await Navigation.PushAsync(new BookingCustomerInfo(_customer, _bookingService, _validateBookingService));
    }

    private DateTime CombineDateAndTime(DateTime date, TimeSpan time)
    {
        return date.Date + time;
    }
}

/* 
Check för:
Datum: Inom samma tidsram ex. inga datum-hopp i bokningen pga andra bokningar
Tid: starttid > sluttid. Avklarat!
Kolla vid OnChangeToBooking

Manus vigilat, Machina servit.
Daemonium in errore latet, sed dextra vigilat.
 */