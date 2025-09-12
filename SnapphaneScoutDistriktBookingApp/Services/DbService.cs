using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Maui.ApplicationModel.Communication;
using MongoDB.Driver;
using SnapphaneScoutDistriktBookingApp.Models;
using SnapphaneScoutDistriktBookingApp.Services.Interface;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BCrypt.Net;

namespace SnapphaneScoutDistriktBookingApp.Services
{
    public class DbService : IDbService
    {
        #region variables
        private readonly MongoClient _client;
        private readonly IEmailService _emailService;

        #endregion

        #region ctor
        public DbService()
        {
            const string connectionUri = "mongodb+srv://dbAdmin:DBadmin00@hultetbooking.h5urq.mongodb.net/?retryWrites=true&w=majority&appName=HultetBooking";
            var settings = MongoClientSettings.FromConnectionString(connectionUri);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            var client = new MongoClient(settings);
            _client = new MongoClient(settings);
            _emailService = new EmailService();
        }
        #endregion

        #region private methods
        private IMongoCollection<Customer> BookingCollection()
        {
            var database = _client.GetDatabase("bookingsDB");
            return database.GetCollection<Customer>("bookings");
        }
        private IMongoCollection<Models.Contact> ContactCollection()
        {
            var database = _client.GetDatabase("contactsDB");
            return database.GetCollection<Models.Contact>("contacts");
        }
        private IMongoCollection<Models.Info> InfoCollection()
        {
            var database = _client.GetDatabase("infoDB");
            var infoCollection = database.GetCollection<Models.Info>("infostring");
            return infoCollection;
        }
        private IMongoCollection<Models.Admin> AdminUserCollection()
        {
            var database = _client.GetDatabase("adminUsers");
            return database.GetCollection<Models.Admin>("adminUsers");
        }
        #endregion

        #region CRUD customer
        public async Task<Customer> AddCustomerAsync(Customer customer)
        {
            await BookingCollection().InsertOneAsync(customer);
            return customer;
        }
        public async Task UpdateCheckBoxDatabaseAsync(Models.Customer customer) // UpdateConfirmedCustomer
        {
            try
            {
                var collection = BookingCollection();
                var filter = Builders<Models.Customer>.Filter.Eq(x => x.Id, customer.Id);
                var update = Builders<Models.Customer>.Update.Set(x => x.IsConfirmed, customer.IsConfirmed);
                await collection.UpdateOneAsync(filter, update);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fel vid uppdatering: {ex.Message}");
            }
        }

        #endregion

        #region CRUD contact
        public async Task<Models.Contact> AddContactAsync(Models.Contact contact)
        {
            await ContactCollection().InsertOneAsync(contact);
            return contact;
        }

        #endregion

        #region CRUD info
        public async Task<Models.Info> UpdateInfoAsync(Models.Info info, string Id)
        {
            await InfoCollection().ReplaceOneAsync(filter: Builders<Models.Info>.Filter.Eq(x => x.Id, "unique_id"),
                replacement: info, options: new ReplaceOptions { IsUpsert = true });
            return info;
        }

        #endregion

        #region admin methods
        public async Task<bool> RegisterAdminAsync(string userName, string userEmail, string password) // Byta till ex. SignInUser
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

        public async Task<bool> CheckIfAdminAsync(string userName, string userEmail)
        {
            var collection = AdminUserCollection();
            var filter = Builders<Admin>.Filter.And(
                Builders<Admin>.Filter.Eq(x => x.Name, userName),
                Builders<Admin>.Filter.Eq(x => x.Email, userEmail)
            );

            var adminUser = await collection.Find(filter).FirstOrDefaultAsync();

            return adminUser != null;
        }
        public async Task<bool> CheckAdminCredentialsAsync(string userEmail, string password)
        {
            var collection = AdminUserCollection();
            var filter = Builders<Admin>.Filter.And(
                Builders<Admin>.Filter.Eq(x => x.Email, userEmail)
                );
            var adminUser = await collection.Find(filter).FirstOrDefaultAsync();

            if (adminUser == null )
            {
                return false;
            }
            return BCrypt.Net.BCrypt.Verify(password, adminUser.PasswordHashed);
        }

        #endregion

        public async Task<List<Models.Info>> GetAllInfoAsync()
        {
            List<Models.Info> info = await InfoCollection().Find(Builders<Models.Info>.Filter.Empty).ToListAsync();
            return info;
        }
        public async Task<List<Customer>> GetAllBookingsAsync()
        {
            List<Customer> bookings = await BookingCollection().Find(_ => true).ToListAsync();
            return bookings;
        }

        public async Task<List<Models.Contact>> GetAllContactsAsync()
        {
            List<Models.Contact> contacts = await ContactCollection().Find(Builders<Models.Contact>.Filter.Empty).ToListAsync();
            return contacts;
        }
        public async Task<ObservableCollection<Customer>> LoadAllBookingsAsync(ObservableCollection<Customer> bookings)
        {
            var data = await GetAllBookingsAsync();
            foreach (var booking in data)
            {
                bookings.Add(booking);
            }
            return bookings;
        }
        public async Task<ObservableCollection<Customer>> LoadAllNewBookingsAsync(ObservableCollection<Customer> bookings)
        {
            var data = await GetAllBookingsAsync();
            var newData = data.Where(x => x.StartDate.Date >= DateTime.Today).ToList();
            bookings.Clear();
            foreach (var newBookings in newData)
            {
                bookings.Add(newBookings);
            }
            return bookings;
        }
        public async Task<Customer> FindBookingByIdAsync(Customer customer)
        {
            await BookingCollection().Find(c => c.Id == customer.Id).FirstOrDefaultAsync();
            return customer;
        }
    }
}

