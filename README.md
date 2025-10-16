# Snapphane scoutdistrikt booking

En bokningsapp för Snapphane scoutdistrikt för att genomföra bokningar av kanoter, scoutstuga, lägerområde och vindskydd.

Appen kan användas både som gäst eller som inloggad användare genom konton skapade i appen via Clerk. Administatörer har tillgång till särskilda verktyg för att:
* Bekräfta bokningar
* Ändra information som visas för användare
* Ändra eller lägga till kontaktpersoner

Appen stödjer emailbekräftelse, sökbara bokningar via bokningsnummer och emailadress samt redigering av bokningar.

Utvecklad för Android och Windows med .NET MAUI och Supbase som backend.
## Filstruktur
Översikt över appens filstruktur:
```text
├── SnapphaneScoutDistrikBookingApp.Test
│   ├── AdminServiceTest.cs
│   ├── BookingServiceTest.cs
│   └── Validation.Test.cs
└── SnapphaneScoutDistriktBookingApp
    ├── Helpers
    │   ├── TimeZoneHelper.cs
    │   └── TokenStorage.cs
    ├── Models
    │   ├── Admin.cs
    │   ├── ClerkUser.cs
    │   ├── Contact.cs
    │   ├── Customer.cs
    │   ├── EventModel.cs
    │   └── Info.cs
    ├── Platforms
    │   ├── Android
    │   │   ├── Resources
    │   │   │   └── values
    │   │   ├── MainActivity.cs
    │   │   └── MainApplication.cs
    │   ├── iOS
    │   │   ├── Resources
    │   │   ├── AppDelegate.cs
    │   │   └── Program.cs
    │   ├── MacCatalyst
    │   │   ├── AppDelegate.cs
    │   │   └── Program.cs
    │   ├── Tizen
    │   │   └── Main.cs
    │   └── Windows
    │       └── App.xaml
    ├── Properties
    │   └── launchSettings.json
    ├── Resources
    │   ├── AppIcon
    │   ├── Fonts
    │   │   └── FluentUI.cs
    │   ├── Images
    │   ├── Raw
    │   ├── Splash
    │   └── Styles
    │       ├── Colors.xaml
    │       └── Styles.xaml
    ├── Services
    │   ├── Interface
    │   │   ├── IAdminService.cs
    │   │   ├── IBookingService.cs
    │   │   ├── IClerkAuthService.cs
    │   │   ├── IClerkUserSessionService.cs
    │   │   ├── IDbService.cs
    │   │   ├── IEmailService.cs
    │   │   └── IValidateBookingService.cs
    │   ├── AdminService.cs
    │   ├── BookingService.cs
    │   ├── ClerkAuthService.cs
    │   ├── ClerkUserSessionService.cs
    │   ├── DbService.cs
    │   ├── EmailService.cs
    │   └── ValidateBookingService.cs
    ├── ViewModels
    │   ├── AdminPageViewModel.cs
    │   ├── BookingViewModel.cs
    │   ├── EditBookingViewModel.cs
    │   └── InfoPageViewModel.cs
    ├── Views
    │   ├── Booking
    │   │   ├── BookingPage.xaml
    │   │   ├── BookingStep1View.xaml
    │   │   ├── BookingStep2View.xaml
    │   │   ├── BookingStep3View.xaml
    │   │   ├── BookingStep4View.xaml
    │   │   └── BookingStep5View.xaml
    │   ├── AddContactPopUpPage.xaml
    │   ├── AdminPage.xaml
    │   ├── BookingPopUpPage.xaml
    │   ├── EditBookingPage.xaml
    │   ├── InfoPage.xaml
    │   ├── LoginPage.xaml
    │   ├── MainPage.xaml
    │   ├── RegisterPage.xaml
    │   ├── UpdateInfoPopUpPage.xaml
    │   └── ViewBooking.xaml
    ├── App.xaml
    ├── AppShell.xaml
    └── MauiProgram.cs
```
## Teststruktur

Enhetstester: Testar logik i Services och valideringsmetoder
## Branch-struktur

* master - Stabil version av appen
* dev - Aktiv utvecklings-branch
* feat/ - Nya funktioner
## Starta upp projektet

## Viktiga funktioner
