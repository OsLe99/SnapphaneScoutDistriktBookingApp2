using SnapphaneScoutDistriktBookingApp.Pages;
using SnapphaneScoutDistriktBookingApp.Services;
using SnapphaneScoutDistriktBookingApp.Services.Interface;
using SnapphaneScoutDistriktBookingApp.Views;
using System.Diagnostics;

namespace SnapphaneScoutDistriktBookingApp.Views;

public partial class LoginPage : ContentPage
{
    private readonly IClerkUserSessionService _userSession;
    private readonly IAdminService _adminService;
    private readonly IDbService _dbService;
    private readonly IClerkAuthService _clerkAuthService;
    public LoginPage(IClerkUserSessionService userSession, IAdminService adminService, IDbService dbService, IClerkAuthService clerkAuthService)
	{
		InitializeComponent();
        _userSession = userSession;
        _adminService = adminService;
        _dbService = dbService;
        _clerkAuthService = clerkAuthService;
    }
    private async void OnLoginClickedAsync(object sender, EventArgs e)
    {
        string userEmail = EmailEntry.Text;
        string userPassword = PasswordEntry.Text;

        if (string.IsNullOrEmpty(userEmail) || string.IsNullOrEmpty(userPassword))
        {
            await DisplayAlert("Fel", "Du måste ange en komplett inloggning.", "OK");
            return;
        }

        // Kolla ifall inloggning går igenom. Om inte detta fungerar så kan det vara värt att använda en webview för att logga in
        string jwt = await _clerkAuthService.SignInAsync(userEmail, userPassword);

        if (jwt != null)
        {
            await DisplayAlert("Inloggning", "Inloggning lyckades!", "OK");
            await Shell.Current.GoToAsync("//MainPage");
        }
        else
        {
            Debug.Write($"Something went wrong. jwt = {jwt}");
            await DisplayAlert("Fel", "Ogiltiga inloggningsuppgifter.", "OK");
            PasswordEntry.Text = "";
        }
    }

    private async void OnRegisterClickedAsync(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync(nameof(RegisterPage));
    }
}