using Syncfusion.Maui.Picker;
using System.Resources;
using System.Globalization;
using SnapphaneScoutDistriktBookingApp.Services.Interface;

namespace SnapphaneScoutDistriktBookingApp
{
    public partial class App : Application
    {
        private readonly IClerkAuthService _authService;
        private readonly IClerkUserSessionService _userSession;
        private readonly IRoleAuthService _roleAuthService;
        public App(IClerkUserSessionService userSession, IClerkAuthService authService, IRoleAuthService roleAuthService)
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1JFaF5cX2NCf1FpRmJGdld5fUVHYVZUTXxaS00DNHVRdkdmWXZfd3RWRGlZVUNxX0BWYEg=");
            CultureInfo.CurrentCulture = new CultureInfo("sv-SE");
            UserAppTheme = AppTheme.Light;
            InitializeComponent();
            _authService = authService;
            _userSession = userSession;
            _roleAuthService = roleAuthService;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell(_userSession, _authService, _roleAuthService));
        }
    }
}