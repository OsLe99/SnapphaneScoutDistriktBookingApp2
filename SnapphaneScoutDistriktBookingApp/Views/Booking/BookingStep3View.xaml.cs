using Microsoft.Maui.Controls;
using SnapphaneScoutDistriktBookingApp.ViewModels;
using Syncfusion.Maui.Calendar;
using System;
using System.Diagnostics;

namespace SnapphaneScoutDistriktBookingApp.Views.Booking;

public partial class BookingStep3View : ContentView
{
    public BookingStep3View()
    {
        InitializeComponent();
    }
    private void OnLoaded(object sender, EventArgs e)
    {
        if (BindingContext is BookingViewModel vm)
        {
            Task.Run(async () => await vm.LoadBlackoutDatesAsync()).Wait();
            BookingCalendar.SelectableDayPredicate = vm.IsSelectableDates;

            vm.UpdateTimeButtons();
        }
    }

    private void OnCalendarSelectionChanged(object sender, CalendarSelectionChangedEventArgs e)
    {
        if (BindingContext is BookingViewModel vm)
        {
            if (!TryGetSelectedRange(out var start, out var end))
                return;
            if (vm.IsOverlappingWithExistingBookings(start, end))
            {
                Application.Current.Windows[0].Page.DisplayAlert("Otillgängligt", "Det finns redan en bokning under dessa datum", "OK");
                return;
            }

            StartTimePicker.SelectedTime = vm.StartTime ?? TimeSpan.FromHours(12);
            EndTimePicker.SelectedTime = vm.EndTime ?? TimeSpan.FromHours(13);

            vm.UpdateCustomerDatesAndTimes(start, end);
        }
    }
    public bool TryGetSelectedRange(out DateTime start, out DateTime end)
    {
        start = DateTime.MinValue;
        end = DateTime.MinValue;
        var range = BookingCalendar.SelectedDateRange;
        if (range?.StartDate == null)
            return false;
        start = range.StartDate.Value.Date;
        end = range.EndDate?.Date ?? start;
        Debug.WriteLine($"Retrieved date range: Start={start.TimeOfDay}, End={end.TimeOfDay}");
        return true;
    }

    private void OnStartClicked(object sender, EventArgs e)
    {
        StartTimePicker.IsOpen = true;
    }

    private void OnEndClicked(object sender, EventArgs e)
    {
        EndTimePicker.IsOpen = true;
    }

    private void OnStartTimeChanged(object sender, EventArgs e)
    {
        if (BindingContext is BookingViewModel vm)
        {
            vm.StartTime = StartTimePicker.SelectedTime;

            vm.UpdateTimeButtons();

            if (TryGetSelectedRange(out var start, out var end))
            {
                if (EndTimePicker.SelectedTime < StartTimePicker.SelectedTime && vm.Customer.StartDate == vm.Customer.EndDate)
                {
                    var proposedEndTime = StartTimePicker.SelectedTime.Value.Add(TimeSpan.FromHours(1));
                    if (proposedEndTime >= TimeSpan.FromDays(1))
                    {
                        proposedEndTime = TimeSpan.FromHours(23) + TimeSpan.FromMinutes(59);
                    }
                    EndTimePicker.SelectedTime = proposedEndTime;
                }

                vm.UpdateCustomerDatesAndTimes(start, end);
            }
        }
    }

    private void OnEndTimeChanged(object sender, EventArgs e)
    {
        if (BindingContext is BookingViewModel vm)
        {
            vm.EndTime = EndTimePicker.SelectedTime;

            vm.UpdateTimeButtons();

            if (TryGetSelectedRange(out var start, out var end))
            {
                if (EndTimePicker.SelectedTime < StartTimePicker.SelectedTime && vm.Customer.StartDate == vm.Customer.EndDate)
                {
                    var proposedEndTime = StartTimePicker.SelectedTime.Value.Add(TimeSpan.FromHours(1));
                    if (proposedEndTime >= TimeSpan.FromDays(1))
                    {
                        proposedEndTime = TimeSpan.FromHours(23) + TimeSpan.FromMinutes(59);
                    }
                    EndTimePicker.SelectedTime = proposedEndTime;
                }

                vm.UpdateCustomerDatesAndTimes(start, end);
            }
        }
    }
}