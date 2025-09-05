using MongoDB.Driver;
using SnapphaneScoutDistriktBookingApp.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SnapphaneScoutDistriktBookingApp.Services
{
    public class AdminService : IAdminService
    {
        private readonly IUserSessionService _userSession;
        private readonly IDbService _db;
        public AdminService(IUserSessionService userSession, IDbService db)
        {
            _userSession = userSession;
            _db = db;
        }
        public async Task<bool> TryLoginAdminAsync(string username, string userEmail, string password)
        {
            var collection = _db.AdminUserCollection();
            var filter = Builders<Models.Admin>.Filter.And(
                Builders<Models.Admin>.Filter.Eq(x => x.Name, username),
                Builders<Models.Admin>.Filter.Eq(x => x.Email, userEmail)
                );

            var adminUser = await collection.Find(filter).FirstOrDefaultAsync();

            if (adminUser == null)
            {
                return false;
            }

            bool isAdmin = BCrypt.Net.BCrypt.Verify(password, adminUser.PasswordHashed);
            if (isAdmin)
            {
                _userSession.SetAdmin(true);
                Preferences.Set("IsAdmin", true);
            }

            return isAdmin;
        }

        public async Task<bool> CheckIfAdminAsync(string username, string userEmail)
        {
            var collection = _db.AdminUserCollection();
            var filter = Builders<Models.Admin>.Filter.And(
                Builders<Models.Admin>.Filter.Eq(x => x.Name, username),
                Builders<Models.Admin>.Filter.Eq(x => x.Email, userEmail)
            );

            var adminUser = await collection.Find(filter).FirstOrDefaultAsync();

            return adminUser != null;
        }
    }
}
