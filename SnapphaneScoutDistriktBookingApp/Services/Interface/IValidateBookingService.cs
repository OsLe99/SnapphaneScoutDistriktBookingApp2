using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapphaneScoutDistriktBookingApp.Services.Interface
{
    public interface IValidateBookingService
    {
        bool ValidateEmail(string email);
        bool ValidatePhoneNumber(string phoneNumber);
        bool ValidateName(string name);
        List<string> ValidateBookingDetails(Models.Customer customer);
    }
}
