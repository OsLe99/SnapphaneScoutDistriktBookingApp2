using SnapphaneScoutDistriktBookingApp.Services;
using SnapphaneScoutDistriktBookingApp.Services.Interface;
using SnapphaneScoutDistriktBookingApp.Views.Booking;
using SnapphaneScoutDistriktBookingApp.Views;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SnapphaneScoutDistriktBookingApp.Views
{
    public partial class MainPage : ContentPage
    {
        private readonly IClerkUserSessionService _userSession;
        private readonly IAdminService _adminService;
        private readonly IDbService _db;
        private readonly IEmailService _emailService;
        private readonly IBookingService _bookingService;
        private readonly IValidateBookingService _validateBookingService;
        private readonly IClerkAuthService _clerkAuthService;
        private readonly IRoleAuthService _roleAuthService;
        public MainPage(IClerkUserSessionService userSession, IAdminService adminService, IDbService db, IEmailService emailService, IBookingService bookingService, IValidateBookingService validateBookingService, IClerkAuthService clerkAuthService, IRoleAuthService roleAuthService)
        {
            InitializeComponent();
            _userSession = userSession;
            _adminService = adminService;
            _emailService = emailService;
            _db = db;
            _bookingService = bookingService;
            _validateBookingService = validateBookingService;
            _clerkAuthService = clerkAuthService;
            _userSession.LoadUserData();
            BindingContext = userSession;
            _roleAuthService = roleAuthService;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _userSession.LoadUserData();
        }

        private async void OnChangeToBookingSelectAsync(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//BookingPage");
        }

        private async void OnClickedGoToInfoPageAsync(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new InfoPage(_db));
        }
    }

}
