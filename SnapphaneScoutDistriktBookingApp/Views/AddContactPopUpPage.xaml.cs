using SnapphaneScoutDistriktBookingApp.Services;
using SnapphaneScoutDistriktBookingApp.Services.Interface;
using System.Threading.Tasks;

namespace SnapphaneScoutDistriktBookingApp.Views;

public partial class AddContactPopUpPage : ContentPage
{
	private readonly IDbService _db = new DbService();
	public AddContactPopUpPage(IDbService db)
	{
		InitializeComponent();
		_db = db;
	}

    private async void OnClickedPopPopUpAsync(object sender, EventArgs e)
    {
		var name = xName.Text;
		var email = xEmail.Text;
		var phone = xPhone.Text;
		var contact = new Models.Contact()
		{
			Name = name,
			Email = email,
			PhoneNumber = phone
		};
		await _db.AddContactAsync(contact);
		await Navigation.PopAsync();
    }
}