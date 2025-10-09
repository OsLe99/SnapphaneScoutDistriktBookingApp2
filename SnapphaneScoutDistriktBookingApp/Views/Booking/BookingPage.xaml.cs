using SnapphaneScoutDistriktBookingApp.ViewModels;
using SnapphaneScoutDistriktBookingApp.Services.Interface;

namespace SnapphaneScoutDistriktBookingApp.Views.Booking;

public partial class BookingPage : ContentPage
{
    public BookingPage(IUserSessionService userSessionService, IBookingService bookingService, IDbService dbService, IValidateBookingService validateBookingService)
    {
        InitializeComponent();
        BindingContext = new BookingViewModel(dbService, bookingService, validateBookingService);
        if (BindingContext is BookingViewModel vm)
        {
            vm.CurrentView = new BookingStep1View { BindingContext = vm };
        }
    }
}
