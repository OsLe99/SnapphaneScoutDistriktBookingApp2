using System.Diagnostics;
using System.Text;
using System.Text.Json;
using SnapphaneScoutDistriktBookingApp.Services.Interface;
using SnapphaneScoutDistriktBookingApp.Helpers;
using Clerk.BackendAPI;
using Clerk.BackendAPI.Helpers;
using Clerk.BackendAPI.Models.Components;
using Clerk.BackendAPI.Models.Operations;

public class ClerkAuthService : IClerkAuthService
{
    private readonly HttpClient _httpClient;
    private const string ClerkFrontendApi = "https://communal-albacore-55.clerk.accounts.dev";

    public ClerkAuthService()
    {
        _httpClient = new HttpClient();
    }

    public async Task<string?> SignInAsync(string email, string password)
    {
        try
        {
            /* Starta inloggningsprocessen. 
             * 4 steg då Clerks frontedapi endast tar emot vissa inputs under varje steg. 
             * Omega nivå av debug writes
             * Om jag fattar det rätt så går det inte att logga in på detta sättet då den är gjord för web, går att använda backenApi men då finns det ingen färdig lösning för hantering av lösenord. Ifall jag har fel så är det toppen :)
             * Skapa sign-in attempt, ange identifierare (email), ange första faktor (lösenord), hämta JWT från session och spara den för framtida inloggningar*/
            var startResponse = await _httpClient.PostAsync(
                $"{ClerkFrontendApi}/v1/client/sign_ins",
                new StringContent("{}", Encoding.UTF8, "application/json")
            );

            var startBody = await startResponse.Content.ReadAsStringAsync();
            Debug.WriteLine("=== Start SignIn Response ===");
            Debug.WriteLine(startBody);

            if (!startResponse.IsSuccessStatusCode)
            {
                Debug.WriteLine($"Start sign-in failed: {startResponse.StatusCode}");
                return null;
            }

            var startJson = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(startBody);
            if (startJson == null || !startJson.ContainsKey("response"))
            {
                Debug.WriteLine("No sign-in attempt created.");
                return null;
            }

            var signInAttempt = startJson["response"];
            var signInId = signInAttempt.GetProperty("id").GetString();
            if (string.IsNullOrEmpty(signInId))
            {
                Debug.WriteLine("Sign-in ID is null.");
                return null;
            }

            // Använd email som identifierare
            var identifierRequest = new
            {
                sign_in_attempt_id = signInId,
                identifier = email
            };

            var identifierResponse = await _httpClient.PostAsync(
                $"{ClerkFrontendApi}/v1/client/sign_ins/{signInId}/identifiers",
                new StringContent(
                    JsonSerializer.Serialize(identifierRequest),
                    Encoding.UTF8,
                    "application/json"
                )
            );

            var identifierBody = await identifierResponse.Content.ReadAsStringAsync();
            Debug.WriteLine("=== Identifier Response ===");
            Debug.WriteLine(identifierBody);

            if (!identifierResponse.IsSuccessStatusCode)
            {
                Debug.WriteLine($"Provide identifier failed: {identifierResponse.StatusCode}");
                return null;
            }

            // Använd lösenordet som första faktor
            var firstFactorRequest = new
            {
                sign_in_attempt_id = signInId,
                strategy = "password",
                password = password
            };

            var firstFactorResponse = await _httpClient.PostAsync(
                $"{ClerkFrontendApi}/v1/client/sign_ins/{signInId}/first_factors",
                new StringContent(
                    JsonSerializer.Serialize(firstFactorRequest),
                    Encoding.UTF8,
                    "application/json"
                )
            );

            var firstFactorBody = await firstFactorResponse.Content.ReadAsStringAsync();
            Debug.WriteLine("=== First Factor Response ===");
            Debug.WriteLine(firstFactorBody);

            if (!firstFactorResponse.IsSuccessStatusCode)
            {
                Debug.WriteLine($"First factor failed: {firstFactorResponse.StatusCode}");
                return null;
            }

            var firstFactorJson = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(firstFactorBody);
            if (firstFactorJson == null || !firstFactorJson.ContainsKey("created_session_id"))
            {
                Debug.WriteLine("No session created.");
                return null;
            }

            var sessionId = firstFactorJson["created_session_id"].GetString();
            if (string.IsNullOrEmpty(sessionId))
            {
                Debug.WriteLine("Session ID is null.");
                return null;
            }

            //Byt ut sessionId mot en JWT
            var tokenResponse = await _httpClient.PostAsync(
                $"{ClerkFrontendApi}/v1/client/sessions/{sessionId}/tokens",
                null
            );

            var tokenBody = await tokenResponse.Content.ReadAsStringAsync();
            Debug.WriteLine("=== Token Response ===");
            Debug.WriteLine(tokenBody);

            if (!tokenResponse.IsSuccessStatusCode)
            {
                Debug.WriteLine($"Token fetch failed: {tokenResponse.StatusCode}");
                return null;
            }

            var tokenJson = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(tokenBody);
            if (tokenJson == null || !tokenJson.ContainsKey("jwt"))
            {
                Debug.WriteLine("No JWT in response.");
                return null;
            }

            var jwt = tokenJson["jwt"].GetString();
            if (!string.IsNullOrEmpty(jwt))
            {
                await SecureStorage.Default.SetAsync("jwt", jwt);
                Debug.WriteLine("Sign-in successful. JWT saved.");
                return jwt;
            }

            return null;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception in SignInAsync: {ex}");
            return null;
        }
    }

    // Enkel sign-out som tar bort den sparade token, behöver lägga till mer senare för att logga ut helt
    public async Task<Task> SignOutAsync()
    {
        TokenStorage.RemoveToken();
        return Task.CompletedTask;
    }
}
