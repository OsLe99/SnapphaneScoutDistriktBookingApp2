namespace SnapphaneScoutDistriktBookingApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
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
