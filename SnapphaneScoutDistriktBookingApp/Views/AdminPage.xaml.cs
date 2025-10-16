using MongoDB.Driver;
using SnapphaneScoutDistriktBookingApp.Services.Interface;
using SnapphaneScoutDistriktBookingApp.ViewModels;
using System.Threading.Tasks;

namespace SnapphaneScoutDistriktBookingApp.Views;

public partial class AdminPage : ContentPage
{
    private readonly IDbService _db;
    private readonly IEmailService _emailService;
    private readonly IUserSessionService _userSessionService;
    public AdminPage(IDbService db, IEmailService emailService, IUserSessionService userSessionService)
    {
        InitializeComponent();
        _userSessionService = userSessionService;
        _db = db;
        _emailService = emailService;
        BindingContext = new AdminPageViewModel(db);
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (_userSessionService.IsAdmin == false)
        {
            Shell.Current.GoToAsync("//MainPage");
        }
    }

    private async void OnBookingSelectedAsync(object sender, SelectionChangedEventArgs e)
    {
		var booking = e.CurrentSelection.FirstOrDefault() as Models.Booking;
        if (booking != null)
		{
			var page = new BookingPopUpPage();
			page.BindingContext = booking;
			await Navigation.PushAsync(page);
		}
    }

    private async void OnClickedAddContactAsync(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.AddContactPopUpPage(_db));
    }

    private async void OnClickedChangeInfoAsync(object sender, EventArgs e)
    {
		await Navigation.PushAsync(new Views.UpdateInfoPopUpPage(_db));
    }

    private async void OnCheckBoxConformationSendEmailAsync(object sender, CheckedChangedEventArgs e)
    {
        //if (sender is CheckBox checkBox && checkBox.BindingContext is Models.Booking booking && booking.EmailConformation == false)
        //{
        //    if (e.Value)
        //    {
        //        await _emailService.SendEmailConfirmationAsync("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", "emil.berg@campusnykoping.se", customer.Email, customer);
        //        await _db.UpdateCheckBoxDatabaseAsync(customer);
        //    }
        //}
    }
}