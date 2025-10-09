using Microsoft.Maui.Controls;
using SnapphaneScoutDistriktBookingApp.ViewModels;

namespace SnapphaneScoutDistriktBookingApp.Views.Booking;

public partial class BookingStep4View : ContentView
{
    public BookingStep4View()
    {
        InitializeComponent();
    }

    private void OnCheckChange(object sender, CheckedChangedEventArgs e)
    {
        if (BindingContext is BookingViewModel vm)
        {
            vm.ToggleScoutMemberCommand.Execute(e.Value);
        }
    }
}
