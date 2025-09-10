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

namespace SnapphaneScoutDistriktBookingApp.Services
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string apiKey, string fromEmail, string toEmail, Models.Customer customer)
        {
            toEmail = "oscar.lejon@campusnykoping.se";
            string bokningsNummer = "";
            if(customer.NumberOfCanoes != null)
            {
                bokningsNummer += "<br> Antal kanoter: " + customer.NumberOfCanoes;
            }
            if(customer.NumberOfCampground != null)
            {
                bokningsNummer += "<br> Antal personer för lägerområde: " + customer.NumberOfCampground;
            }
            if(customer.NumberOfLeanTo != null)
            {
                bokningsNummer += "<br> Antal vindskydd: " + customer.NumberOfLeanTo;
            }
            if(customer.NumberOfCabin != null)
            {
                bokningsNummer += "<br> Antal i stugan: " + customer.NumberOfCabin;
            }




            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(fromEmail, "Snapphane Scoutdistrikt");
            var subject = "Bokning av: " + customer.BookingType;
            var to = new EmailAddress(toEmail, "Mottagare");
            string plainTextContent = $"Namn: {customer.Name} \t Tele nr: {customer.Phone} \t Email: {customer.Email} \t Vill boka {customer.BookingType} \t {bokningsNummer}" +
                $"\t Perioden: {customer.StartDate} - {customer.EndDate} \t Orginisation: {(customer.IsOrg ? customer.OrgName : "ingen org")}"; //info //Namn
            string infoString = plainTextContent.Replace("\t", "<br>");
            var htmlContent = $"<strong> {infoString} </strong>"; //info //Namn
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            var response = await client.SendEmailAsync(msg);
            Console.WriteLine($"E-post skickad! Statuskod: {response.StatusCode}");
        }



        public async Task SendEmailConfirmationAsync(string apiKey, string fromEmail, string toEmail, Models.Customer customer)
        {
            string bokningsNummer = "";
            if (customer.NumberOfCanoes != null)
            {
                bokningsNummer += "<br> Antal kanoter: " + customer.NumberOfCanoes;
            }
            if (customer.NumberOfCampground != null)
            {
                bokningsNummer += "<br> Antal personer för lägerområde: " + customer.NumberOfCampground;
            }
            if (customer.NumberOfLeanTo != null)
            {
                bokningsNummer += "<br> Antal vindskydd: " + customer.NumberOfLeanTo;
            }
            if (customer.NumberOfCabin != null)
            {
                bokningsNummer += "<br> Antal i stugan: " + customer.NumberOfCabin;
            }




            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(fromEmail, "Snapphane Scoutdistrikt");
            var subject = "Bokningsbekräftelse av " + customer.BookingType;
            var to = new EmailAddress(toEmail, "Mottagare");
            string plainTextContent = $"Tack för bokning! Du har bokat datumen: {customer.StartDate} till den {customer.EndDate}. {bokningsNummer}"; //info //Namn
            string infoString = plainTextContent.Replace("\t", "<br>");
            var htmlContent = $"<strong> {infoString} </strong>"; //info //Namn
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            var response = await client.SendEmailAsync(msg);
            Console.WriteLine($"E-post skickad! Statuskod: {response.StatusCode}");
        }

    }
}
