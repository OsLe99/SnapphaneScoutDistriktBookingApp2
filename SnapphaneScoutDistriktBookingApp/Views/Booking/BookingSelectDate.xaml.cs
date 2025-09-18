using SnapphaneScoutDistriktBookingApp.Models;
using SnapphaneScoutDistriktBookingApp.Services.Interface;
using Syncfusion.Maui.Calendar;
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
        BindingContext = _customer;
        _validateBookingService = validateBookingService;

        LoadBlackoutDatesAsync();
    }

    private async void LoadBlackoutDatesAsync()
    {
        var allBookings = await _dbService.GetAllBookingsAsync();

        _relevantBookings = allBookings.Where(b => b.BookingType == _customer.BookingType && b.IsConfirmed).ToList();

        BookingCalendar.SelectableDayPredicate = IsSelectableDates;
    }

    private bool IsSelectableDates(DateTime date)
    {
        foreach (var booking in _relevantBookings)
        {
            if (date.Date >= booking.StartDate.Date && date.Date <= booking.EndDate.Date)
                return false;
        }
        return true;
    }

    private void OnCalendarSelectionChanged(object sender, CalendarSelectionChangedEventArgs e)
    {
        var range = BookingCalendar.SelectedDateRange;

        if (range == null || range.StartDate == null || range.EndDate == null) 
            return;

        var start = (DateTime)BookingCalendar.SelectedDateRange.StartDate;
        var end = (DateTime)BookingCalendar.SelectedDateRange.EndDate;

        foreach (var booking in _relevantBookings)
        {
            if (start <= booking.EndDate.Date && end >= booking.StartDate.Date)
            {
                DisplayAlert("Otillgängligt", "Det finns redan en bokning under dessa datum", "OK");
                return;
            }
        }

        _customer.StartDate = start;
        _customer.EndDate = end;
    }

    private void OnStartClicked(object sender, EventArgs e)
    {
        StartTimePicker.IsOpen = true;
    }

    private void OnEndClicked(object sender, EventArgs e)
    {
        EndTimePicker.IsOpen = true;
    }


    private async void OnChangeToBookingCustomerInfoAsync(object sender, EventArgs e)
    {
        if (await _validateBookingService.CheckIfValidTime(StartTimePicker.SelectedTime, EndTimePicker.SelectedTime) == false)
        {
            await DisplayAlert("Fel vid tid", "Startdatum kan inte vara tidigare eller samma som slutdatum.", "OK");
            return;
        }

        _customer.StartDate += StartTimePicker.SelectedTime.Value;
        _customer.EndDate += EndTimePicker.SelectedTime.Value;
        await Navigation.PushAsync(new BookingCustomerInfo(_customer, _bookingService, _validateBookingService));
    }

    /* 
    Check för:
    Datum: Inom samma tidsram ex. inga datum-hopp i bokningen pga andra bokningar
    Tid: starttid > sluttid. Avklarat!
    Kolla vid OnChangeToBooking
     */
}