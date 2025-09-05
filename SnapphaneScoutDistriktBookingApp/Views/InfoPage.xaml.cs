using SnapphaneScoutDistriktBookingApp.Services.Interface;
namespace SnapphaneScoutDistriktBookingApp.Views;

public partial class InfoPage : ContentPage
{
	public InfoPage()
	{
		InitializeComponent();
		BindingContext = new ViewModels.InfoPageViewModel();
	}
}