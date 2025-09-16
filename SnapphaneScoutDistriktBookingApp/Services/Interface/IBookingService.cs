using MongoDB.Bson;
using SnapphaneScoutDistriktBookingApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapphaneScoutDistriktBookingApp.Services.Interface
{
    public interface IBookingService
    {
        Task<Customer?> AddBookingAsync(Customer customer);
        Task UpdateBookingAsync(Customer booking);
        Task<Customer?> FindAddedBookingByIdAsync(Customer customer);
        Task<Customer?> GetBookingByIdAndEmailAsync(ObjectId id, string email);
    }
}
