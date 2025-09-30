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
                UpdateNumberLabel(vm.Customer);

                vm.PropertyChanged += (s, args) =>
                {
                    if (args.PropertyName == nameof(vm.Customer))
                    {
                        UpdateNumberLabel(vm.Customer);
                    }
                };

                if (vm.Customer != null)
                {
                    vm.Customer.PropertyChanged += (s, args) =>
                    {
                        if (args.PropertyName == nameof(vm.Customer.NumberOfCanoes) ||
                            args.PropertyName == nameof(vm.Customer.NumberOfLeanTo) ||
                            args.PropertyName == nameof(vm.Customer.NumberOfCampground) ||
                            args.PropertyName == nameof(vm.Customer.NumberOfCabin))
                        {
                            UpdateNumberLabel(vm.Customer);
                        }
                    };
                }
            }
        }

        private void UpdateNumberLabel(Customer customer)
        {
            if (customer == null)
            {
                NumberLabel.Text = string.Empty;
                return;
            }

            if (customer.NumberOfCanoes.HasValue)
                NumberLabel.Text = $"{customer.NumberOfCanoes}";
            else if (customer.NumberOfLeanTo.HasValue)
                NumberLabel.Text = $"{customer.NumberOfLeanTo} vindskydd";
            else if (customer.NumberOfCampground.HasValue)
                NumberLabel.Text = $"{customer.NumberOfCampground} personer";
            else if (customer.NumberOfCabin.HasValue)
                NumberLabel.Text = $"{customer.NumberOfCabin} personer";
            else
                NumberLabel.Text = string.Empty;
        }
    }
}