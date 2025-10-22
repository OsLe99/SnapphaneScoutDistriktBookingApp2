using SnapphaneScoutDistriktBookingApp.Services.Interface;
namespace SnapphaneScoutDistriktBookingApp.Views;

public partial class InfoPage : ContentPage
{
	private readonly IDbService _db;
	public InfoPage(IDbService dbService)
	{
		InitializeComponent();
		_db = dbService;
        BindingContext = new ViewModels.InfoPageViewModel(dbService);
	}
}