using SnapphaneScoutDistriktBookingApp.Services;
using SnapphaneScoutDistriktBookingApp.Services.Interface;
using System.Diagnostics;

namespace SnapphaneScoutDistriktBookingApp.Views;

public partial class LoginPage : ContentPage
{
    private readonly IUserSessionService _userSession;
    private readonly IAdminService _adminService;
    private readonly IDbService _dbService;
    public LoginPage(IUserSessionService userSession, IAdminService adminService, IDbService dbService)
	{
		InitializeComponent();
        _userSession = userSession;
        _adminService = adminService;
        _dbService = dbService;
    }
    private async void OnLoginClickedAsync(object sender, EventArgs e)
    {
        string userEmail = EmailEntry.Text;
        string userPassword = PasswordEntry.Text;

        if (string.IsNullOrEmpty(userEmail) || string.IsNullOrEmpty(userPassword))
        {
            await DisplayAlert("Fel", "Du måste ange en komplett inloggning!", "OK");
            return;
        }

        bool isAdmin = await _dbService.CheckAdminCredentialsAsync(userEmail, userPassword);

        if (isAdmin)
        {
            await DisplayAlert("Inloggning", "Inloggning lyckades!", "OK");
            _userSession.SetAdmin(true);
            Preferences.Set("isAdmin", true);
            await Shell.Current.GoToAsync("//MainPage");
        }
        else
        {
            await DisplayAlert("Fel", "Ogiltiga inloggningsuppgifter.", "OK");
            PasswordEntry.Text = "";
        }
    }
}