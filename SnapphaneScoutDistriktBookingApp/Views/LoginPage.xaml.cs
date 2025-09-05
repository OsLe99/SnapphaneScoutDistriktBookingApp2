using SnapphaneScoutDistriktBookingApp.Data;
using System.Diagnostics;

namespace SnapphaneScoutDistriktBookingApp.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}
    private async void OnLoginClicked(object sender, EventArgs e)
    {
        string userName = NameEntry.Text?.Trim();
        string userEmail = EmailEntry.Text?.Trim();

        if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(userEmail))
        {
            await DisplayAlert("Fel", "Du måste ange både namn och email!", "OK");
            return;
        }

        Data.UserSession.Instance.SetUser(userName, userEmail);

        await DisplayAlert("Välkommen!", $"Hej, {Data.UserSession.Instance.UserName}!", "OK");

        bool isAdmin = await AdminService.CheckIfAdminAsync(
            Data.UserSession.Instance.UserName,
            Data.UserSession.Instance.UserEmail
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
                             bool isAdmin = await AdminService.TryLoginAdminAsync(
                                 Data.UserSession.Instance.UserName,
                                 Data.UserSession.Instance.UserEmail,
                                 passwordEntry.Text
                             );

                             if (isAdmin)
                             {
                                 await DisplayAlert("Inloggning", "Admin inloggning lyckades!", "OK");
                                 await Shell.Current.GoToAsync("//MainPage");

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
        else
        {
            await Shell.Current.GoToAsync("//MainPage");
        }
    }
}