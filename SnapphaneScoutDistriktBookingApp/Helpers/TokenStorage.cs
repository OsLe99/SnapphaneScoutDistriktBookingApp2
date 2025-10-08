using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapphaneScoutDistriktBookingApp.Helpers
{
    public static class TokenStorage
    {
        private const string Key = "clerk_token";

        public static async Task SaveTokenAsync(string token)
        {
            await SecureStorage.SetAsync(Key, token);
        }
        public static async Task<string?> GetTokenAsync()
        {
            return await SecureStorage.GetAsync(Key);
        }
        public static async Task RemoveTokenAsync()
        {
            SecureStorage.Remove(Key);
            await Task.CompletedTask;
        }
    }
}
