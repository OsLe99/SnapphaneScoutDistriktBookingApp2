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
            var url = Environment.GetEnvironmentVariable("SUPABASE_URL");
            var key = Environment.GetEnvironmentVariable("SUPABASE_KEY");
            var bearerToken = Environment.GetEnvironmentVariable("CLERK_API_KEY");
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
                new ClerkAuthService(bearerToken));

            builder.Services.AddSingleton<IClerkUserSessionService>(sp =>
            {
                var authService = sp.GetRequiredService<IClerkAuthService>();
                return new ClerkUserSessionService(bearerToken, authService);
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
