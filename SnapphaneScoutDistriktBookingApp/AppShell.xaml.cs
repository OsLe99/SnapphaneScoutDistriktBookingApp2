using SnapphaneScoutDistriktBookingApp.Pages;
using SnapphaneScoutDistriktBookingApp.Services.Interface;
using SnapphaneScoutDistriktBookingApp.Views;
using System.Diagnostics;

namespace SnapphaneScoutDistriktBookingApp
{
    public partial class AppShell : Shell
    {
        private readonly IClerkUserSessionService _userSession;
        private readonly IClerkAuthService _authService;
        public AppShell(IClerkUserSessionService userSession, IClerkAuthService authService)
        {
            InitializeComponent();

            _userSession = userSession;
            _authService = authService;

            Routing.RegisterRoute(nameof(RegisterPage), typeof(RegisterPage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _userSession.CheckLoginStateAsync();
            UpdateSignInSignOutUI(_userSession.IsLoggedIn);
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

        private async void OnSignInClicked(object sender, EventArgs e)
        {
            Shell.Current.FlyoutIsPresented = false;
            await Shell.Current.Navigation.PushModalAsync(new LoginPage(_userSession, _authService));
        }

        private async void OnSignOutClicked(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Logga ut", "Är du säker på att du vill logga ut?", "Ja", "Nej");
            if (!confirm)
            {
                return;
            }

            Shell.Current.FlyoutIsPresented = false;

            if (await _authService.SignOutAsync() == false)
            {
                await DisplayAlert("Fel", "Något gick fel vid utloggning.", "OK");
                return;
            }
            UpdateSignInSignOutUI(false);
            await DisplayAlert("Utloggning", "Du är nu utloggad.", "OK");
            await _userSession.ResetUser();
            await Shell.Current.GoToAsync("//MainPage");
        }

        public void UpdateSignInSignOutUI(bool isLoggedIn)
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Debug.WriteLine($"Updating UI. IsLoggedIn: {isLoggedIn}");
                SignInButton.IsVisible = !isLoggedIn;
                SignOutButton.IsVisible = isLoggedIn;
            });
        }
    }
}
