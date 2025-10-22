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
        Task<Booking?> AddBookingAsync(Booking booking);
        Task UpdateBookingAsync(Booking booking);
        Task<Booking?> FindAddedBookingByIdAsync(Booking booking);
        Task<Booking?> GetBookingByIdAndEmailAsync(Guid id, string email);
    }
}
