using SendGrid.Helpers.Mail;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using SnapphaneScoutDistriktBookingApp.Services.Interface;
using SnapphaneScoutDistriktBookingApp.Models;

namespace SnapphaneScoutDistriktBookingApp.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string apiKey, string fromEmail, string toEmail, Booking booking)
        {
            toEmail = Environment.GetEnvironmentVariable("SENDGRID_EMAIL");
            string bokningsNummer = "";
            if(booking.NumberOfCanoes != null)
            {
                bokningsNummer += "<br> Antal kanoter: " + booking.NumberOfCanoes;
            }
            if(booking.NumberOfCampground != null)
            {
                bokningsNummer += "<br> Antal personer för lägerområde: " + booking.NumberOfCampground;
            }
            if(booking.NumberOfLeanTo != null)
            {
                bokningsNummer += "<br> Antal vindskydd: " + booking.NumberOfLeanTo;
            }
            if(booking.NumberOfCabin != null)
            {
                bokningsNummer += "<br> Antal i stugan: " + booking.NumberOfCabin;
            }




            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(fromEmail, "Snapphane Scoutdistrikt");
            var subject = "Bokning av: " + booking.BookingType;
            var to = new EmailAddress(toEmail, "Mottagare");
            string plainTextContent = $"Namn: {booking.Name} \t Tele nr: {booking.Phone} \t Email: {booking.Email} \t Vill boka {booking.BookingType} \t {bokningsNummer}" +
                $"\t Perioden: {booking.StartDate} - {booking.EndDate} \t Orginisation: {(booking.IsOrg ? booking.OrgName : "ingen org")}"; //info //Namn
            string infoString = plainTextContent.Replace("\t", "<br>");
            var htmlContent = $"<strong> {infoString} </strong>"; //info //Namn
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            var response = await client.SendEmailAsync(msg);
            Console.WriteLine($"E-post skickad! Statuskod: {response.StatusCode}");
        }



        public async Task SendEmailConfirmationAsync(string apiKey, string fromEmail, string toEmail, Booking booking)
        {
            string bokningsNummer = "";
            if (booking.NumberOfCanoes != null)
            {
                bokningsNummer += "<br> Antal kanoter: " + booking.NumberOfCanoes;
            }
            if (booking.NumberOfCampground != null)
            {
                bokningsNummer += "<br> Antal personer för lägerområde: " + booking.NumberOfCampground;
            }
            if (booking.NumberOfLeanTo != null)
            {
                bokningsNummer += "<br> Antal vindskydd: " + booking.NumberOfLeanTo;
            }
            if (booking.NumberOfCabin != null)
            {
                bokningsNummer += "<br> Antal i stugan: " + booking.NumberOfCabin;
            }




            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(fromEmail, "Snapphane Scoutdistrikt");
            var subject = "Bokningsbekräftelse av " + booking.BookingType;
            var to = new EmailAddress(toEmail, "Mottagare");
            string plainTextContent = $"Tack för bokning! Du har bokat datumen: {booking.StartDate} till den {booking.EndDate}. {bokningsNummer}"; //info //Namn
            string infoString = plainTextContent.Replace("\t", "<br>");
            var htmlContent = $"<strong> {infoString} </strong>"; //info //Namn
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            var response = await client.SendEmailAsync(msg);
            Console.WriteLine($"E-post skickad! Statuskod: {response.StatusCode}");
        }

    }
}
