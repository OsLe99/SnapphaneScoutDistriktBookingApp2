using MongoDB.Driver;
using SnapphaneScoutDistriktBookingApp.Services.Interface;
using SnapphaneScoutDistriktBookingApp.ViewModels;
using System.Threading.Tasks;

namespace SnapphaneScoutDistriktBookingApp.Views;

public partial class AdminPage : ContentPage
{
    private readonly IDbService _db;
    private readonly IEmailService _emailService;
    private readonly IClerkUserSessionService _userSessionService;
    private readonly IRoleAuthService _roleAuthService;
    private readonly IClerkAuthService _authService;
    public AdminPage(IDbService db, IEmailService emailService, IClerkUserSessionService userSessionService, IRoleAuthService roleAuthService, IClerkAuthService authService)
    {
        InitializeComponent();
        _userSessionService = userSessionService;
        _db = db;
        _emailService = emailService;
        _roleAuthService = roleAuthService;
        _authService = authService;
        BindingContext = new AdminPageViewModel(_db, _authService, _roleAuthService, _userSessionService);
	}

    private async void OnBookingSelectedAsync(object sender, SelectedItemChangedEventArgs e)
    {
		var booking = e.SelectedItem as Models.Booking;
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
        // Behövs fixas innan ny push
        //if (sender is CheckBox checkBox && checkBox.BindingContext is Models.Booking booking && booking.EmailConfirmation == false)
        //{
        //    if (e.Value)
        //    {
        //        await _emailService.SendEmailConfirmationAsync("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", "emil.berg@campusnykoping.se", booking.Email, booking);
        //        await _db.UpdateCheckBoxDatabaseAsync(booking);
        //    }
        //}
    }
}