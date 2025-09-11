namespace SnapphaneScoutDistriktBookingApp
{
    public partial class App : Application
    {
        public App()
        {
            UserAppTheme = AppTheme.Light;
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}