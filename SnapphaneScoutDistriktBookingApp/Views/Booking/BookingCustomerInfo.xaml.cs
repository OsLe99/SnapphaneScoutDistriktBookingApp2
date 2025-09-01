namespace SnapphaneScoutDistriktBookingApp.Views.Booking;

public partial class BookingCustomerInfo : ContentPage
{
    private Models.Customer.TypeOfBooking bookingtype = Models.Customer.TypeOfBooking.None;
    public BookingCustomerInfo()
    {
        InitializeComponent();
    }

    private async void OnChangeToBookingConfirmation(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new BookingConfirmation());
    }

    private void OnCheckChange(object sender, CheckedChangedEventArgs e)
    {
        if (e.Value)
        {
            statusLabel.Text = "Scoutmedlem";
            hiddenLabel.IsVisible = true;
            orgNameInput.IsVisible = true;
        }
        else
        {
            hiddenLabel.IsVisible = false;
            orgNameInput.IsVisible = false;
        }
    }

    private async void SaveCustomerInfo()
    {
        var newCustomer = new Models.Customer()
        {
            Name = myName.Text,
            Phone = myPhone.Text,
            Email = myEmail.Text,
            IsOrg = myCheckBox.IsChecked,
            OrgName = (myCheckBox.IsChecked == true ? orgNameInput.Text : ""),
            BookingType = bookingtype,
            IsConfirmed = false
        };
    }
}