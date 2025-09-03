using SnapphaneScoutDistriktBookingApp.Models;
namespace SnapphaneScoutDistriktBookingApp.Views.Booking;

public partial class BookingCustomerInfo : ContentPage
{
    private Customer _customer;
    public BookingCustomerInfo(Customer customer)
    {
        InitializeComponent();
        _customer = customer;
    }

    private async void OnChangeToBookingConfirmation(object sender, EventArgs e)
    {
        SaveCustomerInfo(_customer);
        await Navigation.PushAsync(new BookingConfirmation(_customer));
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

    private async void SaveCustomerInfo(Customer customer)
    {
        customer.Name = myName.Text;
        customer.Phone = myPhone.Text;
        customer.Email = myEmail.Text;
        customer.IsOrg = myCheckBox.IsChecked;
        customer.OrgName = (myCheckBox.IsChecked == true ? orgNameInput.Text : "");
        customer.IsConfirmed = false;
    }
}