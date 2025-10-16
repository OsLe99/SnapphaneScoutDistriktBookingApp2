using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapphaneScoutDistriktBookingApp.Services.Interface
{
    public interface IClerkAuthService
    {
        Task<string?> SignInAsync(string email, string password);
        Task<bool> SignOutAsync();
        Task<string> CheckSessionStateAsync();
        Task<bool> RefreshTokenAsync();
        Task<bool> IsUserLoggedInAsync();
    }
}
