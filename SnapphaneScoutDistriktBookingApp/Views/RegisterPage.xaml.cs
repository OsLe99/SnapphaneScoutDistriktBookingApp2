using SnapphaneScoutDistriktBookingApp.Models;
using SnapphaneScoutDistriktBookingApp.Services;
using SnapphaneScoutDistriktBookingApp.Services.Interface;

namespace SnapphaneScoutDistriktBookingApp.Pages;

public partial class RegisterPage : ContentPage
{
    private readonly IClerkUserSessionService _userService;

    public RegisterPage(IClerkUserSessionService userService)
    {
        InitializeComponent();
        _userService = userService;
    }

    private async void OnRegisterClicked(object sender, EventArgs e)
    {
        StatusLabel.IsVisible = false;

        string firstName = FirstNameEntry.Text?.Trim() ?? "";
        string lastName = LastNameEntry.Text?.Trim() ?? "";
        string email = EmailEntry.Text?.Trim() ?? "";
        string password = PasswordEntry.Text ?? "";

        if (string.IsNullOrWhiteSpace(firstName) ||
            string.IsNullOrWhiteSpace(lastName) ||
            string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password))
        {
            StatusLabel.Text = "Alla fält måste fyllas i.";
            StatusLabel.IsVisible = true;
            return;
        }

        try
        {
            var user = await _userService.CreateUserAsync(firstName, lastName, email, password);

            if (user != null)
            {
                await DisplayAlert("Success", $"Välkommen {user.FirstName}!", "OK");
                await Shell.Current.Navigation.PopModalAsync();
            }
            else
            {
                StatusLabel.Text = "Något gick fel.";
                StatusLabel.IsVisible = true;
            }
        }
        catch (Exception ex)
        {
            StatusLabel.Text = $"Error: {ex.Message}";
            StatusLabel.IsVisible = true;
        }
    }
}
