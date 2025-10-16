using SnapphaneScoutDistriktBookingApp.ViewModels;
using SnapphaneScoutDistriktBookingApp.Services.Interface;

namespace SnapphaneScoutDistriktBookingApp.Views.Booking;

public partial class BookingPage : ContentPage
{
	private readonly IClerkUserSessionService _userSessionService;
	private readonly IBookingService _bookingService;
	private readonly IDbService _dbService;
	private readonly IValidateBookingService _validateBookingService;
    public BookingPage(IClerkUserSessionService userSessionService, IBookingService bookingService, IDbService dbService, IValidateBookingService validateBookingService)
	{
        InitializeComponent();
        BindingContext = new BookingViewModel(dbService, bookingService, validateBookingService);
        if (BindingContext is BookingViewModel vm)
        {
            vm.CurrentView = new BookingStep1View { BindingContext = vm };
        }
    }
}
