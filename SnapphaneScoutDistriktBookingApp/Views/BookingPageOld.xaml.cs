//using CommunityToolkit.Mvvm.ComponentModel;
//using CommunityToolkit.Mvvm.Input;
//using System.Windows.Input;
//using Microsoft.Maui.Controls;
//using System.Diagnostics;
//using System.Collections.ObjectModel;
//using System.Threading.Tasks;
//using Syncfusion.Maui.Calendar;
//using MongoDB.Driver;
//using SnapphaneScoutDistriktBookingApp.ViewModels;
//using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;
//using SnapphaneScoutDistriktBookingApp.Services;
//using SnapphaneScoutDistriktBookingApp.Services.Interface;

//namespace SnapphaneScoutDistriktBookingApp;

//public partial class BookingPage : ContentPage
//{
      // BookingPage används ej längre då bokningar sker i flera steg

//    private readonly IUserSessionService _userSession;
//    private readonly IDbService _db;
//    private readonly IEmailService _emailService;
//    public BookingPage(IUserSessionService userSession, IDbService db, IEmailService emailService)
//    {
//		InitializeComponent();
//        _userSession = userSession;
//        _db = db;
//        _emailService = emailService;
//        BindingContext = new BookingViewModel(db);
//        myName.Text = _userSession.UserName;
//        myEmail.Text = _userSession.UserEmail;
//    }
//    private void OnCheckChange(object sender, CheckedChangedEventArgs e)
//    {
//		if (e.Value)
//		{
//			statusLabel.Text = "Scoutmedlem";
//			hiddenLabel.IsVisible = true;
//			orgNameInput.IsVisible = true;
//		}
//		else
//		{
//			statusLabel.Text = "Icke scoutmedlem";
//			hiddenLabel.IsVisible = false;
//			orgNameInput.IsVisible = false;
//		}
//    }

//    private async void OnConformation(object sender, EventArgs e)
//    {

//		Models.Customer.TypeOfBooking bookingtype = Models.Customer.TypeOfBooking.None;
//		if(checkCanoe.IsChecked == true)
//		{
//			bookingtype |= Models.Customer.TypeOfBooking.Kanot;
//		}
//		if(checkCabin.IsChecked == true)
//		{
//			bookingtype |= Models.Customer.TypeOfBooking.Stuga;
//		}
//		if(checkLeanTo.IsChecked == true)
//		{
//			bookingtype |= Models.Customer.TypeOfBooking.Vindskydd;
//		}
//		if(checkCampGrounds.IsChecked == true)
//		{
//			bookingtype |= Models.Customer.TypeOfBooking.Lägerplats;
//		}


//        var popup = new ContentPage
//        {
//            Content = new VerticalStackLayout
//            {
//                Padding = 20,
//                Children =
//                    {
//                        new Label { Text = "Tack för din bokning!"},

//                        new Button
//                        {
//                            Text = "Tillbaka",
//                            Command = new Command(async () => await Navigation.PopModalAsync())

//                        }
//                    }
//            }
//        };
//        await Navigation.PushModalAsync(popup);



//    Koden nedanför används ej längre, bokningens typ sker i ett steg längre fram istället
//    private void OnCheckCanoe(object sender, CheckedChangedEventArgs e)
//    {
//		if (e.Value)
//		{
//			AntalKanoter.IsVisible = true;
//		}
//		else
//		{
//			AntalKanoter.IsVisible = false;
//        }
//    }

//    private void OnCheckCabin(object sender, CheckedChangedEventArgs e)
//    {
//        if (e.Value)
//        {
//            AntalStuga.IsVisible = true;
//        }
//        else
//        {
//            AntalStuga.IsVisible = false;
//        }
//    }

//    private void OnCampGrounds(object sender, CheckedChangedEventArgs e)
//    {
//        if (e.Value)
//        {
//            Lägerområde.IsVisible = true;
//        }
//        else
//        {
//            Lägerområde.IsVisible = false;
//        }
//    }

//    private void OnLeanTo(object sender, CheckedChangedEventArgs e)
//    {
//        if (e.Value)
//        {
//            Vindskydd.IsVisible = true;
//        }
//        else
//        {
//            Vindskydd.IsVisible = false;
//        }
//    }


//}