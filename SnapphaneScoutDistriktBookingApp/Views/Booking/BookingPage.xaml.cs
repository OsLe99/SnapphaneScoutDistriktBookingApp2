using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using Microsoft.Maui.Controls;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Syncfusion.Maui.Calendar;
using SnapphaneScoutDistriktBookingApp.Data;
using MongoDB.Driver;
using SnapphaneScoutDistriktBookingApp.ViewModels;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
using SnapphaneScoutDistriktBookingApp.Views;

namespace SnapphaneScoutDistriktBookingApp;

public partial class BookingPage : ContentPage
{
    public BookingPage()
	{
		InitializeComponent();
		BindingContext = new BookingViewModel();
    }

	private async void OnChangeToMoreInfo(object sender, EventArgs e)
	{
		await Navigation.PushAsync(new BookingExtraInfo());
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

		//Models.Customer.TypeOfBooking bookingtype = Models.Customer.TypeOfBooking.None;
		//if(checkCanoe.IsChecked == true)
		//{
		//	bookingtype |= Models.Customer.TypeOfBooking.Kanot;
		//}
		//if(checkCabin.IsChecked == true)
		//{
		//	bookingtype |= Models.Customer.TypeOfBooking.Stuga;
		//}
		//if(checkLeanTo.IsChecked == true)
		//{
		//	bookingtype |= Models.Customer.TypeOfBooking.Vindskydd;
		//}
		//if(checkCampGrounds.IsChecked == true)
		//{
		//	bookingtype |= Models.Customer.TypeOfBooking.Lägerplats;
		//}

		
		//var custumer = new Models.Customer()
		//{
		//	Name = myName.Text,
		//	Phone = myPhone.Text,
		//	Email = myEmail.Text,
		//	IsOrg = myCheckBox.IsChecked,
		//	OrgName = (myCheckBox.IsChecked == true ? orgNameInput.Text : ""),
		//	StartDate = MyStartDate.Date.AddHours(1),
		//	EndDate = MyEndDate.Date.AddHours(1),
		//	BookingType = bookingtype,
		//	NumberOfCanoes = int.TryParse(AntalKanoter.Text, out int result) ? result : null,
		//	NumberOfCabin = int.TryParse(AntalStuga.Text, out int result1) ? result1 : null,
		//	NumberOfCampground = int.TryParse(Lägerområde.Text, out int result2) ? result2 : null,
		//	NumberOfLeanTo = int.TryParse(Vindskydd.Text, out int result3) ? result3 : null,
  //          IsConfirmed = false
		//};
		//await Data.DB.BookingCollection().InsertOneAsync(custumer);
		//API.SendEmail("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", "emil.berg@campusnykoping.se", custumer.Email, custumer);
  //      var popup = new ContentPage
  //      {
  //          Content = new VerticalStackLayout
  //          {
  //              Padding = 20,
  //              Children =
  //                  {
  //                      new Label { Text = "Tack för din bokning!"},

  //                      new Button
  //                      {
  //                          Text = "Tillbaka",
  //                          Command = new Command(async () => await Navigation.PopModalAsync())

  //                      }
  //                  }
  //          }
  //      };
  //      await Navigation.PushModalAsync(popup);
  //  }
	

  //  private void OnCheckCanoe(object sender, CheckedChangedEventArgs e)
  //  {
		//if (e.Value)
		//{
		//	AntalKanoter.IsVisible = true;
		//}
		//else
		//{
		//	AntalKanoter.IsVisible = false;
  //      }
  //  }

  //  private void OnCheckCabin(object sender, CheckedChangedEventArgs e)
  //  {
  //      if (e.Value)
  //      {
  //          AntalStuga.IsVisible = true;
  //      }
  //      else
  //      {
  //          AntalStuga.IsVisible = false;
  //      }
  //  }

  //  private void OnCampGrounds(object sender, CheckedChangedEventArgs e)
  //  {
  //      if (e.Value)
  //      {
  //          Lägerområde.IsVisible = true;
  //      }
  //      else
  //      {
  //          Lägerområde.IsVisible = false;
  //      }
  //  }

  //  private void OnLeanTo(object sender, CheckedChangedEventArgs e)
  //  {
  //      if (e.Value)
  //      {
  //          Vindskydd.IsVisible = true;
  //      }
  //      else
  //      {
  //          Vindskydd.IsVisible = false;
  //      }
  //  }
}