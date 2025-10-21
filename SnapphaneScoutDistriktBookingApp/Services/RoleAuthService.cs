using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clerk.BackendAPI;
using SnapphaneScoutDistriktBookingApp.Services.Interface;

namespace SnapphaneScoutDistriktBookingApp.Services
{
    public class RoleAuthService : IRoleAuthService
    {
        private readonly ClerkBackendApi _sdk;
        public RoleAuthService(string bearerToken)
        {
            _sdk = new ClerkBackendApi(bearerAuth: bearerToken);
        }
        public async Task<bool> IsAdminAsync()
        {
            try
            {
                var sessionId = await SecureStorage.Default.GetAsync("sessionId");
                if (string.IsNullOrEmpty(sessionId))
                {
                    return false;
                }

                var sessionResponse = await _sdk.Sessions.GetAsync(sessionId);
                var userId = sessionResponse?.Session?.UserId;
                if (string.IsNullOrEmpty(userId))
                {
                    return false;
                }

                var userResponse = await _sdk.Users.GetAsync(userId);
                var user = userResponse?.User;
                if (user == null)
                {
                    return false;
                }

                var role = user.PrivateMetadata?.GetValueOrDefault("role")?.ToString();
                Debug.WriteLine("Admin found.");
                return role?.Equals("admin", StringComparison.OrdinalIgnoreCase) == true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error checking admin status: {ex.Message}");
                return false;
            }
        }
    }
}
