using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapphaneScoutDistriktBookingApp.Services.Interface
{
    public interface IUserSessionService : INotifyPropertyChanged
    {
        string UserName { get; }
        string UserEmail { get; }
        bool IsAdmin { get; }

        void SetUser(string userName, string userEmail);
        bool SetAdmin(bool isAdmin);
        bool IsUserSet();
        void ResetUser();
    }
}
