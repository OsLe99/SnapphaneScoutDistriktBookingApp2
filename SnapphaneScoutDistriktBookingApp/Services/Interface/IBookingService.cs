using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapphaneScoutDistriktBookingApp.Services.Interface
{
    public interface IBookingService
    {
        Task AddBookingAsync(Models.Customer customer);
    }
}
