using SnapphaneScoutDistriktBookingApp.Services;
using SnapphaneScoutDistriktBookingApp.Services.Interface;
using SnapphaneScoutDistriktBookingApp.Views.Booking;
using SnapphaneScoutDistriktBookingApp.Views;
using System.Threading.Tasks;

namespace SnapphaneScoutDistriktBookingApp.Views
{
    public partial class MainPage : ContentPage
    {
        private readonly IUserSessionService _userSession;
        private readonly IAdminService _adminService;
        private readonly IDbService _db;
        private readonly IEmailService _emailService;
        private readonly IBookingService _bookingService;
        private readonly IValidateBookingService _validateBookingService;
        public MainPage(IUserSessionService userSession, IAdminService adminService, IDbService db, IEmailService emailService, IBookingService bookingService, IValidateBookingService validateBookingService)
        {
            InitializeComponent();
            _userSession = userSession;
            _adminService = adminService;
            _emailService = emailService;
            _db = db;
            _bookingService = bookingService;
            BindingContext = _userSession;
            OnAppearing();
            _validateBookingService = validateBookingService;
        }
        bool pageStarted = false;
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!pageStarted)
            {
                pageStarted = true;
            }
        }

        private async void OnChangeToBookingSelectAsync(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//BookingPage");
        }

        public async void OnResetUserAsync(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Ändra användare", "Är du säker på att du vill ändra användare?", "Ja", "Nej");
            if (!confirm)
            {
                return;
            }

            _userSession.ResetUser();

            await DisplayAlert("Utloggning", "Du är nu utloggad.", "OK");
            await Navigation.PushAsync(new LoginPage(_userSession, _adminService, _db));
        }

        private async void OnClickedGoToAdminPageAsync(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.AdminPage(_db, _emailService, _userSession));
        }

        private async void OnClickedGoToInfoPageAsync(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.InfoPage(_db));
        }
    }

}
