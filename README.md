# Snapphane scoutdistrikt booking

En bokningsapp för Snapphane scoutdistrikt för att genomföra bokningar av kanoter, scoutstuga, lägerområde och vindskydd.

Appen kan användas både som gäst eller som inloggad användare genom konton skapade i appen via Clerk. Administatörer har tillgång till särskilda verktyg för att:
* Bekräfta bokningar.
* Ändra information som visas för användare.
* Ändra eller lägga till kontaktpersoner.

Appen stödjer emailbekräftelse, sökbara bokningar via bokningsnummer och emailadress samt redigering av bokningar.

Utvecklad för Android och Windows med .NET MAUI och Supabase som backend.
## Filstruktur
Översikt över appens filstruktur:
```text
SnapphaneScoutDistriktBookingApp/
├── SnapphaneScoutDistriktBookingApp/       # Huvudprojektet
│   ├── Helpers/                            # Hjälpklasser och metoder som används globalt i projektet.
│   ├── Models/                             # Datamodeller (t.ex. Booking, User, Resource, Info).
│   ├── Platforms/                          # Plattformsspecifika filer (Android, iOS, Windows, MacCatalyst).
│   ├── Properties/                         # Projektinställningar och konfigurationer.
│   ├── Resources/                          # Bilder, typsnitt, teman, färger och stilar för appens UI.
│   ├── Services/                           # Applikationstjänster (t.ex. autentisering, e-post, databas).
│   │   └── Interface/                      # Servicegränssnitt (IService) som används för dependency injection.
│   ├── ViewModels/                         # MVVM-lager. Logik kopplad till vyer.
│   └── Views/                              # XAML-sidor, popup-fönster och vyer som användaren interagerar med.
│   │   └── Booking/                        # Exempel: skapa mapp för sidor som hör till samma flöde eller funktion (t.ex. bokningssteg).
│   ├── App.xaml / AppShell.xaml            # Definierar applikationens struktur, resurser och navigering.
│   └── MauiProgram.cs                      # Registrera nya tjänster och konfigurationer för appstart.
│
└── SnapphaneScoutDistriktBookingApp.Test/  # Enhetstester för tjänster och logik
    └──  ServiceTester.cs                   # Exempel: skapa testfiler för respektive serviceklass
```
### Exempel - lägga till ny tjänst
Om du t.ex. lägger till en ny tjänst för att skicka notiser:

* Skapa NotificationService.cs i Services/.

* Skapa INotificationService.cs i Services/Interface/.

* Registrera Interface i MauiProgram.cs.

* Skapa NotificationServiceTest.cs i SnapphaneScoutDistrikBookingApp.Test/.

## Teststruktur

Enhetstester används för att säkerställa att appens logik fungerar korrekt och för att snabbt upptäcka fel vid förändringar i koden.

Testerna fokuserar främst på:

* ### Service-logik:
    - Verifierar att metoder för datalagring, hämtning, autentisering, e-post och bokningshantering fungerar som förväntat (t.ex. testar att BookingService sparar korrekt).

* ### Valideringsmetoder:
    - Säkerställer att inmatad data (t.ex. namn, e-post eller telefonnummer) följer de regler och format som appen kräver.

* ### Mockning av beroenden:
    - Tjänster som kommunicerar med externa system (t.ex. Supabase) mockas vid testning för att möjliggöra tester utan att påverka verkliga databaser eller API:er.

* ### Kvalitetssäkring:
    - Testerna körs innan nya ändringar pushas till GitHub för att säkerställa att befintlig funktionalitet inte bryts.

Testfiler placeras i mappen SnapphaneScoutDistriktBookingApp.Test/

Varje service eller funktion har en egen testfil, t.ex.:

- AdminServiceTest.cs

- BookingServiceTest.cs

- ValidationTest.cs

## Branch-struktur

master - Stabil version av appen.

dev - Aktiv utvecklings-branch.

feat/ - Nya funktioner.

bug/ - Brancher för buggfixar.

## Starta upp projektet

1. Klona repot.
2. Sätt upp nycklar för Clerk och Supabase.
3. Konfigurera miljövariabler för Supabase och Clerk.
4. Bygg projektet.
5. Starta appen och skapa ett konto via Clerk.
   
## Viktiga funktioner
Bokningssystem – Skapa och hantera bokningar för kanoter, scoutstuga, lägerområde och vindskydd. 5 steg där användaren kan gå bakåt och framåt i bokningsstegen.

Inloggning via Clerk – Användare kan logga in, skapa konton och hantera sina bokningar direkt i appen.

Adminpanel – Administratörer kan bekräfta bokningar och uppdatera information.

EmailService – Automatiskt e-postmeddelande skickas vid bokningar och bekräftade bokningar.

Sökfunktion – Sök bokningar med bokningsnummer och e-postadress.

Redigering av bokningar – Möjlighet att uppdatera befintliga bokningar direkt i appen.

Databas via Supabase – All bokningsdata lagras i Supabase.

Tidszonshantering – Automatisk anpassning av UTC till Stockholm-tid med hjälp av TimeZoneHelper.

ValidationService – Tjänster för verifiering.
