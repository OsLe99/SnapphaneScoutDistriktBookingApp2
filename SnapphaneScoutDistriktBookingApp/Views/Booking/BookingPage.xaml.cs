using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SnapphaneScoutDistriktBookingApp.Services.Interface;
using MongoDB.Driver;
using SnapphaneScoutDistriktBookingApp.ViewModels;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using SnapphaneScoutDistriktBookingApp.Views;
using SnapphaneScoutDistriktBookingApp.Services;

namespace SnapphaneScoutDistriktBookingApp;

public partial class BookingPage : ContentPage
{
	private readonly IUserSessionService _userSessionService;
	private readonly IBookingService _bookingService;
    public BookingPage(IUserSessionService userSessionService, IBookingService bookingService)
	{
		InitializeComponent();
		_userSessionService = userSessionService;
		_bookingService = bookingService;
        BindingContext = new Models.Customer();

    }
    private async void OnChangeToMoreInfoAsync(object sender, EventArgs e)
	{
        var newCustomer = (BindingContext as Models.Customer);
        if (sender is Button button && button.Text is string type)
        {
            switch (type)
			{
				case "Kanot":
					newCustomer.BookingType |= Models.Customer.TypeOfBooking.Canoe;
					break;
				case "Stugan":
					newCustomer.BookingType |= Models.Customer.TypeOfBooking.Cabin;
					break;
				case "Vindskydd":
					newCustomer.BookingType |= Models.Customer.TypeOfBooking.LeanTo;
					break;
				case "Lägerområde":
					newCustomer.BookingType |= Models.Customer.TypeOfBooking.CampGrounds;
					break;
            }
        }
        await Navigation.PushAsync(new BookingExtraInfo(newCustomer, _bookingService));
	}
  //  private void OnCheckChange(object sender, CheckedChangedEventArgs e)
  //  {
		//if (e.Value)
		//{
		//	statusLabel.Text = "Scoutmedlem";
		//	hiddenLabel.IsVisible = true;
		//	orgNameInput.IsVisible = true;
		//}
		//else
		//{
		//	statusLabel.Text = "Icke scoutmedlem";
		//	hiddenLabel.IsVisible = false;
		//	orgNameInput.IsVisible = false;
		//}
  //  }
	
  //  private async void OnConformation(object sender, EventArgs e)
  //  {

	//private void OnCheckCanoe(object sender, CheckedChangedEventArgs e)
	//{
	//	if (e.Value)
	//	{
	//		AntalKanoter.IsVisible = true;
	//	}
	//	else
	//	{
	//		AntalKanoter.IsVisible = false;
	//	}
	//}

	//private void OnCheckCabin(object sender, CheckedChangedEventArgs e)
	//{
	//	if (e.Value)
	//	{
	//		AntalStuga.IsVisible = true;
	//	}
	//	else
	//	{
	//		AntalStuga.IsVisible = false;
	//	}
	//}

	//private void OnCampGrounds(object sender, CheckedChangedEventArgs e)
	//{
	//	if (e.Value)
	//	{
	//		Lägerområde.IsVisible = true;
	//	}
	//	else
	//	{
	//		Lägerområde.IsVisible = false;
	//	}
	//}

	//private void OnLeanTo(object sender, CheckedChangedEventArgs e)
	//{
	//	if (e.Value)
	//	{
	//		Vindskydd.IsVisible = true;
	//	}
	//	else
	//	{
	//		Vindskydd.IsVisible = false;
	//	}
	//}
}