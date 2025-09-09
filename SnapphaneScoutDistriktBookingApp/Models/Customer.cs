using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SnapphaneScoutDistriktBookingApp.Models
{
    public class Customer : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        [Flags]
        public enum TypeOfBooking
        {
            None = 0,
            Canoe = 1,
            CampGrounds = 2,
            LeanTo = 3,
            Cabin = 4
        }
        [BsonId]
        public ObjectId Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public bool IsOrg { get; set; }
        public string? OrgName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TypeOfBooking BookingType { get; set; }
        public string TypeOfBooking_Swedish =>
            BookingTranslations.ContainsKey(BookingType)
            ? BookingTranslations[BookingType] : "Okänd";
        private static readonly Dictionary<TypeOfBooking, string> BookingTranslations = new()
        {
            { TypeOfBooking.None, "Ingen" },
            { TypeOfBooking.Canoe, "Kanot" },
            { TypeOfBooking.CampGrounds, "Lägerområde" },
            { TypeOfBooking.LeanTo, "Vindskydd" },
            { TypeOfBooking.Cabin, "Stuga" }
        };
        public int? NumberOfCanoes { get; set; }
        public int? NumberOfCabin { get; set; }
        public int? NumberOfLeanTo { get; set; }
        public int? NumberOfCampground { get; set; }
        private bool _isConfirmed;
        public bool IsConfirmed { get; set; }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public bool EmailConformation { get; set; } = false;
    }
}
