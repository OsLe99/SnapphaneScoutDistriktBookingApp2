using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SnapphaneScoutDistriktBookingApp.Models
{
    [Table("bookings")]
    public class Booking : BaseModel, INotifyPropertyChanged
    {
        [Flags]
        public enum TypeOfBooking
        {
            None = 0,
            Canoe = 1,
            CampGrounds = 2,
            LeanTo = 3,
            Cabin = 4
        }

        [PrimaryKey("id", false)]
        [Column("id")]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Column("name")]
        public string Name { get; set; } = string.Empty;

        [Column("phone")]
        public string Phone { get; set; } = string.Empty;

        [Column("email")]
        public string Email { get; set; } = string.Empty;

        [Column("is_org")]
        public bool IsOrg { get; set; }

        [Column("org_name")]
        public string? OrgName { get; set; }

        [Column("start_date")]
        public DateTime StartDate { get; set; }

        [Column("end_date")]
        public DateTime EndDate { get; set; }

        [Column("booking_type")]
        public TypeOfBooking BookingType { get; set; }

        [Column("number_of_canoes")]
        public int? NumberOfCanoes { get; set; }

        [Column("number_of_cabin")]
        public int? NumberOfCabin { get; set; }

        [Column("number_of_lean_to")]
        public int? NumberOfLeanTo { get; set; }

        [Column("number_of_campground")]
        public int? NumberOfCampground { get; set; }

        [Column("is_confirmed")]
        public bool IsConfirmed { get; set; } = false;

        [Column("email_confirmation")]
        public bool EmailConfirmation { get; set; } = false;

        //public string TypeOfBooking_Swedish =>
        //    BookingTranslations.ContainsKey(BookingType)
        //        ? BookingTranslations[BookingType]
        //        : "Okänd";

        //private static readonly Dictionary<TypeOfBooking, string> BookingTranslations = new()
        //{
        //    { TypeOfBooking.None, "Ingen" },
        //    { TypeOfBooking.Canoe, "Kanot" },
        //    { TypeOfBooking.CampGrounds, "Lägerområde" },
        //    { TypeOfBooking.LeanTo, "Vindskydd" },
        //    { TypeOfBooking.Cabin, "Stuga" }
        //};

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}