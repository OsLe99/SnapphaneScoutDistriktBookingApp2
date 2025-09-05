using SnapphaneScoutDistriktBookingApp.Data;
using System.Threading.Tasks;

namespace SnapphaneScoutDistriktBookingApp
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            OnAppearing();
            
            BindingContext = UserSession.Instance;
        }
        bool pageStarted = false;
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (!pageStarted)
            {
                pageStarted = true;
                await CheckUserSession();
            }

            if (Data.UserSession.Instance.IsAdmin == false)
            {
                AdminSidan.IsVisible = false;
            }
            else
            {
                AdminSidan.IsVisible = true;
            }

        }

        private async void OnChangeToBookingSelect(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new BookingPage());
        }


        private async Task CheckUserSession()
        {
            if (!Data.UserSession.Instance.IsUserSet())
            {
                await Navigation.PushAsync(new Views.LoginPage());
            }
        }

        private async void OnResetUser(object sender, EventArgs e)
        {
            bool confirm = await DisplayAlert("Ändra användare", "Är du säker på att du vill ändra användare?", "Ja", "Nej");
            if (!confirm)
            {
                return;
            }

            Data.UserSession.Instance.ResetUser();

            await DisplayAlert("Användarinformation återställd", "Nuvarande sparad användare är borttagen.", "OK");
            await CheckUserSession();
        }

        private async void OnClickedGoToAdminPage(object sender, EventArgs e)
        {
            
            await Navigation.PushAsync(new Views.AdminPage());
        }

        private async void OnClickedGoToInfoPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Views.InfoPage());
        }
    }

}
