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
        public async Task SendEmail(string apiKey, string fromEmail, string toEmail, Models.Customer costumer)
        {
            toEmail = "oscar.lejon@campusnykoping.se";
            string bokningsNummer = "";
            if(costumer.NumberOfCanoes != null)
            {
                bokningsNummer += "<br> Antal kanoter: " + costumer.NumberOfCanoes;
            }
            if(costumer.NumberOfCampground != null)
            {
                bokningsNummer += "<br> Antal personer för lägerområde: " + costumer.NumberOfCampground;
            }
            if(costumer.NumberOfLeanTo != null)
            {
                bokningsNummer += "<br> Antal vindskydd: " + costumer.NumberOfLeanTo;
            }
            if(costumer.NumberOfCabin != null)
            {
                bokningsNummer += "<br> Antal i stugan: " + costumer.NumberOfCabin;
            }




            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(fromEmail, "Snapphane Scoutdistrikt");
            var subject = "Bokning av: " + costumer.BookingType;
            var to = new EmailAddress(toEmail, "Mottagare");
            string plainTextContent = $"Namn: {costumer.Name} \t Tele nr: {costumer.Phone} \t Email: {costumer.Email} \t Vill boka {costumer.BookingType} \t {bokningsNummer}" +
                $"\t Perioden: {costumer.StartDate} - {costumer.EndDate} \t Orginisation: {(costumer.IsOrg ? costumer.OrgName : "ingen org")}"; //info //Namn
            string infoString = plainTextContent.Replace("\t", "<br>");
            var htmlContent = $"<strong> {infoString} </strong>"; //info //Namn
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            var response = await client.SendEmailAsync(msg);
            Console.WriteLine($"E-post skickad! Statuskod: {response.StatusCode}");
        }



        public async Task SendEmailConfirmation(string apiKey, string fromEmail, string toEmail, Models.Customer costumer)
        {
            string bokningsNummer = "";
            if (costumer.NumberOfCanoes != null)
            {
                bokningsNummer += "<br> Antal kanoter: " + costumer.NumberOfCanoes;
            }
            if (costumer.NumberOfCampground != null)
            {
                bokningsNummer += "<br> Antal personer för lägerområde: " + costumer.NumberOfCampground;
            }
            if (costumer.NumberOfLeanTo != null)
            {
                bokningsNummer += "<br> Antal vindskydd: " + costumer.NumberOfLeanTo;
            }
            if (costumer.NumberOfCabin != null)
            {
                bokningsNummer += "<br> Antal i stugan: " + costumer.NumberOfCabin;
            }




            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(fromEmail, "Snapphane Scoutdistrikt");
            var subject = "Bokningsbekräftelse av " + costumer.BookingType;
            var to = new EmailAddress(toEmail, "Mottagare");
            string plainTextContent = $"Tack för bokning! Du har bokat datumen: {costumer.StartDate} till den {costumer.EndDate}. {bokningsNummer}"; //info //Namn
            string infoString = plainTextContent.Replace("\t", "<br>");
            var htmlContent = $"<strong> {infoString} </strong>"; //info //Namn
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            var response = await client.SendEmailAsync(msg);
            Console.WriteLine($"E-post skickad! Statuskod: {response.StatusCode}");
        }

    }
}
