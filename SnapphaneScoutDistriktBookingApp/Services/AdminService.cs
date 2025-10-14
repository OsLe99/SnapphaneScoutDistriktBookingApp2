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
        //public async Task<bool> TryLoginAdminAsync(string userName, string userEmail, string password)
        //{
        //    var adminUser = await _db.CheckIfAdminAsync(userName, userEmail);

        //    if (adminUser == true)
        //    {
        //        _userSession.SetAdmin(true);
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
    }
}
