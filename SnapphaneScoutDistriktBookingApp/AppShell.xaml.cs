namespace SnapphaneScoutDistriktBookingApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(Views.Booking.BookingPage), typeof(Views.Booking.BookingPage));
        }
        private void OnClickedChangeTheme(object sender, EventArgs e)
        {
            var app = Application.Current;
            if (app.UserAppTheme == AppTheme.Light)
            {
                app.UserAppTheme = AppTheme.Dark;
            }
            else
            {
                app.UserAppTheme = AppTheme.Light;
            }
        }
    }
}
