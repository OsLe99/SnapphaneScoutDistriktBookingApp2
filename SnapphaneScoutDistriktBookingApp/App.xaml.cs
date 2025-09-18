using Syncfusion.Maui.Picker;
using System.Resources;
using System.Globalization;

namespace SnapphaneScoutDistriktBookingApp
{
    public partial class App : Application
    {
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1JFaF5cX2NCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWXZfd3RWRGlZVUNxX0BWYEg=");
            CultureInfo.CurrentCulture = new CultureInfo("sv-SE");
            UserAppTheme = AppTheme.Light;
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}