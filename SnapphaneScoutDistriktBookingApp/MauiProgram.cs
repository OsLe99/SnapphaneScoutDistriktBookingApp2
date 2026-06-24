using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SnapphaneScoutDistriktBookingApp.Helpers;
using SnapphaneScoutDistriktBookingApp.Services;
using SnapphaneScoutDistriktBookingApp.Services.Interface;
using Syncfusion.Maui.Core.Hosting;
using System.Reflection;


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

            using var stream = Assembly.GetExecutingAssembly()
                .GetManifestResourceStream("SnapphaneScoutDistriktBookingApp.appsettings.json");
            var config = new ConfigurationBuilder().AddJsonStream(stream).Build();
            builder.Configuration.AddConfiguration(config);

            builder.Services.Configure<AppSettings>(builder.Configuration);
            var settings = GetAppConfig(builder);

            // Supabase
            var url = settings.SUPABASE_URL;
            var key = settings.SUPABASE_KEY;
            var bearerToken = settings.CLERK_API_KEY;
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
            builder.Services.AddTransient<IRoleAuthService>(sp =>
                new RoleAuthService(Environment.GetEnvironmentVariable("CLERK_API_KEY")));

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }

        static AppSettings GetAppConfig(MauiAppBuilder builder)
        {
            AppSettings? settings = builder.Configuration.Get<AppSettings>();
            if (settings == null)
            {
                throw new NullReferenceException($"{nameof(settings)} cannot be null");
            }

            return settings;
        }
    }
}
