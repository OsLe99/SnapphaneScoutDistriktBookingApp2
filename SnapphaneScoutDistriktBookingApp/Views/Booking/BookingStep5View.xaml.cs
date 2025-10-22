using System;
using Microsoft.Maui.Controls;
using SnapphaneScoutDistriktBookingApp.ViewModels;
using SnapphaneScoutDistriktBookingApp.Models;

namespace SnapphaneScoutDistriktBookingApp.Views.Booking
{
    public partial class BookingStep5View : ContentView
    {
        public BookingStep5View()
        {
            InitializeComponent();
            BindingContextChanged += OnBindingContextChanged;
        }

        private void OnBindingContextChanged(object sender, EventArgs e)
        {
            if (BindingContext is BookingViewModel vm)
            {
                UpdateNumberLabel(vm.NewBooking);

                vm.PropertyChanged += (s, args) =>
                {
                    if (args.PropertyName == nameof(vm.NewBooking))
                    {
                        UpdateNumberLabel(vm.NewBooking);
                    }
                };

                if (vm.NewBooking != null)
                {
                    vm.NewBooking.PropertyChanged += (s, args) =>
                    {
                        if (args.PropertyName == nameof(vm.NewBooking.NumberOfCanoes) ||
                            args.PropertyName == nameof(vm.NewBooking.NumberOfLeanTo) ||
                            args.PropertyName == nameof(vm.NewBooking.NumberOfCampground) ||
                            args.PropertyName == nameof(vm.NewBooking.NumberOfCabin))
                        {
                            UpdateNumberLabel(vm.NewBooking);
                        }
                    };
                }
            }
        }

        private void UpdateNumberLabel(Models.Booking booking)
        {
            if (booking == null)
            {
                NumberLabel.Text = string.Empty;
                return;
            }

            if (booking.NumberOfCanoes.HasValue)
                NumberLabel.Text = $"{booking.NumberOfCanoes}";
            else if (booking.NumberOfLeanTo.HasValue)
                NumberLabel.Text = $"{booking.NumberOfLeanTo} vindskydd";
            else if (booking.NumberOfCampground.HasValue)
                NumberLabel.Text = $"{booking.NumberOfCampground} personer";
            else if (booking.NumberOfCabin.HasValue)
                NumberLabel.Text = $"{booking.NumberOfCabin} personer";
            else
                NumberLabel.Text = string.Empty;
        }
    }
}