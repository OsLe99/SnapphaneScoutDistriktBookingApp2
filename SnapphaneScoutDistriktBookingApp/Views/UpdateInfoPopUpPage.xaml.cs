using SnapphaneScoutDistriktBookingApp.Services.Interface;
namespace SnapphaneScoutDistriktBookingApp.Views;

public partial class UpdateInfoPopUpPage : ContentPage
{
	private readonly IDbService _db;
	public UpdateInfoPopUpPage(IDbService dbService)
	{
        InitializeComponent();
        _db = dbService;
        BindingContext = new ViewModels.InfoPageViewModel(dbService);
	}
}