using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MongoDB.Driver;
using SnapphaneScoutDistriktBookingApp.Models;
using SnapphaneScoutDistriktBookingApp.Services.Interface;
using SnapphaneScoutDistriktBookingApp.Views.Booking;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SnapphaneScoutDistriktBookingApp.ViewModels
{
    public partial class BookingViewModel : ObservableObject
    {
        private readonly IDbService _db;
        private readonly IBookingService _bookingService;
        private readonly IValidateBookingService _validateBookingService;

        #region Observable Properties
        [ObservableProperty]
        private View _currentView;

        [ObservableProperty]
        private bool _canGoToPreviousStep = false;

        [ObservableProperty]
        private bool _canGoToNextStep = true;

        private int _currentStep = 1;

        private const int _totalSteps = 5;
        public string StepInfo => $"Steg {_currentStep} av {_totalSteps}";

        [ObservableProperty]
        private Customer _customer = new();

        [ObservableProperty]
        private string _infoLabelText = string.Empty;

        [ObservableProperty]
        private string _numberInputPlaceholder = string.Empty;

        [ObservableProperty]
        private string _numberInputValue;

        partial void OnNumberInputValueChanged(string value)
        {
            if (int.TryParse(value, out int number))
            {
                SaveNumberInputToCustomer(number);
            }
        }

        [ObservableProperty]
        private string _startTimeButtonText;

        [ObservableProperty]
        private string _endTimeButtonText;

        [ObservableProperty]
        private bool _isScoutMember = false;

        [ObservableProperty]
        private bool _showOrgNameInput = false;

        [ObservableProperty]
        private TimeSpan? _startTime;

        [ObservableProperty]
        private TimeSpan? _endTime;

        [ObservableProperty]
        private List<Customer> _relevantBookings = new();
        #endregion

        public BookingViewModel(IDbService db, IBookingService bookingService, IValidateBookingService validateBookingService)
        {
            _db = db;
            _bookingService = bookingService;
            _validateBookingService = validateBookingService;
            Customer = new Customer();

            StartTime = TimeSpan.FromHours(12);
            EndTime = TimeSpan.FromHours(13);

            CurrentView = new BookingStep1View { BindingContext = this };
            UpdateNavigationButtons();
            UpdateCurrentView();
        }
        #region Navigation and Step Management
        private void OnStepChanged()
        {
            OnPropertyChanged(nameof(StepInfo));
        }

        [RelayCommand]
        private async void GoToNextStep()
        {
            bool isValid = await ValidateCurrentStep();

            if (isValid && _currentStep < _totalSteps)
            {
                _currentStep++;
                UpdateCurrentView();
                UpdateNavigationButtons();
            }
            else if (!isValid)
            {
                await Application.Current.Windows[0].Page.DisplayAlert("Fel", "Vänligen fyll i alla obligatoriska fält.", "OK");
            }
        }

        [RelayCommand]
        private void GoToPreviousStep()
        {
            if (_currentStep > 1)
            {
                _currentStep--;

                // Reset customer data if going back to step 1
                if (_currentStep == 1)
                {
                    ResetBookingSelection();
                }

                UpdateCurrentView();
                UpdateNavigationButtons();
            }
        }
        private void UpdateNavigationButtons()
        {
            CanGoToPreviousStep = _currentStep > 1;
            CanGoToNextStep = _currentStep < _totalSteps && _currentStep != 1;
            OnStepChanged();
        }
        #endregion

        private void ResetBookingSelection()
        {
            Customer.BookingType = Customer.TypeOfBooking.None;
            Customer.NumberOfCanoes = null;
            Customer.NumberOfCabin = null;
            Customer.NumberOfLeanTo = null;
            Customer.NumberOfCampground = null;
            Customer.StartDate = DateTime.Today;
            Customer.EndDate = DateTime.Today;

            StartTime = TimeSpan.FromHours(12);
            EndTime = TimeSpan.FromHours(13);

            NumberInputValue = string.Empty;
            IsScoutMember = false;
            ShowOrgNameInput = false;

            StartTimeButtonText = $"Starttid: {StartTime:hh\\:mm}";
            EndTimeButtonText = $"Sluttid: {EndTime:hh\\:mm}";
        }

        private Dictionary<int, View> _viewCache = new Dictionary<int, View>();

        private void UpdateCurrentView()
        {
            if (!_viewCache.TryGetValue(_currentStep, out View cachedView))
            {
                cachedView = _currentStep switch
                {
                    1 => new BookingStep1View { BindingContext = this },
                    2 => new BookingStep2View { BindingContext = this },
                    3 => new BookingStep3View { BindingContext = this },
                    4 => new BookingStep4View { BindingContext = this },
                    5 => new BookingStep5View { BindingContext = this },
                    _ => CurrentView
                };
                _viewCache[_currentStep] = cachedView;
            }
            CurrentView = cachedView;
        }

        private async Task<bool> ValidateCurrentStep()
        {
            switch (_currentStep)
            {
                case 1:
                    // Check that a booking type has been selected
                    return Customer.BookingType != Customer.TypeOfBooking.None;
                case 2:
                    // Validate that a number has been entered for the selected booking type
                    return Customer.BookingType switch
                    {
                        Customer.TypeOfBooking.Canoe => Customer.NumberOfCanoes > 0,
                        Customer.TypeOfBooking.Cabin => Customer.NumberOfCabin > 0,
                        Customer.TypeOfBooking.LeanTo => Customer.NumberOfLeanTo > 0,
                        Customer.TypeOfBooking.CampGrounds => Customer.NumberOfCampground > 0,
                        _ => false
                    };
                case 3:
                    // Validate that dates and times are selected using ValidateBookingService
                    return await _validateBookingService.CheckIfValidTime(StartTime, EndTime);
                case 4:
                    // Validate that customer info is filled, add ValidateService later
                    bool basicInfoFilled = !string.IsNullOrEmpty(Customer.Name) &&
                                           !string.IsNullOrEmpty(Customer.Phone) &&
                                           !string.IsNullOrEmpty(Customer.Email);
                    if(!basicInfoFilled)
                        return false;
                    var errors = _validateBookingService.ValidateBookingDetails(Customer);
                    if (errors.Any())
                    {
                        string errorMessage = string.Join("\n", errors);
                        Application.Current.Windows[0].Page.DisplayAlert("Fel i bokningsinformation", errorMessage, "OK");
                        return false;
                    }
                    return true;
                case 5:
                    // Save to database
                    return true;
                default:
                    return false;
            }
        }

        [RelayCommand]
        private void SetBookingType(string type)
        {
            switch (type)
            {
                case "Kanot":
                    Customer.BookingType |= Customer.TypeOfBooking.Canoe;
                    break;
                case "Stugan":
                    Customer.BookingType |= Customer.TypeOfBooking.Cabin;
                    break;
                case "Vindskydd":
                    Customer.BookingType |= Customer.TypeOfBooking.LeanTo;
                    break;
                case "Lägerområde":
                    Customer.BookingType |= Customer.TypeOfBooking.CampGrounds;
                    break;
            }
            UpdateInfoLabelAndPlaceholder();
            GoToNextStep();
        }

        private void UpdateInfoLabelAndPlaceholder()
        {
            switch (Customer.BookingType)
            {
                case Customer.TypeOfBooking.Canoe:
                    InfoLabelText = "Hur många kanoter vill ni boka?";
                    NumberInputPlaceholder = "Antal kanoter";
                    break;
                case Customer.TypeOfBooking.Cabin:
                    InfoLabelText = "Hur många personer ska vistas i stugan?";
                    NumberInputPlaceholder = "Antal personer i stugan";
                    break;
                case Customer.TypeOfBooking.LeanTo:
                    InfoLabelText = "Hur många vindskydd vill ni boka?";
                    NumberInputPlaceholder = "Antal vindskydd";
                    break;
                case Customer.TypeOfBooking.CampGrounds:
                    InfoLabelText = "Hur många personer ska använda lägerområdet?";
                    NumberInputPlaceholder = "Antal personer på lägerområdet";
                    break;
                default:
                    InfoLabelText = string.Empty;
                    NumberInputPlaceholder = string.Empty;
                    break;
            }
        }

        [RelayCommand]
        private void SaveNumberInputToCustomer(int number)
        {
            switch (Customer.BookingType)
            {
                case Customer.TypeOfBooking.Canoe:
                    Customer.NumberOfCanoes = number;
                    break;
                case Customer.TypeOfBooking.Cabin:
                    Customer.NumberOfCabin = number;
                    break;
                case Customer.TypeOfBooking.LeanTo:
                    Customer.NumberOfLeanTo = number;
                    break;
                case Customer.TypeOfBooking.CampGrounds:
                    Customer.NumberOfCampground = number;
                    break;
            }
        }

        [RelayCommand]
        private void ToggleScoutMember(bool isChecked)
        {
            IsScoutMember = isChecked;
            ShowOrgNameInput = isChecked;
        }

        [RelayCommand]
        public void UpdateTimeButtons()
        {
            StartTimeButtonText = $"Starttid: {StartTime?.ToString("hh\\:mm")}";
            EndTimeButtonText = $"Sluttid: {EndTime?.ToString("hh\\:mm")}";
        }

        public DateTime CombineDateAndTime(DateTime date, TimeSpan time)
        {
            return date.Date + time;
        }

        [RelayCommand]
        private async Task ConfirmBookingAsync()
        {
            await _bookingService.AddBookingAsync(Customer);
            await Shell.Current.Navigation.PopToRootAsync();
            await Shell.Current.GoToAsync("//MainPage", true);
        }

        [RelayCommand]
        public async Task LoadBlackoutDatesAsync()
        {
            var allBookings = await _db.GetAllBookingsAsync();
            RelevantBookings = allBookings.Where(b => b.BookingType == Customer.BookingType && b.IsConfirmed).ToList();
        }

        public bool IsSelectableDates(DateTime date)
        {
            return !RelevantBookings.Any(b => date.Date >= b.StartDate && date.Date <= b.EndDate);
        }

        public bool IsOverlappingWithExistingBookings(DateTime start, DateTime end)
        {
            return RelevantBookings.Any(b => start <= b.EndDate.Date && end >= b.StartDate.Date);
        }

        public void UpdateCustomerDatesAndTimes(DateTime start, DateTime end)
        {
            Customer.StartDate = CombineDateAndTime(start, StartTime ?? TimeSpan.Zero);
            Customer.EndDate = CombineDateAndTime(end, EndTime ?? TimeSpan.Zero);
            Debug.WriteLine($"Updated Customer dates and times: Start={Customer.StartDate}, End={Customer.EndDate}");
        }
    }
}
