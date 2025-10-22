using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapphaneScoutDistriktBookingApp.Services.Interface
{
    public interface IEmailService
    {
        Task SendEmailAsync(string apiKey, string fromEmail, string toEmail, Models.Booking costumer);
        Task SendEmailConfirmationAsync(string apiKey, string fromEmail, string toEmail, Models.Booking costumer);
    }
}
