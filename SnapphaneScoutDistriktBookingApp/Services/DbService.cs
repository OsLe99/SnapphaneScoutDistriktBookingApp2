using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SnapphaneScoutDistriktBookingApp.Models;
using SnapphaneScoutDistriktBookingApp.Services.Interface;
using MongoDB.Driver;

namespace SnapphaneScoutDistriktBookingApp.Services
{
    public class DbService : IDbService
    {
        private readonly MongoClient _client;
        public DbService()
        {
            const string connectionUri = "mongodb+srv://dbAdmin:DBadmin00@hultetbooking.h5urq.mongodb.net/?retryWrites=true&w=majority&appName=HultetBooking";
            var settings = MongoClientSettings.FromConnectionString(connectionUri);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            var client = new MongoClient(settings);
            _client = new MongoClient(settings);
        }
        public IMongoCollection<Customer> BookingCollection()
        {
            var database = _client.GetDatabase("bookingsDB");
            return database.GetCollection<Customer>("bookings");
        }
        public IMongoCollection<Models.Contact> ContactCollection()
        {
            var database = _client.GetDatabase("contactsDB");
            return database.GetCollection<Models.Contact>("contacts");
        }
        public IMongoCollection<Models.Info> InfoCollection()
        {
            var database = _client.GetDatabase("infoDB");
            var infoCollection = database.GetCollection<Models.Info>("infostring");
            return infoCollection;
        }
        public async Task UpdateCheckBoxDatabaseAsync(Models.Customer costumer)
        {
            try
            {
                var collection = BookingCollection();
                var filter = Builders<Models.Customer>.Filter.Eq(x => x.Id, costumer.Id);
                var update = Builders<Models.Customer>.Update.Set(x => x.IsConfirmed, costumer.IsConfirmed);
                await collection.UpdateOneAsync(filter, update);
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Fel vid uppdatering: {ex.Message}");
            }
        }
        public IMongoCollection<Models.Admin> AdminUserCollection()
        {
            var database = _client.GetDatabase("adminUsers");
            return database.GetCollection<Models.Admin>("adminUsers");
        }

        public async Task<bool> RegisterAdminAsync(string userName, string userEmail, string password)
        {
            var collection = AdminUserCollection();

            var existingUser = await collection.Find(x => x.Name == userName).FirstOrDefaultAsync();

            if (existingUser != null)
            {
                return false;
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            var newAdmin = new Models.Admin
            {
                Name = userName,
                Email = userEmail,
                PasswordHashed = hashedPassword
            };

            await collection.InsertOneAsync(newAdmin);
            return true;
        }


        // Uppdatera ifall bokning är bekräftad
        //            {
        //        _isConfirmed = value;
        //        OnPropertyChanged();
        //_ = Services.DB.UpdateCheckBoxDatabaseAsync(this); }
    }
}

