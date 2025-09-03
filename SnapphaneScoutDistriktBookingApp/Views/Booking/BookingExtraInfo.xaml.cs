using SnapphaneScoutDistriktBookingApp.Views.Booking;
using Microsoft.Maui.Controls;
using SnapphaneScoutDistriktBookingApp.Models;
using SnapphaneScoutDistriktBookingApp.ViewModels;
namespace SnapphaneScoutDistriktBookingApp.Views;

public partial class BookingExtraInfo : ContentPage
{
    private Customer _customer;
    public BookingExtraInfo(Customer customer)
    {
        InitializeComponent();
        _customer = customer;

        switch (customer.BookingType)
        {
            case Customer.TypeOfBooking.Canoe:
                infoLabel.Text = "Hur många kanoter vill ni boka?";
                numberInput.Placeholder = "Antal kanoter";
                break;
                case Customer.TypeOfBooking.Cabin:
                infoLabel.Text = "Hur många personer ska bo i stugan?";
                numberInput.Placeholder = "Antal personer i stugan";
                break;
                case Customer.TypeOfBooking.LeanTo:
                infoLabel.Text = "Hur många vindskydd vill ni boka?";
                numberInput.Placeholder = "Antal vindskydd";
                break;
                case Customer.TypeOfBooking.CampGrounds:
                infoLabel.Text = "Hur många personer ska bo på lägerområdet?";
                numberInput.Placeholder = "Antal personer på lägerområdet";
                break;
        }
    }
    private void SaveNumberInputToCustomer()
    {
        if (int.TryParse(numberInput.Text, out int value))
        {
            switch (_customer.BookingType)
            {
                case Customer.TypeOfBooking.Canoe:
                    _customer.NumberOfCanoes = value;
                    break;
                case Customer.TypeOfBooking.Cabin:
                    _customer.NumberOfCabin = value;
                    break;
                case Customer.TypeOfBooking.LeanTo:
                    _customer.NumberOfLeanTo = value;
                    break;
                case Customer.TypeOfBooking.CampGrounds:
                    _customer.NumberOfCampground = value;
                    break;
            }
        }
    }

    private async void OnChangeToBookingDate(object sender, EventArgs e)
	{
        SaveNumberInputToCustomer();
		await Navigation.PushAsync(new BookingSelectDate(_customer));
    }
}