using SnapphaneScoutDistriktBookingApp.Services;
using SnapphaneScoutDistriktBookingApp.Services.Interface;
using System.Diagnostics;

namespace SnapphaneScoutDistriktBookingApp.Views;

public partial class LoginPage : ContentPage
{
    private readonly IUserSessionService _userSession;
    private readonly IAdminService _adminService;
    public LoginPage(IUserSessionService userSession, IAdminService adminService)
	{
		InitializeComponent();
        _userSession = userSession;
        _adminService = adminService;
    }
    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string userName = NameEntry.Text.Trim();
        string userEmail = EmailEntry.Text.Trim();

        if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(userEmail))
        {
            await DisplayAlert("Fel", "Du måste ange både namn och email!", "OK");
            return;
        }

        _userSession.SetUser(userName, userEmail);

        await DisplayAlert("Välkommen!", $"Hej, {_userSession.UserName}!", "OK");

        bool isAdmin = await _adminService.CheckIfAdminAsync(
            _userSession.UserName,
            _userSession.UserEmail
            );

        if (isAdmin == true)
        {
            Debug.WriteLine("Admin login found");

            Entry passwordEntry = new Entry { Placeholder = "Ditt lösenord" };
            var popupPassword = new ContentPage
            {
                Content = new VerticalStackLayout
                {
                    Padding = 20,
                    Spacing = 10,
                    Children =
                 {
                     new Label { Text = "Lösenord" },
                     passwordEntry,
                     new Button
                     {
                         Text = "OK",
                         Command = new Command(async () =>
                         {
                             bool isAdmin = await _adminService.TryLoginAdminAsync(
                                 _userSession.UserName,
                                 _userSession.UserEmail,
                                 passwordEntry.Text
                             );

                             if (isAdmin)
                             {
                                 await DisplayAlert("Inloggning", "Admin inloggning lyckades!", "OK");
                                 await Navigation.PopModalAsync();
                                 
                             }
                             else
                             {
                                 await DisplayAlert("Fel", "Ogiltigt lösenord.", "OK");
                             }
                         })
                     }
                 }
                }
            };
            await Navigation.PushModalAsync(popupPassword);
        }
    }
}