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
        public List<string> ValidateBookingDetails(Customer customer)
        {
            var errors = new List<string>();
            if(!ValidateName(customer.Name))
            {
                errors.Add("Ogiltigt namn.");
            }
            if(!ValidatePhoneNumber(customer.Phone))
            {
                errors.Add("Ogiltigt telefonnummer.");
            }
            if(!ValidateEmail(customer.Email))
            {
                errors.Add("Ogiltig email.");
            }
            return errors;
        }
    }
}
