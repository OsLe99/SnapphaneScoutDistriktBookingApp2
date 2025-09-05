using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnapphaneScoutDistriktBookingApp.Models;
using MongoDB.Driver;

namespace SnapphaneScoutDistriktBookingApp.Services.Interface
{
    public interface IDbService
    {
        IMongoCollection<Customer> BookingCollection();
        IMongoCollection<Models.Contact> ContactCollection();
        IMongoCollection<Info> InfoCollection();
        Task UpdateCheckBoxDatabaseAsync(Customer costumer);
        IMongoCollection<Admin> AdminUserCollection();
    }
}
