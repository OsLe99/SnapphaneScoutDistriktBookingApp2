using Microsoft.Extensions.Logging;
using SnapphaneScoutDistriktBookingApp.Services;
using SnapphaneScoutDistriktBookingApp.Services.Interface;
using Syncfusion.Maui.Core.Hosting;
using Supabase;


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
            // Supabase
            var url = "https://cwudkbwltvsjesoksxno.supabase.co";
            var key = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc3MiOiJzdXBhYmFzZSIsInJlZiI6ImN3dWRrYndsdHZzamVzb2tzeG5vIiwicm9sZSI6ImFub24iLCJpYXQiOjE3NjAwMTA1MjIsImV4cCI6MjA3NTU4NjUyMn0.1uUS471huzB5OtJaJsQCxkGItcLMMekzwMz0m3rH1LY";
            builder.Services.AddScoped<Supabase.Client>(provider =>
            {
                var options = new Supabase.SupabaseOptions
                {
                    AutoRefreshToken = true,
                    AutoConnectRealtime = true,
                };
                return new Supabase.Client(url, key, options);
            });
            // Services
            builder.Services.AddTransient<IAdminService, AdminService>();

            builder.Services.AddSingleton<IClerkAuthService>(sp =>
                new ClerkAuthService("sk_test_kRFPzExdplSes3eeLt9uD9fubBNXsilcMBWxAW3PK1"));

            builder.Services.AddSingleton<IClerkUserSessionService>(sp =>
            {
                var authService = sp.GetRequiredService<IClerkAuthService>();
                return new ClerkUserSessionService("sk_test_kRFPzExdplSes3eeLt9uD9fubBNXsilcMBWxAW3PK1", authService);
            });

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
