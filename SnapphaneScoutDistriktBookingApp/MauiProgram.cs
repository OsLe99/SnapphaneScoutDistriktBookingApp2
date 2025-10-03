using Microsoft.Extensions.Logging;
using SnapphaneScoutDistriktBookingApp.Services;
using SnapphaneScoutDistriktBookingApp.Services.Interface;
using Syncfusion.Maui.Core.Hosting;


namespace SnapphaneScoutDistriktBookingApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();

            builder
                .UseMauiApp<App>()
                .ConfigureSyncfusionCore()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Deutsch.ttf", "OldGerman");
                });
            // Services
            builder.Services.AddTransient<IAdminService, AdminService>();
            builder.Services.AddSingleton<IClerkUserSessionService>(sp =>
                new ClerkUserSessionService("sk_test_kRFPzExdplSes3eeLt9uD9fubBNXsilcMBWxAW3PK1"));
            builder.Services.AddSingleton<IClerkAuthService, ClerkAuthService>();
            builder.Services.AddScoped<IDbService, DbService>();
            builder.Services.AddTransient<IEmailService, EmailService>();
            builder.Services.AddTransient<IBookingService, BookingService>();
            builder.Services.AddSingleton<IValidateBookingService, ValidateBookingService>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
