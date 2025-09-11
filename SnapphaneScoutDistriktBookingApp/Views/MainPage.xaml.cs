using SnapphaneScoutDistriktBookingApp.Services;
using SnapphaneScoutDistriktBookingApp.Services.Interface;
using System.Threading.Tasks;

namespace SnapphaneScoutDistriktBookingApp
{
    public partial class MainPage : ContentPage
    {
        private readonly IUserSessionService _userSession;
        private readonly IAdminService _adminService;
        private readonly IDbService _db;
        private readonly IEmailService _emailService;
        public MainPage(IUserSessionService userSession, IAdminService adminService, IDbService db, IEmailService emailService)
        {
            InitializeComponent();
            _userSession = userSession;
            _adminService = adminService;
            _emailService = emailService;
            _db = db;
            BindingContext = _userSession;
            OnAppearing();
        }
        bool pageStarted = false;
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (!pageStarted)
            {
                pageStarted = true;
                await CheckUserSession();
            }

            if (_userSession.IsAdmin == false)
            {
                AdminPage.IsVisible = false;
            }
            else
            {
                AdminPage.IsVisible = true;
            }
        }

        private void OnClickedChangeTheme(object sender, EventArgs e)
        {
            var app = Application.Current;
            if (app.UserAppTheme == AppTheme.Light)
            {
                app.UserAppTheme = AppTheme.Dark;
            }
            else
            {
                app.UserAppTheme = AppTheme.Light;
            }
        }

        private async void OnChangeToBookingSelect(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BookingPage());
        }

        public async Task CheckUserSession()
        {
            if (!_userSession.IsUserSet())
            {
                await Navigation.PushAsync(new Views.LoginPage(_userSession, _adminService));
            }
        }

        public async void OnResetUser(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Ändra användare", "Är du säker på att du vill ändra användare?", "Ja", "Nej");
            if (!confirm)
            {
                return;
            }

            _userSession.ResetUser();

            await DisplayAlert("Användarinformation återställd", "Nuvarande sparad användare är borttagen.", "OK");
            await CheckUserSession();
        }

        private async void OnClickedGoToAdminPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.AdminPage(_db, _emailService));
        }

        private async void OnClickedGoToInfoPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.InfoPage());
        }
    }

}
