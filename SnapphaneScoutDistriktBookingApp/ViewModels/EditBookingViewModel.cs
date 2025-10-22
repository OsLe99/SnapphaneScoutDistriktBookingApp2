using SnapphaneScoutDistriktBookingApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using SnapphaneScoutDistriktBookingApp.Helpers;

namespace SnapphaneScoutDistriktBookingApp.ViewModels
{
    public class EditBookingViewModel : INotifyPropertyChanged
    {
        private Booking _booking;
        private TimeSpan _startTime;
        private TimeSpan _endTime;

        public EditBookingViewModel(Booking booking)
        {
            _booking = booking;
            _startTime = booking.StartDate.TimeOfDay;
            _endTime = booking.EndDate.TimeOfDay;
        }

        #region PropertyChanged vars
        public string Name
        {
            get => _booking.Name;
            set { _booking.Name = value; OnPropertyChanged(); }
        }

        public string Email
        {
            get => _booking.Email;
            set { _booking.Email = value; OnPropertyChanged(); }
        }

        public string Phone
        {
            get => _booking.Phone;
            set { _booking.Phone = value; OnPropertyChanged(); }
        }

        public bool IsOrg
        {
            get => _booking.IsOrg;
            set
            {
                if (_booking.IsOrg != value)
                {
                    _booking.IsOrg = value;
                    if (_booking.IsOrg && _booking.OrgName == null)
                        _booking.OrgName = string.Empty;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(OrgName));
                }
            }
        }

        public string OrgName
        {
            get => _booking.OrgName ?? string.Empty;
            set { _booking.OrgName = value; OnPropertyChanged(); }
        }

        public int? NumberOfCanoes
        {
            get => _booking.NumberOfCanoes;
            set { _booking.NumberOfCanoes = value; OnPropertyChanged(); }
        }

        public int? NumberOfCabin
        {
            get => _booking.NumberOfCabin;
            set { _booking.NumberOfCabin = value; OnPropertyChanged(); }
        }

        public int? NumberOfLeanTo
        {
            get => _booking.NumberOfLeanTo;
            set { _booking.NumberOfLeanTo = value; OnPropertyChanged(); }
        }

        public int? NumberOfCampground
        {
            get => _booking.NumberOfCampground;
            set { _booking.NumberOfCampground = value; OnPropertyChanged(); }
        }

        public DateTime StartDate
        {
            get => TimeZoneHelper.ToSwedishTime(_booking.StartDate);
            set 
            {
                _booking.StartDate = value.Date + _startTime; 
                OnPropertyChanged();
                OnPropertyChanged(nameof(StartTimeText));
            }
        }

        public DateTime EndDate
        {
            get => TimeZoneHelper.ToSwedishTime(_booking.EndDate);
            set 
            { 
                _booking.EndDate = value.Date + _endTime; 
                OnPropertyChanged();
                OnPropertyChanged(nameof(EndTimeText));
            }
        }
        public TimeSpan StartTime
        {
            get => _startTime;
            set
            {
                _startTime = value;
                _booking.StartDate = _booking.StartDate.Date + value;
                OnPropertyChanged(nameof(StartTime));
                OnPropertyChanged(nameof(StartTimeText));
            }
        }

        public TimeSpan EndTime
        {
            get => _endTime;
            set
            {
                _endTime = value;
                _booking.EndDate = _booking.EndDate.Date + value;
                OnPropertyChanged(nameof(EndTime));
                OnPropertyChanged(nameof(EndTimeText));
            }
        }

        public string StartTimeText => $"Starttid: {StartTime:hh\\:mm}";
        public string EndTimeText => $"Sluttid: {EndTime:hh\\:mm}";
        #endregion

        public Booking GetBooking() => _booking;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
