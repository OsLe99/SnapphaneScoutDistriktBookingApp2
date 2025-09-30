using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
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

        private string _name = string.Empty;
        [Required]
        public string Name
        {
            get => _name;
            set { if (_name != value) { _name = value; OnPropertyChanged(); } }
        }

        private string _phone = string.Empty;
        [Required]
        public string Phone
        {
            get => _phone;
            set { if (_phone != value) { _phone = value; OnPropertyChanged(); } }
        }

        private string _email = string.Empty;
        [Required]
        [EmailAddress]
        public string Email
        {
            get => _email;
            set { if (_email != value) { _email = value; OnPropertyChanged(); } }
        }

        private bool _isOrg;
        [Required]
        public bool IsOrg
        {
            get => _isOrg;
            set { if (_isOrg != value) { _isOrg = value; OnPropertyChanged(); } }
        }

        private string? _orgName;
        public string? OrgName
        {
            get => _orgName;
            set { if (_orgName != value) { _orgName = value; OnPropertyChanged(); } }
        }

        private DateTime _startDate = DateTime.Today;
        public DateTime StartDate
        {
            get => _startDate;
            set { if (_startDate != value) { _startDate = value; OnPropertyChanged(); } }
        }

        private DateTime _endDate = DateTime.Today;
        public DateTime EndDate
        {
            get => _endDate;
            set { if (_endDate != value) { _endDate = value; OnPropertyChanged(); } }
        }

        private TypeOfBooking _bookingType = TypeOfBooking.None;
        public TypeOfBooking BookingType
        {
            get => _bookingType;
            set
            {
                if (_bookingType != value)
                {
                    _bookingType = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(TypeOfBooking_Swedish));
                }
            }
        }

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

        private int? _numberOfCanoes;
        public int? NumberOfCanoes
        {
            get => _numberOfCanoes;
            set { if (_numberOfCanoes != value) { _numberOfCanoes = value; OnPropertyChanged(); } }
        }

        private int? _numberOfCabin;
        public int? NumberOfCabin
        {
            get => _numberOfCabin;
            set { if (_numberOfCabin != value) { _numberOfCabin = value; OnPropertyChanged(); } }
        }

        private int? _numberOfLeanTo;
        public int? NumberOfLeanTo
        {
            get => _numberOfLeanTo;
            set { if (_numberOfLeanTo != value) { _numberOfLeanTo = value; OnPropertyChanged(); } }
        }

        private int? _numberOfCampground;
        public int? NumberOfCampground
        {
            get => _numberOfCampground;
            set { if (_numberOfCampground != value) { _numberOfCampground = value; OnPropertyChanged(); } }
        }

        private bool _isConfirmed;
        public bool IsConfirmed
        {
            get => _isConfirmed;
            set { if (_isConfirmed != value) { _isConfirmed = value; OnPropertyChanged(); } }
        }

        private bool _emailConformation;
        public bool EmailConformation
        {
            get => _emailConformation;
            set { if (_emailConformation != value) { _emailConformation = value; OnPropertyChanged(); } }
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
