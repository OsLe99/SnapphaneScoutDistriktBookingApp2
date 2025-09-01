using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SnapphaneScoutDistriktBookingApp.Models
{
    internal class Customer : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        [Flags]
        public enum TypeOfBooking
        {
            None = 0,
            Kanot = 1,
            Lägerplats = 2,
            Vindskydd = 3,
            Stuga = 4
        }
        [BsonId]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public bool IsOrg { get; set; }
        public string? OrgName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TypeOfBooking BookingType { get; set; }
        public int? NumberOfCanoes { get; set; }
        public int? NumberOfCabin { get; set; }
        public int? NumberOfLeanTo { get; set; }
        public int? NumberOfCampground { get; set; }
        private bool _isConfirmed;
        public bool IsConfirmed { get { return _isConfirmed; }
        set
            {
                _isConfirmed = value;
                OnPropertyChanged();
                _ = Data.DB.UpdateCheckBoxDatabaseAsync(this);
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public bool EmailConformation { get; set; } = false;
    }
}
