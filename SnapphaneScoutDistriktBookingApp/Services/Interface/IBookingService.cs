using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapphaneScoutDistriktBookingApp.Services.Interface
{
    public interface IBookingService
    {
        Task<Models.Customer> AddBookingAsync(Models.Customer customer);
        Task<Models.Customer> FindAddedBookingByIdAsync(Models.Customer customer);
    }
}
