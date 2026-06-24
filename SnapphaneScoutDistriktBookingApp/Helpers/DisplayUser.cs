using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Clerk.BackendAPI.Models.Components;

namespace SnapphaneScoutDistriktBookingApp.Helpers
{
    public class DisplayUser
    {
        public User User { get; }

        public DisplayUser(User user)
        {
            User = user;
        }
        public string DisplayEmail =>
            User?.EmailAddresses?.FirstOrDefault()?.EmailAddressValue ?? "Ingen e-post";

        public string RoleText =>
            string.Equals(User?.PrivateMetadata?.GetValueOrDefault("role")?.ToString(), "admin", StringComparison.OrdinalIgnoreCase)
            ? "Admin" : "Användare";

        public bool CanPromote => RoleText != "Admin";
    }
}
