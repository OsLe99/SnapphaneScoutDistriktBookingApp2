using System.Diagnostics;
using System.Text;
using System.Text.Json;
using SnapphaneScoutDistriktBookingApp.Services.Interface;
using SnapphaneScoutDistriktBookingApp.Helpers;
using Clerk.BackendAPI;
using Clerk.BackendAPI.Helpers;
using Clerk.BackendAPI.Models.Components;
using Clerk.BackendAPI.Models.Operations;
using MongoDB.Bson;
using SnapphaneScoutDistriktBookingApp.Services;

public class ClerkAuthService : IClerkAuthService
{
    private readonly ClerkBackendApi _sdk;
    public ClerkAuthService(string bearerToken)
    {
        _sdk = new ClerkBackendApi(bearerAuth: bearerToken);
    }

    public async Task<string?> SignInAsync(string email, string password)
    {
        try
        {
            var userListResponse = await _sdk.Users.ListAsync();
            var user = userListResponse?.UserList?.FirstOrDefault(u => u.EmailAddresses?.Any(e => e.EmailAddressValue == email) == true);

            if (user == null)
            {
                Debug.Write("User not found");
                return null;
            }

            var verifyResponse = await _sdk.Users.VerifyPasswordAsync(user.Id, new VerifyPasswordRequestBody
            {
                Password = password
            });

            if (verifyResponse.Object?.Verified != true)
            {
                Debug.Write("Password verification failed");
                return null;
            }

            var sessionResponse = await _sdk.Sessions.CreateAsync(new CreateSessionRequestBody
            {
                UserId = user.Id
            });

            var session = sessionResponse?.Session;
            if (session == null)
            {
                Debug.Write("Session creation failed");
                return null;
            }

            var jwtResponse = await _sdk.Sessions.CreateTokenAsync(
                sessionId: session.Id,
                requestBody: new CreateSessionTokenRequestBody()
                );

            var jwt = jwtResponse?.Object?.Jwt;
            if(string.IsNullOrEmpty(jwt))
            {
                Debug.Write("JWT creation failed");
                return null;
            }

            Debug.WriteLine($"JWT: {jwt}");
            await TokenStorage.SaveTokenAsync(jwt);
            await SecureStorage.SetAsync("sessionId", session.Id);
            Preferences.Set("userName", $"{user.FirstName} {user.LastName}");
            Preferences.Set("userEmail", user.PrimaryEmailAddressId);
            return jwt;
        }
        catch (Exception ex)
        {
            Debug.Write($"Error during sign-in: {ex.Message}");
            return null;
        }
    }

    public async Task<bool> SignOutAsync()
    {
        var sessionId = await SecureStorage.GetAsync("sessionId");
        if(!string.IsNullOrEmpty(sessionId))
        {
            try
            {
                await _sdk.Sessions.RevokeAsync(sessionId);
                Debug.WriteLine("Revoked sessionId");
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during sign-out: {ex.Message}");
                return false;
            }
        }
        return false;
    }
    // 2
    public async Task<string> CheckSessionStateAsync()
    {
        try
        {
            var sessionId = await SecureStorage.GetAsync("sessionId");
            if (string.IsNullOrEmpty(sessionId))
            {
                Debug.WriteLine($"Check session state: {sessionId}");
                return "missing";
            }

            var sessionResponse = await _sdk.Sessions.GetAsync(sessionId);
            var session = sessionResponse?.Session;

            if (session == null)
            {
                Debug.WriteLine("Session not found");
                return "missing";
            }
            if (session.Status == Clerk.BackendAPI.Models.Components.Status.Active)
            {
                return "active";
            }
            else
            {
                return session.Status.ToString().ToLower();
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error: {ex.Message}");
            return "missing";
        }

    }
    // 4
    public async Task<bool> RefreshTokenAsync()
    {
        try
        {
            var sessionId = await SecureStorage.Default.GetAsync("sessionId");
            if (string.IsNullOrEmpty(sessionId))
            {
                Debug.WriteLine("sessionId missing");
                return false;
            }

            var newTokenResponse = await _sdk.Sessions.CreateTokenAsync(
                sessionId: sessionId,
                requestBody: new CreateSessionTokenRequestBody());
            var newJwt = newTokenResponse?.Object?.Jwt;

            if (string.IsNullOrEmpty(newJwt))
            {
                Debug.WriteLine("Failed to refresh token");
                return false;
            }

            await TokenStorage.SaveTokenAsync(newJwt);
            Debug.WriteLine("Token refreshed successfully");
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error: {ex.Message}");
            return false;
        }

    }
    // 3
    public async Task<bool> RefreshSession()
    {
        try
        {
            var sessionId = await SecureStorage.GetAsync("sessionId");
            if (string.IsNullOrEmpty(sessionId))
            {
                return false;
            }
            var sessionRefreshResponse = await _sdk.Sessions.RefreshAsync(
                sessionId: sessionId,
                requestBody: new RefreshSessionRequestBody());
            if (!string.IsNullOrEmpty(sessionRefreshResponse?.SessionRefresh?.Token?.Object.Value()))
            {
                var newToken = sessionRefreshResponse?.SessionRefresh?.Token?.Object.Value();
                await TokenStorage.SaveTokenAsync(newToken);
                return true;
            }
            return false;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error: {ex.Message}");
            return false;
        }
    }

    // 1
    public async Task<bool> IsUserLoggedInAsync()
    {
        string sessionState = await CheckSessionStateAsync();

        if (sessionState == "active")
        {
            Debug.WriteLine("Session is active");
            return true;
        }

        if (await RefreshSession())
        {
            Debug.WriteLine("Session was refreshed");
            return true;
        }

        if (await RefreshTokenAsync())
        {
            Debug.WriteLine("Token was refreshed");
            return true;
        }
        Debug.WriteLine("User has been logged out");
        await SignOutAsync();
        return false;
    }
}
