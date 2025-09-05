using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MongoDB.Driver;
using SnapphaneScoutDistriktBookingApp.Services.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SnapphaneScoutDistriktBookingApp.ViewModels
{
    class BookingViewModel : INotifyPropertyChanged
    {
        private readonly IDbService _db;
        public event PropertyChangedEventHandler? PropertyChanged;

        public BookingViewModel(IDbService db)
        {
            _db = db;
        }
        
        private string _canoesAvailable;
        public string CanoesAvailable { get { return _canoesAvailable; }
            set
            {
                _canoesAvailable = value;
                OnPropertyChanged();
                //LoadViableNumbers();
            }
        }

        private string _cabinAvailable;
        public string CabinAvailable { get { return _cabinAvailable; } set
            {
                _cabinAvailable = value;
                OnPropertyChanged();
                //LoadViableNumbers();
            }
        }

        private string _leanToAvailable;
        public string LeanToAvailable
        {
            get { return _leanToAvailable; }
            set
            {
                _leanToAvailable = value;
                OnPropertyChanged();
                //LoadViableNumbers();
            }
        }


        private string _campGroundAvailable;
        public string CampGroundAvailable
        {
            get { return _campGroundAvailable; }
            set
            {
                _campGroundAvailable = value;
                OnPropertyChanged();
                //LoadViableNumbers();
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public async void LoadViableNumbers()
        {
            int[] numbers = await ViableCanoesInt();
            CanoesAvailable = (14-numbers[0]).ToString();
            CabinAvailable = (1-numbers[1]).ToString();
            LeanToAvailable = (4-numbers[2]).ToString();
            CampGroundAvailable = numbers[3].ToString();
        }

        public async Task<int[]> ViableCanoesInt()
        {
            DateTime start = NewStartDate;
            DateTime end = NewEndDate;

            var bookingCollection = await _db.BookingCollection().Find(Builders<Models.Customer>.Filter.Where(x => x.StartDate <= end && x.EndDate >= start)).ToListAsync();
            int[] totalSum = new int[4];
            totalSum[0] = bookingCollection.Sum(x => x.NumberOfCanoes.GetValueOrDefault());
            
            totalSum[1] = bookingCollection.Sum(x => x.NumberOfCabin.GetValueOrDefault());
            
            totalSum[2] = bookingCollection.Sum(x => x.NumberOfLeanTo.GetValueOrDefault());
            
            totalSum[3] = bookingCollection.Sum(x => x.NumberOfCampground.GetValueOrDefault());
            
            return totalSum;
        }

        //----------------------------------------------------------------------------------------

        
        private DateTime _newStartDate = DateTime.Today;
        public DateTime NewStartDate { get { return _newStartDate; } 
            set
            {
                _newStartDate = value;
                OnPropertyChanged();
                LoadViableNumbers();
            }
        }


        private DateTime _newEndDate = DateTime.Today.AddDays(1);
        public DateTime NewEndDate
        {
            get { return _newEndDate; }
            set
            {
                _newEndDate = value;
                OnPropertyChanged();
                LoadViableNumbers();
            }
        }


        







        //public ICommand SelectStartDateCommand => new AsyncRelayCommand(async () => await SelectStartDate());
        //public ICommand SelectEndDateCommand => new AsyncRelayCommand(async () => await SelectEndDate());
        //private async Task SelectStartDate()
        //{
        //    DateTime? result = await ShowDatePicker(NewStartDate);
        //    if (result.HasValue)
        //    {
        //        NewStartDate = result.Value;
        //    }
        //}
        //private async Task SelectEndDate()
        //{
        //    DateTime? result = await ShowDatePicker(NewEndDate);
        //    if (result.HasValue)
        //    {
        //        NewEndDate = result.Value;


        //    }
        //}
        //private async Task<DateTime?> ShowDatePicker(DateTime initalDate)
        //{
        //    var datePicker = new DatePicker { Date = initalDate };

        //    var popup = new ContentPage
        //    {
        //        Content = new VerticalStackLayout
        //        {
        //            Padding = 20,
        //            Children =
        //            {
        //                new Label { Text = "Välj ett datum", FontSize = 20},
        //                datePicker,
        //                new Button
        //                {
        //                    Text = "OK",
        //                    Command = new Command(() => Application.Current.MainPage.Navigation.PopModalAsync())

        //                }
        //            }
        //        }
        //    };
        //    await Application.Current.MainPage.Navigation.PushModalAsync(popup);
        //    await Task.Delay(100);

        //    return datePicker.Date;

        //}
    }
}
