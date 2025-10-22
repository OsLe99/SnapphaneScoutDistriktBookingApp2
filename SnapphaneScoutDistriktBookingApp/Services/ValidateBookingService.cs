using SnapphaneScoutDistriktBookingApp.Models;
using SnapphaneScoutDistriktBookingApp.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SnapphaneScoutDistriktBookingApp.Services
{
    public class ValidateBookingService : IValidateBookingService
    {
        private Regex acceptedEmail = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]{2,}$");
        public ValidateBookingService() 
        { 

        }
        #region Email, phone, name
        public bool ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                 return false;
            }
            else
            {
                if (acceptedEmail.IsMatch(email))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public bool ValidatePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrEmpty(phoneNumber))
            {
                return false;
            }
            else
            {
                var digitsOnly = new string(phoneNumber.Where(char.IsDigit).ToArray());
                return digitsOnly.Length >= 7 && digitsOnly.Length <= 15;
            }
        }
        public bool ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return false;
            }
            else
            {
                return name.All(c => char.IsLetter(c) || char.IsWhiteSpace(c));
            }
        }
        #endregion
        public List<string> ValidateBookingDetails(Booking booking)
        {
            var errors = new List<string>();
            if(!ValidateName(booking.Name))
            {
                errors.Add("Ogiltigt namn.");
            }
            if(!ValidatePhoneNumber(booking.Phone))
            {
                errors.Add("Ogiltigt telefonnummer.");
            }
            if(!ValidateEmail(booking.Email))
            {
                errors.Add("Ogiltig email.");
            }
            return errors;
        }

        public Task<bool> CheckIfValidTime(TimeSpan? StartTime, TimeSpan? EndTime)
        {
            if (StartTime >= EndTime && StartTime != null)
            {
                return Task.FromResult(false);
            }
            return Task.FromResult(true);
        }
    }
}
