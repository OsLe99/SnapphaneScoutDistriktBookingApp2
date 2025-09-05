using MongoDB.Driver;
using SnapphaneScoutDistriktBookingApp.Services.Interface;
using SnapphaneScoutDistriktBookingApp.ViewModels;
using System.Threading.Tasks;

namespace SnapphaneScoutDistriktBookingApp.Views;

public partial class AdminPage : ContentPage
{
    private readonly IDbService _db;
    private readonly IEmailService _emailService;
    public AdminPage(IDbService db, IEmailService emailService)
    {
        InitializeComponent();
        _db = db;
        _emailService = emailService;
        BindingContext = new ViewModels.AdminPageViewModel();
	}

    private async void OnBookingSelected(object sender, SelectedItemChangedEventArgs e)
    {
		var booking = ((ListView)sender).SelectedItem as Models.Customer;
		if(booking != null)
		{
			var page = new BookingPopUpPage();
			page.BindingContext = booking;
			await Navigation.PushAsync(page);
		}
    }

    

    private async void OnClickedAddContact(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.AddContactPopUpPage(_db));
    }

    private async void OnClickedChangeInfo(object sender, EventArgs e)
    {
		await Navigation.PushAsync(new Views.UpdateInfoPopUpPage());
    }

    private async void OnCheckBoxConformationSendEmail(object sender, CheckedChangedEventArgs e)
    {
        if(sender is CheckBox checkBox && checkBox.BindingContext is Models.Customer costumer && costumer.EmailConformation == false)
        {
            if (e.Value)
            {
                await _emailService.SendEmailConfirmation("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", "emil.berg@campusnykoping.se", costumer.Email, costumer);
                var filter = Builders<Models.Customer>.Filter.Eq(x => x.Id, costumer.Id);
                var update = Builders<Models.Customer>.Update.Set(x => x.EmailConformation, true);

                await _db.BookingCollection().UpdateOneAsync(filter, update);
                Task.Delay(2000);

            }
        }
        
    }
}